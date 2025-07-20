using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneManagerMobile : MonoBehaviour
{
    public Image fadeImage;         // Assign a full-screen black UI Image
    public float fadeDuration = 1f; // Fade in/out time
    public GameObject pausePanel;   // Reference to the pause panel UI

    private bool isPaused = false;

    void Start()
    {
        if (fadeImage != null)
            StartCoroutine(FadeIn());

        // Hide the pause panel initially
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    public void StartGame(string sceneName)
    {
        // Temporarily unpause the game
        Time.timeScale = 1f;
        StartCoroutine(FadeOutAndLoadSceneAsync(sceneName));
    }

    public void RestartGame()
    {
        // Temporarily unpause the game
        Time.timeScale = 1f;
        string currentScene = SceneManager.GetActiveScene().name;
        StartCoroutine(FadeOutAndLoadSceneAsync(currentScene));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        // Pause the game
        Time.timeScale = 0f;
        isPaused = true;

        // Show the pause panel
        if (pausePanel != null)
            pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        // Resume the game
        Time.timeScale = 1f;
        isPaused = false;

        // Hide the pause panel
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    IEnumerator FadeIn()
    {
        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;
        for (float t = fadeDuration; t >= 0; t -= Time.deltaTime)
        {
            color.a = t / fadeDuration;
            fadeImage.color = color;
            yield return null;
        }
        fadeImage.color = new Color(color.r, color.g, color.b, 0f);
        fadeImage.gameObject.SetActive(false);
    }

    IEnumerator FadeOutAndLoadSceneAsync(string sceneName)
    {
        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;

        // Fade to black
        for (float t = 0; t <= fadeDuration; t += Time.deltaTime)
        {
            color.a = t / fadeDuration;
            fadeImage.color = color;
            yield return null;
        }

        // Start async loading
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // Wait until the scene is almost loaded
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        // Optional delay (e.g., 0.5 sec before switching)
        yield return new WaitForSeconds(0.5f);

        // Activate the loaded scene
        asyncLoad.allowSceneActivation = true;
    }
}
