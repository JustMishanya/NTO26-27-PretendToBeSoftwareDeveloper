using System.Collections;

public abstract class Service
{
    public abstract IEnumerator OnStart();
    public abstract void OnUpdate(float deltaTime);
}