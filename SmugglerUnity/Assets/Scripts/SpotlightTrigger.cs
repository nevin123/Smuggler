using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightTrigger : MonoBehaviour {
    private SpotlightController parent;
    private Collider spotCollider;

    // Use this for initialization
    void Start() {
        parent = GetComponentInParent<SpotlightController>();
        spotCollider = GetComponent<Collider>();
    }
    // Update is called once per frame
    void OnTriggerEnter(Collider hitCollider) {
        parent.OnChildTriggerEnter(spotCollider, hitCollider);
    }

    void OnTriggerExit(Collider hitCollider) {
        parent.OnChildTriggerExit(spotCollider, hitCollider);
    }
    void OnTriggerStay(Collider hitCollider) {
        parent.OnChildTriggerStay(spotCollider, hitCollider);
    }
}
