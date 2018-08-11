using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class WaterController : MonoBehaviour {

    public Sprite Water;
    public Vector2 TileOffSet = Vector2.one;
    List<SpriteRenderer> waterRenderers = new List<SpriteRenderer>();


    // Use this for initialization
    void Start () {
        SetupRenderers();        
    }

    public void UpdateTiling(Vector2 _newCenterPosition)
    {
        Vector2Int centerIndex = GetCenterTileIndex(_newCenterPosition);
        List<SpriteRenderer> wRendererNew = new List<SpriteRenderer>();
        for (int i = 0; i < 9; i++)
            wRendererNew.Add(null);

        Vector2Int genericIndexModifier = centerIndex - Vector2Int.one;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int x = i - genericIndexModifier.x;
                bool hasToFlipX = false;
                if (x < 0)
                {
                    hasToFlipX = true;
                    x = 2;
                }

                int y = j - genericIndexModifier.y;
                bool hasToFlipY = false;
                if (y < 0)
                {
                    hasToFlipY = true;
                    y = 2;
                }

                wRendererNew[i * 3 + j] = waterRenderers[x* 3 + y];

                if (hasToFlipX)
                    wRendererNew[i * 3 + j].flipX = !wRendererNew[i * 3 + j].flipX;
                if (hasToFlipY)
                    wRendererNew[i * 3 + j].flipY = !wRendererNew[i * 3 + j].flipY;

                wRendererNew[i*3 +j].transform.localPosition = new Vector3(
                    (i - 1) * TileOffSet.x + waterRenderers[4].transform.localPosition.x,
                    (j - 1) * TileOffSet.y + waterRenderers[4].transform.localPosition.y,
                    0);
            }
        }

        waterRenderers = wRendererNew;
    }

    /// <summary>
    /// Create a child GameObject with a spriteRenderer (that renders _image)
    /// </summary>
    /// <param name="_image"></param>
    /// <returns></returns>
    SpriteRenderer CreateRender(Sprite _image)
    {
        GameObject newGameObject = new GameObject(_image.name + " Water Renderer");
        newGameObject.transform.parent = transform;

        SpriteRenderer newRenderer = newGameObject.AddComponent<SpriteRenderer>();
        newRenderer.sprite = _image;

        return newRenderer;
    }

    void SetupRenderers()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                SpriteRenderer newRend = CreateRender(Water);
                newRend.sortingLayerID = SortingLayer.layers[0].id;

                if (i != 1)
                    newRend.flipX = !newRend.flipX;
                if (j != 1)
                    newRend.flipY = !newRend.flipY;

                newRend.transform.localPosition = new Vector3(
                    (i - 1) * TileOffSet.x,
                    (j - 1) * TileOffSet.y,
                    0);

                waterRenderers.Add(newRend);
            }
        }
    }

    /// <summary>
    /// Return the closer tile to a give world center position
    /// </summary>
    /// <param name="_centerPos"></param>
    /// <returns></returns>
    Vector2Int GetCenterTileIndex(Vector2 _centerPos)
    {
        Vector2Int centerIndex = Vector2Int.zero;
        float shorterDis = TileOffSet.x * 1.41f;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (Vector2.Distance(waterRenderers[i * 3 + j].transform.position, _centerPos) < shorterDis)
                {
                    centerIndex.x = i;
                    centerIndex.y = j;
                }
            }
        }

        return centerIndex;
    }
}
