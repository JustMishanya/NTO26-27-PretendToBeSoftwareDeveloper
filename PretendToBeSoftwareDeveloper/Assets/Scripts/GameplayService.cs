using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameplayService : Service{
    public GameplayState state {get;private set;} = GameplayState.NotStarted;
    float sessionTime;
    public event Action onStarted, onPaused, onResumed, onEnded;
    public event Action <float> onTimeChanged;
    public float remainingTime {get;private set;}
    public void StartGame(float sessionTime)
    {
        if(state != GameplayState.NotStarted) return;
        this.sessionTime = sessionTime;
        remainingTime = this.sessionTime;
        state = GameplayState.Running;
        onStarted?.Invoke();
    }
    public void PauseGame()
    {
        if(state != GameplayState.Running) return;
        state = GameplayState.Pause;
        onPaused?.Invoke();
    }
    public void ResumeGame()
    {
        if(state != GameplayState.Pause) return;
        state = GameplayState.Running;
        onResumed?.Invoke();
    }
    public void EndGame()
    {
        if(state == GameplayState.Ended) return;
        state = GameplayState.Ended;
        onEnded?.Invoke();
    }
    public override IEnumerator OnStart()
    {
        yield return new WaitForEndOfFrame();
    }
    public override void OnUpdate(float deltaTime)
    {
        if(state != GameplayState.Running) return;
        remainingTime -= deltaTime;
        onTimeChanged?.Invoke(remainingTime);
        
        if(remainingTime <= 0f)
        {
            remainingTime = 0f;
            EndGame();
        }
    }
}
public enum GameplayState{NotStarted, Running, Pause, Ended}