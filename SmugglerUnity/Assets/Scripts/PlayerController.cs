using UnityEngine;
using UnityEngine.Networking;

[RequireComponent (typeof(PlayerMotor))]
public class PlayerController : NetworkBehaviour {

    PlayerMotor Motor;

    void Start()
    {
        PlayerManager.instance.RegisterPlayer(netId.ToString(), gameObject);

        //Move the player to the correct spawn point
        SpawnPlayers.instance.SpawnPlayer(gameObject, isServer);

        //Remore the script if its not the local player
        if(!isLocalPlayer)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<PlayerMotor>().enabled = false;
            Destroy(this);
            return;
        }

        Motor = GetComponent<PlayerMotor>();

        PlayerManager.instance.PlayerName = gameObject.name;
    }

	void FixedUpdate()
    {
        //Move Player Function
        int _multiplier = 0;

        if (isServer)
            _multiplier = 1;
        else
            _multiplier = -1;

        Vector3 _direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, _multiplier * Motor.ZMovementMultiplier * Input.GetAxisRaw("Vertical")).normalized;
        
        Motor.MovePlayer(_direction);

        //Jump Function
        if (Input.GetButton("Jump"))
        {
            Motor.Jump();
        } else
        {
            Motor.StopJump();
        }

        //Give Function
        if(Input.GetButtonDown("Use"))
        {
            Motor.HandOverItem();
        }
    }

    //If the player Collides with the package, make him pick it up
    void OnCollisionEnter(Collision col)
    {
        if(col.transform.tag == "Package")
        {
            col.transform.GetComponent<BoxCollider>().enabled = false;
            col.transform.GetComponent<Rigidbody>().isKinematic = true;

            col.transform.parent = Motor.Holder.transform;
            col.transform.localPosition = Vector3.zero;
            col.transform.localRotation = Quaternion.identity;

            Motor.CmdHandOverItem(transform.name, transform.name);
        }
    }
}
