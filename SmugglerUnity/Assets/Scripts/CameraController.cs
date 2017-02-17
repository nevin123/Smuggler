using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Camera playerCamera;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        playerCamera.transform.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 2, -8);
    }
}
