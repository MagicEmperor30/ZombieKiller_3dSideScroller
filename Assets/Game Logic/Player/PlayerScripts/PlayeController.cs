using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public float acceleration = 10f;

    [Header("Combat")]
    public float fireRate = 0.5f;
    public Transform shootPoint;
    public LayerMask hitMask;
    public GameObject forceField;

    [Header("Driving")]
    [HideInInspector] public VehicleControllerWithWheels currentVehicle;
    [HideInInspector] public Transform vehicleSeatTarget;
    public Transform PlayerHipTransform;
    [Header ("Defend UI Components")]
    public TMP_Text defendCooldownText;
    public TMP_Text defendDurationText;

    [Header("Status")]
    public bool isDead = false;

    private Rigidbody rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public StateMachine stateMachine;

    private Vector2 input;
    private Vector3 currentVelocity;
    [HideInInspector] public float currentSpeed;
    [HideInInspector] public float nextFireTime;
    [HideInInspector] public float defendTimer;

   [HideInInspector] public bool inCombat;
   [HideInInspector] public bool isRunning;
    [HideInInspector]public bool shootInput;
   [HideInInspector] public bool defendInput;
   [HideInInspector] public bool brakeInput;
    [HideInInspector] public bool isDriving;
    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool canControl = true;
    [HideInInspector] public bool isDefending;
    [HideInInspector] public bool isDefendingCooldown;
    [HideInInspector] public bool wasFacingRightBeforeDriving;
    [HideInInspector] public PlayerHealth playerHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerHealth =GetComponent<PlayerHealth>();
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;

        animator = GetComponentInChildren<Animator>();
        stateMachine = new StateMachine();
        stateMachine.SetState(new IdleState(this));
    }

    void Update()
    {
        if (isDead) return;

        ReadMobileInput();

        HandleCombatToggle();
        HandleVehicleToggle();
        HandleDefend();

        HandleDefendCooldown();
        stateMachine.Update();

        if (!isDriving && canControl && input.x != 0)
            CheckAndFlip(input.x);

        if (WantsToShoot() && inCombat && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void FixedUpdate()
    {
        if (isDriving || !canControl) return;

        Vector3 targetVelocity = Vector3.zero;

        if (currentSpeed > 0 && input.magnitude > 0)
        {
            targetVelocity = new Vector3(input.x, 0, 0).normalized * currentSpeed;
        }

        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);
    }

    // ========================== Input Handling ==========================

    private void ReadMobileInput()
    {
        input = MobileInput.Instance.GetMoveInput();
        isRunning = MobileInput.Instance.isRunning;
        shootInput = MobileInput.Instance.isShooting;
        defendInput = MobileInput.Instance.isDefending;
        brakeInput = MobileInput.Instance.isBraking;
    }

    private void HandleCombatToggle()
    {
        if (MobileInput.Instance.toggleCombat && !isDriving && canControl)
        {
            SetCombatMode(!inCombat);
            stateMachine.SetState(inCombat ? new CombatMovementState(this) : new IdleState(this));
        }
    }

    private void HandleVehicleToggle()
    {
        if (MobileInput.Instance.wantsToDrive && currentVehicle != null)
        {
            if (!isDriving)
                stateMachine.SetState(new DrivingState(this, currentVehicle));
            else
                currentVehicle.ExitVehicle();
        }
    }

    private void HandleDefend()
    {
        if (!isDriving && canControl && defendInput && CanDefend())
        {
            stateMachine.SetState(new DefendState(this));
        }
    }

    private void HandleDefendCooldown()
    {
        if (isDefendingCooldown)
        {
            defendTimer -= Time.deltaTime;

            // ✅ Update cooldown text here
            if (defendCooldownText != null)
                defendCooldownText.text = Mathf.Ceil(defendTimer).ToString("F0") + "s";

            if (defendTimer <= 0f)
            {
                isDefendingCooldown = false;

                // ✅ Clear the text when cooldown is over
                if (defendCooldownText != null)
                    defendCooldownText.text = "";
            }
        }
    }

    private void CheckAndFlip(float inputX)
    {
        if (inputX > 0 && !facingRight || inputX < 0 && facingRight)
            Flip();
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // ========================== Utility Methods ==========================

    public void SetAnimSpeed(float value) => animator?.SetFloat("speed", value);

    public void Move(float speed) => currentSpeed = speed;

    public void UpdateLocomotionAnimation()
    {
        float animSpeed = IsMoving() ? (IsRunning() ? 1f : 0.5f) : 0f;
        animator.SetFloat("speed", animSpeed);
    }

    public bool IsMoving() => Mathf.Abs(input.x) > 0.1f;
    public bool IsRunning() => isRunning && IsMoving();
    public bool WantsToShoot() => shootInput;
    public bool WantsToDefend() => defendInput;
    public bool WantsToBrake() => brakeInput;
    public bool IsInCombat() => inCombat;
    public bool IsDefending() => isDefending;
    public bool CanDefend() => !isDefendingCooldown && !isDefending;
    public Vector2 GetInput() => input;


    // ========================== Input System Methods ==========================

    public void OnMove(InputAction.CallbackContext ctx) => input = ctx.ReadValue<Vector2>();
    public void OnRun(InputAction.CallbackContext ctx) => isRunning = ctx.performed || ctx.started;
    public void OnShoot(InputAction.CallbackContext ctx) => shootInput = ctx.performed || ctx.started;
    public void OnDefend(InputAction.CallbackContext ctx) => defendInput = ctx.performed || ctx.started;
    public void OnBrake(InputAction.CallbackContext ctx) => brakeInput = ctx.performed || ctx.started;

    public void OnToggleCombat(InputAction.CallbackContext ctx)
    {
        if (isDriving || !canControl || !ctx.performed) return;
        SetCombatMode(!inCombat);
        stateMachine.SetState(inCombat ? new CombatMovementState(this) : new IdleState(this));
    }

    public void OnDrive(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && currentVehicle != null)
        {
            if (!isDriving)
                stateMachine.SetState(new DrivingState(this, currentVehicle));
            else
                currentVehicle.ExitVehicle();
        }
    }

    // ========================== Combat ==========================

    public void SetCombatMode(bool value)
    {
        inCombat = value;
        animator.SetBool("isInCombat", value);
    }

    public void SetDefend(bool value)
    {
        isDefending = value;
        if (forceField != null) forceField.SetActive(value);

        if (!value)
        {
            isDefendingCooldown = true;
            defendTimer = 5f; // reuse default
        }
    }

    public void Shoot()
    {
      Vector3 shootDir = facingRight ? Vector3.right : Vector3.left;
      Vector3 offset = shootDir * 0.1f; // 10cm forward
      Vector3 spawnPos = shootPoint.position + offset;
      Quaternion spawnRot = Quaternion.Euler(0f, 90f, 0f);
      GameObject bullet = BulletPool.Instance.GetBullet(spawnPos, spawnRot);
      float bulletSpeed = 10f + Vector3.Dot(currentVelocity, shootDir);

        if (bullet.TryGetComponent<Rigidbody>(out var bulletRb))
        {
            bulletRb.linearVelocity = Vector3.zero;
            bulletRb.angularVelocity = Vector3.zero;
            bulletRb.linearVelocity = shootDir * bulletSpeed;
        }

        Debug.DrawRay(shootPoint.position, shootDir * 2f, Color.red, 1f);
    }
    // ========================== Vehicle ==========================

    public void EnterVehicle(VehicleControllerWithWheels vehicle)
    {
        currentVehicle = vehicle;
        isDriving = true;
        canControl = false;

        if (vehicle.seatPoint != null)
        {
            transform.SetParent(vehicle.seatPoint);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            wasFacingRightBeforeDriving = facingRight;
            if (!facingRight)
                FlipToRight();

            PlayerHipTransform.localPosition = vehicle.seatPoint.localPosition;
            PlayerHipTransform.localRotation = Quaternion.identity;
        }

        rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;
        animator.SetBool("isDriving", true);
        SetAnimSpeed(0f);

    }

    public void ExitVehicle(Transform exitPoint)
    {
        isDriving = false;
        currentVehicle = null;
        canControl = true;

        transform.SetParent(null);
        if (exitPoint != null)
        {
            transform.position = exitPoint.position;
            transform.rotation = Quaternion.identity;

            facingRight = wasFacingRightBeforeDriving;
            Vector3 scale = transform.localScale;
            scale.x = facingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        rb.isKinematic = false;
        GetComponent<Collider>().enabled = true;
        animator.SetBool("isDriving", false);

        stateMachine.SetState(new IdleState(this));
  
    }

    private void FlipToRight()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
}
