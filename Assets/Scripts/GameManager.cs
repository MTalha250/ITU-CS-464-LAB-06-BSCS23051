using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] int targetScore = 10;
    [SerializeField] Health player;
    [SerializeField] TMP_Text scoreText, livesText;
    [SerializeField] GameObject winPanel, losePanel;
    int score;

    void Awake() => Instance = this;
    void Start() => UpdateUI();

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
        if (score >= targetScore) Win();
    }

    public void UpdateUI()
    {
        scoreText.text = $"Score: {score} / {targetScore}";
        livesText.text = $"Health: {Mathf.Max(player.Current, 0)} / {player.Max}";
    }

    public void PlayerDied() => Lose();

    void Win()
    {
        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void Lose()
    {
        losePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
