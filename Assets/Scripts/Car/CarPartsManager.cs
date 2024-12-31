using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CarPartsManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private CarParts[] _carParts;

    private TuningCamera _tuningCamera;

    private void Start()
    {
        _tuningCamera = FindObjectOfType<TuningCamera>();

        //SaveCustomizationState();
    }

    public void ActivePart(CarParts part)
    {
        for (int i = 0; i < _carParts.Length; i++)
        {
            if (_tuningCamera.NamePosition() == _carParts[i].NameTip)
            {
                if (_carParts[i].Active)
                    DeactivePart(_carParts[i]);
            }
        }
        part.ActivePart();
    }

    public void DeactivePart(CarParts part)
    {
        part.DeactivePart();
    }

    /*public void SaveCustomizationState()
    {
        if (_carParts == null || _carParts.Length == 0)
        {
            Debug.LogError("Car parts array is not initialized or empty.");
            return;
        }

        bool[] partsActiveState = new bool[_carParts.Length];

        for (int i = 0; i < _carParts.Length; i++)
        {
            partsActiveState[i] = _carParts[i].gameObject.activeSelf;
        }

        ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
        playerProperties["partsActiveState"] = partsActiveState;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);

        Debug.Log("Customization state saved.");
    }


    // Метод для восстановления состояния всех деталей
    public void LoadCustomizationState()
    {
        if (_carParts == null || _carParts.Length == 0)
        {
            Debug.LogError("Car parts array is not initialized or empty.");
            return;
        }

        if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("partsActiveState"))
        {
            Debug.LogError("No customization data found in CustomProperties.");
            return;
        }

        bool[] partsActiveState = (bool[])PhotonNetwork.LocalPlayer.CustomProperties["partsActiveState"];

        if (partsActiveState == null || partsActiveState.Length != _carParts.Length)
        {
            Debug.LogWarning("Customization data is invalid or mismatched. Resetting to defaults.");

            // Сбрасываем все детали в состояние по умолчанию
            for (int i = 0; i < _carParts.Length; i++)
            {
                _carParts[i].gameObject.SetActive(false);
            }
            return;
        }

        for (int i = 0; i < _carParts.Length; i++)
        {
            if (_carParts[i] != null)
            {
                _carParts[i].gameObject.SetActive(partsActiveState[i]);
            }
            else
            {
                Debug.LogError($"Car part at index {i} is null.");
            }
        }

        Debug.Log("Customization state loaded successfully.");
    }*/
}
