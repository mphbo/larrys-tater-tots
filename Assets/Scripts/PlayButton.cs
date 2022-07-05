using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) 
    {
        SceneManager.LoadScene(1);
        Debug.Log(other.gameObject.name);
        if (other.gameObject.name == "Larry")
        {
            Destroy(GameObject.Find("Greg"));
        }
        if (other.gameObject.name == "Greg")
        {
            Destroy(GameObject.Find("Larry"));
        }
    }
}
