using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerMath))]
public class Player : MonoBehaviour
{
    // Singleton
    public static Player Instance;

    // Operaciónes matemáticas del jugador
    public PlayerMath PlayerMath { private set; get; }

    // Controlador del personaje del jugador
    public PlayerController PlayerController { private set; get; }

    // Singleton
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        else
        {
            Destroy(this);
            return;
        }

        PlayerMath = GetComponent<PlayerMath>();
        PlayerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        // Mueve al jugador
        PlayerController.Move();
    }
}
