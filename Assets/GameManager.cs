using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score;
    public TMPro.TextMeshProUGUI scoreText;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
    }

    void Update()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet")){
            RestartGame();
        }
    }
    private void RestartGame()
    {
        score = 0;
    }
}
