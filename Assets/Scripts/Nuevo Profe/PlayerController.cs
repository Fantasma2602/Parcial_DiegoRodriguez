using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private WeatherSystem weatherSystem;

    [Header("Movement")]
    public float walkingSpeed;
    public float runningSpeed;
    public float groundDrag;

    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;

    private Rigidbody rb;

    public CharacterState currentState;
    public Coroutine attackCoroutine;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            weatherSystem.SetWeather(Weather.Sunny);

        if (Input.GetKeyDown(KeyCode.X))
            weatherSystem.SetWeather(Weather.Cloudy);

        if (Input.GetKeyDown(KeyCode.C))
            weatherSystem.SetWeather(Weather.Rainny);

        switch (currentState)
        {
            case CharacterState.Idle:
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        ChangeCharacterState(CharacterState.Attacking);
                        break;
                    }

                    if (Input.GetKey(KeyCode.W) ||
                        Input.GetKey(KeyCode.A) ||
                        Input.GetKey(KeyCode.S) ||
                        Input.GetKey(KeyCode.D))
                    {
                        ChangeCharacterState(CharacterState.Walking);
                    }

                    break;
                }

            case CharacterState.Walking:
                {
                    if (!IsMoving())
                    {
                        ChangeCharacterState(CharacterState.Idle);
                        break;
                    }

                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        ChangeCharacterState(CharacterState.Running);
                        break;
                    }

                    MyInput();
                    rb.linearDamping = groundDrag;

                    break;
                }

            case CharacterState.Running:
                {
                    if (!IsMoving())
                    {
                        ChangeCharacterState(CharacterState.Idle);
                        break;
                    }

                    if (!Input.GetKey(KeyCode.LeftShift))
                    {
                        ChangeCharacterState(CharacterState.Walking);
                        break;
                    }

                    MyInput();
                    rb.linearDamping = groundDrag;

                    break;
                }

            case CharacterState.Attacking:
                {
                    if (attackCoroutine == null)
                    {
                        attackCoroutine = StartCoroutine(AttackCoroutine());
                    }

                    break;
                }
        }
    }

    public void ChangeCharacterState(CharacterState newState)
    {
        currentState = newState;
    }

    private IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(1f);

        attackCoroutine = null;
        ChangeCharacterState(CharacterState.Idle);
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        MovePlayer();
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        float currentSpeed = currentState == CharacterState.Running
            ? runningSpeed
            : walkingSpeed;

        rb.AddForce(moveDirection.normalized * currentSpeed * 10f, ForceMode.Force);
    }

    private bool IsMoving()
    {
        return Input.GetKey(KeyCode.W) ||
               Input.GetKey(KeyCode.A) ||
               Input.GetKey(KeyCode.S) ||
               Input.GetKey(KeyCode.D);
    }
}

[System.Serializable]
public enum CharacterState
{
    Idle,
    Walking,
    Running,
    Attacking,
}