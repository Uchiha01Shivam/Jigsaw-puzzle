using System.Collections;
using System.Diagnostics;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Debug = UnityEngine.Debug;
using UnityEngine.Events;

public class CollectionPage : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;/* emailInput, phoneNumberInput*/
    [SerializeField] private TMP_Text _popup;
    

    Process keyboardProcess;

    [SerializeField] private UnityEvent OnEmployeeFound;
    private int id;

    public int Id { get => id; }

    private void Awake()
    {
        TouchSimulation.Enable();
    }
    public void OpenKeyboard()
    {
        //System.Diagnostics.Process.Start("tabtip.exe");
        keyboardProcess = Process.Start("osk.exe");
    }
    public void CloseKeyboard()
    {
        keyboardProcess.Kill();
    }
    private void Start()
    {
        EnhancedTouchSupport.Enable();
    }
    public void CheckEmployee()
    {
        bool isFilled = nameInput.text.Length > 0;
        bool isValid = int.TryParse(nameInput.text, out id);
        if(isFilled){
            PlayerPrefs.GetString("currplayer","");
            if(PlayerPrefs.GetString("currplayer")!=nameInput.text){
                  PlayerPrefs.SetString("currplayer",nameInput.text);
                  PlayerPrefs.Save();
            }
        }
        if(!isFilled){
             PlayerPrefs.SetString("currplayer","");
                  PlayerPrefs.Save();
        }
        if (isFilled && isValid)
        {
            StartCoroutine(SendPostRequest());
             OnEmployeeFound?.Invoke();
            nameInput.text = "";
        }
        else
        {
            _popup.text = "Not a Registered User";
             OnEmployeeFound?.Invoke();
        }
    }

    private IEnumerator SendPostRequest()
    {
        // endpoint URL
        string url = "https://www.gokapture.com/ingramevent/check_employee.php";

        //data to send in the request body
        var data = new User
        {
            employee_id = id, // ID from Employee Input
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
                _popup.text = "Some Error Occur in Connection!!";
            }
            else
            {
                // Read the response
                byte[] responseBytes = request.downloadHandler.data;
                string responseJson = Encoding.UTF8.GetString(responseBytes);

                // Deserialize the JSON response into an object
                var result = JsonUtility.FromJson<Response>(responseJson);
                // Access the "exists" property in the response
                bool exists = result.exists;
                HandleResponse(exists);
            }
        }
    }

    private void HandleResponse(bool exist)
    {
        if (exist)
        {
            Debug.Log("User In");
            SendScore.instance.id = id;
            OnEmployeeFound?.Invoke();
        }
        else
        {
            _popup.text = "Not a Registered User";
             OnEmployeeFound?.Invoke();
        }
    }

    // Response class to match the JSON structure
    [System.Serializable]
    private class Response
    {
        public bool exists;
    }
    [System.Serializable] 
    private class User
    {
        public int employee_id = 003;
        public string username = "ingram_usr";
        public string password = "64a81e7baac71";

    }

}

public static class Extension
{
    public static string Capitalize(this string str)
    {
        return $"{str[0].ToString().ToUpper()}{str[1..]}";
    }
}