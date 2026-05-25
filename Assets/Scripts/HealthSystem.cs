using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    public float maxHealth = 100f;
    public float currentHealth;
    public float invincibilityTime = 1f;
    public bool isDead => currentHealth <= 0f;

    public event Action OnDeath;

    private bool isInvincible;
    private Animator animator;
    private IController controller;
    private ICombat combat;
    [SerializeField]
    private HealthBarUI healthBar;
    
    [SerializeField]
    private EnemyHealthBar FloatingHealthBar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
        else
        {
            FloatingHealthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
        
    }
    private void Awake()
    {
        FloatingHealthBar = GetComponentInChildren<EnemyHealthBar>();
        controller = GetComponent<IController>();
        combat = GetComponent<ICombat>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    // public void SetHealth(float healthChange)
    // {
    //     currentHealth += healthChange;
    //     currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

    //     healthBar.setHealth(currentHealth);
    // }

    public void TakeDamage(float damage)
    {
        if (isInvincible) return;
        
        if (isDead) return;

        if (combat != null && combat.isBlocking && damage != 999)
        {
            AudioManager.Instance.Play(SoundType.Hit_Steel);
            return;
        }
        AudioManager.Instance.Play(SoundType.Hit_Flesh);

        currentHealth -= damage;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null)
        {
            healthBar.setHealth(currentHealth);
        }
        else
        {
            FloatingHealthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
        
        Debug.Log($"{gameObject.name} took {damage} damage");
        if (currentHealth <= 0f)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityFrames());
        }
    }
    private System.Collections.IEnumerator InvincibilityFrames()
    {
        isInvincible = true;

        yield return new WaitForSeconds(invincibilityTime);

        isInvincible = false;
    }
    private void Die()
    {
        currentHealth = 0f;
        
        OnDeath?.Invoke();

        int randomDeath = UnityEngine.Random.Range(1, 6);

        animator.SetBool("isRunning", false);
        animator.SetInteger("DeathAnim", randomDeath);
        animator.SetTrigger("Die");        
        controller.DisableControl();

        Debug.Log($"{gameObject.name} died");

        // Example behaviour:
        // disable movement, play animation, destroy, etc.
    }
}
