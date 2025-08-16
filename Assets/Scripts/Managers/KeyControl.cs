using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyControl : MonoBehaviour
{
    private PlayerControl playerControl;
    [SerializeField] private RawImage keyUIImage;
    
    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        playerControl = collision.gameObject.GetComponent<PlayerControl>();
        
        if (playerControl != null)
        {
            CollectKey(collision);
        }
    }
    private void CollectKey(Collider2D collision)
    {
            Debug.Log("Key collected!");
        // add logic here to enable opening door
            playerControl.HaveKey = true; // Example of enabling a key after collection
        if (keyUIImage != null)
        {
            keyUIImage.gameObject.SetActive(true);
        }
            //playerControl.havekey = true; // Example of enabling a weapon after collecting a key
        Destroy(gameObject); // Destroy the key object after collection
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
