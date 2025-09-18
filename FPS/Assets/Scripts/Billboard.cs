using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform target;

    public void Update()
    {
        transform.forward = target.forward;
    }
}
