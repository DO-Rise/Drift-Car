using TMPro;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameMultiplayer : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _yourScoreText;
    [SerializeField] private TMP_Text _timeText;

    [SerializeField] private GameObject _pause;
    [SerializeField] private GameObject _finishScreen;

    private int _scoreValue = 0;

    private float _sec = 0f;
    private int _min = 0;

    private string _secPlus = "";

    private void Update()
    {
        _scoreText.text = _scoreValue.ToString();
        _timeText.text = "0" + _min.ToString() + ":" + _secPlus + ((int)_sec).ToString();
        _yourScoreText.text = "Your score: " + _scoreText.text;

        _secPlus = _sec < 10f ? "0" : "";

        _sec += Time.deltaTime;

        if (_min < 2)
        {
            if (_sec >= 59f)
            {
                _sec = 0f;
                _min++;
            }
        }
        else
            FinishGame();
    }

    public void Score()
    {
        _scoreValue += 5;
    }

    public void Pause(bool active)
    {
        if (active)
        {
            _pause.SetActive(true);
            _yourScoreText.gameObject.SetActive(true);
        }
        else
        {
            _pause.SetActive(false);
            _yourScoreText.gameObject.SetActive(false);
        }
    }

    private void FinishGame()
    {
        _finishScreen.SetActive(true);
        _yourScoreText.gameObject.SetActive(true);
    }

    public void RestartButton(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void QuitButton()
    {
        PlayerPrefs.SetString(_yourScoreText.text, "Score");
        PlayerPrefs.Save();

        //RoomManager.Instance.LeaveRoom();
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Garage");
    }
}
