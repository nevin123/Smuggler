using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepAwake : MonoBehaviour {

    private Rigidbody rigidBody;

    // Use this for initialization
    void Start() {
        rigidBody = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        rigidBody.WakeUp();
    }
}
