using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddLevelColliders : MonoBehaviour
{
    //This script adds a collider to every child object, primarily made to quickly add colliders to an entire level.
    void Start() {
        for (int i = 0; i < transform.childCount; i++) {
            var child = transform.GetChild(i).gameObject;
            if (child.GetComponent<Collider>() == null)
                child.AddComponent<MeshCollider>();
            child.layer = LayerMask.NameToLayer("Environment");
        }
    }
}
