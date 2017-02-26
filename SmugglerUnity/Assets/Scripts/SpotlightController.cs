using System;
using UnityEngine;

public class SpotlightController : MonoBehaviour {

    public float rotationLimit;
    public float rotationSpeed;
    private float speed;

    public float maxLimit;
    public float minLimit;

    private bool hitMax = false;

    public LayerMask layers;
    private Light spotlight;
    public float detectionRange;
    private GameObject lightCone;
    private float lightAngle;

    private Color defaultColor;
    public Color alertColor;

    public bool debugMode;

    // Use this for initialization
    void Start() {
        spotlight = GetComponentInChildren<Light>();
        lightCone = transform.GetChild(0).gameObject;

        defaultColor = spotlight.color;

        //set starting rotationspeed speed
        speed = rotationSpeed;
    }

    // Update is called once per frame
    void Update() {
        //update detection range (cone size)
        lightCone.transform.localScale = new Vector3(detectionRange * 26.44238f, detectionRange * 26.44238f, detectionRange * 26.44238f);

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

    public void OnChildTriggerEnter(Collider childCollider, Collider player) {
        if (player.tag == "Player") {
            if (Physics.Linecast(transform.position, player.transform.position, layers)) {
                if (debugMode) { Debug.DrawLine(transform.position, player.transform.position, Color.green, 3); }
                spotlight.color = defaultColor;
            } else {
                if (debugMode) { Debug.DrawLine(transform.position, player.transform.position, Color.red, 3); }
                spotlight.color = alertColor;
            }
        }
    }
    public void OnChildTriggerExit(Collider childCollider, Collider player) {
        if (player.tag == "Player") {
            spotlight.color = defaultColor;
        }
    }
    public void OnChildTriggerStay(Collider childCollider, Collider player) {
        if (player.tag == "Player") {
            if (Physics.Linecast(transform.position, player.transform.position, layers)) {
                if (debugMode) { Debug.DrawLine(transform.position, player.transform.position, Color.green, 3); }
                spotlight.color = defaultColor;
            } else {
                if (debugMode) { Debug.DrawLine(transform.position, player.transform.position, Color.red, 3); }
                spotlight.color = alertColor;
            }
        }
    }
}