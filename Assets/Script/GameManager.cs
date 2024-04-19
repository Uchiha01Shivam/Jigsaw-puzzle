using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Action OnGameStart, OnGameEnd;
    public bool IsWon;
    public TMP_Text playerNameText;

    public List<imagePieceManager> imgs = new();
    private bool[] isMateched = new bool[12] {false,false,false,false,false,false,false,false,false,false,false,false};

    [SerializeField] private GameObject[] objToDisable;
    [SerializeField] private GameObject _gameOverScreen, sparks;
    [SerializeField] private TMP_Text _gameOverText, _scoreText;
    private bool _isGameStarted;
  public TMP_Text scoretext;
    private void Awake()
    {
        if(instance == null)
            instance = this;

        OnGameStart += () => _isGameStarted = true;
        OnGameEnd += ShowGameOver;
        OnGameEnd += () => _isGameStarted = false;
    }

    public void CheckAllImage()
    {
        for (int i = 0; i < imgs.Count; i++)
        {
            isMateched[i] = imgs[i].CheckParent();
        }
        CheckAllElementsTrue(isMateched);
    }
    void CheckAllElementsTrue(bool[] arr)
    {
        foreach (bool element in arr)
        {
            if (!element)
            {
                return; // Exit early if any element is false
            }
        }

        // All elements are true, invoke OnGameEnd
        IsWon = true;
        OnGameEnd?.Invoke();
        Debug.Log("Good!");
    }

    private void SaveScoreToFile(string playerName, float score)
    {
        PlayerScoresWrapper playerScoresWrapper = PlayerScoresWrapper.LoadFromFile();

        playerScoresWrapper.playerNames.Add(playerName);
        int roundedScore = 60-Mathf.FloorToInt(score);
        playerScoresWrapper.scores.Add(roundedScore);

        playerScoresWrapper.SaveToFile();

        // Debug.Log all saved items inside the JSON file
        Debug.Log($"Saved items in JSON file:\n{JsonConvert.SerializeObject(playerScoresWrapper)}");
    }



    void ShowGameOver()
    {
        sparks.transform.GetChild(0).localScale = Vector2.zero;
        sparks.transform.GetChild(1).localScale = Vector2.zero;
        if (IsWon)
        {
           
            _gameOverText.text = "Well Done!";
            playerNameText.text = PlayerPrefs.GetString("currplayer");
            float score = TimerAndCountdownManager.Instance.CalculateScore();
            SaveScoreToFile(PlayerPrefs.GetString("currplayer"), score);
            Debug.Log(score);
            _scoreText.text = $"You've Scored\n{score}";
           
        }
        else
        {
            _gameOverText.text = "Better Luck\nNext Time!!";
            _scoreText.text = "";
        }
        DisableObj();
        _gameOverScreen.SetActive(true);
        LeanTween.scale(_gameOverScreen, Vector2.one, 0.8f).setEaseOutCirc().setOnComplete(() => {

            if (IsWon)
            {
                LeanTween.scale(sparks.transform.GetChild(0).gameObject, new Vector2(12,5), 0.2f).setEaseOutBounce();
                LeanTween.scale(sparks.transform.GetChild(1).gameObject, Vector2.one, 0.2f).setEaseOutBounce();
                 string newscore = scoretext.text;
            int amount = int.Parse(newscore);
            int scoredamount = amount;
            SendLeaderBoard(scoredamount);
            }
            LeanTween.scale(_gameOverScreen, Vector2.zero, 1f).setDelay(10).setOnComplete(() =>
            {
                
                SceneLoad(0);
            });
        });
    }

    void DisableObj()
    {
        foreach(GameObject obj in objToDisable)
        {
            obj.SetActive(false);
        }
        foreach(imagePieceManager img in imgs)
        {
           img.gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if (_isGameStarted)
        {
            TimerAndCountdownManager.Instance.StartTimer();
        }
        else
        {
            TimerAndCountdownManager.Instance.StartCountdown();
        }
    }

    public void SceneLoad(int buildindex)
    {
        SceneManager.LoadScene(buildindex);
    }

     public  void SendLeaderBoard(int score){
        var request = new UpdatePlayerStatisticsRequest{
            Statistics = new List<StatisticUpdate>{
                new StatisticUpdate{
                    StatisticName = "jigsaw",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request,OnLeaderBoardUpdate,OnError);
    }

     void OnLeaderBoardUpdate(UpdatePlayerStatisticsResult result){
        Debug.Log("successfull leaderboard sent");
    }

     void OnError(PlayFabError error){
        Debug.Log("error while logging in");
        Debug.Log(error.GenerateErrorReport());
    }



}

[System.Serializable]
public class PlayerScoresWrapper
{
    public List<string> playerNames = new List<string>();
    public List<float> scores = new List<float>();

    public void SaveToFile()
    {
        string filePath = Application.persistentDataPath + "/scores.json";
        string json = JsonConvert.SerializeObject(this);
        File.WriteAllText(filePath, json);
    }

    public static PlayerScoresWrapper LoadFromFile()
    {
        string filePath = Application.persistentDataPath + "/scores.json";

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<PlayerScoresWrapper>(json) ?? new PlayerScoresWrapper();
        }
        else
        {
            return new PlayerScoresWrapper();
        }
    }
}


