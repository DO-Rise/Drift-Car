using UnityEngine;
using UnityEngine.UI;

public class MyCars : MonoBehaviour
{
    [SerializeField] private GameObject _prefabButton;
    [SerializeField] private Transform _content;
    [SerializeField] private Sprite[] _sprites;

    private int _count = 1;

    public void AddCarButton(string name)
    {
        GameObject newButton = Instantiate(_prefabButton, _content);
        Image buttonImage = newButton.GetComponent<Image>();
        RectTransform buttonRectTransform = newButton.GetComponent<RectTransform>();

        foreach (Sprite s in _sprites)
        {
            if (s.name == name)
            {
                buttonImage.sprite = s;
                break;
            }
        }

        newButton.name = name;

        buttonRectTransform.anchoredPosition = new Vector2(buttonRectTransform.anchoredPosition.x, buttonRectTransform.anchoredPosition.y - (180 * _count));
        _count++;
    }
}
