using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceHole : Collideable
{
    //IceHole has three states, default, cracked and hole.
    //Spawns as default, waits one wave and cracks.
    //Cracked and hole states are deadly for player.

    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite crackedSprite;
    [SerializeField] private Sprite holeSprite;

    private SpriteRenderer rend;
    private CircleCollider2D col;


    public override void OnInstantiate()
    {
        col = GetComponent<CircleCollider2D>();
        col.enabled = false;
        rend = GetComponentInChildren<SpriteRenderer>();
        rend.enabled = false;
        gameObject.SetActive(false);
    }

    public override void OnPreSpawn()
    {
        //return back to pool before the start of next game
        GameManager.instance.GameState.OnLobbyMenuStartEvent.AddListener(ReturnToPool);
        //wait one wave as default (safe) and turn into cracked
        GameManager.instance.WaveHandler.OnLatencyStartEvent.AddListener(CrackIce);
    }

    public override void Setup()
    {
        rend.sprite = defaultSprite;
        rend.enabled = true;
        col.enabled = false;
        gameObject.SetActive(true);
    }


    public void CrackIce()
    {
        col.enabled = true;
        rend.sprite = crackedSprite;
    }

    public override void CollisionAction(Player player)
    {
        //cracked turns into hole on collision with player, drown player
        rend.sprite = holeSprite;
        player.Drown(transform.position);
    }


    public override void ReturnToPool()
    {
        col.enabled = false;
        rend.enabled = false;
        base.ReturnToPool();
    }

}
