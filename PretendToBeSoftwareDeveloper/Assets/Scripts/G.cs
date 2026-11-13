using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
public class G : MonoBehaviour
{
    private static G _instance;
    private Dictionary<Type, Service> _services;
    private GameplayService _gameplayService;
    private EventService _eventService;
    private AssetService _assetService;
    private int penalities;
    public event Action<int> onPenalitiesChanged;
    private float wp;
    public float maxWP{get;private set;}
    public event Action onMaxWPReached;
    public event Action<float> onWPChanged;
    private int pushes;
    public event Action<int> onPushChanged;
    public static G Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<G>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(G).Name);
                    _instance = singletonObject.AddComponent<G>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
        private set{}
    }
    public void AddWP(float additiveWP)
    {
        if(wp < maxWP)
        {
            wp+=additiveWP;
            onWPChanged?.Invoke(wp);
            if(wp >= maxWP)
            {
                onMaxWPReached?.Invoke();
            }
        }
    }
    public void Penality(){
        penalities += 1;
        Debug.Log("PENALITY");
        onPenalitiesChanged?.Invoke(penalities);
    }
    public void Push()
    {
        if(maxWP > wp) return;
        wp = 0;
        onWPChanged?.Invoke(wp);
        pushes += 1;
        Debug.Log("Made a push");
        onPushChanged?.Invoke(pushes);
    }
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        _assetService = new AssetService();
        _gameplayService = new GameplayService();
        _eventService = new EventService();
        _services = new()
        {
            [typeof(AssetService)] = _assetService, 
            [typeof(GameplayService)] = _gameplayService,
            [typeof(EventService)] = _eventService
        };
        foreach(Service service in _services.Values)
        {
            StartCoroutine(service.OnStart());
        }
    }
    void Update()
    {
        foreach(Service service in _services.Values)
        {
            service.OnUpdate(Time.deltaTime);
        }
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Gameplay" || scene.name == "TestScene")
        {
            wp = 0f;
            maxWP = 37f;
            penalities = 0;
            var events = _assetService.GetAllAssets<EventDefintion>();
            _eventService.SetEvents(events);
            Workplace workplace = FindFirstObjectByType<Workplace>();
            workplace.Init();
            Boss boss = FindFirstObjectByType<Boss>();
            boss.Init("Jerry");
            _gameplayService.StartGame(600f);
        }
    }
    public T GetService<T>() where T : Service
    {
        var type = typeof(T);
        if (_services.ContainsKey(type))
        {
            return (T)_services[type];
        }
        else
        {
            Debug.LogError("This type is not exist: "+type.ToString());
            return null;
        }
    }
    public static bool CanPerformAction() => Instance._gameplayService.state == GameplayState.Running;

}