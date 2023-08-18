using UnityEngine;
using Newtonsoft.Json;
using TMPro;

public class NewtonsoftTest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI jsonTestText;

    private void Start()
    {
        jsonTestText.text = JsonConvert.SerializeObject(new {
            name = "Anton",
            lastname = "Ficalis",
            age = 59,
            city = "Samara"
        }, Formatting.Indented);
    }
}