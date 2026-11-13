using UnityEngine;
using System;
public class Workplace : MonoBehaviour
{
    //WP - WorkPoints
    [SerializeField, Min(0.7f)] float WPThreshold = 10f;
    private float lastWPTime;
    [SerializeField] Transform uiCanvas;
    public bool isUsingAI{get;private set;}
    [SerializeField] float AIThreshold = 0.5f;
    private float lastAITime;
    public void AddPoints(float additiveWP){
        if(!G.CanPerformAction() || isUsingAI) return;
        float curTime = Time.time;
        if (curTime-lastWPTime >= WPThreshold)
        {
            G.Instance.AddWP(additiveWP);
            lastWPTime = curTime;
        }
    }
    public void SetAIActive(bool state) {isUsingAI = state;}
    public void Update()
    {
        float curTime = Time.time;
        if (G.CanPerformAction() && isUsingAI && curTime-lastAITime>=AIThreshold)
        {
            G.Instance.AddWP(-1);
            lastAITime = curTime;
        }
    }
    public void Init()
    {
        foreach (var monitor in FindObjectsByType<Monitor>(FindObjectsSortMode.InstanceID))
        {
            monitor.Init(this);
        }
        foreach (var keyboard in FindObjectsByType<Keyboard>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID))
        {
            Debug.Log(keyboard);
            keyboard.Init(this);
        }
    }
}
