using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    private SpawnPoint[] _spawnPoint;
    private List<int> _usedSpawnPoints = new List<int>();

    private void Awake()
    {
        Instance = this;
        _spawnPoint = GetComponentsInChildren<SpawnPoint>();
    }

    private void Start()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
    }

    public Transform GetSpawnPoint()
    {
        int spawnIndex;

        // Ищем свободную точку
        do
        {
            spawnIndex = Random.Range(0, _spawnPoint.Length);
        } while (_usedSpawnPoints.Contains(spawnIndex));

        // Помечаем точку как использованную
        _usedSpawnPoints.Add(spawnIndex);

        // Если используется Photon, синхронизируем информацию о занятой точке
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("MarkSpawnPointUsed", RpcTarget.OthersBuffered, spawnIndex);

        return _spawnPoint[spawnIndex].transform;
    }

    [PunRPC]
    private void MarkSpawnPointUsed(int index)
    {
        if (!_usedSpawnPoints.Contains(index))
        {
            _usedSpawnPoints.Add(index);
        }
    }
}
