using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallManager : MonoBehaviour
{
    private PlayerControl playerControl;
    // private void Start()
    // {
    //     playerControl = FindObjectOfType<PlayerControl>();
    //     if (playerControl == null)
    //     {
    //         Debug.LogError("PlayerControl not found in the scene.");
    //     }
    // }
    // Start is called before the first frame update
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerControl>() != null)
        {
            playerControl = collision.gameObject.GetComponent<PlayerControl>();
            if (playerControl != null)
            {
                Debug.Log("Player has fallen!");
                playerControl.IsDead = true; // Set the player's dead state
            }
            else
            {
                Debug.LogError("PlayerControl component not found on the collided object.");
            }
        }
    }

}
