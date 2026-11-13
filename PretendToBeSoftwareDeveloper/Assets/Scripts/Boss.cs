using System.Collections;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Boss : MonoBehaviour
{
    [SerializeField] string eventID;
    [SerializeField] Transform comePoint;
    private SpriteRenderer spriteRenderer;
    Sprite idleSprite, checkSprite, angrySprite;
    public void Init(string id)
    {
        var assetService = G.Instance.GetService<AssetService>();
        var eventService = G.Instance.GetService<EventService>();
        idleSprite = assetService.GetAsset<Sprite>(id+"Idle_0");
        checkSprite = assetService.GetAsset<Sprite>(id+"Check_0");
        angrySprite = assetService.GetAsset<Sprite>(id+"Angry_0");
        spriteRenderer = GetComponent<SpriteRenderer>();
        eventService.onEventStarted += OnStartEvent;
        eventService.onEventEnded += OnEndEvent;
    }
    void OnDestroy()
    {
        var eventService = G.Instance.GetService<EventService>();
        eventService.onEventStarted -= OnStartEvent;
        eventService.onEventEnded -= OnEndEvent;
    }
    public void OnStartEvent(EventDefintion defintion)
    {
        if(eventID != defintion.id) return;
        StartCoroutine(Process(defintion));
    }
    IEnumerator Process(EventDefintion defintion)
    {
        var waitForEndOfFrame = new WaitForEndOfFrame();
        var initPosition = transform.position;
        spriteRenderer.sprite = idleSprite;
        Debug.Log("Im coming");
        while(Vector3.Distance(transform.position, comePoint.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, comePoint.position, 0.1f);
            yield return waitForEndOfFrame;
        }
        float remaining = defintion.duration;
        while(remaining > 0f)
        {
            if(!G.CanPerformAction()) continue;
            remaining -= Time.deltaTime;
            yield return waitForEndOfFrame;
        }
        Debug.Log("Im come, now check");
        spriteRenderer.sprite = checkSprite;
        yield return new WaitForSeconds(1.7f);
        var workplace = FindFirstObjectByType<Workplace>();
        if(!workplace.isUsingAI)
        {
            Debug.Log("YOU BASTARD");
            spriteRenderer.sprite = angrySprite;
            G.Instance.Penality();
        }
        Debug.Log("Im leaving");
        while(Vector3.Distance(initPosition, transform.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, initPosition, 0.1f);
            yield return waitForEndOfFrame;
        }
        Debug.Log("im leave");
        yield return null;
    }
    public void OnEndEvent(EventDefintion defintion)
    {
        if(eventID != defintion.id) return;
    }
}