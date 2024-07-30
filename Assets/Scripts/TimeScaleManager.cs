using System.Collections;
using TMPro;
using UnityEngine;

public class TimeScaleManager : MonoBehaviour
{
    [SerializeField] private GameObject PauseScreen;
    [SerializeField] private float TimeBeforeStarting = 3;
    [SerializeField] private TextMeshProUGUI TextWithTime;

    private bool IsPaused = false;
    private float CurrentTimeBeforeStarting;
    public bool IsGameStarted { get; private set; } = false;

    private void Awake()
    {
        Time.timeScale = 0;
        CurrentTimeBeforeStarting = TimeBeforeStarting;
        StartCoroutine(StartGameWithDelay());
    }

    private IEnumerator StartGameWithDelay()
    {
        while(CurrentTimeBeforeStarting >= 0)
        {
            if (!IsPaused)
            {
                CurrentTimeBeforeStarting -= Time.fixedDeltaTime;
                TextWithTime.text = Mathf.Ceil(CurrentTimeBeforeStarting).ToString();
            }
            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
        }
        IsGameStarted = true;
        TextWithTime.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        IsPaused = true;
        PauseScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void ContinueGame()
    {
        IsPaused = false;
        PauseScreen.SetActive(false);
        if(IsGameStarted)
        {
            Time.timeScale = 1;
        }
    }
}