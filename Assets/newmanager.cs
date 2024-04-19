using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;

public class newmanager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

      public void Login()
    {

        string customId = System.Guid.NewGuid().ToString();
        var request = new LoginWithCustomIDRequest(){
            CustomId = customId,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams{
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request,OnSuccess,OnError);
    }

     void OnError(PlayFabError error){
        Debug.Log("error while logging in");
        Debug.Log(error.GenerateErrorReport());
    }

      void OnSuccess(LoginResult result){
        Debug.Log("successfull login/create account");
        /*string name = null;
        if(result.InfoResultPayload.PlayerProfile != null)
        name = result.InfoResultPayload.PlayerProfile.DisplayName;

        if(name == null){
            namewindow.SetActive(true);
        }
        else{leaderboardwindow.SetActive(true);}*/
    }


     public  void SendLeaderBoard(int score){
        var request = new UpdatePlayerStatisticsRequest{
            Statistics = new List<StatisticUpdate>{
                new StatisticUpdate{
                    StatisticName = "newleaderscore",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request,OnLeaderBoardUpdate,OnError);
    }

     void OnLeaderBoardUpdate(UpdatePlayerStatisticsResult result){
        Debug.Log("successfull leaderboard sent");
    }
}
