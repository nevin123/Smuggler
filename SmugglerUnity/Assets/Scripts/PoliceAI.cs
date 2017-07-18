using UnityEngine;
using UnityEngine.AI;

public class PoliceAI : MonoBehaviour {
    public enum States { waiting, chasing, searching, returning }

    public Vector3 Target;
    NavMeshAgent agent;

    public float loseTargetTime;
    public float searchTime = 5;
    private float loseTimeLeft;
    private float searchTimeLeft;

    private TagDetector playerDetector;

    Vector3 startPosition;

    Vector3 lastPosition;
    private Vector3 lastTargetPos;

    NavMeshPath path;
    NavMeshPath lastPath;
    PlayerMotor motor;

    public States state = States.waiting;
    
    void Start() {
        playerDetector = transform.GetChild(0).gameObject.GetComponent<TagDetector>();

        startPosition = transform.position;

        motor = GetComponent<PlayerMotor>();
        path = new NavMeshPath();
        lastPath = new NavMeshPath();
    }

    void Update() {
        switch (state) {
            case States.waiting:
                motor.MovePlayer(Vector3.zero);
                if (playerDetector.isCollidingWithPlayer) {
                    loseTimeLeft = loseTargetTime;
                    Target = playerDetector.otherObject.transform.position;

                    state = States.chasing;
                }
                break;

            case States.chasing:
                if (loseTimeLeft <= 0) {
                    state = States.searching;
                    return;
                }

                if (playerDetector.isCollidingWithPlayer) {
                    loseTimeLeft = loseTargetTime;
                    Target = playerDetector.otherObject.transform.position;
                } else {
                    loseTimeLeft -= Time.deltaTime;
                }

                Move(Target);

                break;
            case States.searching:
                if (searchTimeLeft > 0) {
                    if (playerDetector.isCollidingWithPlayer) {
                        state = States.chasing;
                        return;
                    }
                    searchTimeLeft -= Time.deltaTime;

                } else {
                    searchTimeLeft = searchTime;
                    state = States.returning;
                }

                break;

            case States.returning:
                if (!Move(startPosition)) {
                    state = States.waiting;
                }

                break;
        }
    }

    private bool Move(Vector3 target) {
        if (transform.position.x > target.x - 0.1f && transform.position.x < target.x + 0.1f &&
            transform.position.z > target.z - 0.1f && transform.position.z < target.z + 0.1f) {
            motor.MovePlayer(Vector3.zero);
            return false;
        }

        NavMesh.CalculatePath(transform.position, target, 1, path);

        if (path == null || path.corners.Length <= 0 || path.status != NavMeshPathStatus.PathComplete ||
            path.status == NavMeshPathStatus.PathInvalid) {

            motor.MovePlayer(Vector3.zero);
            return true;
        }

        Vector3 _dir = path.corners[1] - transform.position;
        _dir = new Vector3(_dir.x, 0, _dir.z).normalized;

        motor.MovePlayer(_dir);
        return true;
    }
}