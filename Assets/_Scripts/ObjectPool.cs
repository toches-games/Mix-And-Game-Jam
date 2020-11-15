using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool sharedInstance;
    [SerializeField]
    private List<GameObject> straightRoadPool;
    [SerializeField]
    private List<GameObject> rightRoadPool;
    [SerializeField]
    private List<GameObject> leftRoadPool;
    //[SerializeField]
    //private GameObject straightRoadToPool;
    //[SerializeField]
    //private GameObject rightRoadToPool;
    //[SerializeField]
    //private GameObject leftRoadToPool;
    [SerializeField]
    private int amountToPool;

    void Awake()
    {
        sharedInstance = sharedInstance == null ? this : sharedInstance;
    }

    void Start()
    {
        straightRoadPool = new List<GameObject>();
        rightRoadPool = new List<GameObject>();
        leftRoadPool = new List<GameObject>();

        GameObject tmp;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < amountToPool; j++)
            {
                tmp = Instantiate(GenerationSystem.sharedInstance.allTheMapBlocks[i]);
                tmp.transform.SetParent(GameObject.Find("ObjectsPool").transform);
                tmp.SetActive(false);

                switch (i)
                {
                    case 0:
                        straightRoadPool.Add(tmp);
                        break;
                    case 1:
                        rightRoadPool.Add(tmp);
                        break;
                    case 2:
                        leftRoadPool.Add(tmp);
                        break;
                    default:
                        break;
                }
            }
        }

    }


    /// <summary>
    /// Devuelve un objeto de la pool
    /// </summary>
    /// <param name="type"> 
    /// tipo de objeto: 
    /// 0: Recta
    /// 1: Curva derecha
    /// 2: Curva izquierda
    /// </param>
    /// <returns></returns>
    public GameObject GetPooledObject(int type)
    {
        for (int i = 0; i < amountToPool; i++)
        {
            switch (type)
            {
                case 0:
                    if (!straightRoadPool[i].activeInHierarchy)
                    {
                        return straightRoadPool[i];
                    }
                    break;
                case 1:
                    if (!rightRoadPool[i].activeInHierarchy)
                    {
                        return rightRoadPool[i];
                    }
                    break;
                case 2:
                    if (!leftRoadPool[i].activeInHierarchy)
                    {
                        return leftRoadPool[i];
                    }
                    break;
                default:
                    break;
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
 