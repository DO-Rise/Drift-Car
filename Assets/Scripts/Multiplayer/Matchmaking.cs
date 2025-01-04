using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class Matchmaking : MonoBehaviourPunCallbacks
{
    private CarPartsManager _carPartsManager;
    private string _nameScene;

    public void StartMultiplayer()
    {
        // —охранение кастомизации перед переходом в гонку
        //_carPartsManager = FindObjectOfType<CarPartsManager>();
        //_carPartsManager.SaveCustomizationState();

        MenuManager.Instance.OpenMenu("LoadingMultiplayer");

        PhotonNetwork.JoinRandomRoom();
        _nameScene = "Multiplayer";
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)//2
        {
            PhotonNetwork.LoadLevel(_nameScene);

            /*if (SceneManager.GetActiveScene().name == _nameScene)
            {
                RoomManager.Instance.SpawnPlayer();
            }*/
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            PhotonNetwork.LoadLevel(_nameScene);
    }

    public void LeaveRoom()
    {
        //RoomManager.Instance.LeaveRoom();
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("Garage");
    }
}
