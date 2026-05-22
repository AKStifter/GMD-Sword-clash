using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaEventManager : MonoBehaviour
{

    public List<MonoBehaviour> events;

    public float startTime;
    private IArenaEvent currentEvent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return new WaitForSeconds(startTime);
        Debug.Log("Event");
        currentEvent = events[Random.Range(0, events.Count)] as IArenaEvent;
        currentEvent.StartEvent();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
