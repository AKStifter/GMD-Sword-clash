using UnityEngine;

public class SpikeTrapEvent : MonoBehaviour, IArenaEvent
{
    private SpikeTrapGate[] pits;

    private void Awake()
    {
        pits = FindObjectsByType<SpikeTrapGate>();
    }
    public void StartEvent()
    {
        AudioManager.Instance.Play(SoundType.SpikesDeclaraion);
        
        foreach (SpikeTrapGate pit in pits)
        {
            pit.ActivateTrap();
        }
    }
}
