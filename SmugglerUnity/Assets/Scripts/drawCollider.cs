using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode()]
public class drawCollider : MonoBehaviour {
    private Collider collider;
    private Vector3[] colliderBoxSizes;

    public Color triggeredColor = Color.red;
    public Color defaultColor = Color.yellow;
    private Color colliderColor = Color.red;
    // Use this for initialization
    void Start() {
        collider = GetComponent<BoxCollider>();
        colliderColor = defaultColor;
    }

    // Update is called once per frame
    void OnDrawGizmos() {
        drawBox();
    }

    void drawBox() {
        Gizmos.color = colliderColor;
        Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
        Gizmos.DrawWireCube(collider.bounds.center, new Vector3(collider.bounds.size.x, collider.bounds.size.y, 0));
        Gizmos.DrawWireCube(collider.bounds.center, new Vector3(collider.bounds.size.x, 0, collider.bounds.size.z));
        Gizmos.DrawWireCube(collider.bounds.center, new Vector3(0, collider.bounds.size.y, collider.bounds.size.z));
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            colliderColor = triggeredColor;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            colliderColor = defaultColor;
        }
    }
}
