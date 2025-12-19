using UnityEngine;
using TMPro; // Nécessaire pour le texte

public class GameHUD : MonoBehaviour
{
    [Header("Références Textes UI")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xpText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI statsBonusText; // Pour afficher les bonus (Dégâts, Vitesse...)

    [Header("Références Scripts")]
    public PlayerController player;
    // Pas besoin de variable public pour ExperienceManager car c'est un Singleton (Instance)

    void Update()
    {
        UpdateLevelAndXP();
        UpdateHealth();
        UpdateStatsBonuses();
    }

    void UpdateLevelAndXP()
    {
        if (ExperienceManager.Instance != null)
        {
            // Niveau
            levelText.text = "Level " + ExperienceManager.Instance.currentLevel;

            // XP (Format : 45 / 100 XP)
            // "F0" veut dire 0 chiffre après la virgule
            xpText.text = $"{ExperienceManager.Instance.currentXp:F0} / {ExperienceManager.Instance.targetXp:F0} XP";
        }
    }

    void UpdateHealth()
    {
        if (player != null)
        {
            // PV (Format : 15 / 20)
            hpText.text = $"{player.CurrentHealth} / {player.MaxHealth}";
        }
    }

    void UpdateStatsBonuses()
    {
        if (player != null)
        {
            // On construit une chaîne de caractères avec les bonus
            string stats = "";

            // Dégâts (Affiche seulement si > 0)
            if (player.BonusDamage > 0)
                stats += $"Damage : <color=red>+{player.BonusDamage}</color>\n";

            // Vitesse d'attaque
            if (player.BonusAttackSpeedPct > 0)
                stats += $"Attack Speed : <color=yellow>+{player.BonusAttackSpeedPct:F0}%</color>\n";
            // Vitesse de déplacement
            if (player.BonusMoveSpeedPct > 0)
                stats += $"Move Speed : <color=#00FFFF>+{player.BonusMoveSpeedPct:F0}%</color>";

            // Si aucun bonus, on affiche un texte vide ou un message par défaut
            if (stats == "") stats = "No Bonuses";

            statsBonusText.text = stats;
        }
    }
}