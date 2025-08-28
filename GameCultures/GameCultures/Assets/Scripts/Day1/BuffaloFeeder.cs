using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class BuffaloFeeder : MonoBehaviour
{
    void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        var corn = other.GetComponent<DraggableCorn>();
        if (corn != null && corn.IsBeingCarried && !corn.consumed)
        {
            BuffaloStats.Instance.FeedCorn(corn);
            
        }
    }
}
