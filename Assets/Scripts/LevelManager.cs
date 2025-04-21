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
    public GameObject levelCompleteMenu;
    public GameObject gameoverMenu;

    private PlayerInput inputActions;

    public float levelTime;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player1"))
        {
            levelMessage.text = "Player 1 has escaped.";
            player1.hasEscaped = true;
        }
        if (other.gameObject.CompareTag("Player2"))
        {
            levelMessage.text = "Player 2 has escaped.";
            player2.hasEscaped = true;
        }

    }
    void Awake()
    {
        inputActions = GetComponent<PlayerInput>();
        inputActions.actions["Pause"].performed += OnPause;

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
        yield return new WaitForSeconds(1f); // wait 1 real-time second
        levelCompleteMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        Pause();
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

        // Optional: Check if next scene index is valid
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Time.timeScale = 1;
            SceneManager.LoadSceneAsync(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more levels. You're at the last one!");
            // You can loop back to main menu or restart:
            // SceneManager.LoadSceneAsync(0);
        }
    }

    public string FormatTime(float totalSeconds)
    {
        int minutes = Mathf.FloorToInt(totalSeconds / 60f);
        int seconds = Mathf.FloorToInt(totalSeconds % 60f);
        return string.Format("{0}:{1:00}", minutes, seconds);
    }

}

