using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject container;
    private InputAction PauseAction;

    void Start()
    {
        PauseAction = InputSystem.actions.FindAction("Pause");
        container.SetActive(false);
    }
    void Update()
    {
        if (PauseAction.WasPressedThisFrame())
        {
            container.SetActive(true);
            Time.timeScale = 0;
        }         
    }

    public void ResumeButton()
    {
        container.SetActive(false);
            Time.timeScale = 1;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
