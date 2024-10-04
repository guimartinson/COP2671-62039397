using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public List<GameObject> targets;     
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public GameObject titleScreen;
    private int score;
    float spawnRate = 1.0f;

    public bool isGameActive;



    void Update()
    {
       
    }

    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index]);
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }



    public void GameOver()
    {
        restartButton.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        isGameActive = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void StartGame(int difficulty)
    {
        score = 0;
        isGameActive = true;
        spawnRate = spawnRate / difficulty;

        StartCoroutine(SpawnTarget());
        UpdateScore(0);
        

        titleScreen.gameObject.SetActive(false);
    }


}
