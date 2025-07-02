using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject goldCoin, healthGlobe;
    public Enemies enemyData;
    public GameObject item;
    public float minDropDistance = 10f;
    public float maxDropDistance = 20f;

   

    public void DeathDrop()
    {
        int randomNum = Random.Range(1, 5);

        if (randomNum <= 3)
        {
            Instantiate(healthGlobe, transform.position, Quaternion.identity);
        }

        if (randomNum >= 3)
        {
            int randomAmountOfGold = Random.Range(1, 4);

            for (int i = 0; i < randomAmountOfGold; i++)
            {
                Instantiate(goldCoin, transform.position, Quaternion.identity);

            }
        }
    }

    public void DropItem()
    {
        if (enemyData.itemDrop)
        {
            Vector2 dropOffset = Random.insideUnitCircle.normalized * Random.Range(minDropDistance, maxDropDistance);
            Vector2 dropPosition = (Vector2)transform.position + dropOffset;

            GameObject dropItem = Instantiate(item, dropPosition, Quaternion.identity);
            dropItem.GetComponent<BounceEffect>().StartBounce();
        }
    }
}
