using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melon : MonoBehaviour
{
    [SerializeField] int health = 1;

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            FindObjectOfType<Player>().GainHealth(health);
            Destroy(gameObject);     
        }
    }

}