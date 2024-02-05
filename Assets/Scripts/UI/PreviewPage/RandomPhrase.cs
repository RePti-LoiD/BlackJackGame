using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomPhrase : MonoBehaviour
{
    [SerializeField] private List<string> phrases;
    [SerializeField] private TMP_Text phraseText;

    private void Start()
    {
        phraseText.text = phrases[Random.Range(0, phrases.Count - 1)];
    }
}