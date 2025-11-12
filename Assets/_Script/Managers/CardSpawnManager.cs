using Bajru.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawnManager : MonoBehaviour
{
    [Header("SPAWN POÝNTS")]
    [SerializeField] private List<Transform> cardSpawnPoints;
    [Header("CARD PREFABS")]
    [SerializeField] private List<Card> cards;


    private void Start()
    {
        SpawnCards();
    }


    private void SpawnCards()
    {
        foreach (Transform spawnPoint in cardSpawnPoints)
        {
            int randomIndex = Random.Range(0, cards.Count);
            Card randomCard = cards[randomIndex];

            Instantiate(randomCard, spawnPoint.localPosition, randomCard.transform.localRotation);
        }
    }

    private void OnDrawGizmos()
    {
        foreach (Transform spawnPoint in cardSpawnPoints)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(spawnPoint.position, new Vector3(.2f,.2f,.4f));
        } 
    }
}
