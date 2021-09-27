using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : Unit
{
    [SerializeField]
    private float speed = 3.0F;

    [SerializeField]
    private int lives = 5;

    
    
    public int Lives
    {
        get { return lives; }
        set 
        { 
            if (value <= 5) lives = value;
            livesBar.Refresh();
        }
    }
    private LivesBar livesBar;

    [SerializeField]
    private float jumpForce = 15.0F;

    new private Rigidbody2D rigidbody;

    private Animator animator;

    private SpriteRenderer sprite;

    bool isGrounded = false;
    bool isAlive = true;

    private Bullet bullet;

    private GameObject deathScreen;


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
        livesBar = FindObjectOfType<LivesBar>();

    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && isAlive) Shoot();
        if (isGrounded && isAlive) State = CharState.Idle;
        if (Input.GetButton("Horizontal") && isAlive) Run();
        if (isGrounded && Input.GetButtonDown("Jump") && isAlive) Jump();
        if (Input.GetButtonDown("Submit") & !isAlive) SceneManager.LoadScene("SampleScene");
        if (Input.GetButtonDown("Cancel")) Application.Quit();
        
    }

    private void Run()  
    {

        if(isGrounded && isAlive) State = CharState.Run;
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

        if (!isGrounded && isAlive) State = CharState.Jump;
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
        Lives--;

        if (Lives > 0)
        {
            rigidbody.velocity = Vector3.zero; //обнуление ускорения чтоб дамажило мобов сверху
            rigidbody.AddForce(transform.up * 6.0F, ForceMode2D.Impulse);
        }
        else Die();
        
        Debug.Log(lives);
    }

    protected override void Die()
    { 
        isAlive = false;
        State = CharState.Die;

       // if(!deathScreen.activeSelf) deathScreen.SetActive(true);
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet && bullet.Parent != gameObject) ReceiveDamage();

    }
}

public enum CharState
{
    Idle,
    Run,
    Jump,
    Die
}

