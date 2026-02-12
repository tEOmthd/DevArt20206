using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public float openAngle = 90f;
    public float openSpeed = 2f;
    public bool isOpen = false;

    private Quaternion _closedRotation;
    private Quaternion _openRotation;
    private Coroutine _currentCoroutine; 
    void Start()
    {
        _closedRotation = transform.rotation; 
        _openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));
    }


    // Méthode publique pour déclencheazer la porte depuis un autre script
   public void OpenDoor()
{
    if (_currentCoroutine != null)
        StopCoroutine(_currentCoroutine);
    Debug.Log("Door activated");
    _currentCoroutine = StartCoroutine(ToggleDoor());
}
    private IEnumerator ToggleDoor()
    {
        Quaternion targetRotation = isOpen ? _closedRotation : _openRotation;
        isOpen = !isOpen;

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * openSpeed);
            yield return null;
        }

        transform.rotation = targetRotation;
    }
}
