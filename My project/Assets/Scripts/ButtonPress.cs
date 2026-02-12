using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    public Animator animator;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("press");
        }
    }
}
