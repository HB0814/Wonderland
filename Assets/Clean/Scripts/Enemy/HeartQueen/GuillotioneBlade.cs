using UnityEngine;

public class GuillotioneBlade : MonoBehaviour
{
    Player player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.TakeDamage(15.0f);
        }
    }
}
