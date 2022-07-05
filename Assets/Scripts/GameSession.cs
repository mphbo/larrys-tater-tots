using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;

    float timeTillRestart = 1.5f;
    bool isLarry = true;
    bool isGreg = true;

    void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
    }

    public void StartOnePlayer(string player)
    {
        if (player == "Larry")
        {
            isGreg = false;
        }
        if (player == "Greg")
        {
            isLarry = false;
        }
    }

    public bool GetIsGreg()
    {
        return isGreg;
    }

    public bool GetIsLarry()
    {
        return isLarry;
    }

    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    void ResetGameSession()
    {
        isLarry = true;
        isGreg = true;
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = score.ToString();
    }

    void TakeLife()
    {
        playerLives -= 1;
        StartCoroutine(WaitToKill());
    }

    IEnumerator WaitToKill()
    {
        yield return new WaitForSeconds(timeTillRestart);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = playerLives.ToString();
    }
}
