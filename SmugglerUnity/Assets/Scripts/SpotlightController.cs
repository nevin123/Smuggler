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
        spotlight = transform.GetChild(0).gameObject;

        startRotationX = spotlight.transform.localEulerAngles.x;

        rotationMax = startRotationX + rotationLimit;
        rotationMin = startRotationX - rotationLimit;

        Debug.Log(rotationMax);
    }

    // Update is called once per frame
    void Update() {

        if (!hitMax) {
            if (spotlight.transform.localEulerAngles.x < rotationMax) {
                spotlight.transform.Rotate(Vector3.up, rotationSpeed);
            }
            else {
                hitMax = true;
            }
        }

        //if (spotlight.transform.localEulerAngles.x < rotationMax && !hitMax) {
        //    spotlight.transform.Rotate(Vector3.up, rotationSpeed);
        //    hitMax = false;
        //}
        //else if (!hitMax) {
        //    hitMax = true;
        //}

        //if (spotlight.transform.localEulerAngles.x > rotationMin && !hitMin) {
        //    spotlight.transform.Rotate(Vector3.up, -rotationSpeed);
        //}
        //else if (!hitMax) {
        //    hitMin = true;
        //}

        //Debug.Log(string.Format("{0} : {1}({2}) : {2}", rotationMin, spotlight.transform.localEulerAngles.x, goBack, rotationMax));
    }
}
