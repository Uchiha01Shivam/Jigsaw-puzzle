using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayFabManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_InputField nameInput;
    string playername;
    //public TMP_Text playersname;
    void Start()
    {
        Login();
    }

    // Update is called once per frame
    void Awake()
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

     public void SubmitNameButton(){
        PlayerPrefs.SetString("user", nameInput.text);
        var request = new UpdateUserTitleDisplayNameRequest {
            DisplayName = nameInput.text,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request,OnDisplayNameUpdate,OnError);
       
    }

 void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result){
        Debug.Log("updated successfully");
        //leaderboardwindow.SetActive(true);
    }

    public void onleaderboard(){
        SceneManager.LoadScene(4);
    }

    

    /*public void getnameofplayer(){
        playername = nameInput.text;
    }*/
}
