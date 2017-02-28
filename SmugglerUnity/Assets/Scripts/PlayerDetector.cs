using UnityEngine;

public class PlayerDetector : MonoBehaviour {


    [Range(1,4)][SerializeField] float DetectorRange = 2;

    public bool isCollidingWithPlayer;
    public GameObject OtherPlayer;
    
    void Start () {
        GetComponent<SphereCollider>().radius = DetectorRange;
	}
	
	void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isCollidingWithPlayer = true;

            if (OtherPlayer == null)
                OtherPlayer = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            isCollidingWithPlayer = false;
    }
}
