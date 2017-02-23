using UnityEngine;

public class SpotlightController : MonoBehaviour {

    GameObject spotlight;

    float startRotationX;
    public float rotationSpeed;
    public float rotationLimit;

    private float rotationMax;
    private float rotationMin;
    private bool goBack;

    // Use this for initialization
    void Start() {
        spotlight = transform.GetChild(0).gameObject;

        startRotationX = spotlight.transform.localEulerAngles.x;

        rotationMax = startRotationX + rotationLimit;
        rotationMin = startRotationX - rotationLimit;
    }

    // Update is called once per frame
    void Update() {

        if (spotlight.transform.localEulerAngles.x < rotationMax) {
            goBack = true;
        }
        if (spotlight.transform.localEulerAngles.x > rotationMin) {
            goBack = false;
        }

        Debug.Log(string.Format("{0} : {1}({2}) : {2}", rotationMin, spotlight.transform.localEulerAngles.x, goBack, rotationMax));

        if (!goBack) {
            spotlight.transform.Rotate(Vector3.up, rotationSpeed);
        }
        else {
            spotlight.transform.Rotate(Vector3.up, -rotationSpeed);
        }
    }
}
