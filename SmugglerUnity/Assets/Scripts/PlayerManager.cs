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
    public string PlayerThatHoldsTheItem = "";

    public GameObject Package;

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

    /// <summary>
    /// Place the visuals of the package to the player that holds the item
    /// </summary>
    public void UpdatePackageVisuals()
    {
        Package.transform.GetComponent<BoxCollider>().enabled = false;
        Package.transform.GetComponent<Rigidbody>().isKinematic = true;

        Package.transform.parent = Players[PlayerThatHoldsTheItem].GetComponent<PlayerMotor>().Holder.transform;
        Package.transform.localPosition = Vector3.zero;
    }
}
