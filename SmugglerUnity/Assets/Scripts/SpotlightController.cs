using System;
using UnityEngine;

public class SpotlightController : MonoBehaviour {

    [Header("Movement")]
    public float maxLimit;
    public float minLimit;
    public float rotationSpeed;

    private float speed;
    private bool hitMax;

    [Header("Player Detection")]
    public bool debugMode;
    public LayerMask layers;
    public float detectionRange;

    private GameObject lightCone;
    private Light spotlight;
    private float lightAngle;

    private Color defaultColor;
    public Color alertColor;
    
    private Color debugHit = Color.red;
    private Color debugHidden = Color.green;

    private bool targetVisible;

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
            RaycastHit hit;
            if (Physics.Linecast(transform.position, player.transform.position, out hit, layers)) {
                if (debugMode) {
                    Debug.DrawLine(transform.position, hit.point, debugHit, 3);
                    Debug.DrawLine(hit.point, player.transform.position, debugHidden, 3);
                }
                spotlight.color = defaultColor;
            } else {
                if (debugMode) { Debug.DrawLine(transform.position, player.transform.position, debugHit, 3); }
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
            RaycastHit hit;
            if (Physics.Linecast(transform.position, player.transform.position, out hit, layers)) {
                if (debugMode) {
                    Debug.DrawLine(transform.position, hit.point, debugHit, 3);
                    Debug.DrawLine(hit.point, player.transform.position, debugHidden, 3);
                }
                spotlight.color = defaultColor;
            } else {
                if (debugMode) { Debug.DrawLine(transform.position, player.transform.position, debugHit, 3); }
                spotlight.color = alertColor;

                Vector3.RotateTowards(transform.position, player.transform.position, 1, 1);
            }
        }
    }
}