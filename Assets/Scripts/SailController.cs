using UnityEngine;
using UnityEngine.InputSystem;

public class SailController : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float openingPercent = 0f;
    [SerializeField, Range(-45f, 45f)] private float rotationDegrees;
    [SerializeField] private Transform sailVisual;
    [SerializeField] private float changeSpeed = 45f;
    public float OpeningPercent => openingPercent;
    public float RotationDegrees => rotationDegrees;

    private void Update()
    //controls for the sail 
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.wKey.isPressed)
            openingPercent += changeSpeed * Time.deltaTime;
        if (Keyboard.current.sKey.isPressed)
            openingPercent -= changeSpeed * Time.deltaTime;
        openingPercent = Mathf.Clamp(openingPercent, 0f, 100f);

        if (Keyboard.current.qKey.isPressed) rotationDegrees -= changeSpeed * Time.deltaTime;
        if (Keyboard.current.eKey.isPressed) rotationDegrees += changeSpeed * Time.deltaTime;
        rotationDegrees = Mathf.Clamp(rotationDegrees, -45f, 45f);

        if (sailVisual != null)
            sailVisual.localRotation = Quaternion.Euler(0f, rotationDegrees, 0f);
    }
}
