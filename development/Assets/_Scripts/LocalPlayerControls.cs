using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LocalPlayerControls : MonoBehaviourPun
{
    public bool isMine = false;

    public bool primaryIndexTrigger = false;
    public bool secondaryIndexTrigger = false;

    Vector3 pos;
    public Transform dir;

    //vrHeight has to be tracking EyeLevel
    public float vrHeight = 1f;
    public float movSpeed = 0.7f;
    public float magnitude = 1f;

    private GameObject ovrCamRig;
    private GameObject centerEyeAnchor;

    

    // Start is called before the first frame update
    void Start()
    {
        ovrCamRig = GameObject.Find("OVRCameraRig");
        ovrCamRig.transform.forward = pos;
        pos.y = vrHeight;

        centerEyeAnchor = GameObject.Find("CenterEyeAnchor");
        dir = centerEyeAnchor.transform;

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
                pos += (dir.transform.forward * movSpeed * magnitude * Time.deltaTime);
                pos.y = vrHeight;
               
            }

            ovrCamRig.transform.position = pos;

            


        }


    }
}
