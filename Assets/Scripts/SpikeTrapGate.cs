using UnityEngine;

public class SpikeTrapGate : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ActivateTrap()
    {
        animator.SetTrigger("Activate");
    }
}
