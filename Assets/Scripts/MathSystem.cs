using UnityEngine;

public class MathSystem : MonoBehaviour
{
    // Singleton
    public static MathSystem Instance;
    
    // Operación del jugador con resultado correcto
    private Operation targetOperation;

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