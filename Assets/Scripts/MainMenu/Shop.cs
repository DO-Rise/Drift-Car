using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class Shop : MonoBehaviour
{
    [SerializeField] private Button[] _carsButtons;

    private MyCars _myCars;

    private void Start()
    {
        _myCars = FindObjectOfType<MyCars>();
    }

    private void Update()
    {
        for (int i = 0; i < _carsButtons.Length; i++)
        {
            TMP_Text buttonText = _carsButtons[i].GetComponentInChildren<TMP_Text>();

            if (int.TryParse(buttonText.text, out int carPrice))
                _carsButtons[i].interactable = carPrice <= Money.Instance.CurrentMoney();
        }
    }

    public void BuyCarButton(GameObject button)
    {
        _myCars.AddCarButton(button.name);

        TMP_Text textButton = button.GetComponentInChildren<TMP_Text>();

        if (int.TryParse(textButton.text, out int price))
            Money.Instance.Spending(price);

        textButton.text = "Sale";

        button.GetComponent<Button>().interactable = false;
    }

}
