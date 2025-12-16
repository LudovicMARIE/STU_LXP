using UnityEngine;
using System.Collections;

public class SwordController : MonoBehaviour
{
    [Header("Stats Arme")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackDuration = 0.3f; // Durée du coup
    [SerializeField] private float attackCooldown = 0.5f; // Temps entre deux coups
    
    [Header("References")]
    [SerializeField] private Transform swordContainer; // Glisse l'objet "SwordContainer"
    [SerializeField] private Collider2D swordCollider; // Glisse l'objet "SwordSprite" (qui a le collider)

    private bool isAttacking = false;
    private float lastAttackTime = -999f;
    private Quaternion targetRotation;

    void Start()
    {
        // Sécurité : on s'assure que le collider est éteint au début
        if(swordCollider != null) swordCollider.enabled = false;
    }

    // Appelée par le PlayerController à chaque frame
    public void HandleRotation(Vector2 mousePos, Vector2 playerPos)
    {
        if (isAttacking) return; // On ne tourne pas l'arme pendant qu'elle frappe

        Vector2 lookDir = mousePos - playerPos;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        
        // On stocke la rotation idéale
        targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = targetRotation;

        // Gestion du flip vertical de l'arme (pour ne pas qu'elle soit à l'envers à gauche)
        Vector3 scale = transform.localScale;
        if (angle > 90 || angle < -90) scale.y = -1f;
        else scale.y = 1f;
        transform.localScale = scale;
    }

    // Appelée par le PlayerController quand on clique
    public void TryAttack()
    {
        if (Time.time >= lastAttackTime + attackCooldown && !isAttacking)
        {
            StartCoroutine(PerformAttack());
        }
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        // 1. Activer la hitbox
        if(swordCollider != null) swordCollider.enabled = true;

        // 2. Animation procédurale (Arc de cercle)
        // On part d'un peu en haut (45 deg) pour aller vers le bas (-45 deg) par rapport à la souris
        float startAngle = 45f;
        float endAngle = -45f;
        float timer = 0f;

        Quaternion baseRot = transform.rotation;

        while (timer < attackDuration)
        {
            timer += Time.deltaTime;
            float t = timer / attackDuration;
            
            // Courbe d'animation (Lerp)
            float currentAddedAngle = Mathf.Lerp(startAngle, endAngle, t);
            
            // On applique la rotation sur le conteneur interne pour faire l'effet de balancier
            swordContainer.localRotation = Quaternion.Euler(0, 0, currentAddedAngle);
            
            yield return null;
        }

        // 3. Reset
        if(swordCollider != null) swordCollider.enabled = false;
        swordContainer.localRotation = Quaternion.identity; // Remise à zéro
        isAttacking = false;
    }

    // Détection des coups
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // C'est ici qu'on prévient l'autre développeur
            // Il devra avoir un script avec une méthode publique : TakeDamage(int damage)
            Debug.Log($"Touché ennemi : {other.name} pour {damage} dégâts");
            
            // Exemple d'interaction future (ne pas copier si le script n'existe pas) :
            // other.GetComponent<EnemyHealth>()?.TakeDamage(damage);
        }
    }
}