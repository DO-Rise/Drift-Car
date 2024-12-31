using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CarButton : MonoBehaviour
{
    [SerializeField] private string _name = "Null";
    [SerializeField] private Sprite[] _imageButtons;

    private void Start()
    {
        Button button = GetComponent<Button>();

        foreach (Sprite sprite in _imageButtons)
        {
            if (sprite.name == _name)
                button.image.sprite = sprite;
        }
    }
}
