using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LocalPlayerControls : MonoBehaviourPun
{
    Vector3 pos;

    public bool isMine = false;

    public bool primaryIndexTrigger = false;
    public bool secondaryIndexTrigger = false;

    public float movSpeed = 0.7f;

    public GameObject ovrCamRig;

    // Start is called before the first frame update
    void Start()
    {
        ovrCamRig = GameObject.Find("OVRCameraRig");
        ovrCamRig.transform.position = pos;
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
                pos += (transform.forward * movSpeed * Time.deltaTime);
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
