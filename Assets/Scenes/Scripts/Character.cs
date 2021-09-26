using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Unit
{
    [SerializeField]
    private float speed = 3.0F;

    [SerializeField]
    private int lives = 5;

    [SerializeField]
    private float jumpForce = 15.0F;

    new private Rigidbody2D rigidbody;

    private Animator animator;

    private SpriteRenderer sprite;

    bool isGrounded = false;

    private Bullet bullet;

    private CharState State
    {
        get { return (CharState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        bullet = Resources.Load<Bullet>("Bullet");

    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1")) Shoot();
        if (isGrounded) State = CharState.Idle;
        if (Input.GetButton("Horizontal")) Run();
        if (isGrounded && Input.GetButtonDown("Jump")) Jump();
    }

    private void Run()  
    {

        if(isGrounded) State = CharState.Run;
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

        sprite.flipX = direction.x < 0.0F;

    }

    private void Jump()
    {
        rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3F);

        isGrounded = colliders.Length > 1;

        if (!isGrounded) State = CharState.Jump;
    }

    private void Shoot()
    {
        Vector3 position = transform.position;      position.y += 0.8F;
        Bullet newBullet = Instantiate(bullet, position, bullet.transform.rotation);

        newBullet.Direction = newBullet.transform.right * (sprite.flipX ? -1.0F : 1.0F);

        newBullet.Parent = gameObject;
    
    }

    public override void ReceiveDamage()
    {
        lives--;
        Debug.Log(lives);

        rigidbody.velocity = Vector3.zero; //обнуление ускорения чтоб дамажило и сверху
        rigidbody.AddForce(transform.up * 6.0F, ForceMode2D.Impulse);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Unit unit = collision.gameObject.GetComponent<Unit>();
        //if (unit) ReceiveDamage();

    }
}

public enum CharState
{
    Idle,
    Run,
    Jump
}

