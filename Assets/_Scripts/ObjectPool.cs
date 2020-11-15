using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool sharedInstance;
    private List<GameObject> straightRoadPool;
    private List<GameObject> rightRoadPool;
    private List<GameObject> leftRoadPool;

    [SerializeField]
    private List<GameObject> allEnemies;

    private List<GameObject> straightEnemy;
    private List<GameObject> followEnemy;
    private List<GameObject> zigzagEnemy;

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

        straightEnemy = new List<GameObject>();
        zigzagEnemy = new List<GameObject>();
        followEnemy = new List<GameObject>();

        GameObject tmp;
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < amountToPool; j++)
            {
                if (i < 3)
                {
                    tmp = Instantiate(GenerationSystem.sharedInstance.allTheMapBlocks[i]);
                }
                else
                {
                    tmp = Instantiate(allEnemies[i-3]);
                }
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
                    case 3:
                        straightEnemy.Add(tmp);
                        break;
                    case 4:
                        zigzagEnemy.Add(tmp);
                        break;
                    case 5:
                        followEnemy.Add(tmp);
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
                case 3:
                    if (!straightEnemy[i].activeInHierarchy)
                    {
                        return straightEnemy[i];
                    }
                    break;
                case 4:
                    if (!zigzagEnemy[i].activeInHierarchy)
                    {
                        return zigzagEnemy[i];
                    }
                    break;
                case 5:
                    if (!followEnemy[i].activeInHierarchy)
                    {
                        return followEnemy[i];
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
 