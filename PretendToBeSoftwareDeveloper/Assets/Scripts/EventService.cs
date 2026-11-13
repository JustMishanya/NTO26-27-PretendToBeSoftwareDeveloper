using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class EventService : Service
{
    public event Action<EventDefintion> onEventStarted;
    public event Action<EventDefintion> onEventEnded;
    private List<EventDefintion> events = new();
    private Dictionary<EventDefintion, Coroutine> active = new();
    private Dictionary<EventDefintion, float> cooldownUntil = new();
    private float rollTimer; 
    public override IEnumerator OnStart()
    {
        yield return new WaitForEndOfFrame();
    }
    public void SetEvents(List<EventDefintion> events)
    {
        this.events = events;
    }
    public void SetEvents(EventDefintion[] events)
    {
        this.events.AddRange(events);
    }
    public override void OnUpdate(float deltaTime)
    {
        if(!G.CanPerformAction()) return;
        rollTimer += deltaTime;
        if(rollTimer >= 5f)
        {
            rollTimer = 0f;
            TryRoll();
        }
    }
    void TryRoll()
    {
        if(UnityEngine.Random.value > 0.5f)return;
        float curTime = Time.time;
        float totalWeight = 0f;
        var canditates = new List<EventDefintion>();
        foreach(var defintion in events)
        {
            if(defintion == null || active.ContainsKey(defintion) || (cooldownUntil.TryGetValue(defintion, out float until) && curTime < until)) continue;
            canditates.Add(defintion);
            totalWeight += defintion.weight;
        }
        if(canditates.Count == 0) return;
        float pick = UnityEngine.Random.value * totalWeight;
        float counter = 0f;
        foreach(var defintion in canditates)
        {
            counter += defintion.weight;
            if(pick <= counter)
            {
                StartEvent(defintion);
                return;
            }
        }
    }
    void StartEvent(EventDefintion defintion)
    {
        active[defintion] = G.Instance.StartCoroutine(RunEvent(defintion));
        onEventStarted?.Invoke(defintion);
    }
    IEnumerator RunEvent(EventDefintion defintion)
    {
        float remaining = defintion.duration;
        while(remaining > 0f)
        {
            if (!G.CanPerformAction())
            {
                if (G.Instance.GetService<GameplayService>().state == GameplayState.Ended) break;
                yield return null;
                continue;
            }
            remaining -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        EndEvent(defintion);
    }
    void EndEvent(EventDefintion defintion)
    {
        if(!active.Remove(defintion)) return;
        cooldownUntil[defintion] = Time.time+defintion.cooldown;
        onEventEnded?.Invoke(defintion);
    }
    public void StopAllEvents()
    {
        var toStop = new List<EventDefintion>(active.Keys);
        foreach(var defintion in toStop)
        {
            if(active.TryGetValue(defintion, out Coroutine coroutine) && coroutine != null)
            {
                G.Instance.StopCoroutine(coroutine);
            }
            EndEvent(defintion);
        }
    }

}