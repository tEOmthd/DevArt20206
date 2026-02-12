using UnityEngine;

public class VRIKBody : MonoBehaviour
{
    [Header("XR Targets")]
    public Transform headTarget;      // Main Camera
    public Transform leftHandTarget;  // Left Controller
    public Transform rightHandTarget; // Right Controller

    [Header("Body Bones")]
    public Transform head;
    public Transform spine;
    public Transform leftUpperArm;
    public Transform leftLowerArm;
    public Transform leftHand;
    public Transform rightUpperArm;
    public Transform rightLowerArm;
    public Transform rightHand;

    [Header("Settings")]
    public float heightOffset = -1.6f;
    public Vector3 leftHandRotOffset;
    public Vector3 rightHandRotOffset;

    void LateUpdate()
    {
        // 1. Corps suit le casque
        Vector3 headPos = headTarget.position;
        transform.position = new Vector3(headPos.x, headPos.y + heightOffset, headPos.z);

        Vector3 forward = headTarget.forward;
        forward.y = 0;
        if (forward.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(forward);

        // 2. Tête suit le casque
        head.rotation = headTarget.rotation;

        // 3. Bras gauche IK
        SolveTwoBoneIK(leftUpperArm, leftLowerArm, leftHand, leftHandTarget, leftHandRotOffset);

        // 4. Bras droit IK
        SolveTwoBoneIK(rightUpperArm, rightLowerArm, rightHand, rightHandTarget, rightHandRotOffset);
    }

    void SolveTwoBoneIK(Transform upper, Transform lower, Transform hand, Transform target, Vector3 rotOffset)
    {
        // Longueurs des os
        float upperLength = Vector3.Distance(upper.position, lower.position);
        float lowerLength = Vector3.Distance(lower.position, hand.position);

        // Direction vers la cible
        Vector3 toTarget = target.position - upper.position;
        float targetDist = toTarget.magnitude;

        // Clamp pour éviter l'extension impossible
        targetDist = Mathf.Clamp(targetDist, Mathf.Abs(upperLength - lowerLength) + 0.01f, upperLength + lowerLength - 0.01f);

        // Angles via loi des cosinus
        float cosUpper = (upperLength * upperLength + targetDist * targetDist - lowerLength * lowerLength) / (2f * upperLength * targetDist);
        float cosLower = (upperLength * upperLength + lowerLength * lowerLength - targetDist * targetDist) / (2f * upperLength * lowerLength);

        cosUpper = Mathf.Clamp(cosUpper, -1f, 1f);
        cosLower = Mathf.Clamp(cosLower, -1f, 1f);

        float angleUpper = Mathf.Acos(cosUpper) * Mathf.Rad2Deg;
        float angleLower = Mathf.Acos(cosLower) * Mathf.Rad2Deg;

        // Rotation du bras supérieur vers la cible
        Vector3 dir = toTarget.normalized;
        Quaternion lookRot = Quaternion.LookRotation(dir);

        // Hint : coude pointe vers le bas/arrière
        Vector3 hint = Vector3.Cross(dir, Vector3.up).normalized;
        if (hint.sqrMagnitude < 0.001f) hint = Vector3.forward;

        Quaternion upperRot = Quaternion.LookRotation(dir, hint);
        upper.rotation = upperRot * Quaternion.AngleAxis(-angleUpper, Vector3.right);

        // Coude
        Vector3 toLower = (target.position - lower.position).normalized;
        lower.rotation = Quaternion.LookRotation(toLower, hint);

        // Main suit la rotation du contrôleur
        hand.rotation = target.rotation * Quaternion.Euler(rotOffset);
    }
}