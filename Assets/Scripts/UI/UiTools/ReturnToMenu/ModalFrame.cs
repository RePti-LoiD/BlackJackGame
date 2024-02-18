using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModalFrame : ActivatableObject
{
    [SerializeField] private TextMeshProUGUI labelText;
    [SerializeField] private TextMeshProUGUI primaryButtonText;
    [SerializeField] private TextMeshProUGUI secondaryButtonText;
    [SerializeField] private Button primaryButton;
    [SerializeField] private Button secondaryButton;

    public Action<ModalFrameButton> OnAnswer;

    public void Awake()
    {
        primaryButton.onClick.AddListener(() => OnAnswer?.Invoke(ModalFrameButton.Primary));
        secondaryButton.onClick.AddListener(() => OnAnswer?.Invoke(ModalFrameButton.Secondary));
    }

    public ModalFrame SetLabel(string label)
    {
        labelText.text = label;

        return this;
    }

    public ModalFrame SetPrimaryButton(string text)
    {
        primaryButtonText.text = text;

        return this;
    }

    public ModalFrame SetSecondaryButton(string text)
    {
        secondaryButtonText.text = text;

        return this;
    }

    public void CloseModalFrame()
    {
        OnAnswer?.Invoke(ModalFrameButton.Secondary);
    }
}

public enum ModalFrameButton
{
    Primary,
    Secondary
}