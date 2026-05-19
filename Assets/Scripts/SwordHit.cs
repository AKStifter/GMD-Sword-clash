using UnityEngine;

public class SwordHit : MonoBehaviour
{
    public float damage = 25f;
    private bool canDealDamage;
     public void EnableHitbox()
    {
        canDealDamage = true;
    }

    public void DisableHitbox()
    {
        canDealDamage = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        // if( !canDealDamage) return;
        
        HealthSystem health = other.GetComponent<HealthSystem>();

        if (health != null)
        {
            health.TakeDamage(damage);
        }
    }
}
