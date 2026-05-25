using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviour
{

    public GameObject Player;
    public GameObject Enemy;
    public MonoBehaviour enemyController;
    public ArenaEventManager EventManager;

    private bool PlayerDefeat;
    private bool PlayerVictory;
    private bool SequenceStarted;

    private IMatch EnemyScript;
    private float recordingLength;
    private void Awake()
    {
        EnemyScript = enemyController as IMatch;
    }

    IEnumerator Start()
    {

        recordingLength = AudioManager.Instance.Play(SoundType.BeggininingAnnouncement);
        yield return new WaitForSeconds(recordingLength);

        AudioManager.Instance.ChangeMusic(SoundType.Battle_Music);

        EventManager.MatchStart();
        Player.GetComponent<PlayerController>().MatchStart();
        EnemyScript.MatchStart();        
        
    }

    void Update()
    {

        if (!SequenceStarted)
        {
            
            PlayerVictory = Enemy.GetComponent<HealthSystem>().isDead;
            PlayerDefeat = Player.GetComponent<HealthSystem>().isDead;
            if (PlayerDefeat)
            {
                SequenceStarted = true;
                recordingLength = AudioManager.Instance.Play(SoundType.Defeat);

                StartCoroutine(RestartScene(recordingLength));
            }
            else if (PlayerVictory)
            {
                SequenceStarted = true;

                AudioManager.Instance.StopMusic();
                EventManager.StopEvents();
                recordingLength = AudioManager.Instance.Play(SoundType.Victory);
                StartCoroutine(NextScene(recordingLength));
            }
        }
    }

    IEnumerator RestartScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime + 5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator NextScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime + 5f);

        int currentScene = SceneManager.GetActiveScene().buildIndex;

        if (currentScene + 1 >= SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(0); // Main menu scene
        }
        else
        {
            SceneManager.LoadScene(currentScene + 1);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
