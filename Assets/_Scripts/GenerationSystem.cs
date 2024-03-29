﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationSystem : MonoBehaviour
{
    public static GenerationSystem sharedInstance;

    public List<GameObject> allTheMapBlocks = new List<GameObject>();
    public List<GameObject> currentBlocks = new List<GameObject>();

    public List<GameObject> enemyPositions = new List<GameObject>();

    [SerializeField, Tooltip("Prefab del mapa inicial")]
    private GameObject initMapBlock;
    [SerializeField]
    private List<GameObject> pooledObjects;
    
    public static int blocksAmount = 0;

    [SerializeField]
    private Transform mapStartPoint;

    private void Awake()
    {
        sharedInstance = sharedInstance == null ? this : sharedInstance;
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateInitialBlocks();
    }

    private void Update()
    {
    }

    public void GenerateBlock()
    {
        GameObject block;
        Vector3 spawnPosition = Vector3.zero;
        Vector3 spawnLocalPosition = Vector3.zero;

        if (blocksAmount == 0)
        {
            block = Instantiate(initMapBlock);
            
            spawnPosition = mapStartPoint.position;
            //Debug.Log("Spawn global: " + spawnPosition);

        }
        else
        {
            //block = ObjectPool.sharedInstance.GetPooledObject(Random.Range(0,3));
            block = ObjectPool.sharedInstance.GetPooledObject(0);
            
            if (block != null)
            {

                //MapBlock tmp = FindObjectsOfType<MapBlock>()[FindObjectsOfType<MapBlock>().Length - 1];
                MapBlock tmp = currentBlocks[currentBlocks.Count-1].GetComponent<MapBlock>();

                spawnPosition = tmp.endPoint.position;
                spawnLocalPosition = tmp.endPoint.transform.localPosition;

                //Debug.Log("Spawn global: " + spawnPosition);

                //foreach (var item in FindObjectsOfType<MapBlock>())
                //{
                //    Debug.Log(item.endPoint.position);

                //}
                //Debug.Log("Spawn local: " + spawnLocalPosition);
                //Debug.Log(FindObjectsOfType<MapBlock>()[FindObjectsOfType<MapBlock>().Length - 1].
                //    endPoint.localPosition);

            }
        }

        /**
        Esto funciona asi, tomo el spawnPosition en cada coordenada
        Que viene siendo el punto final del block anterior
        Y tomo el block.starPoint en cada una de sus coordenadas
        Esto vendria siendo lo que mide desde el centro del block
        Hasta su punto inicial, es decir, su parte mas hacia
        la izquierda del block como tal, entonces este numero siempre
        Sera negativo, por el hecho de que esta del centro hacia la
        Izquierda, entonces al hacer la resta lo nros se terminaran
        sumando, y me daria el valor exacto para poner el nuevo block
        el punto final del block anterior, por ejm 25 menos la distancia
        del centro del block hasta la punta izquierda, algo asi
        25 - (-8) = 33 me daria que debo poner ese siguiente block
        en x = 33 para que su punta izquierda que esta a -8 de ese 33
        concuerde exacto con el pto final del block anterior.
        **/
        Vector3 correction = new Vector3(
            spawnPosition.x - block.GetComponent<MapBlock>().startPoint.localPosition.x,
            spawnPosition.y - block.GetComponent<MapBlock>().startPoint.localPosition.y,
            0);
        //Debug.Log(spawnPosition.x);
        //Debug.Log(block.GetComponent<MapBlock>().startPoint.position.x);
        //Debug.Log(spawnPosition.y);
        //Debug.Log(block.GetComponent<MapBlock>().startPoint.position.y);
        //Debug.Log("correccion " + correction);

        block.transform.SetParent(this.transform, false);
        block.transform.position = correction;

        currentBlocks.Add(block);

        blocksAmount++;
        block.SetActive(true);
    }

    public void RemovedLevelBlock()
    {

       // //se elimina siempre el bloque de la posicion 0
       // //porque la lista al ir eliminando ella va
       // //modificando los indices y siempre el primero
       //MapBlock oldBlock = currentMapBlocks[0];
       // //va a ser el 0
       // currentMapBlocks.Remove(oldBlock);
       // Destroy(oldBlock.gameObject);
    }

    public void RemoveAllLevelBlocks()
    {
        //while (currentMapBlocks.Count > 0)
        //{
        //    RemovedLevelBlock();
        //}
    }

    public void GenerateInitialBlocks()
    {
        for (int i = 0; i < 2; i++)
        {
            GenerateBlock();
        }

    }

    private void FillPool()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < allTheMapBlocks.Count; i++)
        {
            tmp = Instantiate(allTheMapBlocks[i].gameObject);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }

    private GameObject GetPooledObject()
    {
        for (int i = 0; i < allTheMapBlocks.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}
