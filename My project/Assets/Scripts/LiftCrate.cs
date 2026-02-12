using UnityEngine;


public class LiftCrate : MonoBehaviour
{
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor leftHandInteractor;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor rightHandInteractor;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (leftHandInteractor.hasSelection && rightHandInteractor.hasSelection)
        {
            var leftObject = leftHandInteractor.firstInteractableSelected;
            var rightObject = rightHandInteractor.firstInteractableSelected;

            if (leftObject == rightObject)
            {
                Debug.Log("Les deux mains tiennent le même objet !");
            }
        }
    }
}
