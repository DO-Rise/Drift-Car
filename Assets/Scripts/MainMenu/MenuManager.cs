using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] private Menu[] menus;
    [SerializeField] private RectTransform _carsMenu;
    [SerializeField] private Sprite[] _arrows;

    private bool _openMenuCars = true;
    private Vector2 _hidenPosition = new(115, -75);
    private Vector2 _visiblePosition = new(-285, -75);
    private float _duration = 0.5f;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].Name == menuName)
                OpenMenu(menus[i]);
        }
    }

    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].OpenActive)
                CloseMenu(menus[i]);
        }
        menu.Open();
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }

    public void OpenYourCar()
    {
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        Button button = clickedButton.GetComponent<Button>();

        StopAllCoroutines();

        if (_openMenuCars)
        {
            button.image.sprite = _arrows[1];
            StartCoroutine(SmoothMove(_carsMenu, _visiblePosition));
        }
        else
        {
            button.image.sprite = _arrows[0];
            StartCoroutine(SmoothMove(_carsMenu, _hidenPosition));
        }

        _openMenuCars = !_openMenuCars;
    }

    private IEnumerator SmoothMove(RectTransform rect, Vector2 targetPosition)
    {
        Vector2 startPosition = rect.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            rect.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsed / _duration);
            yield return null;
        }

        rect.anchoredPosition = targetPosition;
    }

    public void CareerButton()
    {
        SceneManager.LoadScene("Career");
    }
}
