using System;
using System.Collections.Generic;
using UnityEngine;
public class G : MonoBehaviour
{
    private static G _instance;
    private Dictionary<Type, Service> _services;
    private GameplayService _gameplayService;
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
    void Awake()
    {
        _gameplayService = new GameplayService();
        _services = new()
        {
            [typeof(GameplayService)] = _gameplayService
        };
        foreach(Service service in _services.Values)
        {
            service.OnStart();
        }
    }
    void Update()
    {
        foreach(Service service in _services.Values)
        {
            service.OnUpdate(Time.deltaTime);
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