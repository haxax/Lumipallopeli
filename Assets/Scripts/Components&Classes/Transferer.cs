using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Transferer : MonoBehaviour
{
    //Moves transform from startpoint to endpoint using given speed.

    [Tooltip("World units per second")]
    [SerializeField] private float speed;
    public Vector2 StartPoint { get; set; }
    public Vector2 EndPoint { get; set; }

    //calculated speed to increase progress relative to world speed
    private float progressSpeed = 0;
    //progress from start to end, from 0 to 1
    private float progress = 0;

    public UnityEvent OnMoveEndEvent { get; set; } = new UnityEvent();
    public UnityEvent OnMoveUpdateEvent { get; set; } = new UnityEvent();

    public void OnInstantiate()
    {
        Disable();
    }

    public void Update()
    {
        progress += Time.deltaTime / progressSpeed;
        if (progress >= 1)
        {
            //EndPoint reached, stop transferer
            progress = 0;
            StartPoint = EndPoint;
            OnMoveEndEvent.Invoke();
            this.enabled = false;
        }
        else
        {
            OnMoveUpdateEvent.Invoke();
        }
        transform.localPosition = new Vector2(StartPoint.x + ((EndPoint.x - StartPoint.x) * progress), StartPoint.y + ((EndPoint.y - StartPoint.y) * progress));
    }

    /// <summary>
    /// Launch method for Transferer.
    /// </summary>
    public void MoveTo(Vector2 targetPoint)
    {
        StartPoint = transform.position;
        EndPoint = targetPoint;
        progress = 0;
        progressSpeed = Vector2.Distance(StartPoint, EndPoint) / speed;
        this.enabled = true;
    }

    public void Stop()
    {
        Disable();
    }

    private void Disable()
    {
        progress = 0;
        StartPoint = transform.localPosition;
        EndPoint = transform.localPosition;
        this.enabled = false;
    }
}
