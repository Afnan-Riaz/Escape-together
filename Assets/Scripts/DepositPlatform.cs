using UnityEngine;
using TMPro;

public class DepositPlatform : MonoBehaviour
{
    [Tooltip("Total cubes required to win")]
    public int totalCubes = 6;

    [Tooltip("TextMeshProUGUI component showing progress")]
    public TextMeshProUGUI counterText;

    private int depositedCount = 0;

    public bool Completed => depositedCount >= totalCubes;
    public int DepositedCount => depositedCount;

    void Start()
    {
        UpdateUI();
    }

    void OnTriggerEnter(Collider other)
    {
        TryRegisterDeposit(other.gameObject);
    }

    public bool TryRegisterDeposit(GameObject cube)
    {
        var rb = cube.GetComponent<Rigidbody>();
        if (cube.CompareTag("Cube") && rb != null && !rb.isKinematic)
        {
            depositedCount++;
            UpdateUI();
            Destroy(cube);

            if (depositedCount >= totalCubes)
                Debug.Log("All cubes collected! Level complete.");

            return true;
        }
        return false;
    }

    private void UpdateUI()
    {
        if (counterText != null)
            counterText.text = $"{depositedCount}/{totalCubes}";
    }
}
