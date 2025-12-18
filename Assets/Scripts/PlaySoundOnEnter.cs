using UnityEngine;

public class PlaySoundOnEnter : StateMachineBehaviour
{
    [Header("Réglages du Son")]
    public AudioClip soundClip; // Le son à jouer (Whoosh)
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.5f, 1.5f)] public float pitchRandom = 1f; // Pour varier un peu le son (optionnel)

    // Optionnel : un petit délai si le son part trop vite par rapport à l'animation
    public float delay = 0f; 

    private AudioSource _source;
    private float _timer;
    private bool _hasPlayed;

    // Se lance quand on ENTRE dans l'état (ex: Attack)
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 1. On cherche l'AudioSource sur le PARENT (Le Player)
        if (_source == null)
        {
            _source = animator.GetComponentInParent<AudioSource>();
        }

        _hasPlayed = false;
        _timer = 0f;

        // Si le délai est à 0, on joue tout de suite pour être réactif
        if (delay <= 0f)
        {
            PlayTheSound();
        }
    }

    // Se lance à chaque frame de l'animation
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Si on a mis un délai et que le son n'a pas encore été joué
        if (delay > 0f && !_hasPlayed)
        {
            _timer += Time.deltaTime;
            if (_timer >= delay)
            {
                PlayTheSound();
            }
        }
    }

    private void PlayTheSound()
    {
        if (_source != null && soundClip != null)
        {
            // Petite variation de pitch pour que chaque coup d'épée sonne un peu différent
            // Si pitchRandom est à 1, ça ne change rien.
            _source.pitch = Random.Range(1f - (pitchRandom - 1f), pitchRandom);
            
            _source.PlayOneShot(soundClip, volume);
            
            // Remettre le pitch à la normale pour les autres sons
            // (Note: PlayOneShot utilise le pitch actuel de la source)
             // Idéalement on reset après, ou on accepte la variation.
        }
        _hasPlayed = true;
    }
}