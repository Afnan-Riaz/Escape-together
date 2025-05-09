using UnityEngine;

public class ColorTile : MonoBehaviour
{
    public enum ColorState { None, Red, Green, Blue, Yellow }
    private ColorState currentState;

    public bool isInputTile = true;
    [HideInInspector]
    public ColorState correctState;
    private Renderer rend;
    private ColorPatternManager manager;
    private void Awake()
    {
        manager = GetComponentInParent<ColorPatternManager>();
        Debug.Log($"Tile {gameObject.name} found manager: {manager}");
    }

    public void Start()
    {
        rend = GetComponent<Renderer>();
    }

    public void SetMaterial(Material mat)
    {
        if (rend == null) rend = GetComponent<Renderer>();
            rend.material = mat;
    }
    public void SetColorState(ColorState state, Material mat)
    {
        if(isInputTile){
            correctState = state;
        }
        else{
            currentState = state;
            SetMaterial(mat);
        }
    }
    public void CycleColor()
    {
        if (!isInputTile) return;
        currentState = currentState == ColorState.Yellow ? ColorState.Red : currentState + 1;
        if (manager != null)
            SetMaterial(manager.colorMaterials[(int)currentState]);
        Debug.Log(manager);
        manager.CheckPuzzle();
    }

    public bool IsCorrect()
    {
        Debug.Log($"Current State: {currentState}, Correct State: {correctState} of tile {gameObject.name}");
        return currentState == correctState;
    }
}
