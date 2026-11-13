using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class AssetService : Service
{
    private Dictionary<Type, Dictionary<string, UnityEngine.Object>> _assets = new();
    public override IEnumerator OnStart()
    {
        foreach(var asset in Resources.LoadAll("Resources")){
            var type = asset.GetType();
            if (!_assets.ContainsKey(type))
            {
                _assets[type] = new();
            }
            if (_assets[type].ContainsKey(asset.name))
            {
                Debug.LogWarning("Этот ассет уже записан в AssetService и был пропущен: "+asset.ToString());
                continue;
            }
            _assets[type][asset.name] = asset;
        }
        yield return new WaitForEndOfFrame();
    }
    public T[] GetAllAssets<T>() where T : UnityEngine.Object
    {
        var type = typeof(T);
        if (_assets.TryGetValue(type, out var dict))
        {
            return dict.Values.Cast<T>().ToArray();
        }
        return Array.Empty<T>();
    }
    public T GetAsset<T>(string assetName) where T : UnityEngine.Object
    {
        if(_assets.ContainsKey(typeof(T)))
        {
            var dict = _assets[typeof(T)];
            if(dict.ContainsKey(assetName)) return (T)dict[assetName];
        }
        else
        {
            Debug.LogWarning("Ассет такого типа не существует");
        }
        return null;
    }
    public override void OnUpdate(float deltaTime)
    {
        return;
    }
}