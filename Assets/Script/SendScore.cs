using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class SendScore : MonoBehaviour
{
    public static SendScore instance;
    public int id;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void InsertScore(int scoreVal)
    {
        StartCoroutine(SendScorePostRequest(scoreVal));
    }

    private IEnumerator SendScorePostRequest(int scoreVal)
    {
        //endpoint URL
        string url = "https://www.gokapture.com/ingramevent/insert_score.php";

        //data to send in the request body
        var data = new UserData
        {
            employee_id = id,
            game_name = "jigsaw_game",
            score = scoreVal,
            username = "ingram_usr",
            password = "64a81e7baac71",
        };

        // Convert the data to JSON
        string json = JsonConvert.SerializeObject(data);
        Debug.Log(json);

        // Create the request object
        using (UnityWebRequest request = new(url, UnityWebRequest.kHttpVerbPOST))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // Send the request
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                // Handle any error that occurred during the request
                Debug.Log("Some Error occur" + request.error);
            }
            else
            {
                // Read the response
                byte[] responseBytes = request.downloadHandler.data;
                string responseJson = Encoding.UTF8.GetString(responseBytes);
                Debug.Log(responseJson);
                // Deserialize the JSON response into an object
                var result = JsonUtility.FromJson<Response>(responseJson);
                // Access the "success" property in the response
                bool success = result.success;
                Debug.Log("Is Score Added:" + success);
            }
        }
    }

    [System.Serializable]
    private class Response
    {
        public bool success;
    }

    [SerializeField]
    private class UserData
    {
        public int employee_id = 12345;
        public string game_name = "jigsaw_game";
        public int score = 0;
        public string username = "ingram_usr";
        public string password = "64a81e7baac71";

    }
}
