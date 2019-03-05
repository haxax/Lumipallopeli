using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collideable : Poolable
{
    //Called by Player when collision occurs. Action defined by Collideable.
    public virtual void CollisionAction(Player player) { }
}
