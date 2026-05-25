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
        AudioManager.Instance.Play(SoundType.DiscoDeclaration);
        
        DiscoBall.SetActive(true);
        AudioManager.Instance.ChangeMusic(SoundType.DiscoEventMusic);
        cameraController.ShakeCamera(60f, 0.1f);
    }

}
