using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    public IZombieState currentState;
    public ZombieIdleState idleState;
    public ZombieWalkState walkState;
    public ZombieAttackState attackState;
    public ZombieRunState runState;

    public Transform player;
    public bool isChasing = false;
    public bool isDead = false;
    public float walkSpeed = 3f;
    public float runSpeed = 5f;
    public PlayerController playerController;

    public float detectionRange = 10f;
    public float attackRange = 2f;

    [HideInInspector]
    public Animator animator;
    [HideInInspector]public Rigidbody rb;
    public ZombieHealth zombieHealth;
    public float pushBackForce = 5f;

    private void Awake()
    {
        idleState = new ZombieIdleState(this);
        walkState = new ZombieWalkState(this);
        attackState = new ZombieAttackState(this);
        runState = new ZombieRunState(this);

        zombieHealth = GetComponent<ZombieHealth>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        currentState = idleState;
    }

    private void Update()
    {
        if (isDead) return;
        currentState.UpdateState();
    }

    public void TransitionToState(IZombieState newState)
    {
        currentState = newState;
        SetAnimationState(false,false,false);
    }

    public bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, player.position) < attackRange;
    }
    public void DealDamage()
    {
        if (isDead || playerController == null || playerController.isDead)
            return;
        if (IsPlayerInRange())
        {
            player.GetComponent<PlayerHealth>().TakeDamage(10);
        }
    }

    public void SetAnimationState(bool idle, bool walking, bool running)
    {
        animator?.SetBool("IsIdle", idle);
        animator?.SetBool("IsWalking", walking);
        animator?.SetBool("IsRunning", running);
    }
        public void SetAttackAnimation()
        {
            animator?.ResetTrigger("Attack"); 
           animator?.SetTrigger("Attack");
        }
        public void StopAttackAnimation()
        {
            animator?.ResetTrigger("Attack"); 
        }

        public void TriggerDeathAnimation()
        {
            animator?.SetTrigger("Die");
        }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")){
            playerController = other.gameObject.GetComponent<PlayerController>();
        }
        if(other.CompareTag("Shield")){
            Vector3 pushDirection = (transform.position - other.transform.position).normalized;
            // Optional: add a small upward force to prevent ground clipping
            pushDirection.y = 0.1f;

            // Apply push using Rigidbody
            if (rb != null)
            {
                rb.AddForce(pushDirection * pushBackForce, ForceMode.Impulse);
            }
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Shield")){
            Vector3 pushDirection = (transform.position - other.transform.position).normalized;
            // Optional: add a small upward force to prevent ground clipping
            pushDirection.y = 0.1f;

            // Apply push using Rigidbody
            if (rb != null)
            {
                rb.AddForce(pushDirection * 0.5f, ForceMode.Impulse);
            }
        } 
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

}
