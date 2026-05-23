using UnityEngine;

public class DiscoEvent : MonoBehaviour, IArenaEvent
{
    public GameObject DiscoBall;
    public GameObject Camera;
    private CameraController cameraController;

    void Start()
    {
        cameraController = Camera.GetComponent<CameraController>();  
    }

    public void StartEvent()
    {
        DiscoBall.SetActive(true);
        AudioManager.Instance.ChangeMusic(SoundType.DiscoEventMusic);
        cameraController.ShakeCamera(60f, 0.1f);
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
