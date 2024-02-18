using System;
using UnityEngine;

public class ActivatableObject : MonoBehaviour
{
    public Action OnActivate;
    public Action OnDeactivate;

    public virtual void OnEnable()
    {
        OnActivate?.Invoke();
    }

    public virtual void OnDisable()
    {
        OnDeactivate?.Invoke();
    }
}