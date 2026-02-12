using UnityEngine;

public class VRIKBody : MonoBehaviour
{
    [Header("XR Targets")]
    public Transform headTarget;
    public Transform leftHandTarget;
    public Transform rightHandTarget;

    [Header("Head")]
    public Transform head;

    [Header("Left Arm")]
    public Transform leftUpperArm;
    public Transform leftLowerArm;
    public Transform leftHand;

    [Header("Right Arm")]
    public Transform rightUpperArm;
    public Transform rightLowerArm;
    public Transform rightHand;

    [Header("Settings")]
    public Vector3 bodyOffset = new Vector3(0, -1.6f, 0);
    public Vector3 leftHandRotOffset;
    public Vector3 rightHandRotOffset;

    void LateUpdate()
    {
        // Corps suit le casque
        transform.position = headTarget.position + bodyOffset;
        Vector3 forward = headTarget.forward;
        forward.y = 0;
        if (forward.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(forward);

        // Tête
        head.rotation = headTarget.rotation;

        // Bras
        SolveTwoBoneIK(leftUpperArm, leftLowerArm, leftHand, leftHandTarget, -1, leftHandRotOffset);
        SolveTwoBoneIK(rightUpperArm, rightLowerArm, rightHand, rightHandTarget, 1, rightHandRotOffset);
    }

    void SolveTwoBoneIK(Transform upper, Transform lower, Transform hand, Transform target, float side, Vector3 handRotOffset)
    {
        float upperLen = Vector3.Distance(upper.position, lower.position);
        float lowerLen = Vector3.Distance(lower.position, hand.position);
        float totalLen = upperLen + lowerLen;

        Vector3 targetPos = target.position;
        Vector3 toTarget = targetPos - upper.position;
        float targetDist = toTarget.magnitude;

        // Clamp : le bras ne peut pas s'étirer plus que sa longueur totale
        if (targetDist > totalLen * 0.999f)
        {
            targetPos = upper.position + toTarget.normalized * totalLen * 0.999f;
            targetDist = totalLen * 0.999f;
        }

        // Hint direction (coude pointe vers l'arrière et l'extérieur)
        Vector3 hint = upper.position + (-transform.forward + transform.up * -0.5f + transform.right * side).normalized * 0.5f;

        // Résolution Two Bone IK
        Vector3 a = targetPos - upper.position;
        float aLen = a.magnitude;

        float cosAngle = Mathf.Clamp(
            (upperLen * upperLen + aLen * aLen - lowerLen * lowerLen) / (2f * upperLen * aLen),
            -1f, 1f
        );
        float angle = Mathf.Acos(cosAngle);

        // Plan de rotation du coude
        Vector3 cross = Vector3.Cross(a, hint - upper.position);
        Vector3 axis = Vector3.Cross(a, cross).normalized;

        // Rotation upper arm
        Quaternion upperRot = Quaternion.LookRotation(a, axis);
        upperRot = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Quaternion.LookRotation(a, axis) * Vector3.right) * upperRot;
        upper.rotation = upperRot;

        // Rotation lower arm vers la cible
        Vector3 toLower = targetPos - lower.position;
        lower.rotation = Quaternion.LookRotation(toLower, axis);

        // Main suit la rotation du contrôleur
        hand.rotation = target.rotation * Quaternion.Euler(handRotOffset);
    }
}