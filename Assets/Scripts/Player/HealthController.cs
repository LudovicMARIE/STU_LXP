using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public Image healthBar;
    public Image energyBar;
    public float healthAmount = 100f;
    public float energyAmount = 100f;
    
    private InputAction healAction;
    private InputAction damageAction;
    
    
    private void Awake()
    {
        healAction = new InputAction("Heal", InputActionType.Button, "<Keyboard>/space");
        damageAction = new InputAction("Damage", InputActionType.Button, "<Keyboard>/enter");

        healAction.performed += _ => Heal(5f);
        damageAction.performed += _ => TakeDamage(10f);
    }

    private void OnEnable()
    {
        healAction.Enable();
        damageAction.Enable();
    }

    private void OnDisable()
    {
        healAction.Disable();
        damageAction.Disable();
    }

    private void Start()
    {
        // Important : initialise la barre selon la vie de départ (ex: 50 => 0.5)
        healthAmount = Mathf.Clamp(healthAmount, 0f, 100f);
        healthBar.fillAmount = healthAmount / 100f;
    }

    private void Update()
    {
        if (healthAmount <= 0f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void TakeDamage(float damage)
    {
        healthAmount = Mathf.Clamp(healthAmount - damage, 0f, 100f);
        energyAmount = Mathf.Clamp(energyAmount - damage, 0f, 100f);
        healthBar.fillAmount = healthAmount / 100f;
        energyBar.fillAmount = energyAmount / 100f;
    }

    public void Heal(float healingAmount)
    {
        healthAmount = Mathf.Clamp(healthAmount + healingAmount, 0f, 100f);
        energyAmount = Mathf.Clamp(energyAmount + healingAmount, 0f, 100f);
        healthBar.fillAmount = healthAmount / 100f;
        energyBar.fillAmount = energyAmount / 100f;
    }
}
