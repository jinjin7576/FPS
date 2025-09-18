using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    private void Awake()
    {
        if(gm == null)
        {
            gm = this;
        }
    }
}
