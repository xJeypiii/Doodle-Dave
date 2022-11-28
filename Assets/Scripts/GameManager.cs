using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;
    public float gameSpeed { get; private set; }

    // public TextMeshProUGUI scoreText;
    // public TextMeshProUGUI hiscoreText;
    public TextMeshProUGUI gameOverText;
    public Button retryButton;
    public GameObject messageBox;

    private Player player;
    private Spawner spawner;

    // private float score;

    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        spawner = FindObjectOfType<Spawner>();

        NewGame();
    }

    public void NewGame()
    {
        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();

        foreach (var obstacle in obstacles) {
            Destroy(obstacle.gameObject);
        }

        // score = 0f;
        gameSpeed = initialGameSpeed;
        enabled = true;

        player.transform.localScale = new Vector3(1, 1, 1);
        player.transform.position = new Vector3(-7, 0, 0);
        player.transform.eulerAngles = new Vector3(0, 0, 0);

        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
        messageBox.SetActive(false);

        // UpdateHiscore();
    }

    public void GameOver(Collider other)
    {
        gameSpeed = 0f;
        enabled = false;
        
        StartCoroutine(AnimateGameOver(other));

    }

    IEnumerator AnimateGameOver(Collider other)
    {
        StartCoroutine(AbsorbPlayer(other, 1.1f));
        yield return new WaitForSeconds(1.1f);

        player.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);
        messageBox.SetActive(true);

        // UpdateHiscore();
    }

    IEnumerator AbsorbPlayer(Collider other, float duration)
    {
        Vector3 startSize = player.transform.localScale;
        Vector3 endSize = new Vector3(0, 0, 0);
        Vector3 startPosition = player.transform.position;
        Vector3 endPosition = other.transform.position;
        float t = 0.0f;
        player.GetComponent<Animator>().Play("DeathAnim");
        while ( t  < duration )
        {
            t += Time.deltaTime;
            Vector3 newSize = Vector3.Lerp(startSize, endSize, t / duration);
            player.transform.localScale = newSize;
            Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, t / duration);
            player.transform.position = newPosition;
            yield return null;
        }
    }

    private void Update()
    {
        gameSpeed += gameSpeedIncrease * Time.deltaTime;
        // score += gameSpeed * Time.deltaTime;
        // scoreText.text = Mathf.FloorToInt(score).ToString("D5");
    }

    // private void UpdateHiscore()
    // {
    //     float hiscore = PlayerPrefs.GetFloat("hiscore", 0);

    //     if (score > hiscore)
    //     {
    //         hiscore = score;
    //         PlayerPrefs.SetFloat("hiscore", hiscore);
    //     }

    //     hiscoreText.text = Mathf.FloorToInt(hiscore).ToString("D5");
    // }

}
