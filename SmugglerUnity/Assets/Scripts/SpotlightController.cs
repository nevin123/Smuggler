using System;
using UnityEngine;

public class SpotlightController : MonoBehaviour {

    public float rotationLimit;
    public float rotationSpeed;
    private float speed;

    private bool hitMax = false;

    // Use this for initialization
    void Start() {
        speed = rotationSpeed;
    }

    // Update is called once per frame
    void Update() {
        Debug.Log(string.Format("rotationMax: {0} rotationMin: {1} hitMax: {3}\neulerAngles: {2}", rotationLimit, 360 - rotationLimit, transform.rotation.eulerAngles.z, hitMax));

        if (!hitMax) {
            if (transform.rotation.eulerAngles.z >= rotationLimit - 2 &&
                transform.rotation.eulerAngles.z <= rotationLimit + 2) {
                hitMax = true;
            }
            speed = rotationSpeed;
        }

        if (hitMax) {
            if (transform.rotation.eulerAngles.z >= 360 - rotationLimit - 2 &&
                transform.rotation.eulerAngles.z <= 360 - rotationLimit + 2) {
                hitMax = false;
            }
            speed = -rotationSpeed;
        }
        transform.Rotate(Vector3.forward, speed);

        //if (!hitMax)
        //{
        //    if (rotationLimit > transform.rotation.eulerAngles.z)
        //    {
        //        transform.Rotate(Vector3.forward, rotationSpeed);
        //    }
        //    else
        //    {
        //        hitMax = true;
        //    }
        //}
        //if (hitMax)
        //{
        //    if (transform.rotation.eulerAngles.z >= 0 && transform.rotation.eulerAngles.z <= rotationLimit ||
        //        transform.rotation.eulerAngles.z <= 360 && transform.rotation.eulerAngles.z > rotationLimit)
        //    {

        //        transform.Rotate(Vector3.back, rotationSpeed);

        //    }
        //    else if (transform.rotation.eulerAngles.z <= 360 - rotationLimit &&
        //             transform.rotation.eulerAngles.z > rotationLimit + 1)
        //    {
        //        hitMax = false;
        //    }

        //    //if (rotationMin < transform.rotation.eulerAngles.z || rotationMin < transform.rotation.eulerAngles.z) {
        //    //    transform.Rotate(Vector3.back, rotationSpeed);
        //    //} else {
        //    //    hitMax = true;
        //    //}
        //}
    }
}