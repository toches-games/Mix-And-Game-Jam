using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private static float score;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.sharedInstance.CurrentState != GameState.inGame)
        {
            return;
        }

        UIManager.SI.RefreshScore(score += Time.deltaTime * PlayerController.sharedInstance.GetCurrentSpeed() * 0.3f);
    }

    public float GetScore()
    {
        return score;
    }

    public void ScoreMultiplier(int multi)
    {
        score *= multi;
        //TODO Animation
    }
}
