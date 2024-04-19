using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Dater : MonoBehaviour
{
    // Start is called before the first frame update
    
    private DateTime fixedate = new DateTime(2023,7,21);
      private static GameTimer instance;
      [SerializeField] GameObject Gokapture;
       [SerializeField] GameObject form;

    void Start()
    {
       
        Debug.Log(DateTime.Now);
    }

    // Update is called once per frame
    void Update()
    {

        
          if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }


      if(DateTime.Now>fixedate){
        Debug.Log("not available");
       Gokapture.SetActive(true);
       form.SetActive(false);
      }
      else{Debug.Log("available");}

        DontDestroyOnLoad(gameObject);
    }

    void StopGame(){
       
    }
}
