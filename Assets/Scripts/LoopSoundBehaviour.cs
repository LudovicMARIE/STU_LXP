using UnityEngine;

public class LoopSoundBehaviour : StateMachineBehaviour
{
    [Header("Réglages")]
    public float interval = 0.3f; // Temps en secondes entre deux pas
    [Range(0f, 1f)] public float volume = 1f;
    
    // Timer interne
    private float _timer;
    private PlayerAudio _playerAudio; // Référence au script créé précédemment

    // Se lance quand on ENTRE dans l'état "Run"
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // On récupère le script PlayerAudio (ou AudioSource) sur le joueur
        if (_playerAudio == null)
        {
            _playerAudio = animator.GetComponentInParent<PlayerAudio>();
            Debug.Log(_playerAudio.name);
        }

        

        // On réinitialise le timer pour jouer un son tout de suite (ou attendre, au choix)
        _timer = interval; 
    }

    // Se lance à CHAQUE FRAME tant qu'on est dans l'état "Run"
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _timer += Time.deltaTime;

        if (_timer >= interval)
        {
            PlayStep();
            _timer = 0f; // Reset du timer
        }
    }

    private void PlayStep()
    {
        if (_playerAudio != null)
        {
            // Appelle la méthode qu'on a créée tout à l'heure
            _playerAudio.PlayFootstep();
        }
    }
}