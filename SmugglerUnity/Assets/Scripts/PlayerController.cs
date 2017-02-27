using UnityEngine;
using UnityEngine.Networking;

[RequireComponent (typeof(PlayerMotor))]
public class PlayerController : NetworkBehaviour {

    PlayerMotor Motor;

    bool jump;
    float jumpTimer;

    void Start()
    {
        //Move the player to the correct spawn point
        SpawnPlayers.instance.SpawnPlayer(gameObject, isServer);

        //Remore the script if its not the local player
        if(!isLocalPlayer)
        {
            Destroy(this);
            return;
        }

        Motor = GetComponent<PlayerMotor>();
    }

	void FixedUpdate()
    {
        //Move Player
        Motor.MovePlayer(Input.GetAxisRaw("Horizontal"));

        //Jump
        if (Input.GetButton("Jump"))
        {
            Motor.Jump();
        } else
        {
            Motor.StopJump();
        }

        //Give
        if(Input.GetButtonDown("Use"))
        {
            //Debug.DrawLine(transform.position, transform.position + transform.forward * 3);
            RaycastHit hit;
            Vector3 _dir = Vector3.zero;

            if (isServer)
                _dir = transform.forward;
            else
                _dir = -transform.forward;

            Debug.DrawLine(transform.position, _dir, Color.red);

            if(Physics.Raycast(transform.position, _dir, out hit))
            {
                if(hit.transform.tag == "Player")
                {
                    Debug.Log("Give");
                }
            }
        }
    }
}
