using UnityEngine;

[RequireComponent(typeof(Animator))]
public class VRAvatarIK : MonoBehaviour
{
    [Header("VR Targets")]
    public Transform headTarget;       // Main Camera
    public Transform leftHandTarget;   // Left Controller
    public Transform rightHandTarget;  // Right Controller

    [Header("Body Settings")]
    public float turnSmoothness = 5f;
    public Vector3 headOffset;
    public Vector3 leftHandPosOffset;
    public Vector3 leftHandRotOffset;
    public Vector3 rightHandPosOffset;
    public Vector3 rightHandRotOffset;

    private Animator animator;
    private float bodyYOffset;

    void Start()
    {
        animator = GetComponent<Animator>();
        // Calcul de l'offset entre la tête et les pieds
        bodyYOffset = transform.position.y - headTarget.position.y;
    }

    void LateUpdate()
    {
        // Position du corps sous le casque
        Vector3 headPos = headTarget.position;
        transform.position = new Vector3(headPos.x, headPos.y + bodyYOffset, headPos.z);

        // Rotation du corps suit le regard horizontal
        Vector3 forward = headTarget.forward;
        forward.y = 0;
        if (forward.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * turnSmoothness);
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (animator == null) return;

        // === TÊTE : regard vers la direction du casque ===
        animator.SetLookAtWeight(1f, 0.4f, 1f, 0f, 0.5f);
        animator.SetLookAtPosition(headTarget.position + headTarget.forward * 2f);

        // === MAIN GAUCHE ===
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
        Vector3 leftPos = leftHandTarget.position + leftHandTarget.TransformVector(leftHandPosOffset);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftPos);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation * Quaternion.Euler(leftHandRotOffset));

        // === MAIN DROITE ===
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
        Vector3 rightPos = rightHandTarget.position + rightHandTarget.TransformVector(rightHandPosOffset);
        animator.SetIKPosition(AvatarIKGoal.RightHand, rightPos);
        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation * Quaternion.Euler(rightHandRotOffset));
    }
}