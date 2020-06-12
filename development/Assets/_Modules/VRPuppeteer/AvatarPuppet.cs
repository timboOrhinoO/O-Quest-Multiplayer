using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AvatarPuppet : MonoBehaviourPun
{
    public Transform head, handL, handR;

    void Start()
    {
        PhotonView view = GetComponent<PhotonView>();
        if(view.IsMine){
            LocalController local = FindObjectOfType<LocalController>();
            local.remoteHead = head;
            local.remoteHandL = handL;
            local.remoteHandR = handR;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
