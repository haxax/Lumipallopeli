using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : Collideable
{
    private SpriteRenderer rend;
    private CircleCollider2D col;
    private Transferer transferer;

    //Tells if the snowball have already hit something successfully
    private bool isHit = false;

    //target coordinate for snowball to fly to
    private BytePair target = new BytePair(0, 0);
    //shooter of the snowball
    private Player shooter;


    public override void OnInstantiate()
    {
        col = GetComponent<CircleCollider2D>();
        col.enabled = false;

        rend = GetComponentInChildren<SpriteRenderer>();
        rend.enabled = false;

        transferer = GetComponent<Transferer>();
        transferer.OnInstantiate();
        //return snowball back to the pool if end point reached
        transferer.OnMoveEndEvent.AddListener(Miss);

        gameObject.SetActive(false);
    }

    public override void Setup()
    {
        col.enabled = true;
        rend.enabled = true;

        isHit = false;

        gameObject.SetActive(true);
    }

    /// <summary>
    /// Launch method for the snowball
    /// </summary>
    public void SetTargetAndShoot(Player _shooter, BytePair _target)
    {
        SetTarget(_shooter, _target);
        transferer.MoveTo(target.ToPositionVector2());
    }
    public void SetTarget(Player _shooter, BytePair _target)
    {
        shooter = _shooter;
        target = _target;
        base.SetLocalPosition(StartPointFromTo(_shooter.transform.position, target.ToPositionVector2(), SpawnMagnitude(_shooter.Col, col)));
    }


    public override void CollisionAction(Player player)
    {
        //Do not recollide with dead players
        if (player.IsDead) { return; }

        //Collision allowed only once
        if(isHit) { return; }

        //If player is crouching, ball should pass unless that's target position
        if (player.IsCrouching && target != player.TargetPosition) { return; }

        //Do not collide with the shooter of the ball
        if (player.PlayerData != shooter.PlayerData)
        {
            //return to pool on collision, kill player
            //TODO splash effect
            isHit = true;
            player.Hit(shooter);
            ReturnToPool();
        }
    }

    public void Miss()
    {
        //TODO splash effect
        ReturnToPool();
    }

    public override void ReturnToPool()
    {
        transferer.Stop();
        col.enabled = false;
        rend.enabled = false;
        base.ReturnToPool();
    }



    /// <summary>
    /// Get wanted distance from the player to the spawn point of the snowball
    /// </summary>
    private float SpawnMagnitude(CircleCollider2D a, CircleCollider2D b)
    {
        //TODO doesn't take on account the scale of the objects
        return (a.radius + b.radius) * 1.05f;
    }

    /// <summary>
    /// Calculate start point for the snowball
    /// </summary>
    private Vector3 StartPointFromTo(Vector2 from, Vector2 to, float magnitude)
    {
        Vector3 result = from;
        result += Vector3.Normalize(new Vector3(to.x - from.x, to.y - from.y, 0)) * magnitude;
        return result;
    }
}
