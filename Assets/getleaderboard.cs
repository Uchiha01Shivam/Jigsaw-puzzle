using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class getleaderboard : MonoBehaviour
{
    // Start is called before the first frame update

      public TMP_Text[] scores;
          public TMP_Text[] rankses;
          public TMP_Text[] names;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        LoadLeaderboardData();
    }

    void LoadLeaderboardData()
    {
        // Load scores from the JSON file
        PlayerScoresWrapper playerScoresWrapper = PlayerScoresWrapper.LoadFromFile();

        // Sort the scores in ascending order
        IEnumerable<int> sortedScores = playerScoresWrapper.scores.Select(score => Mathf.FloorToInt(score)).OrderBy(score => score);

        // Assign the scores, names, and ranks
        int i = 0;
        foreach (int score in sortedScores.Take(scores.Length))
        {
            int rank = playerScoresWrapper.scores.IndexOf(score) + 1;
            string playerName = playerScoresWrapper.playerNames[playerScoresWrapper.scores.IndexOf(score)];

            // Display the data in your TMP_Text components
            scores[i].text = score.ToString();
            rankses[i].text = rank.ToString();
            names[i].text = playerName;

            i++;
        }
    }

    public void onhome(){
        SceneManager.LoadScene(0);
    }
}
