using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public string Name;
    public bool OpenActive;

    public void Open()
    {
        OpenActive = true;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        OpenActive = false;
        gameObject.SetActive(false);
    }
}
