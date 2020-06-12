using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using InputTracking = UnityEngine.XR.InputTracking;
using Node = UnityEngine.XR.XRNode;

public class LocalPlayerControls : MonoBehaviourPunCallbacks
{
    public GameObject ovrCamRig;
    public Transform leftHand;
    public Transform rightHand;
    public Camera centerEye;
    Vector3 pos;

    public bool isMine = false;

    public bool primaryIndexTrigger = false;
    public bool secondaryIndexTrigger = false;

    public float movSpeed = 0.7f;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMine)
        {
            Destroy(ovrCamRig);
        }
        else
        {
            // take care of camera when other player joins
            if(centerEye.tag != "MainCamera")
            {
                centerEye.tag = "MainCamera";
                centerEye.enabled = true;
            }

            //take care of handPos tracking
            leftHand.localRotation = InputTracking.GetLocalRotation(Node.LeftHand);
            leftHand.localPosition = InputTracking.GetLocalPosition(Node.RightHand);

            rightHand.localRotation = InputTracking.GetLocalRotation(Node.RightHand);
            rightHand.localPosition = InputTracking.GetLocalPosition(Node.RightHand);

            //handle pos and rot of player
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

            transform.position = pos;


            Vector3 euler = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(euler);
            //maybe set local rot too?
            transform.localRotation = Quaternion.Euler(euler);
            


        }


    }
}
