using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectionMenu : MonoBehaviour
{
    [SerializeField] private float maxTimer;
    private float timer;
    [SerializeField] private Slider slider;

    [SerializeField] private UnityEvent OnSelectionTimeEnded;
    void OnEnable()
    {
        timer = maxTimer;
        slider.maxValue = timer;
        slider.value = maxTimer;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        slider.value = timer;
        if(timer < 0)
        {
            OnSelectionTimeEnded?.Invoke();
        }
    }
}
