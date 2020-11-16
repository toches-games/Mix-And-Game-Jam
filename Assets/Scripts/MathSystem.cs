using System.Collections;
using UnityEngine;

public class MathSystem : MonoBehaviour
{
    // Singleton
    public static MathSystem Instance;
    
    // Operación del jugador con resultado correcto
    private Operation targetOperation;
    // Prefabs de los numeros del 0 al 9
    [SerializeField] private Sprite[] numberPrefabs;
    // Objects de los numeros de las operaciones
    [SerializeField] private Transform numbers;
    // Objects de los signos de las operaciones
    [SerializeField] private Transform symbols;
    [SerializeField, Range(1, 100)] private int maxNumber = 100;

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

    // Crea la imagen del numero que salió
    private void CreateNumberImage(Transform number)
    {
        char[] numberChar = number.name.ToCharArray();

        if(numberChar[0] == '-')
        {
            number.GetChild(0).gameObject.SetActive(true);
            
            for (int i = 1; i < numberChar.Length; i++)
            {
                number.GetChild(i).GetComponent<SpriteRenderer>().sprite = numberPrefabs[(int)char.GetNumericValue(numberChar[i])];
                number.GetChild(i).gameObject.SetActive(true);
            }
        }

        else
        {
            number.GetChild(0).gameObject.SetActive(false);

            for (int i = 0; i < numberChar.Length; i++)
            {
                number.GetChild(i+1).GetComponent<SpriteRenderer>().sprite = numberPrefabs[(int)char.GetNumericValue(numberChar[i])];
                number.GetChild(i+1).gameObject.SetActive(true);
            }
        }

        for(int i=numberChar.Length; i<number.childCount-1; i++)
        {
            number.GetChild(i+1).gameObject.SetActive(false);
        }

        BoxCollider2D bc = number.GetComponent<BoxCollider2D>();
        bc.offset = new Vector2(number.name.Length * 1.3f / 2f, 0f);
        bc.size = new Vector2(number.name.Length * 4f / 2f, 3f);
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
        int xCorrectPosition = Random.Range(-1, 2);
        Operation playerOperation = Player.Instance.PlayerMath.GetCurrentOperation();

        // Cuando elige un signo, solo se generan signos
        if (playerOperation != null && playerOperation.State == OperationState.Symbol)
        {
            for (int i = 0; i < symbols.childCount; i++)
            {
                int randomSymbol = Random.Range(0, symbols.childCount);
                Vector2 targetPosition = new Vector2(xPosition++ * 5f, Player.Instance.transform.position.y + 30f);

                while (symbols.GetChild(randomSymbol).gameObject.activeSelf)
                {
                    randomSymbol = Random.Range(0, symbols.childCount);
                }

                symbols.GetChild(randomSymbol).position = targetPosition;
                symbols.GetChild(randomSymbol).gameObject.SetActive(true);
            }
        }

        // Cuando elige numeros, solo elige numeros del 0 al numero maximo
        else if (playerOperation == null || playerOperation.State != OperationState.Result)
        {
            for(int i = 0; i < numbers.childCount; i++)
            {
                int randomNumber = Random.Range(0, maxNumber);
                Vector2 targetPosition = new Vector2(xPosition++ * 5f, Player.Instance.transform.position.y + 30f);

                numbers.GetChild(i).position = targetPosition;
                numbers.GetChild(i).gameObject.SetActive(true);
                numbers.GetChild(i).name = randomNumber + "";

                CreateNumberImage(numbers.GetChild(i));
            }
        }

        // Genera todos los numeros al momento de elegir resultado
        else
        {
            for(int i = 0; i < numbers.childCount; i++)
            {
                // Falta arreglar que se generen solo numeros cercanos al resultado
                //int randomNumber = targetOperation.Result + i - 1;

                int randomNumber = Random.Range(0, maxNumber * maxNumber);
                Vector2 targetPosition = new Vector2(xPosition++ * 5f, Player.Instance.transform.position.y + 30f);

                if (xPosition - 1 == xCorrectPosition)
                {
                    randomNumber = targetOperation.Result;
                    targetPosition.x = xCorrectPosition * 5f;
                }

                numbers.GetChild(i).position = targetPosition;
                numbers.GetChild(i).gameObject.SetActive(true);
                numbers.GetChild(i).name = randomNumber + "";

                CreateNumberImage(numbers.GetChild(i));
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

// Los estados que podrá tener una operación
public enum OperationState
{
    Num1,
    Num2,
    Symbol,
    Result
}

public class Operation
{
    public int Num1 { set; get; }
    public int Num2 { set; get; }
    public string Symbol { set; get; }
    public int Result { set; get; }
    public int Points { set; get; }
    public OperationState State { set; get; }

    public Operation()
    {
        State = OperationState.Num1;
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