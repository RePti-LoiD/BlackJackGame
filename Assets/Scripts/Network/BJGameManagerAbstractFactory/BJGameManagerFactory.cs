using UnityEngine;

public abstract class BJGameManagerFactory
{
    public abstract BJGameManager CreateManager(GameObject targetGameObject, BJGameLoadData data);
}