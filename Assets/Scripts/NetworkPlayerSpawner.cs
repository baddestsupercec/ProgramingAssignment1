using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    private GameObject spawnedPlayerObject;

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Vector3 spawnPos = new Vector3(0, 52.8f, 0);
        GameObject newPlayer = PhotonNetwork.Instantiate("NetworkPlayer",transform.position,transform.rotation);
        
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(spawnedPlayerObject);
    }
}
