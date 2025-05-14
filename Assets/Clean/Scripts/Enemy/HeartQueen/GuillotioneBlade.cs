using UnityEngine;

public class GuillotioneBlade : MonoBehaviour
{
    Player player;
    [SerializeField] float damage;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.TakeDamage(damage);
        }
    }
}
