using TMPro;
using UnityEngine;

public class ScoreDrift : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;

    private int _scoreValue = 0;

    private void Update()
    {
        _scoreText.text = _scoreValue.ToString();
    }

    public void Score()
    {
        _scoreValue += 5;
    }
}
