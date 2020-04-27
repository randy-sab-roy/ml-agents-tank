using UnityEngine;

public class TimeWatcher : MonoBehaviour
{
    public float StopTime = 300;
    void Update()
    {
        if (Time.time >= StopTime)
        {
            Debug.Break();
        }
    }
}
