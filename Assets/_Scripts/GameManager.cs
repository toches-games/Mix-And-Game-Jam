using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    private void MoveMapDown()
    {
        GenerationSystem.sharedInstance.gameObject.transform.localPosition = new Vector3(
            GenerationSystem.sharedInstance.gameObject.transform.localPosition.x,
            GenerationSystem.sharedInstance.gameObject.transform.localPosition.y - Time.deltaTime * mapVelocity,
            GenerationSystem.sharedInstance.gameObject.transform.localPosition.z);
    }
}
