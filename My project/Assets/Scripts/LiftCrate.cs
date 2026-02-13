using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LiftCrate : UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable
{
    void Start()
    {
        
    }
    public void CheckIfTwoHands()
    {
        if (interactorsSelecting.Count == 2)
        {
            Debug.Log("Deux mains sur l'objet");
        }
        else if (interactorsSelecting.Count == 1)
        {
            Debug.Log("Une seule main");
        }
        else
        {
            Debug.Log("Aucune main");
        }
    }
}