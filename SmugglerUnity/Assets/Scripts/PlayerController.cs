using UnityEngine;
using UnityEngine.Networking;

[RequireComponent (typeof(PlayerMotor))]
public class PlayerController : NetworkBehaviour {

    PlayerMotor Motor;

    bool jump;
    float jumpTimer;

    void Start()
    {
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
        Motor.MovePlayer(Input.GetAxis("Horizontal"));

        //Jump
        if (Input.GetButton("Jump"))
        {
            Motor.Jump();
        } else
        {
            Motor.StopJump();
        }
    }
}
