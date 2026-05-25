using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaEventManager : MonoBehaviour, IMatch
{

    public List<MonoBehaviour> events;
    public float startTime;
    private IArenaEvent currentEvent;
    private bool matchStarted = false;
    private Coroutine eventCoroutine;

    public void MatchStart()
    {
        if (matchStarted) return;

        matchStarted = true;

        eventCoroutine = StartCoroutine(StartEvent());
    }

    public void StopEvents()
    {
        matchStarted = false;

        if (eventCoroutine != null)
        {
            StopCoroutine(eventCoroutine);
            eventCoroutine = null;
        }

        Debug.Log("Arena events stopped");
    }
    IEnumerator StartEvent()
    {
        if (matchStarted)
        {
           yield return new WaitForSeconds(startTime);
        Debug.Log("Event");
        currentEvent = events[Random.Range(0, events.Count)] as IArenaEvent;
        currentEvent.StartEvent(); 
        }
    }
}
