using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    inMenu,
    inGame,
    inPause
}

public class GameManager : MonoBehaviour
{
    public static GameManager sharedInstance;

    public GameState CurrentState { get; set; } = GameState.inGame;

    [SerializeField]
    private int mapVelocity;

    private void Awake()
    {
        sharedInstance = sharedInstance == null ? this : sharedInstance;

    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentState = GameState.inGame;
    }

    // Update is called once per frame
    void Update()
    {
        MoveMapDown();
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;

        if(newState == GameState.inMenu)
        {

        }else if(newState == GameState.inGame)
        {

        }else if(newState == GameState.inPause)
        {

        }
    }

    private void MoveMapDown()
    {
        GenerationSystem.sharedInstance.gameObject.transform.position = new Vector3(
            GenerationSystem.sharedInstance.gameObject.transform.position.x,
            GenerationSystem.sharedInstance.gameObject.transform.position.y - Time.deltaTime * mapVelocity,
            GenerationSystem.sharedInstance.gameObject.transform.position.z);
    }


}
