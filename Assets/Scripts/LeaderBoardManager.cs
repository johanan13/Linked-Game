using System.IO;
using UnityEngine;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private TMP_Text leaderboardText;

    private void Start()
    {
        DisplayLeaderboard();
    }

    private void DisplayLeaderboard()
    {
        string filePath = Application.dataPath + "/leaderboard.txt";

        if (File.Exists(filePath))
        {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);
            string leaderboard = "";

            for (int i = 0; i < lines.Length; i++)
            {
                leaderboard += (i + 1) + ". " + lines[i] + "\n";
            }

            leaderboardText.text = leaderboard;
        }
        else
        {
            leaderboardText.text = "No leaderboard data available.";
        }
    }
}
