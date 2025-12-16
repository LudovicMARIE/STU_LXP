using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("References")]
    [SerializeField] private Transform weaponPivot;
    [SerializeField] private SpriteRenderer characterSprite;
    
    private Animator animator; 

    [Header("Inputs")]
    public InputAction moveAction;
    public InputAction attackAction;

    private Rigidbody2D rb;
    private Camera mainCam;
    private Vector2 movementInput;
    private Vector2 mousePos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;

        animator = GetComponentInChildren<Animator>(); 

        if (moveAction.bindings.Count == 0)
        {
            moveAction.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/z")
                .With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/q")
                .With("Right", "<Keyboard>/d");
        }
        if (attackAction.bindings.Count == 0)
            attackAction.AddBinding("<Mouse>/leftButton");
    }

    void OnEnable() { moveAction.Enable(); attackAction.Enable(); }
    void OnDisable() { moveAction.Disable(); attackAction.Disable(); }

    void Update()
    {
        movementInput = moveAction.ReadValue<Vector2>();

        if (animator != null)
        {
            animator.SetFloat("Speed", movementInput.magnitude);
        }

        if (Mouse.current != null)
            mousePos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (characterSprite != null)
        {
            if (mousePos.x < rb.position.x) characterSprite.flipX = true;
            else characterSprite.flipX = false;
        }
        
        if (attackAction.WasPressedThisFrame())
        {
            Attack();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
        RotateWeapon();
    }
    
    void RotateWeapon()
    {
        if (weaponPivot == null) return;
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        weaponPivot.rotation = Quaternion.Euler(0, 0, angle);

        Vector3 localScale = Vector3.one;
        if (angle > 90 || angle < -90) localScale.y = -1f;
        else localScale.y = 1f;
        weaponPivot.localScale = localScale;
    }

    void Attack()
    {
        Debug.Log("PAN !");
    }
}