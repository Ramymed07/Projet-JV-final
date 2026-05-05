using UnityEngine;

public class WalkingSound : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource walkingAudio;

    [Header("Movement Keys")]
    public KeyCode forwardKey = KeyCode.W;
    public KeyCode backwardKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;

    void Update()
    {
        bool isPressingMovementKey =
            Input.GetKey(forwardKey) ||
            Input.GetKey(backwardKey) ||
            Input.GetKey(leftKey) ||
            Input.GetKey(rightKey);

        if (isPressingMovementKey)
        {
            if (!walkingAudio.isPlaying)
            {
                walkingAudio.Play();
            }
        }
        else
        {
            if (walkingAudio.isPlaying)
            {
                walkingAudio.Stop();
            }
        }
    }
}
