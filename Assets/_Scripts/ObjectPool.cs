using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool sharedInstance;
    public List<GameObject> pooledObjects;
    //public GameObject objectToPool;
    public int amountToPool;

    void Awake()
    {
        sharedInstance = sharedInstance == null ? this : sharedInstance;
    }

    void Start()
    {
        /**
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
        **/


    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
    /** Codigo para script donde se vaya a instanciar el objeto
    
    GameObject bullet = ObjectPool.sharedInstance.GetPooledObject(); 
    if (bullet != null) 
    { 
        bullet.transform.position = turret.transform.position; 
        bullet.transform.rotation = turret.transform.rotation; 
        bullet.SetActive(true); 
    }

    //Dentro del script del objeto a instanciar para destruirlo lo desactivamos
    gameobject.SetActive(false);
    **/
    
}
 