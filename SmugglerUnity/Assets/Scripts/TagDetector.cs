using UnityEngine;

public class TagDetector : MonoBehaviour {


    [Range(1, 4)]
    [SerializeField]
    float DetectorRange = 2;

    public bool isCollidingWithPlayer;
    public string[] tags;
    public GameObject otherObject;

    void Start() {
        GetComponent<SphereCollider>().radius = DetectorRange;
    }

    void OnTriggerEnter(Collider other) {
        foreach (string tag in tags) {
            if (other.tag == tag) {
                isCollidingWithPlayer = true;

                if (otherObject == null)
                    otherObject = other.gameObject;
            }
        }
    }

    void OnTriggerExit(Collider other) {
        foreach (string tag in tags) {
            if (other.tag == tag)
                isCollidingWithPlayer = false;
        }
    }
}
