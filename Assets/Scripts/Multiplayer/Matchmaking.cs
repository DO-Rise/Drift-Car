using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class Matchmaking : MonoBehaviourPunCallbacks
{
    private CarPartsManager _carPartsManager;
    private int _numberScene = 0;

    public void StartMultiplayer(int numberScene)
    {
        // —охранение кастомизации перед переходом в гонку
        _carPartsManager = FindObjectOfType<CarPartsManager>();
        //_carPartsManager.SaveCustomizationState();

        MenuManager.Instance.OpenMenu("LoadingMultiplayer");

        PhotonNetwork.JoinRandomRoom();
        _numberScene = numberScene;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.LoadLevel(_numberScene);

            if (SceneManager.GetActiveScene().buildIndex == 2)
                RoomManager.Instance.SpawnPlayer();
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            

            PhotonNetwork.LoadLevel(_numberScene);
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("Multiplayer");
    }
}
