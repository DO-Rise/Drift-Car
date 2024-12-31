using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Point : MonoBehaviour
{
    private void Start()
    {
        GameObject playerPrefab = Resources.Load<GameObject>(Path.Combine("PhotonPrefabs", "Player"));
        Instantiate(playerPrefab, gameObject.transform.position, transform.rotation);
    }
}
