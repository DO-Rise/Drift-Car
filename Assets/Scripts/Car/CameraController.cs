using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _car;

    private Rigidbody _rb;

    private Vector3 _offset = new(0, 5, -4);
    private float _speed = 3;
    private Vector3 _velocitySmoothed;
/*
    private void Awake()
    {
        gameObject.SetActive(false);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Garage")
            gameObject.SetActive(true);
    }*/

    private void Start()
    {
        _rb = _car.GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        // Сглаживание скорости автомобиля для камеры
        _velocitySmoothed = Vector3.Lerp(_velocitySmoothed, _rb.velocity, Time.deltaTime * 2f);

        // Рассчет позиции камеры
        Vector3 carForward = (_velocitySmoothed + _car.forward).normalized;
        Vector3 targetPosition = _car.position + _car.TransformVector(_offset) + carForward * -5f;

        // Сглаживание движения камеры
        transform.position = Vector3.Lerp(transform.position, targetPosition, _speed * Time.deltaTime);

        // Направление камеры
        transform.LookAt(_car.position + new Vector3(0, 2, 0));
    }
/*
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }*/
}
