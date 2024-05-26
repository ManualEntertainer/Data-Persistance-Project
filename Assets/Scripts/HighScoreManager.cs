using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class HighScoreData
{
    public int HighScore;
    public string PlayerName;
}

public class HighScoreManager : MonoBehaviour
{

    private string _filePath;

    private void Awake()
    {
        _filePath = Path.Combine(Application.persistentDataPath, "highscore.json");
    }

    public void SaveHighScore(int score, string playerName)
    {
        HighScoreData data = new HighScoreData();
        data.HighScore = score;
        data.PlayerName = playerName;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(_filePath, json);
    }

    public HighScoreData LoadHighScore()
    {
        if (File.Exists(_filePath))
        {
            string json = File.ReadAllText(_filePath);
            return JsonUtility.FromJson<HighScoreData>(json);
        }
        else
        {
            // Return a default high score data if no file exists
            return new HighScoreData { HighScore = 0, PlayerName = "None" };
        }
    }
}
