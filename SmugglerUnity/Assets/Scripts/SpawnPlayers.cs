using UnityEngine;

public class SpawnPlayers : MonoBehaviour {

    public static SpawnPlayers instance = null;

    [SerializeField] Transform SpawnPointPlayer1;
    [SerializeField] Transform SpawnPointPlayer2;

	void Awake()
    {
        instance = this;
    }

    public void SpawnPlayer(GameObject player, bool isServerPlayer)
    {
        if (isServerPlayer)
            player.transform.position = SpawnPointPlayer1.position;
        else
            player.transform.position = SpawnPointPlayer2.position;
    }
}
