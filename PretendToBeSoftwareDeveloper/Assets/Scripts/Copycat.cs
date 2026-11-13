using UnityEngine;
public class Copycat : MonoBehaviour
{
    [SerializeField] GameObject[] objects;
    void OnDisable() => DoAction(false);
    void OnEnable() => DoAction(true);
    void DoAction(bool state)
    {
        foreach(var go in objects){
            go.SetActive(state);
        }
    }
}