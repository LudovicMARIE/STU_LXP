using UnityEngine;
using UnityEngine.UI;
using TMPro; // Si vous utilisez TextMeshPro pour le texte du niveau

public class ExperienceManager : MonoBehaviour
{
    public static ExperienceManager Instance;

    [Header("Interface")]
    public Image xpBarImage; // L'image "Fill" de votre barre (Type: Filled)
    public TextMeshProUGUI levelText; // Optionnel : affiche "Niveau 5"

    [Header("Paramètres")]
    public int currentLevel = 1;
    public float currentXp = 0;
    public float targetXp = 100;
    public float xpGrowthMultiplier = 1.2f; // Chaque niveau demande 20% d'XP en plus

    [Header("Références")]
    public LevelUpManager levelUpManager; // Le script qui gère le menu

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateUI();
    }

    public void AddExperience(float amount)
    {
        currentXp += amount;

        // Gestion de la montée de niveau
        CheckLevelUp();
        
        UpdateUI();
    }

    void CheckLevelUp()
    {

        while (currentXp >= targetXp)
        {
            currentXp -= targetXp; 
            
            currentLevel++;

            targetXp = Mathf.RoundToInt(targetXp * xpGrowthMultiplier);

            levelUpManager.ShowLevelUpMenu();
        }
    }

    void UpdateUI()
    {
        // Ajout de (float) devant pour forcer la division décimale
        float ratio = (float)currentXp / (float)targetXp;

        // Sécurité : On s'assure que le ratio est entre 0 et 1
        ratio = Mathf.Clamp01(ratio);

        xpBarImage.fillAmount = ratio;

        if (levelText != null)
            levelText.text = "Lvl " + currentLevel;
    }
}