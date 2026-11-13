using UnityEngine;
[CreateAssetMenu(fileName = "EventDefintion")]
public class EventDefintion:ScriptableObject
{
    public string id;
    public float chance = 0.5f;
    public float weight = 1f;
    public float duration = 3f;
    public float cooldown = 10f;
}