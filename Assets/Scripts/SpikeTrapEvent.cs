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
        foreach (SpikeTrapGate pit in pits)
        {
            pit.ActivateTrap();
        }
    }

    public void StopEvent()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateEvent()
    {
        throw new System.NotImplementedException();
    }
}
