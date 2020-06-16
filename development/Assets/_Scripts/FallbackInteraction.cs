using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallbackInteraction : MonoBehaviour
{
    private CharacterController _controller;

    [Header("MouseFallback")]
    public float speedH = 2.0f;
    public float speedV = 2.0f;
    private float yaw = 0.0f;
    private float pitch = 0.0f;

    [Header("Movement")]
    private Vector3 move;
    public float movSpeed = 1.5f;
    private float yRot = 0f;

    [Header("Gravity")]
    public float Gravity = -1f;
    private Vector3 velocity;

    [Header("Movement")]
    public Camera playerCam;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        velocity.y += Gravity * Time.deltaTime;
        _controller.Move(velocity * Time.deltaTime);

        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        if (Input.GetKey(KeyCode.W))
        {
            playerCam.transform.eulerAngles = new Vector3(pitch, yaw, 0.1f);
            yRot = playerCam.transform.rotation.y;

            transform.rotation = playerCam.transform.rotation;
            move = Quaternion.Euler(0, yRot, 0) * transform.forward;
            
            _controller.Move(move * Time.deltaTime * movSpeed);

        }

    }
}
