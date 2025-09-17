using UnityEngine;

public class BombEffect : MonoBehaviour
{
    public float destroyTime = 1.5f;
    float currentTime = 0;

    private void Update()
    {
        if (currentTime > destroyTime)
        {
            Destroy(gameObject);
        }

        currentTime += Time.deltaTime;
    }
}
