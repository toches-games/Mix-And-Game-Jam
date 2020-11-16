using System.Collections;
using UnityEngine;

public class MathSystem : MonoBehaviour
{
    // Singleton
    public static MathSystem Instance;
    
    // Operación del jugador con resultado correcto
    private Operation targetOperation;
    // Prefabs de los numeros del 0 al 9
    [SerializeField] private SpriteRenderer[] numberPrefabs;
    // Objects de los numeros de las operaciones
    [SerializeField] private Transform numbers;
    // Objects de los signos de las operaciones
    [SerializeField] private Transform symbols;
    [SerializeField, Range(1, 5)] private int numbersCount = 3;

    // Singleton
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        CreateNumberImages();
        InstantiateNumbers();
        StartCoroutine(CheckNumbers());
    }

    // Verifica si el jugador no agarra ningun numero
    private IEnumerator CheckNumbers()
    {
        while (true)
        {
            if(GameObject.FindGameObjectWithTag("Math").transform.position.y < Player.Instance.transform.position.y - 5f)
            {
                InstantiateNumbers();
            }

            yield return new WaitForSeconds(1f);
        }
    }

    // Oculta los numeros que hay en pantalla, para que no pueda agarrar varios a la vez
    private void DestroyNumbers()
    {
        GameObject[] instantiatedNumbers = GameObject.FindGameObjectsWithTag("Math");

        foreach(GameObject go in instantiatedNumbers)
        {
            go.SetActive(false);
        }
    }

    // Crea tres numeros en la pantalla
    public void InstantiateNumbers()
    {
        DestroyNumbers();

        int xPosition = -1;

        for (int i = 0; i < numbersCount; i++)
        {
            Vector2 targetPosition = new Vector2(xPosition++ * 5f, Player.Instance.transform.position.y + 30f);
            Operation playerOperation = Player.Instance.PlayerMath.GetCurrentOperation();

            // Cuando elige un signo, solo se generan signos
            if (playerOperation != null && playerOperation.State == "Symbol")
            {
                int randomSymbol = Random.Range(0, symbols.childCount);

                while (symbols.GetChild(randomSymbol).gameObject.activeSelf && symbols.GetChild(randomSymbol).position.y > Player.Instance.transform.position.y - 5f)
                {
                    randomSymbol = Random.Range(0, symbols.childCount);
                }

                symbols.GetChild(randomSymbol).position = targetPosition;
                symbols.GetChild(randomSymbol).gameObject.SetActive(true);
            }

            // Cuando elige numeros, solo elige numeros del 0 al 10
            else if (playerOperation == null || playerOperation.State != "Result")
            {
                int randomNumber = Random.Range(0, 11);

                while (numbers.GetChild(randomNumber).gameObject.activeSelf && numbers.GetChild(randomNumber).position.y > Player.Instance.transform.position.y - 5f)
                {
                    randomNumber = Random.Range(0, 11);
                }

                numbers.GetChild(randomNumber).position = targetPosition;
                numbers.GetChild(randomNumber).gameObject.SetActive(true);
            }

            // Genera todos los numeros al momento de elegir resultado
            else
            {
                int randomNumber = Random.Range(0, numbers.childCount);

                while (numbers.GetChild(randomNumber).gameObject.activeSelf && numbers.GetChild(randomNumber).position.y > Player.Instance.transform.position.y - 5f)
                {
                    randomNumber = Random.Range(0, numbers.childCount);
                }

                numbers.GetChild(randomNumber).position = targetPosition;
                numbers.GetChild(randomNumber).gameObject.SetActive(true);
            }
        }
    }

    // Crea los numeros que saldrán como imagenes
    private void CreateNumberImages()
    {
        for (int i = 0; i <= 100; i++)
        {
            GameObject go = new GameObject(i + "");
            go.tag = "Math";
            go.transform.SetParent(numbers);
            go.transform.localScale = Vector2.one * 0.4f;
            go.SetActive(false);

            if (i < 10)
            {
                SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = numberPrefabs[i].sprite;

                BoxCollider2D bc = go.AddComponent<BoxCollider2D>();
                bc.isTrigger = true;
            }

            else
            {
                BoxCollider2D bc = go.AddComponent<BoxCollider2D>();
                bc.isTrigger = true;

                char[] numString = i.ToString().ToCharArray();

                for(int j=0; j < numString.Length; j++)
                {
                    int digitValue = (int)char.GetNumericValue(numString[j]);
                    GameObject go1 = new GameObject(digitValue + "");
                    SpriteRenderer sr1 = go1.AddComponent<SpriteRenderer>();
                    sr1.sprite = numberPrefabs[digitValue].sprite;
                    go1.transform.SetParent(go.transform);
                    go1.transform.localScale = Vector2.one;

                    if (i < 100)
                    {
                        if(j == 0) go1.transform.localPosition = new Vector2(-1.5f, 0f);
                        else go1.transform.localPosition = new Vector2(1.5f, 0f);

                        bc.size = new Vector2(5.5f, 4.5f);
                    }

                    else if(i == 100)
                    {
                        if(j == 0) go1.transform.localPosition = new Vector2(-3f, 0f);
                        else if(j == 1) go1.transform.localPosition = new Vector2(0f, 0f);
                        else go1.transform.localPosition = new Vector2(3f, 0f);

                        bc.size = new Vector2(8.5f, 4.5f);
                    }
                }
            }
        }
    }

    // Crea la operación con el resultado correcto apartir de la operación del jugador
    public void GenerateNewOperation(Operation playerOperation)
    {
        targetOperation = new Operation(playerOperation);
    }

    // Regresa la operación con el resultado correcto
    public Operation GetCurrentOperation()
    {
        return targetOperation;
    }

    // Compara si son iguales o no la operación correcta y la del jugador (solo compara el resultado)
    public bool ComparePlayerOperation(Operation playerOperation)
    {
        return targetOperation.Compare(playerOperation);
    }
}

public class Operation
{
    public int Num1 { set; get; }
    public int Num2 { set; get; }
    public string Symbol { set; get; }
    public int Result { set; get; }
    public int Points { set; get; }
    public string State { set; get; }

    public Operation()
    {
        State = "Num1";
    }

    // Crea una operación apartir de otra, se usa solo para crear la operación
    // con el resultado correcto (la crea apartir de la operación del jugador)
    public Operation(Operation other)
    {
        Num1 = other.Num1;
        Num2 = other.Num2;
        Symbol = other.Symbol;
        GenerateResult();
    }

    // Compara el resultado de dos operaciones
    public bool Compare(Operation other)
    {
        return Result == other.Result;
    }

    // Crea el resultado de la operación dependiendo del signo
    private void GenerateResult()
    {
        switch (Symbol)
        {
            case "+":
                Result = Num1 + Num2;
                Points = 100;
                break;
            case "-":
                Result = Num1 - Num2;
                Points = 250;
                break;
            case "*":
                Result = Num1 * Num2;
                Points = 500;
                break;
            default:
                Result = 0;
                Points = 0;
                break;
        }
    }
}