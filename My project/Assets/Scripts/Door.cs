using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    public float openAngle = 90f;
    public float openSpeed = 200f;
    public float openDuration = 3f;

    [Header("Interaction Settings")]
    public float interactionDistance = 3f;
    public Transform playerCamera; // Main Camera du XR Origin

    [Header("UI")]
    public TextMeshProUGUI interactionText;
    public Image interactionImage;
    public float messageDuration = 2f;

    private bool isBusy = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Coroutine messageCoroutine;

    public CloningManager cloningmanager;

    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable;

    void Awake()
    {

        if (interactable == null)
        {
            Debug.LogError("XRBaseInteractable manquant sur " + gameObject.name);
        }
    }

    void OnEnable()
    {
        if (interactable != null)
            interactable.selectEntered.AddListener(OnSelected);
    }

    void OnDisable()
    {
        if (interactable != null)
            interactable.selectEntered.RemoveListener(OnSelected);
    }

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));
        HideUI();
    }

    private void OnSelected(SelectEnterEventArgs args)
    {
        if (isBusy) return;

        float distance = Vector3.Distance(playerCamera.position, transform.position);

        if (distance > interactionDistance)
        {
            ShowMessage("Trop loin pour interagir");
            return;
        }

        ShowMessage("Ouverture...");
        StartCoroutine(OpenAndClose());
        if(cloningmanager.isCloning == true)
        StartCoroutine(WaitCloningTime());

    }

    IEnumerator OpenAndClose()
    {
        isBusy = true;


        // OUVERTURE
        while (Quaternion.Angle(transform.rotation, openRotation) > 0.5f)
        {
            
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                openRotation,
                openSpeed * Time.deltaTime
            );

            yield return null;
        }

        yield return new WaitForSeconds(openDuration);

        // FERMETURE
        while (Quaternion.Angle(transform.rotation, closedRotation) > 0.5f)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                closedRotation,
                openSpeed * Time.deltaTime
            );

            yield return null;
        }

        isBusy = false;
    }

    void ShowMessage(string message)
    {
        if (interactionText != null)
        {
            interactionText.text = message;
            interactionText.gameObject.SetActive(true);
        }

        if (interactionImage != null)
            interactionImage.gameObject.SetActive(true);

        if (messageCoroutine != null)
            StopCoroutine(messageCoroutine);

        messageCoroutine = StartCoroutine(HideMessageAfterDelay());
    }

    IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDuration);
        HideUI();
    }

    void HideUI()
    {
        if (interactionText != null)
            interactionText.gameObject.SetActive(false);

        if (interactionImage != null)
            interactionImage.gameObject.SetActive(false);
    }

    IEnumerator WaitCloningTime()
    {
        yield return new WaitForSeconds(10.0f);
        StartCoroutine(OpenAndClose());
    }
}
