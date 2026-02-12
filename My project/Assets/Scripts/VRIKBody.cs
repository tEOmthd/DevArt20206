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

        // Bras gauche — hint coude vers l'extérieur gauche + bas
        Vector3 leftHint = leftUpperArm.position - transform.right * 0.5f - transform.up * 0.3f;
        SolveTwoBoneIK(leftUpperArm, leftLowerArm, leftHand, leftHandTarget.position, leftHint);
        leftHand.rotation = leftHandTarget.rotation * Quaternion.Euler(leftHandRotOffset);

        // Bras droit — hint coude vers l'extérieur droit + bas
        Vector3 rightHint = rightUpperArm.position + transform.right * 0.5f - transform.up * 0.3f;
        SolveTwoBoneIK(rightUpperArm, rightLowerArm, rightHand, rightHandTarget.position, rightHint);
        rightHand.rotation = rightHandTarget.rotation * Quaternion.Euler(rightHandRotOffset);
    }

    void SolveTwoBoneIK(Transform root, Transform mid, Transform tip, Vector3 targetPos, Vector3 hintPos)
    {
        float a = Vector3.Distance(root.position, mid.position);
        float b = Vector3.Distance(mid.position, tip.position);
        float maxLen = a + b;

        Vector3 aVec = targetPos - root.position;
        float c = Mathf.Min(aVec.magnitude, maxLen * 0.999f);

        if (c < 0.001f) return;

        // Angle au root (épaule)
        float cosA = Mathf.Clamp((a * a + c * c - b * b) / (2f * a * c), -1f, 1f);
        float angleA = Mathf.Acos(cosA);

        // Angle au mid (coude)
        float cosB = Mathf.Clamp((a * a + b * b - c * c) / (2f * a * b), -1f, 1f);
        float angleB = Mathf.Acos(cosB);

        // Direction root -> target
        Vector3 dirToTarget = (targetPos - root.position).normalized;

        // Plan de pliage basé sur le hint
        Vector3 dirToHint = (hintPos - root.position).normalized;
        Vector3 normal = Vector3.Cross(dirToTarget, dirToHint);
        if (normal.sqrMagnitude < 0.0001f)
            normal = Vector3.Cross(dirToTarget, Vector3.up);
        normal.Normalize();

        // Axe de pliage perpendiculaire
        Vector3 bendAxis = Vector3.Cross(dirToTarget, normal).normalized;

        // Rotation du root (épaule)
        Quaternion lookRot = Quaternion.LookRotation(dirToTarget, normal);
        root.rotation = lookRot * Quaternion.AngleAxis(-angleA * Mathf.Rad2Deg, Vector3.right);

        // Recalcul position mid après rotation root
        // Rotation du mid (coude) vers la target
        Vector3 midToTarget = (targetPos - mid.position).normalized;
        mid.rotation = Quaternion.LookRotation(midToTarget, normal);
    }
}