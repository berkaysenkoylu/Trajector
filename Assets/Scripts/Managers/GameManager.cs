using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverScreen;

    //bool gameOver = false;

    void Start()
    {
        GameIsOver.OnGameOver += OnGameOver;
    }

    private void OnDestroy()
    {
        GameIsOver.OnGameOver -= OnGameOver;
    }

    void Update()
    {

    }

    void OnGameOver()
    {
        // Set the game over flag
        //gameOver = true;

        // Freeze the game
        Time.timeScale = 0;

        // Make the pause / menu visible
        gameOverScreen.SetActive(true);
    }

    public void RestartGame()
    {
        StartCoroutine("RestartProcess");
    }

    IEnumerator RestartProcess()
    {
        gameOverScreen.GetComponent<Animator>().SetTrigger("gameStarted");

        yield return new WaitForSecondsRealtime(1.0f);

        gameOverScreen.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Time.timeScale = 1;
    }
}
