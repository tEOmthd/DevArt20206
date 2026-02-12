using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    public Animator animator;
    public Door door;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("Press");
            if (door != null)
            {
                door.OpenDoor();  // méthode à créer dans Door.cs
            }
        }
    }
}

/*
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
*/

