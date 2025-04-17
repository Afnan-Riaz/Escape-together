using UnityEngine;

public class AnimationEventsHandler : MonoBehaviour
{
    public AudioClip landAudio; // Assign the land audio clip in the Inspector
    public AudioClip footstepAudio; // Assign the footstep audio clip in the Inspector
    private AudioSource audioSource;

    private float footstepCooldown = 0.2f; // Minimum time between footstep sounds
    private float lastFootstepTime = 0f;

    void Start()
    {
        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void OnLand()
    {
        // Play the land audio
        if (landAudio != null)
        {
            audioSource.PlayOneShot(landAudio, 0.6f);
        }
    }

    public void OnFootstep()
    {
        // Prevent overlapping footstep sounds
        if (Time.time - lastFootstepTime >= footstepCooldown)
        {
            if (footstepAudio != null)
            {
                audioSource.PlayOneShot(footstepAudio);
            }
            lastFootstepTime = Time.time; // Update the last footstep time
        }
    }
}