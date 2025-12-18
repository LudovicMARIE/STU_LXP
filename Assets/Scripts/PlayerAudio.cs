using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource source; // Glissez l'AudioSource du joueur ici

    [Header("Clips")]
    public AudioClip swingSound;
    public AudioClip[] stepSounds; // Tableau pour varier les bruits de pas (plus réaliste)
    public AudioClip hurtSound;
    public AudioClip deathSound;
    public AudioClip hitEnemySound; 

    private void Awake()
    {
        if (source == null) source = GetComponent<AudioSource>();
    }

    // --- APPELÉ PAR LES ANIMATIONS (Animation Events) ---

    // Fonction à appeler dans l'event de l'animation "Run" ou "Walk"
    public void PlayFootstep()
    {
        if (stepSounds.Length > 0)
        {
            print("Play footstep sound");
            // Prend un son au hasard pour éviter l'effet "mitraillette"
            AudioClip clip = stepSounds[Random.Range(0, stepSounds.Length)];
            // Change un peu le pitch pour varier
            source.pitch = Random.Range(0.9f, 1.1f); 
            source.PlayOneShot(clip);
        }
    }

    // Fonction à appeler dans l'event de l'animation "Attack"
    public void PlaySwing()
    {
        source.pitch = 1f; // Remet le pitch normal
        if (swingSound != null) source.PlayOneShot(swingSound);
    }

    // --- APPELÉ PAR LES SCRIPTS (Code) ---

    public void PlayHurt()
    {
        if (hurtSound != null) source.PlayOneShot(hurtSound);
    }

    public void PlayDeath()
    {
        print("Play death sound");
        if (deathSound != null) {
            source.PlayOneShot(deathSound);
        }
        else
        {
            Debug.LogWarning("Death sound is not assigned in PlayerAudio!");
        }
    }
    
    public void PlayHitImpulse()
    {
         if (hitEnemySound != null) source.PlayOneShot(hitEnemySound);
    }
}