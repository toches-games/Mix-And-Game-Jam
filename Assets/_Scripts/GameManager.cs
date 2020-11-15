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

    public GameState currentState { get; set; } = GameState.inMenu;

    [SerializeField]
    private int mapVelocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveMapDown();
    }

    public void SetState(GameState newState)
    {
        currentState = newState;

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
        GenerationSystem.sharedInstance.gameObject.transform.localPosition = new Vector3(
            GenerationSystem.sharedInstance.gameObject.transform.localPosition.x,
            GenerationSystem.sharedInstance.gameObject.transform.localPosition.y - Time.deltaTime * mapVelocity,
            GenerationSystem.sharedInstance.gameObject.transform.localPosition.z);
    }


}
