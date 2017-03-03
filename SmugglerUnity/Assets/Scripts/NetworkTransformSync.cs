using UnityEngine;
using UnityEngine.Networking;

public class NetworkTransformSync : NetworkBehaviour {

    [SerializeField] [Range(1,30)] int UpdateFrequenty;

    [SerializeField] float MovementInterpolation;

    float SendTimer;

    Vector3 LastPosition = Vector3.zero;
    float NewYRotation = 0;
    Vector3 NewPosition = Vector3.zero;
    Vector3 CurrentVelocity = Vector3.zero;

    void Start()
    {
        SendTimer = 1f / UpdateFrequenty;
    }

    void Update()
    {
        //Dont send position if its not the localplayer, but move it to the received position instead
        if (!isLocalPlayer)
        {
            transform.position = Vector3.SmoothDamp(transform.position, NewPosition, ref CurrentVelocity, MovementInterpolation);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, NewYRotation, 0), 15 * Time.deltaTime);

            return;
        }

        //Check if the position of the player is changed, else return
        if (transform.position == LastPosition)
        {
            SendTimer = 1f / UpdateFrequenty;
            return;
        }

        //Countdown
        SendTimer -= Time.deltaTime;

        if (SendTimer > 0)
            return;

        CmdUpdatePlayerTransform(transform.name, transform.position, transform.rotation.eulerAngles.y);
        LastPosition = transform.position;
        SendTimer = 1f / UpdateFrequenty;
    }

    //Send the player ID + position to the server
    [Command]
    void CmdUpdatePlayerTransform(string playerName, Vector3 newPosition, float newYRoation)
    {
        RpcSetPlayers(playerName, newPosition, newYRoation);
    }

    //Send the new Player position to all the clients
    [ClientRpc]
    public void RpcSetPlayers(string _name, Vector3 _position, float _newYRotation)
    {
        //If this is not your own player
        if (PlayerManager.instance.PlayerName == _name)
            return;

        //Checks if the player name exists, if it does, set the new player position
        if (PlayerManager.instance.Players.ContainsKey(_name))
        {
            NewPosition = _position;
            NewYRotation = _newYRotation;
        }
    }
}
