using UnityEngine;

public class EnemyXpDrop : MonoBehaviour
{
    public float baseXpAmount = 10f;

    // Appelez cette fonction quand l'ennemi meurt (dans son script de vie)
    public void DropExperience()
    {
        // Trouve le spawner pour connaître la vague actuelle
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        
        float multiplier = 1f;
        if (spawner != null)
        {
            // Exemple : +10% d'XP par vague
            multiplier = 1f + (spawner.CurrentWave * 0.1f);
        }

        float finalXp = baseXpAmount * multiplier;

        // Donne l'XP au manager
        if (ExperienceManager.Instance != null)
        {
            ExperienceManager.Instance.AddExperience(finalXp);
        }
        
        // Dire au spawner qu'on est mort (votre logique actuelle)
        if (spawner != null) spawner.OnEnemyKilled();
    }
}