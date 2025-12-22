using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int scoreValue = 1;
    public int lifeTime = 5;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(scoreValue);
            }

            Destroy(collision.gameObject);
        }

        Destroy(gameObject);
    }
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
