using UnityEngine;
using TMPro;

public class EscapeTriggerManager : MonoBehaviour
{
    public TextMeshProUGUI levelMessage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            string playerName = other.gameObject.name;
            levelMessage.text = $"{playerName} has escaped.";
        }
    }
}
