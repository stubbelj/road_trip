using Unity.Netcode;
using UnityEngine;
[DefaultExecutionOrder(0)] // Execute before ClientNetworkTransform
public class ServerPlayerMove : NetworkBehaviour {
    public PlayerSpawnPoints playerSpawnPoints;
    public override void OnNetworkSpawn()
    {
        // Only execute on the Server
        if (!IsServer)
        {
            enabled = false;
            return;
        }
        SpawnPlayer();
        base.OnNetworkSpawn();
    }

    // Move to the next available position when spawning
    void SpawnPlayer() {
        var spawnPoint = PlayerSpawnPoints.inst.GetRandomSpawnPoint();
        var spawnPosition = spawnPoint ? spawnPoint.transform.position : Vector3.zero;
        transform.position = spawnPosition;
    }
}
