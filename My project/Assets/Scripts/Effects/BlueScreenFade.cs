using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlueScreenFade : MonoBehaviour
{
    [Header("Volume et couleur")]
    public Volume volume; // le Volume global avec Color Adjustments
    public Color targetColor; // bleu léger
    public float duration = 2f; // temps pour atteindre la couleur

    private ColorAdjustments colorAdjustments;
    private Color initialColor;
    private float timer = 0f;
    private bool fading = false;

    void Awake()
    {
        if (volume == null)
        {
            Debug.LogError("Volume non assigné !");
            enabled = false;
            return;
        }

        if (!volume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
        {
            Debug.LogError("ColorAdjustments non trouvé dans le Volume !");
            enabled = false;
            return;
        }

        initialColor = colorAdjustments.colorFilter.value;
    }

    void Update()
    {
        if (fading)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            colorAdjustments.colorFilter.value = Color.Lerp(initialColor, targetColor, t);
        }
    }

    /// <summary>
    /// Appelle cette fonction pour démarrer le dégradé bleu
    /// </summary>
    public void StartFade()
    {
        Debug.Log("Mise de leffet");
        fading = true;
        timer = 0f;
    }

    /// <summary>
    /// Réinitialise la couleur initiale
    /// </summary>
    public void ResetFade()
    {
        fading = false;
        timer = 0f;
        colorAdjustments.colorFilter.value = initialColor;
    }
}