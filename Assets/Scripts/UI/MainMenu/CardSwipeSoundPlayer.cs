using UnityEngine;

public class CardSwipeSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip cardSwipeSound;
    [SerializeField] private AudioSource source;
    [SerializeField] private IAction cardSwipeEventHandler;

    private void Start()
    {
        cardSwipeEventHandler.OnBalanceChangeAction += (data, _, _) =>
        {
            source.PlayOneShot(cardSwipeSound);
        };
    }
}