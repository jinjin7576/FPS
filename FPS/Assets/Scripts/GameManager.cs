using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

    public enum GameState
    {
        Ready,
        Run,
        GameOver,
    }

    public GameState gState;
    public GameObject gameLabel;
    PlayerMove player;
    Text gameText;
    private void Start()
    {
        gState = GameState.Ready;
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
        gameText = gameLabel.GetComponent<Text>();
        gameText.text = "Ready...";
        //ÁÖÈ²»ö
        gameText.color = new Color(255, 185, 0, 255);
        StartCoroutine(ReadyToStart());
    }

    IEnumerator ReadyToStart()
    {
        yield return new WaitForSeconds(2f);
        gameText.text = "Go!";

        yield return new WaitForSeconds(0.5f);
        gameLabel.SetActive(false);
        gState = GameState.Run;
    }

    void Update()
    {
        if(player.hp <= 0)
        {
            gameLabel.SetActive(true);
            gameText.text = "Game Over";
            gameText.color = Color.red;
            gState = GameState.GameOver;
            player.GetComponentInChildren<Animator>().SetFloat("MoveMotion",0f);
        }
    }
}
