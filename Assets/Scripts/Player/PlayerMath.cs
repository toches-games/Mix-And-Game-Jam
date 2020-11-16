using UnityEngine;

public class PlayerMath : MonoBehaviour
{
    // Guarda la operación que está creando el jugador
    private Operation currentOperation;

    // Efecto de particulas
    [SerializeField] private ParticleSystem particles;

    // Asigna el primer numero de la operación del jugador
    public void SetOperationNum1(int num1)
    {
        // Cuando se agrega el primer numero de una nueva operación se crea una nueva operación
        // y se borra la actual
        currentOperation = new Operation();
        currentOperation.Num1 = num1;
        currentOperation.State = OperationState.Symbol;
    }

    // Asigna el signo de la operación del jugador
    public void SetOperationSymbol(string symbol)
    {
        currentOperation.Symbol = symbol;
        currentOperation.State = OperationState.Num2;
    }

    // Asigna el segundo numero de la operación del jugador
    public void SetOperationNum2(int num2)
    {
        currentOperation.Num2 = num2;
        currentOperation.State = OperationState.Result;

        // Cuando el jugador agarra el segundo numero de la operación se crea
        // la solución en el MathSystem para comprobar el resultado
        MathSystem.Instance.GenerateNewOperation(currentOperation);
    }

    // Asigna el resultado de la operación del jugador
    public void SetOperationResult(int result)
    {
        currentOperation.Result = result;
        currentOperation.State = OperationState.Num1;
    }

    // Asigna los puntos que dará la operación por ser resuelta
    public void SetOperationPoints(int points)
    {
        currentOperation.Points = points;
    }

    // Regresa la operación que está creando el jugador
    public Operation GetCurrentOperation()
    {
        return currentOperation;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Math")) return;

        particles.Play();

        // Si la operación no tiene el primer numero se le asigna cuando choca con tag "Math"
        if (currentOperation == null || currentOperation.State == OperationState.Num1)
        {
            SetOperationNum1(int.Parse(collision.name));
        }

        // Si la operación no tiene un simbolo y lo que choca no es un numero
        else if(currentOperation.State == OperationState.Symbol && !char.IsDigit(collision.name[0]))
        {
            SetOperationSymbol(collision.name);
        }

        // Si el estado de la operación es agregar el segundo numero entonces lo agrega
        // cuando choca con algo que tiene el tag "Math"
        else if(currentOperation.State == OperationState.Num2)
        {
            SetOperationNum2(int.Parse(collision.name));
        }

        // Si es momento de agregar el resultado cuando choca, lo agrega a la operación
        else if(currentOperation.State == OperationState.Result)
        {
            SetOperationResult(int.Parse(collision.name));

            // Para comparar el resultado de la operación del jugador con la operación correcta
            // retorna true (si son mismo resultado) y false (si son diferentes)
            print(MathSystem.Instance.ComparePlayerOperation(currentOperation));

            // Para obtener la operación correcta
            // (retorna un objeto de la clase Operation)
            // Los puntos solo se obtienen de la operación correcta
            Operation o = MathSystem.Instance.GetCurrentOperation();
            Debug.LogWarning("--- Operación correcta ---");
            Debug.LogWarning(o.Num1 + " --- " + o.Symbol + " --- " + o.Num2 + " --- " + o.Result + " --- " + o.Points);
        }

        MathSystem.Instance.InstantiateNumbers();

        print("--- Operación del jugador ---");
        print(currentOperation.Num1 + " --- " +
           currentOperation.Symbol + " --- " +
           currentOperation.Num2 + " --- " +
           currentOperation.Result);
    }
}
