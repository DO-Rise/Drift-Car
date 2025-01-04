using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameCareer : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _yourScoreText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _moneyText;

    [SerializeField] private GameObject _pause;
    [SerializeField] private GameObject _finishScreen;
    [SerializeField] private GameObject _finishScreenButton;

    private int _scoreValue = 0;
    private int _moneyCareer = 0;

    private float _sec = 0f;
    private int _min = 0;

    private string _secPlus = "";

    private void Start()
    {
        Time.timeScale = 1f;
    }

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
            FinishGame(0);
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

            Time.timeScale = 0f;
        }
        else
        {
            _pause.SetActive(false);
            _yourScoreText.gameObject.SetActive(false);

            Time.timeScale = 1f;
        }
    }

    public void FinishGame(int number)
    {
        Time.timeScale = 0f;

        _yourScoreText.gameObject.SetActive(true);

        if (number == 0)
        {
            _finishScreen.SetActive(true);
            MoneyWin();
        }
        else if (number == 1)
        {
            _finishScreen.SetActive(false);
            _finishScreenButton.SetActive(true);
        }
    }

    private void MoneyWin()
    {
        _moneyCareer = (_scoreValue / 100) * 15;
        _moneyText.text = "You earned it: " + _moneyCareer.ToString() + "$";

        PlayerPrefs.SetInt("Money", _moneyCareer);
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("Career");
    }

    public void QuitButton()
    {
        //PlayerPrefs.SetString("Score", _yourScoreText.text);
        //PlayerPrefs.Save();

        SceneManager.LoadScene("Garage");
    }
}
