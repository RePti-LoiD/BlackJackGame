using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossPanelNavigator : MonoBehaviour
{
    [Header("Использовать соответсвие кнопок и панелей")]
    [InspectorName("Навигация")]
    [SerializeField] private RectTransform panelHandler;
    [SerializeField] private List<Button> buttonList = new List<Button>();   

    [Header("Preferences")]
    [SerializeField] private int startPage;
    [SerializeField] private int pageSize;
    [SerializeField] private float transitionSpeed;

    float currentOffset;

    private void Start()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            int j = i;

            buttonList[j].onClick.AddListener(() =>
            {
                currentOffset = (-(pageSize * j)) + 540;
            });
        }

        buttonList[startPage].onClick.Invoke();
    }

    private void Update()
    {
        panelHandler.position = Vector2.Lerp(panelHandler.position, new Vector2(currentOffset, panelHandler.position.y), transitionSpeed * Time.deltaTime);
    }
}