using UnityEngine;

public class SpotlightController : MonoBehaviour {

    float startRotationX;
    public float rotationSpeed;
    public float rotationLimit;

    private float rotationMax;
    private float rotationMin;
    private bool goBack;

    // Use this for initialization
    void Start() {
        startRotationX = transform.localEulerAngles.x;

        rotationMax = startRotationX + rotationLimit;
        rotationMin = startRotationX - rotationLimit;
    }

    // Update is called once per frame
    void Update() {

        if (transform.localEulerAngles.x < rotationMax) {
            goBack = true;
        }
        if (transform.localEulerAngles.x > rotationMin) {
            goBack = false;
        }

        Debug.Log(string.Format("{0} : {1}({2}) : {2}", rotationMin, transform.localEulerAngles.x, goBack, rotationMax));

        if (!goBack) {
            transform.Rotate(Vector3.up, rotationSpeed);
        }
        else {
            transform.Rotate(Vector3.up, -rotationSpeed);
        }
    }
}
