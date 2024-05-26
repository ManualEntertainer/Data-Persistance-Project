using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;

    private HighScoreManager highScoreManager;
    private PlayerNameManager playerNameManager;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize HighScoreManager
        highScoreManager = FindObjectOfType<HighScoreManager>();
        if (highScoreManager == null)
        {
            Debug.LogError("HighScoreManager not found!");
            return;
        }

        // Initialize PlayerNameManager
        playerNameManager = FindObjectOfType<PlayerNameManager>();
        if (playerNameManager == null)
        {
            Debug.LogError("PlayerNameManager not found!");
            return;
        }

        if (string.IsNullOrEmpty(PlayerNameManager.playerName))
        {
            Debug.LogError("PlayerName is null or empty!");
            return;
        }

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        UpdateHighScore(m_Points, PlayerNameManager.playerName);

        HighScoreData highScoreData = highScoreManager.LoadHighScore();
        Debug.Log("High Score: " + highScoreData.HighScore + " by " + highScoreData.PlayerName);
        HighScoreText.text = "Best score : " + highScoreData.PlayerName + " : " + highScoreData.HighScore;
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = PlayerNameManager.playerName + $" : {m_Points}";
    }

    public void GameOver()
    {
        UpdateHighScore(m_Points, PlayerNameManager.playerName);
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    public void UpdateHighScore(int newScore, string playerName)
    {
        if (highScoreManager == null)
        {
            Debug.LogError("HighScoreManager is not initialized.");
            return;
        }

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogError("PlayerName is null or empty!");
            return;
        }

        HighScoreData currentHighScore = highScoreManager.LoadHighScore();

        if (newScore > currentHighScore.HighScore)
        {
            highScoreManager.SaveHighScore(newScore, playerName);
            Debug.Log("New high score: " + newScore + " by " + playerName);
        }
    }
}
