using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Settings = UnityEngine.XR.XRSettings;
using Node = UnityEngine.XR.XRNode;

public class LocalPlayerControls : MonoBehaviourPun
{
    public bool isMine = false;

    public bool primaryIndexTrigger = false;
    public bool secondaryIndexTrigger = false;

    Vector3 pos;
    public Transform dir;

    public float vrHeight = 1f;
    public float movSpeed = 0.7f;

    public GameObject ovrCamRig;
    public GameObject centerEyeAnchor;

    // Start is called before the first frame update
    void Start()
    {
        ovrCamRig = GameObject.Find("OVRCameraRig");
        ovrCamRig.transform.forward = pos;
        pos.y = vrHeight;

        centerEyeAnchor = GameObject.Find("CenterEyeAnchor");
        dir = centerEyeAnchor.transform;
        //pos = transform.position;
        PhotonView view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        isMine = false;

        PhotonView view = GetComponent<PhotonView>();
        if (view.IsMine)
        {
            isMine = true;

            //handle input
            primaryIndexTrigger = false;
            secondaryIndexTrigger = false;

            if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
            {
                primaryIndexTrigger = true;
            }

            if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            {
                secondaryIndexTrigger = true;
            }

            if (primaryIndexTrigger & secondaryIndexTrigger)
            {
                pos += (dir.transform.forward * movSpeed * Time.deltaTime);
                pos.y = vrHeight;
               // pos += (transform.forward * movSpeed * Time.deltaTime);
            }

            ovrCamRig.transform.position = pos;

            /*
            Vector3 euler = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(euler);
            transform.localRotation = Quaternion.Euler(euler);
            */


        }


    }
}
