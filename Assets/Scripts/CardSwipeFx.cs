using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class CardSwipeFx : MonoBehaviour
{
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private Transform container;
    [SerializeField] private Transform instantTransform;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private IAction balanceChange;
    [SerializeField] private int randomWeight;

    [Header("colors")]
    [SerializeField] private Color plusDoubleBalanceColor;
    [SerializeField] private Color plusLowBalanceColor;
    [SerializeField] private Color plusMiddleBalanceColor;
    [SerializeField] private Color plusHugeBalanceColor;
    
    private ObjectPool<GameObject> objectPool;

    private void Start()
    {
        objectPool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject obj = Instantiate(textPrefab, container);

                return obj;
            },
            actionOnGet: (particleText) => particleText.SetActive(true),
            actionOnRelease: (particleText) => particleText.SetActive(false),
            actionOnDestroy: (particleText) => Destroy(particleText)
        );

        balanceChange.OnBalanceChangeAction += (balanceChangeMessage) =>
        {
            GameObject text = objectPool.Get();
            MainMenuFxHandler fxHandler = text.GetComponent<MainMenuFxHandler>();
            fxHandler.SetText(balanceChangeMessage);
            
            Vector2 randomPoint = Random.insideUnitCircle;
            text.gameObject.transform.position = instantTransform.position + new Vector3(randomPoint.x * randomWeight, randomPoint.y * randomWeight);
            fxHandler.SetTargetPosition(new Vector2(targetTransform.position.x + (randomPoint.x * randomWeight), 
                    targetTransform.position.y + (randomPoint.y * randomWeight)));


            if (balanceChangeMessage.Contains("x"))
                fxHandler.SetColor(plusDoubleBalanceColor);
            else
            {
                int balance = int.Parse(balanceChangeMessage[0..^1]);
                
                if (balance <= 4)
                    fxHandler.SetColor(plusLowBalanceColor);
                else if (balance >= 5 && balance <= 7)
                    fxHandler.SetColor(plusMiddleBalanceColor);
                else
                    fxHandler.SetColor(plusHugeBalanceColor);
            }

            StartCoroutine(ReturnToPoolWithDuration(text, 1f));
        };
    }

    private IEnumerator ReturnToPoolWithDuration(GameObject text, float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);

        objectPool.Release(text);
    }
}