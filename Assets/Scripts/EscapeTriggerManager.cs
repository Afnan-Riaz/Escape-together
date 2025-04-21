using UnityEngine;
using TMPro;

public class EscapeTriggerManager : MonoBehaviour
{
    public TextMeshProUGUI levelMessage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player1")){
            levelMessage.text = "Player 1 has escaped.";
        }
    }
}
