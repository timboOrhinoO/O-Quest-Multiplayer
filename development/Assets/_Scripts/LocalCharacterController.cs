using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


[RequireComponent(typeof(CharacterController))]

public class LocalCharacterController : MonoBehaviourPun
{
    private CharacterController _controller;
    private GameObject avatarPuppet;

    public bool isMoving = false;

    [Header("Multiplayer Check")]
    public bool canvasButtonPressed = false;
    public bool isMine = false;
    public GameObject leftHandAnchor;
    private Vector3 current;
    private Vector3 previous;

    [Header("VR Interaction")]
    public bool primaryIndexTrigger = false;
    public bool secondaryIndexTrigger = false;
    private Vector3 move;
    public Camera playerCam;
    public float movSpeed = 0.7f;
    public float magnitude = 1f;
    public float multiplier = 5f;
    private float stime = 0.0f;

    [Header("MouseFallback")]
    public bool noVR = false;
    public Camera fallbackCam;
    public float speedH = 2.0f;
    public float speedV = 2.0f;
    private float yaw = 0.1f;
    private float pitch = 0.1f;

    [Header("Character Settings")]
    public float Gravity = -1f;
    private Vector3 velocity;
    
    

    void Start()
    {
        _controller = GetComponent<CharacterController>();
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
                // triggers audio footsteps while moving
                isMoving = false;

                // if needed for playmaker interaction
                isMine = true;

                //handle input
                primaryIndexTrigger = false;
                secondaryIndexTrigger = false;

                // keeping the player grounded
                velocity.y += Gravity * Time.deltaTime;
                _controller.Move(velocity * Time.deltaTime);
                
                //magnitude calculation
                magnitude = Mathf.Abs(Mathf.SmoothDamp(previous.magnitude, leftHandAnchor.transform.position.magnitude, ref stime, 0, 3) - transform.position.magnitude) * multiplier;
                previous = leftHandAnchor.transform.position;

                if (magnitude > movSpeed)
                {
                    magnitude = movSpeed;
                }

                    // noVR movement based on VR interaction
                    // W as movement along camera view direction
                if (noVR)
                {
                    // moving the mouse is moving the camera
                    // setup their parameters
                    yaw += speedH * Input.GetAxis("Mouse X");
                    pitch -= speedV * Input.GetAxis("Mouse Y");

                    fallbackCam.transform.eulerAngles = new Vector3(pitch, yaw, 0.1f);
                    transform.rotation = fallbackCam.transform.rotation;

                    // move forward by press W
                    // direction forward is based on fallback cam forward direction 
                if (Input.GetKey(KeyCode.W))
                {
                        isMoving = true;
                        movSpeed = 1f;
                        move = fallbackCam.transform.forward;
                        _controller.Move(move * Time.deltaTime * movSpeed);

                        if (Input.GetKey(KeyCode.LeftShift))
                        {
                            movSpeed = 2f;
                            move = fallbackCam.transform.forward;
                            _controller.Move(move * Time.deltaTime * movSpeed);
                        }
                }

                if (Input.GetKey(KeyCode.S))
                    {
                        isMoving = true;
                        movSpeed = 1f;
                        move = -fallbackCam.transform.forward;
                        _controller.Move(move * Time.deltaTime * movSpeed);
                    }

                if (Input.GetKey(KeyCode.A))
                    {
                        isMoving = true;
                        movSpeed = 1f;
                        move = -fallbackCam.transform.right;
                        _controller.Move(move * Time.deltaTime * movSpeed);
                    }

                    if (Input.GetKey(KeyCode.D))
                    {
                        isMoving = true;
                        movSpeed = 1f;
                        move = fallbackCam.transform.right;
                        _controller.Move(move * Time.deltaTime * movSpeed);
                    }

                }

                // VR Interaction
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
                    isMoving = true;

                    // move character
                    move = playerCam.transform.forward;                    
                    _controller.Move(move * Time.deltaTime * movSpeed * magnitude);

                }

            }

        }

    }

}
