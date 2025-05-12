using UnityEngine;

public class HealItem : ItemAttract
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            player.Heal(10.0f);
            gameObject.SetActive(false);
        }
    }
}
