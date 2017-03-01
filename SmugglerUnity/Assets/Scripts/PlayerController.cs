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

            Motor.CmdHandOverItem(transform.name, transform.name);
        }
    }
}
