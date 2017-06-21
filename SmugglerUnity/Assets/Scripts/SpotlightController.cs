using System;
using System.Security.Permissions;
using UnityEngine;

public class SpotlightController : MonoBehaviour {

    [Header("Movement")]
    public float maxLimit;
    public float minLimit;
    public float rotationSpeed;
    private Quaternion defaultRotation;

    private float speed;
    private bool hitMax;

    [Header("Player Detection")]
    public LayerMask blockingLayers;
    public float detectionRange;
    private bool targetVisible;
    private Vector3 lastPlayer;

    private GameObject lightCone;
    private Light spotlight;
    private float lightAngle;

    private Color defaultColor;
    public Color alertColor;

    [Header("Debug")]
    public bool debugMode;
    public Color debugHit = Color.red;
    public Color debugExit = Color.red;
    public Color debugHidden = Color.green;


    // Use this for initialization
    void Start() {
        spotlight = GetComponentInChildren<Light>();
        lightCone = transform.GetChild(0).gameObject;

        defaultColor = spotlight.color;
        defaultRotation = transform.rotation;

        //set starting rotationspeed speed
        speed = rotationSpeed;
    }

    // Update is called once per frame
    void Update() {
        //update detection range (cone size)
        lightCone.transform.localScale = new Vector3(detectionRange * 26.44238f, detectionRange * 26.44238f, detectionRange * 26.44238f);
        //never change 26.
        CameraLoop();

        if (!targetVisible) {
        } else
        {
            //somehow make the spitlight chase the player 

            //lastPlayer = new Vector3(lastPlayer.x, transform.position.y, lastPlayer.y);
            //transform.rotation = Quaternion.LookRotation(lastPlayer.position - transform.position);
            //transform.LookAt(lastPlayer);
        }
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

    //public void OnChildTriggerEnter(Collider childCollider, Collider player) {
    //    if (player.tag == "Player") {
    //        RaycastHit hit;
    //        if (Physics.Linecast(transform.position, player.transform.position, out hit, blockingLayers)) {
    //            if (debugMode) {
    //                Debug.DrawLine(transform.position, hit.point, debugHit, 3);
    //                Debug.DrawLine(hit.point, player.transform.position, debugHidden, 3);
    //                targetVisible = false;
    //            }
    //            spotlight.color = defaultColor;
    //        } else {
    //            if (debugMode) { Debug.DrawLine(transform.position, player.transform.position, debugHit, 3); }
    //            spotlight.color = alertColor;

    //            targetVisible = true;
    //        }
    //    }
    //}
    //public void OnChildTriggerExit(Collider childCollider, Collider player) {
    //    if (player.tag == "Player") {
    //        spotlight.color = defaultColor;

    //        targetVisible = false;
    //        lastPlayer = player.transform.position;
    //        transform.rotation = defaultRotation;
    //    }
    //}
    public void OnChildTriggerStay(Collider childCollider, Collider player) {
        if (player.tag == "Player") {
            RaycastHit hit;
            if (Physics.Linecast(transform.position, player.transform.position, out hit, blockingLayers)) {
                if (debugMode) {
                    Debug.DrawLine(transform.position, hit.point, debugHit, 3);
                    Debug.DrawLine(hit.point, player.transform.position, debugExit, 3);

                    targetVisible = false;
                }
                spotlight.color = defaultColor;
            } else {
                if (debugMode) { Debug.DrawLine(transform.position, player.transform.position, debugHit, 3); }
                spotlight.color = alertColor;

                targetVisible = true;
            }
            lastPlayer = player.transform.position;
        }
    }
}