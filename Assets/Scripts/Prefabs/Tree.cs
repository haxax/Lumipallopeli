using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Poolable
{
    [SerializeField] private List<Sprite> alternateSprites = new List<Sprite>();
    [SerializeField] private float positionFluctuationRange = 0.2f;
    [SerializeField] private Vector2 scaleFluctuationRange = Vector2.one;
    private SpriteRenderer rend;


    public override void OnInstantiate()
    {
        rend = GetComponent<SpriteRenderer>();
        gameObject.SetActive(false);
    }

    public override void OnPreSpawn()
    {
        //randomise sprite to have variation
        rend.sprite = alternateSprites[Random.Range(0, alternateSprites.Count)];

        gameObject.SetActive(true);

        //return trees back to the pool before new set of trees are set at the start of lobby menu
        GameManager.instance.GameState.OnLobbyMenuStartEvent.AddListener(base.ReturnToPool);
    }
    public override void Setup()
    {
        //set some fluctuation to the position and scale of the tree
        transform.position = Extensions.FluctuatePosition(transform.position, positionFluctuationRange);
        transform.localScale = Extensions.SameRandomVector(scaleFluctuationRange);

        //set trees to overlap correctly
        Extensions.SetVerticalSpriteOrder(transform, rend);
    }
}
