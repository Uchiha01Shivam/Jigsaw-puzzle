using UnityEngine;

public class GameTimer : MonoBehaviour
{
    // The duration in seconds after which the game will stop
    public float targetDuration = 1 * 60 ; // 2 days in seconds
     private static GameTimer instance;
    void Start()
    {
        // Load the previously saved timer value
    
        float savedTime = PlayerPrefs.GetFloat("SavedTime");
        float timeElapsed = Time.time - savedTime;
       Debug.Log(PlayerPrefs.GetFloat("SavedTime"));
        // Check if the target duration has already passed
        if (timeElapsed >= targetDuration)
        {
            StopGame();
        }
    }

    void Update()
    {

          if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        // Save the current timer value
        if(PlayerPrefs.GetFloat("SavedTime")<=Time.time){
        PlayerPrefs.SetFloat("SavedTime", Time.time);
        }
        else{
            float newtime = (PlayerPrefs.GetFloat("SavedTime")+Time.time)/60f;
            PlayerPrefs.SetFloat("SavedTime",newtime);
        }

        if (Time.time >= targetDuration)
        {
            StopGame();
        }

         DontDestroyOnLoad(gameObject);
    }

    private void StopGame()
    {
        // Add any necessary code to stop the game (e.g., showing a "Game Over" screen, disabling player controls, etc.)
        // You may also save player progress or any necessary data before quitting the game.
        // After stopping the game, you can use Application.Quit() to exit the application in standalone builds.
        // However, keep in mind that Application.Quit() doesn't work in the Unity Editor.

        // For demonstration purposes, we'll use Application.Quit() here:
         Debug.Log("game has ended");
    }
}
