using UnityEngine;
using UnityEngine.Networking;

public class NetworkTransformSync : NetworkBehaviour {

    [SerializeField] [Range(1,30)]
    int UpdateFrequenty;

    float SendTimer;

    Vector3 LastPosition = Vector3.zero;

    void Start()
    {
        SendTimer = 1f / UpdateFrequenty;
    }

    void Update()
    {
        //Dont send position if its not the localplayer
        if (!isLocalPlayer)
            return;

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

        CmdUpdatePlayerTransform(transform.name, transform.position);
        LastPosition = transform.position;
        SendTimer = 1f / UpdateFrequenty;
    }

    [Command]
    void CmdUpdatePlayerTransform(string playerName, Vector3 newPosition)
    {
        RpcSetPlayers(playerName, newPosition);
    }

    [ClientRpc]
    public void RpcSetPlayers(string _name, Vector3 _position)
    {
        //If this is not your own player
        if (PlayerManager.instance.PlayerName == _name)
            return;

        //Checks if the player name exists, if it does, set the new player position
        if (PlayerManager.instance.Players.ContainsKey(_name))
            PlayerManager.instance.Players[_name].transform.position = _position;
    }
}
