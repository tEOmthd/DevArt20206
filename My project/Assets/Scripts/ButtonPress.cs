using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    public Animator animator;
    public Door door;
    private bool lastPressed = false;

    void Update()
{
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





