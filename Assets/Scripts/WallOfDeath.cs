using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WallOfDeath : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Character")
        {
            collision.gameObject.GetComponent<Character>().Lives = -5;
            ResolutionAndExitManager re = new ResolutionAndExitManager();
            re.Restart();
        }
    }
}
