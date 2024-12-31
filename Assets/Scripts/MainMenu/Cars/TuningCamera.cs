using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuningCamera : MonoBehaviour
{
    [SerializeField] private Transform[] _pointPosition;
    [SerializeField] private GameObject[] _partsView;

    private Camera _camera;
    private string _name = "Null";

    private void Start()
    {
        _camera = Camera.main;

        DefaultPosition();
    }

    public void SelectionPart(string name)
    {
        foreach (Transform point in _pointPosition)
        {
            if (name == point.name)
                _camera.transform.SetPositionAndRotation(point.position, point.rotation);
        }

        foreach (GameObject part in _partsView)
        {
            if (part.name == name)
                part.SetActive(true);
            else
            {
                if (part.activeSelf)
                    part.SetActive(false);
            }
        }

        _name = name;
    }

    public void ViewButton()
    {
        SelectionPart("nul");

        DefaultPosition();
    }

    public string NamePosition()
    {
        foreach (Transform point in _pointPosition)
        {
            if (_name == point.name)
                return _name;
        }

        return null;
    }

    public void DefaultPosition()
    {
        _camera.transform.SetPositionAndRotation(_pointPosition[0].position, _pointPosition[0].rotation);
    }
}
