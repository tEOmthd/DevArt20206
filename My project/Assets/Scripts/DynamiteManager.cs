using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DynamiteActivator : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI countdownText;
    
    [Header("Dynamite Props")]
    public GameObject dynamite1;
    public GameObject dynamite2;
    public GameObject dynamite3;
    public GameObject dynamite4;
    
    [Header("Trigger Zone")]
    public Collider triggerVolume;
    
    [Header("Target")]
    public GameObject objectToDestroy;
    
    [Header("Settings")]
    public float countdownDuration = 3f;
    
    private HashSet<GameObject> dynamitesInZone = new HashSet<GameObject>();
    private bool isCountdownActive = false;
    private bool hasExploded = false;
    
    void Start()
    {
        if (countdownText != null)
            countdownText.text = "";
        
        if (triggerVolume == null)
        {
            triggerVolume = GetComponent<Collider>();
        }
        
        if (triggerVolume != null)
        {
            triggerVolume.isTrigger = true;
        }
        else
        {
            Debug.LogError("Aucun Collider trouvé ! Ajoute un Collider en mode Trigger.");
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        // Vérifie si c'est une des dynamites
        if (IsDynamite(other.gameObject))
        {
            dynamitesInZone.Add(other.gameObject);
            Debug.Log($"{other.gameObject.name} est entrée dans la zone. Total: {dynamitesInZone.Count}/4");
            
            CheckActivation();
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        // Vérifie si c'est une des dynamites
        if (IsDynamite(other.gameObject))
        {
            dynamitesInZone.Remove(other.gameObject);
            Debug.Log($"{other.gameObject.name} est sortie de la zone. Total: {dynamitesInZone.Count}/4");
            
            // Annule le compte à rebours si une dynamite sort
            if (dynamitesInZone.Count < 4 && isCountdownActive && !hasExploded)
            {
                StopAllCoroutines();
                isCountdownActive = false;
                if (countdownText != null)
                    countdownText.text = "";
                Debug.Log("Compte à rebours annulé !");
            }
        }
    }
    
    bool IsDynamite(GameObject obj)
    {
        return obj == dynamite1 || obj == dynamite2 || obj == dynamite3 || obj == dynamite4;
    }
    
    void CheckActivation()
    {
        // Si les 4 dynamites sont dans la zone et qu'aucun compte à rebours n'est actif
        if (dynamitesInZone.Count >= 4 && !isCountdownActive && !hasExploded)
        {
            Debug.Log("Toutes les dynamites sont en place ! Début du compte à rebours !");
            StartCoroutine(CountdownRoutine());
        }
    }
    
    IEnumerator CountdownRoutine()
    {
        isCountdownActive = true;
        float timeRemaining = countdownDuration;
        
        while (timeRemaining > 0)
        {
            // Affiche le temps restant
            if (countdownText != null)
            {
                countdownText.text = $"EXPLOSION DANS\n{timeRemaining:F1}s";
            }
            
            timeRemaining -= Time.deltaTime;
            yield return null;
        }
        
        // Explosion !
        if (countdownText != null)
            countdownText.text = "BOOM !";
        
        hasExploded = true;
        
        // Détruit l'objet cible
        if (objectToDestroy != null)
        {
            Debug.Log($"Destruction de {objectToDestroy.name} !");
            Destroy(objectToDestroy);
        }
        else
        {
            Debug.LogWarning("Aucun objet à détruire n'a été assigné !");
        }
        
        // Optionnel : efface le texte après 1 seconde
        yield return new WaitForSeconds(1f);
        if (countdownText != null)
            countdownText.text = "";
    }
}