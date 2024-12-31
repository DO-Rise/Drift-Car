using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    private PhotonView PV;
    //private CarPartsManager _carPartsManager;

    private void Awake()
    {
        Instance = this;
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (PV.IsMine)
            CreateController();
    }

    private void CreateController()
    {
        Transform spawnpoint = SpawnManager.Instance.GetSpawnPoint();
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), spawnpoint.position, spawnpoint.rotation);

        /*_carPartsManager = FindObjectOfType<CarPartsManager>();
        _carPartsManager.LoadCustomizationState();*/
    }
}
