using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsbarController : MonoBehaviour
{
    public Image healthBar;
    public Image energyBar;

    public float healthAmount;
    public float energyAmount;
    
    [Header("player_references")]
    [SerializeField] private PlayerController _playerController;
    
    private void Awake()
    {
        if (_playerController == null)
            _playerController = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();
    }

    private void Start()
    {
        RefreshBarsFromPlayer();
    }

    private void Update()
    {
        RefreshBarsFromPlayer();
    }

    private void RefreshBarsFromPlayer()
{
    if (_playerController == null) return;

    float healthRatio = (float)_playerController.CurrentHealth / _playerController.MaxHealth;
    float energyRatio = (float)_playerController.CurrentEnergy / _playerController.MaxEnergy;

    if (_playerController.CurrentHealth > 0)
    {
        healthRatio = Mathf.Max(healthRatio, 0.05f); 
    }

    healthRatio = Mathf.Clamp01(healthRatio);
    energyRatio = Mathf.Clamp01(energyRatio);

    if (healthBar != null) healthBar.fillAmount = healthRatio;
    if (energyBar != null) energyBar.fillAmount = energyRatio;
}

}
