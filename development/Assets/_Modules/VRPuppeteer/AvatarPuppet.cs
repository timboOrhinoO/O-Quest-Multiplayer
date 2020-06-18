using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AvatarPuppet : MonoBehaviourPun
{
    [Header("Puppettiering")]
    public Transform head;
    public Transform handL;
    public Transform handR;

    [Header("noVR Behaviour")]
    public bool noVR = false;
    public GameObject ovrHandL;
    public GameObject ovrHandR;
    private GameObject ovrRig;
    private Component charControl;

    void Start()
    {
        PhotonView view = GetComponent<PhotonView>();
        if(view.IsMine)
        {
            LocalController local = FindObjectOfType<LocalController>();
            local.remoteHead = head;
            local.remoteHandL = handL;
            local.remoteHandR = handR;

            ovrRig = GameObject.Find("OVRCameraRig");
            noVR = ovrRig.GetComponent<LocalCharacterController>().noVR;

            if (noVR)
            {
                ovrHandL.SetActive(false);
                ovrHandR.SetActive(false);
            }
        }
    }

}
