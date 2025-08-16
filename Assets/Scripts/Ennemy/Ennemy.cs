using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : MonoBehaviour
{
    private PlayerControl playerControl;
    [SerializeField] private float speed = 2f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        playerControl = collision.gameObject.GetComponent<PlayerControl>();
        
        if (playerControl != null)
        {
            // Logic to handle enemy 
            Debug.Log("Player has encountered an enemy!");
            
            playerControl.IsDead = true; 
            
        }
    }
}
