using UnityEngine;

[ExecuteAlways]
public class DoorGlow : MonoBehaviour
{
    [Header("Glow Settings")]
    public Color glowColor = Color.cyan;
    public float emissionIntensity = 2f;

    [Header("Optional Scene Light")]
    public bool usePointLight = false;
    public Light glowLight;
    public float lightRange = 3f;
    public float lightIntensity = 1.5f;

    private Renderer objectRenderer;
    private Material materialInstance;
    private const string emissionColorProp = "_EmissionColor";

    void Awake()
    {
        EnsureMaterialInstance();
        RefreshGlow();
    }

    void OnValidate()
    {
        EnsureMaterialInstance();
        RefreshGlow();
    }

    void OnEnable()
    {
        RefreshGlow();
    }

    void OnDisable()
    {
        if (materialInstance != null)
        {
            materialInstance.DisableKeyword("_EMISSION");
        }

        if (glowLight != null)
        {
            glowLight.enabled = false;
        }
    }

    private void EnsureMaterialInstance()
    {
        if (objectRenderer == null)
            objectRenderer = GetComponent<Renderer>();

        if (objectRenderer == null)
            objectRenderer = GetComponentInChildren<Renderer>();

        if (objectRenderer == null)
            return;

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            materialInstance = objectRenderer.sharedMaterial;
            return;
        }
#endif

        materialInstance = objectRenderer.material;
    }

    private void RefreshGlow()
    {
        if (objectRenderer != null && materialInstance != null)
        {
            materialInstance.EnableKeyword("_EMISSION");
            materialInstance.SetColor(emissionColorProp, glowColor * emissionIntensity);
        }

        if (usePointLight)
        {
            if (glowLight == null)
            {
                glowLight = GetComponentInChildren<Light>();
            }

            if (glowLight == null)
            {
                GameObject lightObject = new GameObject("Door Glow Light");
                lightObject.transform.SetParent(transform, false);
                glowLight = lightObject.AddComponent<Light>();
                glowLight.type = LightType.Point;
            }

            glowLight.color = glowColor;
            glowLight.range = lightRange;
            glowLight.intensity = lightIntensity;
            glowLight.enabled = enabled;
        }
    }
}
