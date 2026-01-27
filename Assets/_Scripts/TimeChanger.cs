using UnityEngine;

public class TimeChanger : MonoBehaviour
{
    
    private float fixedDeltaTime;

    void Awake()
    {
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    public void ModifyTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = this.fixedDeltaTime * timeScale;
    }
}
