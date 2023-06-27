using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ManagerComponent : MonoBehaviour
{
    public virtual void Awake()
    {
        Entry.RegisterComponent(this);
    }

    protected virtual void OnDestroy()
    {

    }
}
