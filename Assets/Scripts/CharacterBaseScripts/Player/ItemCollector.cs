using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{      
    public GameObject ParentForCollectedItems { get; private set; }
    public Inventory Inventory { get; private set; }
    public CircleCollider2D CircleCollider { get; private set; }
    public CH_Stats Stats { get; private set; }

    public event Action OnItemPickUp;
    

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        ParentForCollectedItems = GameObject.FindGameObjectWithTag("CollectedItems");
        Inventory = player.GetComponent<Inventory>();
        Stats = player.GetComponent<CH_Stats>();
        CircleCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        Stats.OnStatsChange += EvaluateRadius;
        CircleCollider.radius = Stats.CurrentCollectorRadius;
    }

    private void OnDestroy()
    {
        Stats.OnStatsChange -= EvaluateRadius;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out SpawnableItem item))
        {
            Destroy(item.gameObject);
            OnItemPickUp.Invoke();
        }

        if (collision.TryGetComponent(out GoldCoin coin))
        {
            Stats.GetGold(coin.GetGoldAmount());
            coin.ReleaseFromPool();
        }
    }    

    private void EvaluateRadius()
    {
        CircleCollider.radius = Stats.CurrentCollectorRadius;        
    }
}
