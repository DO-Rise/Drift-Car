using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarButton : MonoBehaviour
{
    public void ClickButton()
    {
        GameObject player = GameObject.Find("Player");

        foreach(Transform child in player.transform)
        {
            // �������������� ��� �������� �������
            child.gameObject.SetActive(false);

            // ������������ ������, ���� ��� ��� ��������� � ������ ������
            if (child.gameObject.name == gameObject.name)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}
