using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBlock : MonoBehaviour
{
    public Transform startPoint, endPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DisableZone"))
        {
            this.gameObject.SetActive(false);

            GenerationSystem.sharedInstance.currentBlocks.Remove(this.gameObject);

        }else if (collision.CompareTag("ActivateZone"))
        {
            GenerationSystem.sharedInstance.GenerateBlock();
        }
    }
}
