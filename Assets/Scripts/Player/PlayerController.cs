using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerController : MonoBehaviour
{

    public static PlayerController sharedInstance;

    #region inspector variables

    // Velocidad inicial que tendrá el jugador
    [SerializeField, Range(1f, 5f)] private float initialSpeed = 3f;
    // Velocidad maxima que podrá alcanzar
    [SerializeField, Range(5f, 20f)] private float maxSpeed = 10f;
    // Aceleracion del jugador, para alcanzar la velocidad maxima
    [SerializeField, Range(0.1f, 1f)] private float acceleration = 2f;

    #endregion

    #region private variables

    private Rigidbody2D rig;
    // Velocidad actual del jugador
    private float currentSpeed;

    // for smoothdamp
    private float currentVelocity;
    private float xSmoothDirection;

    // Dice si chocó o no
    private bool hit;

    // Tiempo desde que chocó
    private float hitTime;

    #endregion

    public int Lives { get; set; } = 3;

    private void Awake()
    {
        sharedInstance = sharedInstance == null ? this : sharedInstance;
        rig = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Physics2D.gravity = Vector2.zero;
        currentSpeed = initialSpeed;
    }

    // Hace que el jugador se mueva y se recupere del choque para que siga andando
    // hacia adelante
    public void Move()
    {
        if (!hit)
        {
            Movement();
            Rotation();
        }

        else
        {
            HitChecker();
        }
    }

    // Hace que el jugador se recupere del choque un tiempo despues
    private void HitChecker()
    {
        hitTime += Time.deltaTime;

        if (hitTime >= 0.3f)
        {
            hitTime = 0f;
            hit = false;
        }
    }

    // Rotación el jugador
    private void Rotation()
    {
        rig.rotation = currentSpeed * 2f * -MovementDirection().x;
    }

    // Movimiento del jugador
    private void Movement()
    {
        // Hace que el jugador pueda frenar
        if(Input.GetAxisRaw("Vertical") < 0)
        {
            currentSpeed -= acceleration * Time.deltaTime;
        }

        else
        {
            currentSpeed += acceleration * Time.deltaTime;
        }

        // Mantiene la velocidad del jugador entre la velocidad inicial cuando frena y la maxima
        // mientras sigue andando
        currentSpeed = Mathf.Clamp(currentSpeed, initialSpeed, maxSpeed);
        rig.velocity = MovementDirection() * currentSpeed;
    }

    // Regresa la dirección hacia donde se moverá el jugador
    private Vector2 MovementDirection()
    {
        xSmoothDirection = Mathf.SmoothDamp(xSmoothDirection, Input.GetAxisRaw("Horizontal"), ref currentVelocity, 0.2f);
        return new Vector2(xSmoothDirection, 1f);
    }

    // Cuando el jugador choca solo con un coche enemigo
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            rig.velocity = direction * -currentSpeed;

            if (Lives >= 0)
            {
                Lives--;
                UIManager.SI.LoseLife();
            }

            //TODO animacion de perder vida
            hit = true;
        }
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
}
