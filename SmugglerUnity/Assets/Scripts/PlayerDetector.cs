using UnityEngine;

public class PlayerDetector : MonoBehaviour {


    [Range(1,4)][SerializeField] float DetectorRange = 2;

    bool isCollidingWithPlayer;
    
    void Start () {
        GetComponent<SphereCollider>().radius = DetectorRange;
	}
	
	void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            isCollidingWithPlayer = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            isCollidingWithPlayer = false;
    }
}
