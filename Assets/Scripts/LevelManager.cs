using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject levelFinishedPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject inGamePanel;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject finishLine;

    private void Start()
    {
        Time.timeScale = 1f;

        // ApplySafeArea(levelFinishedPanel);
        // ApplySafeArea(gameOverPanel);
        ApplySafeArea(inGamePanel);
    }

    private void ApplySafeArea(GameObject panel)
    {
        if (panel == null) return;

        RectTransform rt = panel.GetComponent<RectTransform>();
        if (rt == null) return;

        Rect safeArea = Screen.safeArea;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
    }

    public void UpdateProgress(float t)
    {
        progressSlider.value = Mathf.Clamp01(t);
    }

    public void OnLevelFinish()
    {
        StartCoroutine(OnLevelFinishRoutine());
    }

    IEnumerator OnLevelFinishRoutine()
    {
        playerController.enabled = false;
        finishLine.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 0.9f;
        inGamePanel.SetActive(false);
        levelFinishedPanel.SetActive(true);
    }

    public void OnGameOver()
    {
        StartCoroutine(OnGameOverRoutine());
    }

    IEnumerator OnGameOverRoutine()
    {
        playerController.enabled = false;
        finishLine.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 0.9f;
        inGamePanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void OnNextPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnRetryPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }    
}
