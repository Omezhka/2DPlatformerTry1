using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();
        if (character && character.Lives < 5)
        {
            character.Lives++;
            Destroy(gameObject);
        }
    }
}
