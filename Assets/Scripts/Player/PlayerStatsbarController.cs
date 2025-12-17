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
        RefrehhBarsFromPlayer();
    }

    private void Update()
    {
        RefrehhBarsFromPlayer();
    }

    private void RefrehhBarsFromPlayer()
    {
        if (_playerController == null) return;

        healthAmount = (_playerController.CurrentHealth / (float)_playerController.MaxHealth) * 100f;
        energyAmount = (_playerController.CurrentEnergy / (float)_playerController.MaxEnergy) * 100f;

        healthAmount = Mathf.Clamp(healthAmount, 0f, 100f);
        energyAmount = Mathf.Clamp(energyAmount, 0f, 100f);

        if (healthBar != null) healthBar.fillAmount = healthAmount / 100f;
        if (energyBar != null) energyBar.fillAmount = energyAmount / 100f;
    }

}
