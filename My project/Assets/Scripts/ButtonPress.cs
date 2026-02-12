using UnityEngine;
using UnityEngine.XR;

public class ButtonPress : MonoBehaviour
{
    public Animator animator;
    public Door door;
    private bool lastPressed = false;

    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        bool primaryButtonPressed = false;
        if (device.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonPressed))
        {
            if (primaryButtonPressed && !lastPressed)
            {
                animator.SetTrigger("Press");
            }
            lastPressed = primaryButtonPressed;
        }
    }
}