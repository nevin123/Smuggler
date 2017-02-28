using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour {
    

    //instance
    public static PlayerManager instance = null;

    void Awake()
    {
        instance = this;
    }

    //variables
    public string PlayerName = "";

    public Dictionary<string, GameObject> Players = new Dictionary<string, GameObject>();

    /// <summary>
    /// Register the player in a dictionary
    /// </summary>
    /// <param name="_playerID">The netId of the player</param>
    /// <param name="_playerGameObject">The GameObject of the player</param>
    public void RegisterPlayer(string _playerID, GameObject _playerGameObject)
    {
        string _playerName = "Player " + _playerID;
        _playerGameObject.name = _playerName;

        Players.Add(_playerName, _playerGameObject);
    }
}
