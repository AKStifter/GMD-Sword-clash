using UnityEngine;

public class SwordHit : MonoBehaviour
{
    public float damage = 25f;
    private bool canDealDamage = false;
     public void SetDamageActive(bool isActive)
    {
        canDealDamage = isActive;
    }
    private void OnTriggerEnter(Collider other)
    {
        if( !canDealDamage) return;
        
        HealthSystem health = other.GetComponent<HealthSystem>();

        if (health != null)
        {
            health.TakeDamage(damage);
        }
    }
}
