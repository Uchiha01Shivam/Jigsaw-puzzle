using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimerAndCountdownManager : MonoBehaviour
{
    public static TimerAndCountdownManager Instance;

    [SerializeField] private TMP_Text _timerText, _countdownText;
    [SerializeField] private float _timerMaxValue, _countdownMaxValue;

    [SerializeField] private Image _timerImageFill;

    private float _timerValue, _countdownValue;

    private bool _countEnd;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        _timerValue = _timerMaxValue;
        _countdownValue = _countdownMaxValue;
        _timerText.text = _timerValue.ToString("0");
        _timerImageFill.fillAmount = _timerValue / _timerMaxValue;
    }

    public void StartTimer()
    {
        _timerValue -= Time.deltaTime;
        _timerText.text = _timerValue.ToString("0");
        _timerImageFill.fillAmount = _timerValue / _timerMaxValue;
        ///innerFill.fillAmount = _timerValue / _timerMaxValue;
        if (_timerValue < 0)
        {
            GameManager.instance.IsWon = false;
            GameManager.instance.OnGameEnd?.Invoke();
        }
    }

    public void GameLose()
    {
        _timerValue = 0;
    }

    public void DisableCountDown()
    {
        LeanTween.scale(_countdownText.gameObject, Vector2.zero, 0.1f).setDestroyOnComplete(true);
    }

    public void StartCountdown()
    {
        _countdownValue -= Time.deltaTime;
        _countdownText.text = _countdownValue.ToString("0");
        if (!_countEnd && _countdownValue <= 0)
        {
            GameManager.instance.OnGameStart?.Invoke();
            _countEnd = true;
            DisableCountDown();
        }
    }
    public float CalculateScore()
    {
        float remainingTime = _timerValue;
        if (remainingTime >= 50)
        {
            return remainingTime;
        }
        else if(remainingTime < 50 && remainingTime >= 40)
        {
            return remainingTime;
        }
        else if (remainingTime < 40 && remainingTime >= 30)
        {
            return remainingTime;
        }
        else if (remainingTime < 30 && remainingTime >= 20)
        {
            return remainingTime;
        }
        else if (remainingTime < 20 && remainingTime >= 10)
        {
            return remainingTime;
        }
        else if( remainingTime < 10 && remainingTime > 0)
        {
           return remainingTime;
        }
        else
        {
            return 0;
        }
    }

} 

