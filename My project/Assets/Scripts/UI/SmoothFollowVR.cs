using UnityEngine;

public class SmoothFollowVR : MonoBehaviour
{
    public Transform cameraTransform; // ta caméra VR
    
    [Header("Offset Position")]
    public Vector3 offset = new Vector3(0, -0.1f, 0.5f);
    
    [Header("Offset Rotation (Euler)")]
    public Vector3 offsetRotation; // rotation personnalisée de l'offset
    
    public float positionSmoothTime = 0.1f;
    public float rotationSmoothTime = 0.1f;
    
    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        // --- Rotation finale = rotation caméra + rotation personnalisée ---
        Quaternion finalRotation = cameraTransform.rotation * Quaternion.Euler(offsetRotation);

        // --- Position avec rotation personnalisée ---
        Vector3 targetPosition = cameraTransform.position + finalRotation * offset;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            positionSmoothTime
        );

        // --- Rotation lissée ---
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            finalRotation,
            Time.deltaTime / rotationSmoothTime
        );
    }
}
