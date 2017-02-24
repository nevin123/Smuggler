using System;
using UnityEngine;

public class SpotlightController : MonoBehaviour {

    public float rotationLimit;
    public float rotationSpeed;
    private float speed;

    public float maxLimit;
    public float minLimit;

    private bool hitMax = false;

    // Use this for initialization
    void Start() {
        speed = rotationSpeed;
    }

    // Update is called once per frame
    void Update() {
        CameraLoop();
    }

    private void CameraLoop() {
        //Debug.Log(string.Format("rotationMax: {0} rotationMin: {1} hitMax: {3}\neulerAngles: {2}", rotationLimit, 360 - rotationLimit, transform.rotation.eulerAngles.z, hitMax));
        if (!hitMax) {
            if (transform.rotation.eulerAngles.z >= maxLimit - 2 &&
                transform.rotation.eulerAngles.z <= maxLimit + 2) {
                hitMax = true;
            }
            speed = rotationSpeed;
        }

        if (hitMax) {
            if (transform.rotation.eulerAngles.z >= 360 - minLimit - 2 &&
                transform.rotation.eulerAngles.z <= 360 - minLimit + 2) {
                hitMax = false;
            }
            speed = -rotationSpeed;
        }
        transform.Rotate(Vector3.forward, speed);
    }
}