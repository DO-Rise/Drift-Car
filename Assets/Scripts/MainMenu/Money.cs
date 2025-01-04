using TMPro;
using UnityEngine;

public class Money : MonoBehaviour
{
    public static Money Instance;

    [SerializeField] private TMP_Text _moneyText;

    private int _money = 1000;

    private void Awake()
    {
        Instance = this;

        //if (!PlayerPrefs.HasKey("TotalMoney"));
    }

    private void Start()
    {
        _money = PlayerPrefs.GetInt("TotalMoney", 0);
        _money += PlayerPrefs.GetInt("Money", 0);
        PlayerPrefs.SetInt("TotalMoney", _money);

        PlayerPrefs.DeleteKey("Money");
        //PlayerPrefs.DeleteAll();
    }

    private void Update()
    {
        _moneyText.text = _money.ToString();
    }

    public int CurrentMoney()
    {
        return _money;
    }

    public void Spending(int number)
    {
        _money -= number;
        PlayerPrefs.SetInt("TotalMoney", _money);
    }
}
