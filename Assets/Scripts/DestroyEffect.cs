using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    public float destroyTime = 1.5f;

    float currentTime = 0;

    void Update()
    {
        // 현재 타임이 1.5초가 지난후 사라짐
        if (currentTime > destroyTime)
        {
            Destroy(gameObject);

        }
        currentTime += Time.deltaTime;  
    }
}
