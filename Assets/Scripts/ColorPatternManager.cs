using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorPatternManager : MonoBehaviour
{

    public Material[] colorMaterials; // 0 - Red, 1 - Green, 2 - Blue, 3 - Yellow
    public ColorTile[] codeTiles;
    public ColorTile[] inputTiles;

    public GameObject doorToOpen;
    private bool doorOpening = false;

    private void Start()
    {
        RandomColor();
    }

    void RandomColor()
    {
        List<ColorTile.ColorState> availableColors = new List<ColorTile.ColorState>
        {
            ColorTile.ColorState.Red,
            ColorTile.ColorState.Green,
            ColorTile.ColorState.Blue,
            ColorTile.ColorState.Yellow
        };

        for (int i = 0; i < codeTiles.Length; i++)
        {
            int index = Random.Range(0, availableColors.Count);
            var randomState = availableColors[index];
            availableColors.RemoveAt(index);

            codeTiles[i].SetColorState(randomState, colorMaterials[(int)randomState]);
            inputTiles[i].SetColorState(randomState, colorMaterials[(int)randomState]);
        }
    }

    public void CheckPuzzle()
    {
        for (int i = 0; i < codeTiles.Length; i++)
        {
            if (!inputTiles[i].IsCorrect())
                return;
        }
        if (doorToOpen != null && !doorOpening)
            {
                StartCoroutine(OpenDoor());
                doorOpening = true;
            }
    }
    private IEnumerator OpenDoor()
    {
        Vector3 startPos = doorToOpen.transform.position;
        Vector3 targetPos = startPos + new Vector3(0, 3, 0);
        float elapsed = 0f;
        float duration = 1f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            doorToOpen.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            yield return null;
        }

        doorToOpen.transform.position = targetPos;
    }

}
