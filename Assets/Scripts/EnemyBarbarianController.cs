using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBarbarianController : MonoBehaviour, IController, ICombat, IMatch
{
public bool isBlocking{ get; private set;}
    public Transform player;
    public float attackRange;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private bool isAttacking;
    private HealthSystem playerHealth;
    private SwordHit currentHitbox;
    [Header("Voice Lines")]
    public SoundType[] voiceLines;
    private Coroutine randomSpeaking;
    void Start()
    {
        enabled = false;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentHitbox = GetComponentInChildren<SwordHit>(true);

        if (player != null)
        {
            playerHealth = player.GetComponent<HealthSystem>();
            if (playerHealth != null)
            {
                playerHealth.OnDeath += HandlePlayerDeath;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
            return;

        // Distance to player
        float distance =
            Vector3.Distance(transform.position, player.position);

        // Attack if close enough
        if (distance <= attackRange)
        {
            Attack();
        }
        else
        {
            if (!isAttacking)
            {
                ChasePlayer();
            }
        }

        // Movement animation
        bool isMoving =
            navMeshAgent.velocity.magnitude > 0.1f;

        animator.SetBool("isRunning", isMoving);
    }

    private void Attack()
    {
        if (isAttacking)
            return;

        isAttacking = true;
        AudioManager.Instance.Play(SoundType.AttackSwing);

        // Stop moving
        navMeshAgent.isStopped = true;

        // Face player
        Vector3 lookDirection =
            player.position - transform.position;

        lookDirection.y = 0;

        // Play attack animation
        int randomAttack = Random.Range(1, 4);

        animator.SetInteger("Attack", randomAttack);

        // Reset attack after delay
        Invoke(nameof(ResetAttack), 1.5f);
    }

    private void ChasePlayer()
    {

        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(player.position);

        animator.SetInteger("Attack", 0);
    }


    private void ResetAttack()
    {
        isAttacking = false;
    }

    public void DisableControl()
    {
        enabled = false;
    }

    public void EnableWeaponDamage()
    {
        currentHitbox?.SetDamageActive(true);
    }

    public void DisableWeaponDamage()
    {
        currentHitbox.SetDamageActive(false);
    }

    private void HandlePlayerDeath()
    {
        CancelInvoke();
        if (randomSpeaking != null)
        {
            StopCoroutine(randomSpeaking);
        }

        isAttacking = false;

        navMeshAgent.isStopped = true;
        navMeshAgent.velocity = Vector3.zero;
        animator.SetBool("isRunning", false);

        enabled = false;
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDeath -= HandlePlayerDeath;
        }       
        
        if (randomSpeaking != null)
        {
            StopCoroutine(randomSpeaking);
        } 
    }

    void OnTriggerEnter(Collider other)
    {
        navMeshAgent.speed = 2f;
    }

    public void MatchStart()
    {
        enabled = true;

        randomSpeaking = StartCoroutine(RandomSpeaking());
    }
    IEnumerator RandomSpeaking()
    {
        while (enabled)
        {

            yield return new WaitForSeconds(7f);

            if (!enabled || player == null)
                yield break;

            if (GetComponent<HealthSystem>().isDead)
                yield break;

            if (voiceLines.Length > 0)
            {
                SoundType randomLine =
                    voiceLines[Random.Range(0, voiceLines.Length)];

                AudioManager.Instance.Play(randomLine);
            }
        }
    }

}
