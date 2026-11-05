using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Objective { Score, ReachGoal, Survive, CoinsThenGoal }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] string title = "Level 1 · Collect 10 coins";
    [SerializeField] Objective objective = Objective.Score;
    [SerializeField] int targetScore = 10, lives = 3;
    [SerializeField] float timeLimit = 0f;   // 0 = no timer
    [SerializeField] float respawnDelay = 0.6f, spawnProtection = 1.5f;
    [SerializeField] Health player;
    [SerializeField] Transform respawnPoint;
    [SerializeField] TMP_Text scoreText, livesText, titleText, timerText, winText;
    [SerializeField] GameObject winPanel, losePanel, nextButton;
    int score;
    float timeLeft;
    bool over;

    void Awake() => Instance = this;

    void Start()
    {
        // "collect all coins" levels count the coins placed in the scene
        if (objective == Objective.CoinsThenGoal) targetScore = FindObjectsByType<Coin>().Length;
        timeLeft = timeLimit;
        titleText.text = title;
        UpdateUI();
    }

    void Update()
    {
        if (over || timeLimit <= 0f) return;
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            if (objective == Objective.Survive) Win();
            else Lose();
        }
        timerText.text = $"Time: {Mathf.CeilToInt(timeLeft)}";
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
        if (objective == Objective.Score && score >= targetScore) Win();
    }

    public void ReachGoal()
    {
        if (objective == Objective.ReachGoal) Win();
        else if (objective == Objective.CoinsThenGoal)
        {
            if (score >= targetScore) Win();
            else titleText.text = "Collect every coin first!";
        }
    }

    public void UpdateUI()
    {
        scoreText.text = objective == Objective.Score ? $"Score: {score} / {targetScore}"
                       : objective == Objective.CoinsThenGoal ? $"Coins: {score} / {targetScore}" : "";
        livesText.text = $"Lives: {lives}   Health: {Mathf.Max(player.Current, 0)} / {player.Max}";
        timerText.text = timeLimit > 0f ? $"Time: {Mathf.CeilToInt(timeLeft)}" : "";
    }

    public void PlayerDied()
    {
        if (over) return;
        lives--;
        UpdateUI();
        if (lives <= 0) Lose();
        else StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        // wait a beat, so the knockback queued by the hit doesn't land on the respawned player
        yield return new WaitForSeconds(respawnDelay);
        Rigidbody body = player.GetComponent<Rigidbody>();
        body.position = respawnPoint.position;
        body.linearVelocity = Vector3.zero;
        player.Restore(spawnProtection);
    }

    void Win()
    {
        if (over) return;
        over = true;
        bool last = SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1;
        winText.text = last ? "You beat all 5 levels!" : "Level complete!";
        nextButton.SetActive(!last);
        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void Lose()
    {
        if (over) return;
        over = true;
        losePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
