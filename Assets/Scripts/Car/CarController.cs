using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

[System.Serializable]
public class WheelColliders
{
    public WheelCollider FRWheel;
    public WheelCollider FLWheel;
    public WheelCollider RRWheel;
    public WheelCollider RLWheel;
}

[System.Serializable]
public class WheelMeshes
{
    public MeshRenderer FRWheel;
    public MeshRenderer FLWheel;
    public MeshRenderer RRWheel;
    public MeshRenderer RLWheel;
}

public class WheelParticles
{
    public ParticleSystem FRWheel;
    public ParticleSystem FLWheel;
    public ParticleSystem RRWheel;
    public ParticleSystem RLWheel;

}

public class CarController : MonoBehaviour
{
    public bool IsDrifting;

    [SerializeField] private WheelColliders _wheelColliders;
    [SerializeField] private WheelMeshes _wheelMeshes;

    [SerializeField] private AnimationCurve _steerlingCurve;

    [SerializeField] private GameObject _smokePrefab;
    private WheelParticles _wheelParticles;

    private UIGameMultiplayer _UIMultiplayer;
    private UIGameCareer _UICareer;

    private PhotonView PV;
    private Rigidbody _rb;

    private float _moveX;
    private float _moveZ;
    private float _brakeInput;

    private float _motorPower = 700f;
    private float _brakePower = 50000;
    private float _slipAngle;

    private float _speed;

    private float _driftFactor = 3f; // Увеличивает управляемость во время дрифта
    private float _driftThreshold = 0.5f; // Порог для начала дрифта

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();

        _wheelParticles = new WheelParticles();
        InstantiateSmoke();

        if (SceneManager.GetActiveScene().name == "Multiplayer")
        {
            if (!PV.IsMine)
            {
                Destroy(GetComponentInChildren<Camera>().gameObject);
                Destroy(_rb);
            }
            _UIMultiplayer = FindObjectOfType<UIGameMultiplayer>();
        }

        if (SceneManager.GetActiveScene().name == "Career")
            _UICareer = FindObjectOfType<UIGameCareer>();
    }

    private void InstantiateSmoke()
    {
        _wheelParticles.FRWheel = Instantiate(_smokePrefab, _wheelColliders.FRWheel.transform.position - Vector3.up
            * _wheelColliders.FRWheel.radius, Quaternion.identity, _wheelColliders.FRWheel.transform).GetComponent<ParticleSystem>();
        _wheelParticles.FLWheel = Instantiate(_smokePrefab, _wheelColliders.FLWheel.transform.position - Vector3.up
            * _wheelColliders.FRWheel.radius, Quaternion.identity, _wheelColliders.FLWheel.transform).GetComponent<ParticleSystem>();
        _wheelParticles.RRWheel = Instantiate(_smokePrefab, _wheelColliders.RRWheel.transform.position - Vector3.up
            * _wheelColliders.FRWheel.radius, Quaternion.identity, _wheelColliders.RRWheel.transform).GetComponent<ParticleSystem>();
        _wheelParticles.RLWheel = Instantiate(_smokePrefab, _wheelColliders.RLWheel.transform.position - Vector3.up
            * _wheelColliders.FRWheel.radius, Quaternion.identity, _wheelColliders.RLWheel.transform).GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Multiplayer")
            if (!PV.IsMine)
                return;    
            
        _speed = _rb.velocity.magnitude;

        CheckInput();

        _wheelColliders.FRWheel.brakeTorque = _brakeInput * _brakePower * 0.7f;
        _wheelColliders.FLWheel.brakeTorque = _brakeInput * _brakePower * 0.7f;
        _wheelColliders.RRWheel.brakeTorque = _brakeInput * _brakePower * 0.3f;
        _wheelColliders.RLWheel.brakeTorque = _brakeInput * _brakePower * 0.3f;

        _wheelColliders.RRWheel.motorTorque = _motorPower * _moveX;
        _wheelColliders.RLWheel.motorTorque = _motorPower * _moveX;

        Steering();
        WheelPositions();
        CheckDrift();
    }

    private void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name == "Multiplayer")
            if (!PV.IsMine) return;
    }

    private void UpdateWheels(WheelCollider collider, MeshRenderer wheelMesh)
    {
        Quaternion quaternion;
        Vector3 position;

        collider.GetWorldPose(out position, out quaternion);
        wheelMesh.transform.rotation = quaternion;
    }

    private void WheelPositions()
    {
        UpdateWheels(_wheelColliders.FLWheel, _wheelMeshes.FLWheel);
        UpdateWheels(_wheelColliders.FRWheel, _wheelMeshes.FRWheel);
        UpdateWheels(_wheelColliders.RLWheel, _wheelMeshes.RLWheel);
        UpdateWheels(_wheelColliders.RRWheel, _wheelMeshes.RRWheel);
    }

    private void CheckInput()
    {
        _moveX = Input.GetAxis("Vertical");
        _moveZ = Input.GetAxis("Horizontal");

        _slipAngle = Vector3.Angle(transform.forward, _rb.velocity - transform.forward);

        float movingDirection = Vector3.Dot(transform.forward, _rb.velocity);

        if (movingDirection < -0.5f && _moveX > 0)
            _brakeInput = Mathf.Abs(_moveX);
        else if (movingDirection > 0.5f && _moveX < 0)
            _brakeInput = Mathf.Abs(_moveX);
        else
            _brakeInput = 0;
    }

    private void Steering()
    {
        float steeringAngle = _moveZ * _steerlingCurve.Evaluate(_speed);

        if (_slipAngle < 120f)
            steeringAngle += Vector3.SignedAngle(transform.forward, _rb.velocity + transform.forward, Vector3.up);

        if (IsDrifting)
            steeringAngle *= _driftFactor;

        steeringAngle = Mathf.Clamp(steeringAngle, -60, 60f);

        _wheelColliders.FRWheel.steerAngle = steeringAngle;
        _wheelColliders.FLWheel.steerAngle = steeringAngle;
    }

    private void CheckDrift()
    {
        WheelHit rearRightHit, rearLeftHit;

        _wheelColliders.RRWheel.GetGroundHit(out rearRightHit);
        _wheelColliders.RLWheel.GetGroundHit(out rearLeftHit);

        float averageSlip = (Mathf.Abs(rearRightHit.sidewaysSlip) + Mathf.Abs(rearLeftHit.sidewaysSlip)) / 2;

        IsDrifting = averageSlip > _driftThreshold;

        if (IsDrifting)
        {
            CheckParticles();

            if (SceneManager.GetActiveScene().name == "Multiplayer")
                _UIMultiplayer.Score();
            else if (SceneManager.GetActiveScene().name == "Career")
                _UICareer.Score();
        }
    }

    private void CheckParticles()
    {
        WheelHit[] wheelHits = new WheelHit[4];
        _wheelColliders.FRWheel.GetGroundHit(out wheelHits[0]);
        _wheelColliders.FLWheel.GetGroundHit(out wheelHits[1]);
        _wheelColliders.RRWheel.GetGroundHit(out wheelHits[2]);
        _wheelColliders.RLWheel.GetGroundHit(out wheelHits[3]);

        float slipAllowance = 0.9f;
        float minSpeedForSmoke = 4f;

        bool isDrifting = _speed > minSpeedForSmoke &&
                          (Mathf.Abs(wheelHits[2].sidewaysSlip) > slipAllowance || Mathf.Abs(wheelHits[3].sidewaysSlip) > slipAllowance);

        if (isDrifting)
        {
            _wheelParticles.RRWheel.Play();
            _wheelParticles.RLWheel.Play();
        }
        else
        {
            _wheelParticles.RRWheel.Stop();
            _wheelParticles.RLWheel.Stop();
        }

        if (_speed > minSpeedForSmoke && Mathf.Abs(wheelHits[0].sidewaysSlip) > slipAllowance)
            _wheelParticles.FRWheel.Play();
        else
            _wheelParticles.FRWheel.Stop();

        if (_speed > minSpeedForSmoke && Mathf.Abs(wheelHits[1].sidewaysSlip) > slipAllowance)
            _wheelParticles.FLWheel.Play();
        else
            _wheelParticles.FLWheel.Stop();
    }

    [PunRPC]
    private void SyncWheelData(float frontLeftSteer, float frontRightSteer, float rearLeftRot, float rearRightRot)
    {
        // Обновляем поворот колес
        _wheelColliders.FLWheel.steerAngle = frontLeftSteer;
        _wheelColliders.FRWheel.steerAngle = frontRightSteer;

        // Обновляем вращение колес
        _wheelMeshes.RLWheel.transform.localRotation = Quaternion.Euler(0, 0, rearLeftRot);
        _wheelMeshes.RRWheel.transform.localRotation = Quaternion.Euler(0, 0, rearRightRot);
    }
}
