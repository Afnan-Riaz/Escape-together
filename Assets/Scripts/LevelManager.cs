using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public TextMeshProUGUI levelMessage;
    public TextMeshProUGUI timeLeftText;
    public PlayerInteraction player1;
    public PlayerInteraction player2;
    public GameObject pauseMenu;
    private bool isPaused = false;
    public GameObject levelCompleteMenu;
    public GameObject gameoverMenu;

    public InputActionAsset uiControlsAsset;
    private InputAction pauseAction;

    public float levelTime;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player1")
        {
            levelMessage.text = "Player 1 has escaped.";
            player1.hasEscaped = true;
        }
        if (other.gameObject.name == "Player2")
        {
            levelMessage.text = "Player 2 has escaped.";
            player2.hasEscaped = true;
        }

    }
    void Awake()
    {
        var map = uiControlsAsset.FindActionMap("UI");
        pauseAction = map.FindAction("Pause");

        pauseAction.Enable();
        pauseAction.performed += OnPause;

        timeLeftText.text = FormatTime(levelTime);
        Time.timeScale = 1f;
    }

    private bool levelCompleted = false;

    void Update()
    {
        if (!levelCompleted && player1.hasEscaped && player2.hasEscaped)
        {
            levelCompleted = true; // prevent repeated calls
            StartCoroutine(HandleLevelComplete());
        }

        if (!levelCompleted)
        {
            levelTime -= Time.deltaTime;
            timeLeftText.text = FormatTime(levelTime);

            if (levelTime < 0)
            {
                GameOver();
            }
        }
    }
    IEnumerator HandleLevelComplete()
    {
        yield return new WaitForSeconds(1f); 
        levelCompleteMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if(isPaused)
        {
            Continue();
        }
        else
        {
            Pause();
        }
        isPaused = !isPaused;
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Continue()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void ExitMainMenu()
    {

        SceneManager.LoadSceneAsync("MainMenu");
        Time.timeScale = 1;

    }

    public void Restart()
    {

        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void GameOver()
    {
        timeLeftText.text = "0:00";
        gameoverMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void NextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Time.timeScale = 1;
            SceneManager.LoadSceneAsync(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more levels. You're at the last one!");
        }
    }

    public string FormatTime(float totalSeconds)
    {
        int minutes = Mathf.FloorToInt(totalSeconds / 60f);
        int seconds = Mathf.FloorToInt(totalSeconds % 60f);
        return string.Format("{0}:{1:00}", minutes, seconds);
    }

}

