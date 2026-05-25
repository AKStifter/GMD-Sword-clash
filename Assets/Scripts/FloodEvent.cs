using UnityEngine;

public class FloodEvent : MonoBehaviour, IArenaEvent
{

    public GameObject waterVolume;  // Assign your water object in inspector
    public float targetHeight = 1f;  // The height you want to reach
    public float riseSpeed = 1f; 

    private bool isFlooding = false;
    public void StartEvent()
    {
        AudioManager.Instance.Play(SoundType.FloodDeclaration);
        waterVolume.SetActive(true);
        isFlooding = true;
        AudioManager.Instance.Play(SoundType.WaterGushing);

    }

    public void StopEvent()
    {
        isFlooding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFlooding)
        {
            Transform waterTransform = waterVolume.transform;

            // Gradually moves water upward
            Vector3 pos = waterTransform.position;
            pos.y = Mathf.MoveTowards(pos.y, targetHeight, riseSpeed * Time.deltaTime);
            waterTransform.position = pos;
        }
    }
}
