using UnityEngine;
using UnityEngine.InputSystem;

public class BoatController : MonoBehaviour
{
    //lateral drag is used for the drag on side of the boat
    [SerializeField] private SailController sail;
    [SerializeField] private WindController wind;
    [SerializeField] private float acceleration = 0.15f;
    [SerializeField] private float drag = 0.08f;
    [SerializeField] private float lateralDrag = 1.8f;
    [SerializeField] private float turnSpeed = 30f;
    [SerializeField] private float maxSpeed = 8f;

    private float forwardSpeed;
    private float lateralSpeed;

    private void Update()
    {
        if (sail == null || wind == null) return;

        float deltaTime = Time.deltaTime;
        if (deltaTime <= 0f) return;

        Vector3 forward = -transform.forward;
        Vector3 right = -transform.right;
        Vector3 boatVelocity = forward * forwardSpeed + right * lateralSpeed;
        Vector3 trueWind = wind.transform.forward * wind.Speed;
        Vector3 apparentWind = trueWind - boatVelocity;

        //wind pushes the sail and calculates drive and side force from wind
        Vector3 sailNormal = Quaternion.AngleAxis(sail.RotationDegrees, transform.up) * forward;
        float normalWindSpeed = Vector3.Dot(apparentWind, sailNormal);
        Vector3 sailForce = sailNormal * (normalWindSpeed * Mathf.Abs(normalWindSpeed))
            * (sail.OpeningPercent / 100f) * acceleration;

        //wind works here as movement power and drag reduces speed
        float forwardForce = Vector3.Dot(sailForce, forward);
        float sideForce = Vector3.Dot(sailForce, right) * 0.15f;
        forwardSpeed += (forwardForce - drag * forwardSpeed * Mathf.Abs(forwardSpeed)) * deltaTime;
        lateralSpeed += (sideForce - lateralDrag * lateralSpeed) * deltaTime;
        forwardSpeed = Mathf.Clamp(forwardSpeed, 0f, maxSpeed);
        lateralSpeed = Mathf.Clamp(lateralSpeed, -maxSpeed * 0.35f, maxSpeed * 0.35f);

        float steering = 0f;
        Keyboard keyboard = Keyboard.current;
        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) steering -= 1f;
        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) steering += 1f;

        //if boat stops without movement, it should be able to turn around to catch wind
        float steeringAuthority = Mathf.Max(0.35f, Mathf.Clamp01(forwardSpeed / 2f));
        transform.Rotate(0f, steering * turnSpeed * steeringAuthority * deltaTime, 0f);
        forward = -transform.forward;
        right = -transform.right;
        transform.position += (forward * forwardSpeed + right * lateralSpeed) * deltaTime;
    }
}

