using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject levelUpPanel; // Le panel complet (à désactiver au début)
    public Button[] choiceButtons;  // Vos 3 boutons dans l'UI
    public TextMeshProUGUI[] buttonDescriptions; // Les textes des boutons

    [Header("Player Reference")]
    public PlayerController player; // Référence à votre script de joueur pour modifier les stats

    // Liste des types d'améliorations possibles
    public enum StatType { MaxHealth, AttackSpeed, Damage, MoveSpeed }
    
    private List<StatType> currentChoices;

    void Start()
    {
        levelUpPanel.SetActive(false);
    }

    public void ShowLevelUpMenu()
    {
        // 1. Pause du jeu
        Time.timeScale = 0f; 
        levelUpPanel.SetActive(true);

        // 2. Générer 3 choix aléatoires
        GenerateRandomChoices();
    }

    void GenerateRandomChoices()
    {
        currentChoices = new List<StatType>();
        
        // Liste de toutes les stats possibles
        List<StatType> possibleStats = new List<StatType>() 
        { 
            StatType.MaxHealth, 
            StatType.AttackSpeed, 
            StatType.Damage, 
            StatType.MoveSpeed 
        };

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            // Sécurité si on a moins de stats que de boutons
            if (possibleStats.Count == 0) break;

            // Pioche une stat au hasard
            int randomIndex = Random.Range(0, possibleStats.Count);
            StatType selectedStat = possibleStats[randomIndex];
            
            // L'ajoute aux choix actuels
            currentChoices.Add(selectedStat);
            
            // Met à jour le texte du bouton
            buttonDescriptions[i].text = GetDescription(selectedStat);
            
            // Retire la stat de la liste temporaire pour éviter d'avoir 2 fois la même proposition
            possibleStats.RemoveAt(randomIndex);

            // Configure le clic du bouton (Très important pour gérer le paramètre dynamique)
            int index = i; // Copie locale pour la closure
            choiceButtons[i].onClick.RemoveAllListeners();
            choiceButtons[i].onClick.AddListener(() => OnUpgradeSelected(index));
        }
    }

    string GetDescription(StatType stat)
    {
        switch (stat)
        {
            case StatType.MaxHealth: return "+10 PV Max";
            case StatType.AttackSpeed: return "+10% Vitesse Attaque";
            case StatType.Damage: return "+5 Dégâts";
            case StatType.MoveSpeed: return "+5% Vitesse Course";
            default: return "Bonus inconnu";
        }
    }

    public void OnUpgradeSelected(int buttonIndex)
    {
        StatType chosenStat = currentChoices[buttonIndex];
        ApplyUpgrade(chosenStat);

        // Fermer le menu et relancer le jeu
        levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    void ApplyUpgrade(StatType stat)
    {
        // Ici, vous modifiez les variables de votre PlayerController
        // Exemple (adaptez les noms de variables à votre script) :
        switch (stat)
        {
            case StatType.MaxHealth:
                // player.MaxHealth += 10;
                // player.Heal(10);
                Debug.Log("PV Augmentés !");
                break;
            case StatType.AttackSpeed:
                // player.attackCooldown *= 0.9f;
                Debug.Log("Vitesse d'attaque augmentée !");
                break;
            case StatType.Damage:
                // player.damage += 5;
                Debug.Log("Dégâts augmentés !");
                break;
            case StatType.MoveSpeed:
                // player.moveSpeed *= 1.05f;
                Debug.Log("Vitesse de déplacement augmentée !");
                break;
        }
    }
}