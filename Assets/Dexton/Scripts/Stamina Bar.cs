using UnityEngine;

public class StaminaBar : MonoBehaviour
{
    public float walkSpeed = 3f, runSpeed = 5f;
    [SerializeField] private float moveSpeed = 3f;
    public float stamina, maxStamina, runCost, chargeRate;
    static public bool canSprint = true;
    public KeyCode run = KeyCode.LeftShift;

    public Rigidbody2D rb2d;
    public Animator animator;
    public Image StaminaBar;

    private Coroutine recharge;

    Vector2 movement;

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0)
        {
            //Input
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            movement.Normalize();
        }

        rb2d.linearVelocity = (movement * moveSpeed);

        if (Input.GetKey(run) && stamina > 0 && canSprint == true)
        {
            stamina -= runCost * Time.deltaTime;

            if (stamina <= 0f)
                stamina = 0f;

            StaminaBar.fillAmount = stamina / maxStamina;

            moveSpeed = runSpeed;

            StopAllCoroutines();
        }
        else if (Input.GetKeyUp(run) && canSprint == true)
        {
            moveSpeed = walkSpeed;

            StopAllCoroutines();
            StartCoroutine(RechargeStamina());
        }

        if (stamina <= 0f && canSprint)
        {
            canSprint = false;

            moveSpeed = walkSpeed;

            StopAllCoroutines();
            StartCoroutine(RechargeStamina());
        }
        else if (stamina >= maxStamina)
        {
            canSprint = true;
        }
    }

    private IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(1f);

        while (stamina < maxStamina)
        {
            stamina += chargeRate / 100f;

            if (stamina > maxStamina)
                stamina = maxStamina;

            StaminaBar.fillAmount = stamina / maxStamina;

            yield return new WaitForSeconds(.01f);
        }
    }
}
