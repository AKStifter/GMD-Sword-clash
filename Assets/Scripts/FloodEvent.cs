using UnityEngine;

public class FloodEvent : MonoBehaviour, IArenaEvent
{

    public GameObject waterVolume;  // Assign your water object in inspector
    public float targetHeight = 1f;  // The height you want to reach
    public float riseSpeed = 1f; 

    private bool isFlooding = false;
    public void StartEvent()
    {
        //Play the voice recording for the event start
        waterVolume.SetActive(true);
        isFlooding = true;
    }

    public void StopEvent()
    {
        isFlooding = false;
    }

    public void UpdateEvent()
    {
        throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFlooding)
        {
             Transform waterTransform = waterVolume.transform;

            // Gradually move water upward
            Vector3 pos = waterTransform.position;
            pos.y = Mathf.MoveTowards(pos.y, targetHeight, riseSpeed * Time.deltaTime);
            waterTransform.position = pos;
        }
    }
}
