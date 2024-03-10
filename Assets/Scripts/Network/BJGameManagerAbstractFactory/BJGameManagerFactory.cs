using UnityEngine;

public abstract class BJGameManagerFactory
{
    public abstract (BJGameManager, BJPlayer) CreateManager(GameObject targetGameObject, GameObject targetPlayerObject, BJGameLoadData data);
}