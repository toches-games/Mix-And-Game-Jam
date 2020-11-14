using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationSystem : MonoBehaviour
{
    public static GenerationSystem sharedInstance;
    public List<MapBlock> allTheMapBlocks = new List<MapBlock>();
    public List<MapBlock> currentMapBlocks = new List<MapBlock>();
    public Transform MapStartPosition;

    private void Awake()
    {
        sharedInstance = sharedInstance == null ? this : sharedInstance;
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateInitialBlocks();
    }

    public void AddLevelBlock()
    {
        int randomIdx = Random.Range(1, allTheMapBlocks.Count);

        MapBlock block;

        Vector3 spawnPosition = Vector3.zero;

        if (currentMapBlocks.Count == 0)
        {
            block = Instantiate(allTheMapBlocks[0]);
            spawnPosition = MapStartPosition.position;
        }
        else
        {
            block = Instantiate(allTheMapBlocks[randomIdx]);
            spawnPosition = currentMapBlocks
                [currentMapBlocks.Count - 1].
                endPoint.
                position;
        }

        block.transform.SetParent(this.transform, false);

        //Esto funciona asi, tomo el spawnPosition en cada coordenada
        //Que viene siendo el punto final del block anterior
        //Y tomo el block.starPoint en cada una de sus coordenadas
        //Esto vendria siendo lo que mide desde el centro del block
        //Hasta su punto inicial, es decir, su parte mas hacia
        //la izquierda del block como tal, entonces este numero siempre
        //Sera negativo, por el hecho de que esta del centro hacia la
        //Izquierda, entonces al hacer la resta lo nros se terminaran
        //sumando, y me daria el valor exacto para poner el nuevo block
        //el punto final del block anterior, por ejm 25 menos la distancia
        //del centro del block hasta la punta izquierda, algo asi
        //25 - (-8) = 33 me daria que debo poner ese siguiente block
        //en x = 33 para que su punta izquierda que esta a -8 de ese 33
        //concuerde exacto con el pto final del block anterior.
        Vector3 correction = new Vector3(
            spawnPosition.x - block.startPoint.position.x,
            spawnPosition.y - block.startPoint.position.y,
            0);
        block.transform.position = correction;
        currentMapBlocks.Add(block);
    }

    public void RemovedLevelBlock()
    {
        //se elimina siempre el bloque de la posicion 0
        //porque la lista al ir eliminando ella va
        //modificando los indices y siempre el primero
       MapBlock oldBlock = currentMapBlocks[0];
        //va a ser el 0
        currentMapBlocks.Remove(oldBlock);
        Destroy(oldBlock.gameObject);
    }

    public void RemoveAllLevelBlocks()
    {
        while (currentMapBlocks.Count > 0)
        {
            RemovedLevelBlock();
        }
    }

    public void GenerateInitialBlocks()
    {
        for (int i = 0; i < 2; i++)
        {
            AddLevelBlock();
        }
    }
}
