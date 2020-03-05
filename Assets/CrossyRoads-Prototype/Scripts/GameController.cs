using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using RoadCrossing.Types;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public Transform[] playerObjects;
    public int currentPlayer;
    internal int index = 0;
    public float respawnTime = 1.2f;
    public Transform respawnObject;
    public Transform cameraObject;
    public Transform deathLineObject;
    internal float deathLineTargetPosX;
    public float deathLineSpeed = 1;
    public float deathLineSpeedIncrease = 1;
    public float deathLineSpeedMax = 1.5f;
    static Vector3 targetPosition;
    public int lives = 1;
    public Transform livesText;
    public int score = 0;
    public Transform scoreText;
    internal int highScore = 0;
    internal int scoreMultiplier = 1;
    public string coinsPlayerPrefs = "Coins";
    public float gameSpeed = 1;
    public int levelUpEveryCoins = 5;
    internal int increaseCount = 0;
    public Transform gameCanvas;
    public Transform pauseCanvas;
    public Transform gameOverCanvas;
    public Transform victoryCanvas;
    internal bool isGameOver = false;
    public string mainMenuLevelName = "MainMenu";
    public string confirmButton = "Submit";
    public string pauseButton = "Cancel";
    internal bool isPaused = false;
    void Start()
    {
        Instance = this;
        ChangeScore(0);
        StartCoroutine(ChangeLives(0));

        currentPlayer = PlayerPrefs.GetInt("CurrentPlayer", currentPlayer);
        SetPlayer(currentPlayer);
        if (cameraObject == null)
            cameraObject = GameObject.FindGameObjectWithTag("MainCamera").transform;
        if (gameOverCanvas)
            gameOverCanvas.gameObject.SetActive(false);
        if (victoryCanvas)
            gameOverCanvas.gameObject.SetActive(false);
        highScore = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_HighScore", 0);
        if (deathLineObject)
            deathLineTargetPosX = deathLineObject.position.x;
    }
    void Update()
    {
        if (isGameOver == true)
        {
            if (Input.GetButtonDown(confirmButton))
            {
                Restart();
            }
            if (Input.GetButtonDown(pauseButton))
            {
                MainMenu();
            }
        }
        else
        {
            if (Input.GetButtonDown(pauseButton))
            {
                if (isPaused == true)
                    Unpause();
                else
                    Pause();
            }
        }

        if (cameraObject)
        {
            if (playerObjects[currentPlayer] && playerObjects[currentPlayer].gameObject.activeSelf == true)
                cameraObject.position = new Vector3(Mathf.Lerp(cameraObject.position.x, playerObjects[currentPlayer].position.x, Time.deltaTime * 3), 0, Mathf.Lerp(cameraObject.position.z, playerObjects[currentPlayer].position.z, Time.deltaTime * 3));
            else if (respawnObject && respawnObject.gameObject.activeSelf == true)
                cameraObject.position = new Vector3(Mathf.Lerp(cameraObject.position.x, respawnObject.position.x, Time.deltaTime * 3), 0, Mathf.Lerp(cameraObject.position.z, respawnObject.position.z, Time.deltaTime * 3));

            if (deathLineObject)
            {
                if (cameraObject.position.x > deathLineTargetPosX)
                    deathLineTargetPosX = cameraObject.position.x;
                if (isGameOver == false)
                    deathLineTargetPosX += deathLineSpeed * Time.deltaTime;
                Vector3 newVector3 = new Vector3(deathLineTargetPosX, deathLineObject.position.y, deathLineObject.position.z);
                deathLineObject.position = Vector3.Lerp(deathLineObject.position, newVector3, Time.deltaTime * 0.5f);
            }
        }
    }

    void SetPlayer(int playerNumber)
    {
        if (respawnObject) respawnObject.gameObject.SetActive(false);
        for (index = 0; index < playerObjects.Length; index++)
        {
            if (index != playerNumber)
                playerObjects[index].gameObject.SetActive(false);
            else
                playerObjects[index].gameObject.SetActive(true);
        }
    }
     
    void ChangeScore(int changeValue)
    {
        score += changeValue * scoreMultiplier;

        if (scoreText)
            scoreText.GetComponent<Text>().text = score.ToString();

        increaseCount += changeValue;

        if (increaseCount >= levelUpEveryCoins)
        {
            increaseCount -= levelUpEveryCoins;

            LevelUp();
        }
    }


    void SetScoreMultiplier(int setValue)
    {
        scoreMultiplier = setValue;
    }



    void Pause()
    {
        isPaused = true;

        Time.timeScale = 0;

        if (pauseCanvas)
            pauseCanvas.gameObject.SetActive(true);

        if (gameCanvas)
            gameCanvas.gameObject.SetActive(false);
    }


    void Unpause()
    {
        isPaused = false;

        Time.timeScale = gameSpeed;

        if (pauseCanvas)
            pauseCanvas.gameObject.SetActive(false);

        if (gameCanvas)
            gameCanvas.gameObject.SetActive(true);
    }

    IEnumerator ChangeLives(int changeValue)
    {
        lives += changeValue;

        if (livesText) livesText.GetComponent<Text>().text = lives.ToString();

        if (lives <= 0) StartCoroutine(GameOver(0.5f));
        else if (playerObjects[currentPlayer] && changeValue < 0)
        {

            if (respawnObject)
            {
                respawnObject.gameObject.SetActive(true);

                respawnObject.position = playerObjects[currentPlayer].position;

                respawnObject.rotation = playerObjects[currentPlayer].rotation;

                respawnObject.SendMessage("Spawn");
            }

            yield return new WaitForSeconds(respawnTime);

            if (playerObjects[currentPlayer].gameObject.activeSelf == false)
            {
                playerObjects[currentPlayer].gameObject.SetActive(true);
             playerObjects[currentPlayer].SendMessage("Spawn");

                if (respawnObject)
                {
                    targetPosition = respawnObject.position;

                    playerObjects[currentPlayer].position = targetPosition;

                    playerObjects[currentPlayer].rotation = respawnObject.rotation;

                    respawnObject.gameObject.SetActive(false);
                }
            }
        }
    }

    IEnumerator GameOver(float delay)
    {
        yield return new WaitForSeconds(delay);

        isGameOver = true;

        if (pauseCanvas)
            Destroy(pauseCanvas.gameObject);

        if (gameCanvas)
            Destroy(gameCanvas.gameObject);

        int totalCoins = PlayerPrefs.GetInt(coinsPlayerPrefs, 0);
        totalCoins += score;

        PlayerPrefs.SetInt(coinsPlayerPrefs, totalCoins);

        if (gameOverCanvas)
        {
            gameOverCanvas.gameObject.SetActive(true);

            gameOverCanvas.Find("TextScore").GetComponent<Text>().text = "SCORE " + score.ToString();

            if (score > highScore)
            {
                highScore = score;
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_HighScore", score);

            }

            gameOverCanvas.Find("TextHighScore").GetComponent<Text>().text = "HIGH SCORE " + highScore.ToString();
        }

    }


    IEnumerator Victory(float delay)
    {

        playerObjects[currentPlayer].gameObject.SetActive(true);

        if (respawnObject && respawnObject.gameObject.activeSelf == true)
        {
            targetPosition = respawnObject.position;

            playerObjects[currentPlayer].position = targetPosition;

            playerObjects[currentPlayer].rotation = respawnObject.rotation;

            respawnObject.gameObject.SetActive(false);
        }
        if (playerObjects[currentPlayer]) playerObjects[currentPlayer].SendMessage("Victory");

        yield return new WaitForSeconds(delay);

        isGameOver = true;
        if (pauseCanvas)
            Destroy(pauseCanvas.gameObject);

        if (gameCanvas)
            Destroy(gameCanvas.gameObject);

        int totalCoins = PlayerPrefs.GetInt(coinsPlayerPrefs, 0);

        totalCoins += score;

        PlayerPrefs.SetInt(coinsPlayerPrefs, totalCoins);

        if (victoryCanvas)
        {
            victoryCanvas.gameObject.SetActive(true);

            victoryCanvas.Find("TextScore").GetComponent<Text>().text = "SCORE " + score.ToString();

            if (score > highScore)
            {
                highScore = score;


                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_HighScore", score);
            }
            victoryCanvas.Find("TextHighScore").GetComponent<Text>().text = "HIGH SCORE " + highScore.ToString();
        }


    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void MainMenu()
    {
        SceneManager.LoadScene(mainMenuLevelName);
    }

    void SetGameSpeed(float setValue)
    {
        gameSpeed = setValue;
        Time.timeScale = gameSpeed;
    }
    void ResetDeathLine()
    {
        if (deathLineObject && GameController.Instance.cameraObject) deathLineTargetPosX = GameController.Instance.cameraObject.position.x;
    }

    void LevelUp()
    {
        if (deathLineSpeed + deathLineSpeedIncrease < deathLineSpeedMax)
            deathLineSpeed += deathLineSpeedIncrease;
    }

}