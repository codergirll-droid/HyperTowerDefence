using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    Camera cam;
    public Transform[] spawnPoints;
    public GameObject stickmanPrefab;
    public Text scoreText;
    public Text highScoreText;

    public GameObject waveWarning;
    public GameObject gameOverPanel;
    public GameObject gamePanel;
    public Text scoreTextGameOver;
    public Text highScoreTextGameOver;
    public GameObject gameObjects;

    LayerMask layerMask;

    int score = 0;
    bool isGameOver = false;

    public static GameManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }


        cam = Camera.main;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore").ToString();
        }
        else
        {
            PlayerPrefs.SetInt("HighScore", 0);
            highScoreText.text = "High Score: 0";
        }

        scoreText.text = "Score: 0";

        layerMask |= (1 << LayerMask.NameToLayer("stickman"));

        IEnumerator c = RandomStickmanSpawner(Random.Range(1, 2));
        StartCoroutine(c);

        IEnumerator a = RandomStickmanWaveSpawner(Random.Range(10, 40), Random.Range(0.2f, 0.5f), 10);
        StartCoroutine(a);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            
            if(Physics.Raycast(ray, out hit, 100, layerMask))
            {

                if (hit.collider.gameObject.CompareTag("stickman"))
                {
                    hit.collider.gameObject.GetComponent<NavMeshAgent>().enabled = false;

                    hit.collider.gameObject.GetComponent<AudioSource>().Play();
                    hit.collider.gameObject.GetComponent<Stickman>().EnableRagdoll(true);
                    score++;
                    scoreText.text = "Score: " + score.ToString();

                    Destroy(hit.collider.gameObject, 3f);
                }
            }

        }
    }

    IEnumerator RandomStickmanSpawner(float randomSpawnRate)
    {
        if (!isGameOver)
        {
            int randomSpawnPoint = Random.Range(0, spawnPoints.Length);
            Instantiate(stickmanPrefab, spawnPoints[randomSpawnPoint].position, Quaternion.identity, gameObjects.transform);
            yield return new WaitForSeconds(randomSpawnRate);

            randomSpawnRate = Random.Range(1, 2);

            IEnumerator c = RandomStickmanSpawner(randomSpawnRate);
            StartCoroutine(c);
        }

    }

    IEnumerator RandomStickmanWaveSpawner(float randomWaveSpawnRate, float randomSpawnRate, int spawnAmount)
    {
        if (!isGameOver)
        {
            yield return new WaitForSeconds(randomWaveSpawnRate);
            StartCoroutine(WaveWarning());

            int randomSpawnPoint = Random.Range(0, spawnPoints.Length);
            int tempSpawnAmount = spawnAmount;
            while (tempSpawnAmount > 0)
            {
                Instantiate(stickmanPrefab, spawnPoints[randomSpawnPoint].position, Quaternion.identity, gameObjects.transform);
                tempSpawnAmount--;
                yield return new WaitForSeconds(randomSpawnRate);
            }

            randomSpawnRate = Random.Range(0.2f, 0.5f);
            randomWaveSpawnRate = Random.Range(10, 40);

            IEnumerator c = RandomStickmanWaveSpawner(randomWaveSpawnRate, randomSpawnRate, spawnAmount);
            StartCoroutine(c);
        }

    }

    IEnumerator WaveWarning()
    {
        waveWarning.SetActive(true);
        yield return new WaitForSeconds(3);
        waveWarning.SetActive(false);
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true);
        gamePanel.SetActive(false);
        gameObjects.SetActive(false);

        if (PlayerPrefs.HasKey("HighScore"))
        {
            if(PlayerPrefs.GetInt("HighScore") < score)
            {
                PlayerPrefs.SetInt("HighScore", score);
            }
        }

        scoreTextGameOver.text =  "Score: " + score.ToString();
        highScoreTextGameOver.text =  "High Score: " + PlayerPrefs.GetInt("HighScore");

    }

    public void PlayAgainBtn()
    {
        SceneManager.LoadScene(0);
    }

}
