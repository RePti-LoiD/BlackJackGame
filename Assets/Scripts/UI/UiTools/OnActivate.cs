using UnityEngine;
using UnityEngine.Events;

public class OnActivate : MonoBehaviour
{
    [SerializeField] private UnityEvent onActivate;

    private void OnEnable()
    {
        onActivate.Invoke();
    }

    public void DisableComponent()
    {
        gameObject.SetActive(false);
    }
}