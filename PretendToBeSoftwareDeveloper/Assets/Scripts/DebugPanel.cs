using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DebugPanel:MonoBehaviour
{
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] float sessionTime = 10f;
    void Awake()
    {
        GameplayService gameplayService = G.Instance.GetService<GameplayService>();
        GameObject startButton = Instantiate(buttonPrefab,transform);
        startButton.GetComponent<Button>().onClick.AddListener(()=>gameplayService.StartGame(sessionTime));
        startButton.transform.Find("Text").GetComponent<TMP_Text>().text = "Start "+sessionTime.ToString()+"s";
        GameObject stopButton = Instantiate(buttonPrefab,transform);
        stopButton.GetComponent<Button>().onClick.AddListener(()=>gameplayService.EndGame());
        stopButton.transform.Find("Text").GetComponent<TMP_Text>().text = "End";
        GameObject pauseButton = Instantiate(buttonPrefab,transform);
        pauseButton.GetComponent<Button>().onClick.AddListener(()=>gameplayService.PauseGame());
        pauseButton.transform.Find("Text").GetComponent<TMP_Text>().text = "Pause";
        GameObject resumeButton = Instantiate(buttonPrefab,transform);
        resumeButton.GetComponent<Button>().onClick.AddListener(()=>gameplayService.ResumeGame());
        resumeButton.transform.Find("Text").GetComponent<TMP_Text>().text = "Resume";
        gameplayService.onEnded += ()=> Debug.Log("Game ended");
        gameplayService.onPaused += ()=> Debug.Log("Game Paused");
        gameplayService.onResumed += ()=> Debug.Log("Game resumed");
        gameplayService.onStarted += ()=> Debug.Log("Game started");
        //gameplayService.onTimeChanged += (float time)=> Debug.Log("Time is timeouting:"+time.ToString());
    }
}