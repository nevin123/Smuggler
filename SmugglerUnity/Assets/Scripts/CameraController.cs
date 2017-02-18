using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Camera playerCamera;
    public Vector3 cameraOffset;
    public float speed;
    public float smoothTime = 0.3F;

    private Vector3 newPos;
    private Vector3 velocity = Vector3.zero;

    // Use this for initialization
    void Start() {
        if (playerCamera == null) {
            playerCamera = Camera.main;
        }
    }

    // Update is called once per frame
    void LateUpdate() {
        newPos = transform.position + cameraOffset;

        //playerCamera.transform.transform.position = Vector3.Lerp(playerCamera.transform.position, newPos, Time.deltaTime * speed);
        playerCamera.transform.position = Vector3.SmoothDamp(playerCamera.transform.position, newPos, ref velocity,smoothTime);
    }
}
