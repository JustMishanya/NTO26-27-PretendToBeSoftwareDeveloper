using UnityEngine;
public abstract class Indicator : MonoBehaviour
{
    public string id;
    public abstract void ToIndicate<T>(T value);
}