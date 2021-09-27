using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 10.0F;

    private SpriteRenderer sprite;

    private Vector3 direction; 

    public Vector3 Direction { set { direction = value; } }

    private GameObject parent;

    public GameObject Parent
    {
        get { return parent; }
        set { parent = value; }
    }

    public Color Color
    {
        set { sprite.color = value; }
    }

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }
    private void Start()
    {
        Destroy(gameObject, 2.0F);
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Unit unit = collision.GetComponent<Unit>();
        if (unit && unit.gameObject != parent)
        { 
            Destroy(gameObject);
        }
    }
}
