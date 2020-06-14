using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


[RequireComponent(typeof(CharacterController))]

public class LocalCharacterController : MonoBehaviourPun
{
    private CharacterController _controller;

    [Header("Checks")]
    public bool canvasButtonPressed = false;
    public bool isMine = false;
    public bool primaryIndexTrigger = false;
    public bool secondaryIndexTrigger = false;

    [Header("Gravity")]
    public float Gravity = -1f;
    private Vector3 velocity;

    [Header("Movement")]
    private Vector3 move;
    private float dir;
    public float movSpeed = 0.7f;
    public float magnitude = 1f;

    private GameObject centerEyeAnchor;
    private GameObject avatarPuppet;

    void Start()
    {

        _controller = GetComponent<CharacterController>();

        centerEyeAnchor = GameObject.Find("CenterEyeAnchor");
        dir = centerEyeAnchor.transform.rotation.y;

        canvasButtonPressed = false;

    }

    void Update()
    {
        if (canvasButtonPressed)
        {
            // need to setup - search for the one with "isMine=true"
            avatarPuppet = GameObject.Find("AvatarPuppet(Clone)");
            PhotonView view = avatarPuppet.GetComponent<PhotonView>();

            if (view.IsMine)
            {
                isMine = true;

                //handle input
                primaryIndexTrigger = false;
                secondaryIndexTrigger = false;

                velocity.y += Gravity * Time.deltaTime;
                _controller.Move(velocity * Time.deltaTime);

                if (_controller.isGrounded && velocity.y < 0)
                {
                    velocity.y = 0f;
                }

                // primaryIndex = leftHandIndex
                if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
                {
                    primaryIndexTrigger = true;
                }

                // secondaryIndex =rightHandIndex
                if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
                {
                    secondaryIndexTrigger = true;
                }

                // if both pressed - move character forward in look direction
                if (primaryIndexTrigger && secondaryIndexTrigger)
                {

                    move = Quaternion.Euler(0, dir, 0) * move;
                    move = new Vector3(0, 0, 1);

                    _controller.Move(move * Time.deltaTime * movSpeed * magnitude);
                }

            }

        }

    }

}
