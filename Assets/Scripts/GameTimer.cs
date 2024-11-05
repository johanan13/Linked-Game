using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI startText;
    [SerializeField] private Image startBackground; // Reference to the background image
    [SerializeField] private GameObject pauseMenu; // Reference to the pause menu panel

    private CanvasGroup startTextCanvasGroup; // CanvasGroup for start text
    private CanvasGroup startBackgroundCanvasGroup; // CanvasGroup for background

    private float timer;
    private bool isTiming;
    private bool isPaused;

    private void Start()
    {
        // Get the CanvasGroup components
        startTextCanvasGroup = startText.GetComponent<CanvasGroup>();
        startBackgroundCanvasGroup = startBackground.GetComponent<CanvasGroup>();

        // Set initial alpha values to 1 (fully visible)
        startTextCanvasGroup.alpha = 1;
        startBackgroundCanvasGroup.alpha = 1;

        // Enable the start text and background initially
        startText.gameObject.SetActive(true);
        startBackground.gameObject.SetActive(true);
        timerText.gameObject.SetActive(false);
        pauseMenu.SetActive(false); // Hide the pause menu initially

        // Begin the start countdown
        StartCoroutine(ShowStartText());
    }

    private IEnumerator ShowStartText()
    {
        yield return new WaitForSeconds(1);

        // Fade out "Start" text and background over 1 second
        float fadeDuration = 1.0f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            startTextCanvasGroup.alpha = alpha;
            startBackgroundCanvasGroup.alpha = alpha;
            yield return null; // Wait until the next frame
        }

        // Ensure they are fully hidden at the end
        startTextCanvasGroup.alpha = 0;
        startBackgroundCanvasGroup.alpha = 0;

        startText.gameObject.SetActive(false);
        startBackground.gameObject.SetActive(false);
        timerText.gameObject.SetActive(true); // Show the timer text
        isTiming = true; // Start the timer
    }

    private void Update()
    {
        if (isTiming && !isPaused)
        {
            timer += Time.deltaTime; // Increment timer by delta time each frame
            timerText.text = "Time: " + FormatTime(timer); // Update timer text
        }

        // Check for pause input (e.g., Escape key or a designated button)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        return $"{minutes:00}:{seconds:00}"; // Format as MM:SS
    }

     public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1; // Stop or resume the game
        if (isPaused) {
            GameBGMManager.instance.PauseBGM();
        }
        else {
            GameBGMManager.instance.UnPauseBGM();
        }
    }


    public float GetElapsedTime()
    {
        return timer; // Return the current timer value
    }

    public void StopTimer()
    {
        isTiming = false;
    }
}
