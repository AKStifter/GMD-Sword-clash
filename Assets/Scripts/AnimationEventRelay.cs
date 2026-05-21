using UnityEngine;

public class AnimationEventRelay : MonoBehaviour
{
    private IController receiver;

    private void Awake()
    {
        receiver = GetComponent<IController>();
    }

    public void EnableDamage()
    {
        receiver?.EnableWeaponDamage();
    }

    public void DisableDamage()
    {
        receiver?.DisableWeaponDamage();
    }
}