using UnityEngine;

public class VRIKBody : MonoBehaviour
{
    [Header("XR Targets")]
    public Transform headTarget;
    public Transform leftHandTarget;
    public Transform rightHandTarget;

    [Header("Body Bones")]
    public Transform head;
    public Transform spine;
    public Transform leftHand;
    public Transform rightHand;

    [Header("Settings")]
    public Vector3 headRotOffset;
    public Vector3 leftHandPosOffset;
    public Vector3 leftHandRotOffset;
    public Vector3 rightHandPosOffset;
    public Vector3 rightHandRotOffset;
    public Vector3 bodyOffset = new Vector3(0, -1.6f, 0);

    void LateUpdate()
    {
        // 1. Corps suit le casque
        transform.position = headTarget.position + bodyOffset;

        // Rotation du corps = direction du regard (horizontal)
        Vector3 forward = headTarget.forward;
        forward.y = 0;
        if (forward.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(forward);

        // 2. Tête suit le casque
        head.rotation = headTarget.rotation * Quaternion.Euler(headRotOffset);

        // 3. Main gauche = contrôleur gauche
        leftHand.position = leftHandTarget.position + leftHandTarget.TransformVector(leftHandPosOffset);
        leftHand.rotation = leftHandTarget.rotation * Quaternion.Euler(leftHandRotOffset);

        // 4. Main droite = contrôleur droit
        rightHand.position = rightHandTarget.position + rightHandTarget.TransformVector(rightHandPosOffset);
        rightHand.rotation = rightHandTarget.rotation * Quaternion.Euler(rightHandRotOffset);
    }
}