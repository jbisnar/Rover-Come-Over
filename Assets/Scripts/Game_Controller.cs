using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game_Controller : MonoBehaviour
{
    public GameObject rover;
    bool scoreCounting = true;
    float score;
    float difficultyIncrement = 50;
    float nextDifficultyScore = 50;
    public Text scoretext;
    float distanceIncrement = 8;
    float nextSpawnDistance = 0;
    float distanceStartOffset;
    float distanceRaw = 0;
    float distanceSaved = 0;
    float distanceBatch = 0;
    float startBatchDistance = 0;
    int difficultyPoints = 4;
    int currentDifficultyPoints = 4;
    List<GameObject> enemies;
    int[] spawnpoints;
    public GameObject Homing;
    public GameObject Shotgun;
    public GameObject Machinegun;
    public GameObject Mine;
    public GameObject Baller;
    public GameObject Sniper;
    public Transform tl;
    public Transform br;

    public GameObject GameUI;
    public GameObject PauseUI;
    bool paused = false;
    public GameObject GameOverUI;
    public Text gameOverScoreText;

    // Start is called before the first frame update
    void Start()
    {
        enemies = new List<GameObject>();
        enemies.Add(Homing);
        enemies.Add(Shotgun);
        enemies.Add(Machinegun);
        enemies.Add(Mine);
        enemies.Add(Baller);
        enemies.Add(Sniper);
        spawnpoints = new int[] {2,2,3,1,2,3};

        distanceStartOffset = rover.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        distanceRaw = rover.transform.position.y - distanceStartOffset;
        if (scoreCounting)
        {
            distanceBatch = distanceRaw - startBatchDistance;
        }
        else
        {
            distanceBatch = 0;
        }
        score = distanceSaved + distanceBatch;
        if (score > nextDifficultyScore)
        {
            nextDifficultyScore += difficultyIncrement;
            difficultyPoints++;
        }
        scoretext.text = "Distance traveled: " +Mathf.Floor(score*10)/10+"m";
        gameOverScoreText.text = "Distance traveled: " + Mathf.Floor(score * 10) / 10 + "m";
        if (distanceRaw > nextSpawnDistance)
        {
            SpawnBatch();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void SpawnBatch()
    {
        currentDifficultyPoints = difficultyPoints;
        var pspawnNumber = Random.Range(0,3);
        var pspawnLoc = new Vector2(Random.Range(tl.position.x, br.position.x),Random.Range(tl.position.y,br.position.y));
        var pspawn = GameObject.Instantiate(enemies[pspawnNumber], pspawnLoc, transform.rotation, null);
        currentDifficultyPoints -= spawnpoints[pspawnNumber];
        var espawnNumber = Random.Range(3, 6);
        var espawnLoc = new Vector2(Random.Range(tl.position.x, br.position.x), Random.Range(tl.position.y, br.position.y));
        var espawn = GameObject.Instantiate(enemies[espawnNumber], espawnLoc, transform.rotation, null);
        currentDifficultyPoints -= spawnpoints[espawnNumber];
        while (currentDifficultyPoints > 0)
        {
            var aspawnNumber = Random.Range(0, 6);
            var aspawnLoc = new Vector2(Random.Range(tl.position.x, br.position.x), Random.Range(tl.position.y, br.position.y));
            var aspawn = GameObject.Instantiate(enemies[aspawnNumber], aspawnLoc, transform.rotation, null);
            currentDifficultyPoints -= spawnpoints[aspawnNumber];
        }
        nextSpawnDistance += distanceIncrement;
    }

    public void StopCounting()
    {
        distanceSaved += distanceBatch;
        startBatchDistance = distanceRaw;
        scoreCounting = false;
    }

    public void StartCounting()
    {
        startBatchDistance = Mathf.Max(distanceRaw,startBatchDistance);
        scoreCounting = true;
    }

    public void Pause()
    {
        PauseUI.SetActive(true);
        paused = true;
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        PauseUI.SetActive(false);
        paused = false;
        Time.timeScale = 1f;
    }
    public void GameOver()
    {
        GameUI.SetActive(false);
        GameOverUI.SetActive(true);
    }
    public void Restart()
    {
        Debug.Log("Restarting");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Quit()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
