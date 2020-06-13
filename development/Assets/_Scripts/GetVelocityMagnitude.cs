using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GetVelocityMagnitude : MonoBehaviourPun
{

    public Vector3 FrameVelocity { get; set; }
    public Vector3 PrevPosition { get; set; }

    public float velocityMagnitude = 0f;

    public GameObject avatarPuppet;

    // Start is called before the first frame update
    void Start()
    {
        PhotonView view = avatarPuppet.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {

        PhotonView view = GetComponent<PhotonView>();
        if (view.IsMine)
        {
            //get velocity of object
            Vector3 currFrameVelocity = (transform.position - PrevPosition) / Time.deltaTime;
            FrameVelocity = Vector3.Lerp(FrameVelocity, currFrameVelocity, 0.1f);
            PrevPosition = transform.position;

            velocityMagnitude = Mathf.Clamp(FrameVelocity.magnitude, 0, 1);



            

        }

    }
}
