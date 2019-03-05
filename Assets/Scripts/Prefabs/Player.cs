using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Poolable
{
    [Tooltip("rgb saturation value range, 0.0f-1.0f")]
    [SerializeField] private Vector2 colorCaps = new Vector2();

    public CircleCollider2D Col { get; private set; }
    private Rigidbody2D rb;
    private SpriteRenderer rend;
    private TextMesh nameTxt;
    private Transferer transferer;

    public PlayerData PlayerData { get; set; }

    private CommandAlias firstAction = new CommandAlias(TwitchCommand.none); //First action currently being preformed
    private CommandAlias secondAction = new CommandAlias(TwitchCommand.none); //Second action currently being preformed
    private CommandAlias newFirstAction = new CommandAlias(TwitchCommand.none); //First action to be performed on next wave
    private CommandAlias newSecondAction = new CommandAlias(TwitchCommand.none); //Second action to be performed on next wave
    private byte actionNumber = 0; //Number of the currently performed action

    private bool isDead;
    public bool IsDead
    {
        get { return isDead; }
        set
        {
            if (isDead != value && value)
            { PlayerData.Kill(); }
            isDead = value;
        }
    }

    //If crouching, flying snowballs with different target position wont hit
    public bool IsCrouching { get; set; }

    //Target coordinate where the player either already is or is moving to
    public BytePair TargetPosition { get; set; }


    public override void OnInstantiate()
    {
        Col = GetComponent<CircleCollider2D>();
        Col.enabled = false;

        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        rend = GetComponentInChildren<SpriteRenderer>();
        rend.enabled = false;

        nameTxt = GetComponentInChildren<TextMesh>();

        transferer = GetComponent<Transferer>();
        transferer.OnInstantiate();
        //After ended movement action, recheck for another action to perform
        transferer.OnMoveEndEvent.AddListener(Act);
    }

    public override void OnPreSpawn()
    {
        //return back to pool before the start of next game
        GameManager.instance.GameState.OnLobbyMenuStartEvent.AddListener(ReturnToPool);
    }

    public override void Setup()
    {
        Col.enabled = true;
        rb.isKinematic = false;
        rend.enabled = true;

        //Set random color to each player
        rend.color = Extensions.RandomColor(colorCaps, 1);
        Extensions.SetVerticalSpriteOrder(transform, rend);

        nameTxt.text = PlayerData.Username;

        //At the start, set the player to be on default state with no upcoming actions
        IsCrouching = false;
        ResetAllActions(TwitchCommand.none);

        this.enabled = true;
    }


    /// <summary>
    /// Sets incoming actions to correct perform order
    /// </summary>
    public void SetAction(CommandAlias action)
    {
        if (newFirstAction.Command == TwitchCommand.crouch || newFirstAction.Command == TwitchCommand.none || newFirstAction.Command == action.Command)
        {
            newFirstAction = action;
        }
        else if (newSecondAction.Command == TwitchCommand.crouch || newSecondAction.Command == TwitchCommand.none || newSecondAction.Command == action.Command)
        {
            newSecondAction = action;
        }
    }


    /// <summary>
    /// Preforms the actions
    /// </summary>
    public void Act()
    {
        if (actionNumber == 0 || actionNumber >= 3)
        {
            IsCrouching = false;
            SetNewActionsActive();
            //Reset upcoming actions to Crouch so that if player does nothing else, it will automatically crouch.
            ResetNewActions(TwitchCommand.crouch);
        }

        actionNumber++;
        if (actionNumber == 1)
        {
            DoAction(firstAction);
            //after action, recall Act()
        }
        else if (actionNumber == 2)
        {
            DoAction(secondAction);
            //after action, recall Act()
        }
    }

    private void DoAction(CommandAlias action)
    {
        switch (action.Command)
        {
            case TwitchCommand.walk:
                Walk(action);
                break;
            case TwitchCommand.shoot:
                Shoot(action);
                break;
            case TwitchCommand.crouch:
                Crouch(action);
                break;
            case TwitchCommand.none:
                Act();
                break;
        }
    }


    private void Crouch(CommandAlias action)
    {
        IsCrouching = true;
        Act();
    }

    private void Shoot(CommandAlias action)
    {
        //convert letter-number coordinate input from action.alias[0] to bytepair
        BytePair coord = BytePair.StringListToCoordinates(action.Alias, GameManager.instance.MapHandler.MapHeight - 1, -1);
        if (coord.x == 255 || coord.y == 255) { print("Invalid coordinate"); return; }

        Snowball newBall = (Snowball)Pool.instance.GetFromPool("Assorted", "Snowball");
        newBall.SetTargetAndShoot(this, coord);
        newBall.Setup();

        Act();
    }

    private void Walk(CommandAlias action)
    {
        //convert letter-number coordinate input from action.alias[0] to bytepair
        BytePair coord = BytePair.StringListToCoordinates(action.Alias, GameManager.instance.MapHandler.MapHeight - 1, -1);
        if (coord.x == 255 || coord.y == 255) { print("Invalid coordinate"); return; }

        TargetPosition = coord;
        transferer.MoveTo(TargetPosition.ToPositionVector2());
    }


    public void Drown(Vector3 position)
    {
        IsDead = true;
        ReturnToPool();
    }

    public void Hit(Player shooter)
    {
        shooter.PlayerData.AddKill();
        IsDead = true;
        ReturnToPool();
    }


    private void ResetDefaultAction()
    {
        ResetActions(TwitchCommand.crouch);
        ResetNewActions(TwitchCommand.crouch);
    }
    private void ResetActions(TwitchCommand cmd)
    {
        actionNumber = 0;
        firstAction = new CommandAlias(cmd);
        secondAction = new CommandAlias(cmd);
    }
    private void ResetNewActions(TwitchCommand cmd)
    {
        actionNumber = 0;
        newFirstAction = new CommandAlias(cmd);
        newSecondAction = new CommandAlias(cmd);
    }
    private void ResetAllActions(TwitchCommand cmd)
    {
        actionNumber = 0;
        firstAction = new CommandAlias(cmd);
        secondAction = new CommandAlias(cmd);
        newFirstAction = new CommandAlias(cmd);
        newSecondAction = new CommandAlias(cmd);
    }

    /// <summary>
    /// Sets new actions to be performed
    /// </summary>
    private void SetNewActionsActive()
    {
        firstAction = new CommandAlias(newFirstAction);
        secondAction = new CommandAlias(newSecondAction);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        try
        {
            //Player layer collides only with collideables layer. Actions will be performed by colliding object
            col.gameObject.GetComponent<Collideable>().CollisionAction(this);
        }
        catch
        { Debug.Log("Player collided with non-collideable"); }
    }


    public override void ReturnToPool()
    {
        //Abort if alredy returned to pool
        if (!this.enabled) { return; }
        transferer.Stop();
        Col.enabled = false;
        rb.isKinematic = true;
        rend.enabled = false;
        PlayerData = null;
        this.enabled = false;
        base.ReturnToPool();
    }
}

