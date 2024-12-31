using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarParts : MonoBehaviour
{
    public string NameTip = "Null";
    public bool Active;

    public void ActivePart()
    {
        Active = true;
        gameObject.SetActive(true);
    }

    public void DeactivePart()
    {
        Active = false;
        gameObject.SetActive(false);
    }
}
