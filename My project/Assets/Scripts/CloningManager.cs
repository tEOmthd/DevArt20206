using UnityEngine;
using UnityEngine.XR;

public class CloningManager : MonoBehaviour
{

    public BlueScreenFade blueScreenFade;

    private bool isCloning = false;


    void Update()
    {
        InputDevice leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        bool primaryButtonValue;
        if (leftHand.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonValue) && primaryButtonValue)
        {
            Debug.Log("Début du clonage");
            isCloning = true;

            blueScreenFade.StartFade();


        }


    }


}