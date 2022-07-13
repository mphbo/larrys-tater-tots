using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointButton : MonoBehaviour
{
    GameSession gameSession;

    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        // if (other.gameObject.name == "Greg")
        // {
        //     Debug.Log("HIT");
        //     gameSession.SetGreg();
        // }
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
