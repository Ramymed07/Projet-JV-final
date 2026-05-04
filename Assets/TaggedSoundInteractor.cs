using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TaggedSoundInteractor : MonoBehaviour
{
    [Header("Interaction Settings")]
    public string interactTag = "Interactable";
    public KeyCode interactKey = KeyCode.E;
    public float interactionRange = 3f;
    public Camera playerCamera;

    [Header("Audio Settings")]
    public AudioClip interactSound;
    [Tooltip("Optional: leave null to use the AudioSource already on this GameObject.")]
    public AudioSource audioSource;

    void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            Debug.LogWarning("TaggedSoundInteractor added an AudioSource automatically. Assign a custom AudioSource in the Inspector if needed.");
        }

        if (playerCamera == null)
        {
            Debug.LogWarning("TaggedSoundInteractor could not find a player camera. Assign the Camera in the Inspector.");
        }

        if (interactSound == null)
        {
            Debug.LogWarning("TaggedSoundInteractor has no interactSound assigned. Assign an AudioClip in the Inspector.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            TryPlaySound();
        }
    }

    void TryPlaySound()
    {
        if (playerCamera == null || audioSource == null || interactSound == null)
            return;

        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange))
        {
            if (hit.collider.CompareTag(interactTag))
            {
                audioSource.PlayOneShot(interactSound);
            }
            else
            {
                Debug.Log("TaggedSoundInteractor hit object tagged '" + hit.collider.tag + "' instead of '" + interactTag + "'.");
            }
        }
        else
        {
            Debug.Log("TaggedSoundInteractor did not hit any object within range.");
        }
    }
}
