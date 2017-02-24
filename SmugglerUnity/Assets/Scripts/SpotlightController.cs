using UnityEngine;

public class SpotlightController : MonoBehaviour {

    GameObject spotlight;

    float startRotationX;
    public float rotationSpeed;
    public float rotationLimit;
    
    private float rotationMax;
    private float rotationMin;

    private bool goBack;
    private bool hitMax;
    private bool hitMin;
    
    // Use this for initialization
    void Start() {
        rotationMax = startRotationX + rotationLimit;
        rotationMin = startRotationX - rotationLimit;

        Debug.Log(rotationMax);
    }

    // Update is called once per frame
    void Update() {
        Debug.Log(rotationMax + " : " + transform.rotation.eulerAngles);
        if (!hitMax) {
            if (rotationMax > transform.rotation.eulerAngles.z) {
                transform.Rotate(Vector3.forward, rotationSpeed);
            } else {
                hitMax = true;
            }
        }
        if (hitMax) {
            if (rotationMin < transform.rotation.eulerAngles.z || rotationMin < transform.rotation.eulerAngles.z) {
                transform.Rotate(Vector3.back, rotationSpeed);
            } else {
                hitMax = true;
            }
        }
    }
}

