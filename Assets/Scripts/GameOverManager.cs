using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public static string ElapsedTime { get; set; }

    [SerializeField] private TMP_Text congratsText;
    [SerializeField] private TMP_Text timeText;
    public GameObject deadGirl;
    public GameObject deadBoy;
    public GameObject jumpGirl;
    public GameObject jumpBoy;

    private void Start()
    {
        if (!string.IsNullOrEmpty(ElapsedTime))
        {
            HandleGameWon(ElapsedTime);
        }
        else
        {
            HandleGameOver();
        }
    }

    private void HandleGameWon(string elapsedTime)
{
    jumpGirl.SetActive(true);
    jumpBoy.SetActive(true);
    congratsText.text = "Congratulations";
    timeText.text = "Time: " + elapsedTime;
    Debug.Log("Game Over! You Won!");

    // Combine player names and elapsed time in a sortable format
    string player1Name = PlayerData.instance.playerName[0];
    string player2Name = PlayerData.instance.playerName[1];
    string combinedScore = $"{player1Name} & {player2Name} - Time: {elapsedTime}";

    // Save results for both players
    SaveResultsToTextFile(combinedScore, elapsedTime);
}

private void SaveResultsToTextFile(string resultEntry, string elapsedTime)
{
    string filePath = Application.dataPath + "/leaderboard.txt";
    Debug.Log(filePath);
    
    // Read existing scores
    List<string> scores = new List<string>();
    if (File.Exists(filePath))
    {
        scores.AddRange(File.ReadAllLines(filePath));
    }

    // Add new score entry
    scores.Add(resultEntry);

    // Sort scores based on elapsed time
    scores.Sort((x, y) =>
    {
        // Extract time from strings for comparison
        TimeSpan timeX = ExtractElapsedTime(x);
        TimeSpan timeY = ExtractElapsedTime(y);
        return timeX.CompareTo(timeY);
    });

    // Write sorted scores back to the file
    File.WriteAllLines(filePath, scores);
    Debug.Log("Saved result to: " + filePath);
}

private TimeSpan ExtractElapsedTime(string scoreEntry)
{
    // Extract the elapsed time from the score entry string
    string[] parts = scoreEntry.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
    if (parts.Length > 1)
    {
        string timePart = parts[1].Trim().Replace("Time: ", "");
        TimeSpan time;
        if (TimeSpan.TryParse(timePart, out time))
        {
            return time;
        }
    }
    return TimeSpan.MaxValue; // Return a max value if parsing fails to ensure it gets sorted last
}

    private void HandleGameOver()
    {
        deadGirl.SetActive(true);
        deadBoy.SetActive(true);
        congratsText.text = "Try Again";
        Debug.Log("You Lose! Try Again");
    }

// //DELETE LEADERBOARD DATA
//     public void ClearLeaderboard()
// {
//     string filePath = Application.dataPath + "/leaderboard.txt";
    
//     // Check if the file exists
//     if (File.Exists(filePath))
//     {
//         // Clear the file by overwriting it with an empty string
//         File.WriteAllText(filePath, string.Empty);
//         Debug.Log("Leaderboard cleared at: " + filePath);
//     }
//     else
//     {
//         Debug.LogWarning("No leaderboard file found to clear.");
//     }
// }
}
