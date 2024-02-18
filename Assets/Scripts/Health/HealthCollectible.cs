using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    [SerializeField] private float healthValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !collision.GetComponent<PlayerHealth>().IsMax())
        {
            collision.GetComponent<PlayerHealth>().AddHealth(healthValue);
            gameObject.SetActive(false);
        }
    }
}