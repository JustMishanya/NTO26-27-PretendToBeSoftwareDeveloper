using UnityEngine;
using System;
public class Workplace : MonoBehaviour
{
    //WP - WorkPoints
    private float wp;
    public Action<float> onWPChanged;
    [SerializeField] float WPThreshold = 10f;
    private float lastWPTime;
    [SerializeField] Transform uiCanvas;
    private int penalities;
    public Action<int> onPenalitiesChanged;
    public void AddPoints(float additivePoints){
        Debug.Log("1");
        if(!G.CanPerformAction()) return;
        Debug.Log("2");
        float curTime = Time.time;
        if (curTime-lastWPTime >= WPThreshold)
        {
            Debug.Log(3);
            wp += additivePoints;
            onWPChanged?.Invoke(wp);
            lastWPTime = curTime;
        }
    }
    public void AddPenalities(int additivePenalities){
        if(!G.CanPerformAction()) return;
        penalities += additivePenalities;
        onPenalitiesChanged?.Invoke(penalities);
    }
    
    void Awake()
    {
        if(uiCanvas != null)
        {
            Indicator WPCounter = uiCanvas.Find("WorkPoint").GetComponent<Indicator>();
            if (WPCounter != null) onWPChanged += WPCounter.ToIndicate;
        }
    }
    void OnDestroy()
    {
        if(uiCanvas != null)
        {
            Indicator WPCounter = uiCanvas.Find("WorkPoint").GetComponent<Indicator>();
            if(WPCounter != null) onWPChanged -= WPCounter.ToIndicate;
        }
    }
}
