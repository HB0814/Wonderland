using UnityEngine;

[System.Serializable]
public struct DropData
{
    public GameObject item;
    public float dropChance;
}

public class DropItem : MonoBehaviour
{
    public enum Rate
    {
        Normal, Named
    }
    public Rate rate;
    public DropData[] normalDrops;
    public DropData[] namedDrops;

    private void OnDisable()
    {
        if (!gameObject.scene.isLoaded) 
            return;

        DropItems();
    }

    private void DropItems()
    {
        DropData[] currentDrops = rate == Rate.Named ? namedDrops : normalDrops;

        float rand = Random.value;
        float sum = 0f;

        foreach (var drop in currentDrops)
        {
            sum += drop.dropChance;
            if (rand <= sum)
            {
                Instantiate(drop.item, transform.position, Quaternion.identity);
                return;
            }
        }
    }
}
