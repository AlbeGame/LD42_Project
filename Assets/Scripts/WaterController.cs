using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class WaterController : MonoBehaviour {

    public Sprite Water;
    public Vector2 TileOffSet = Vector2.one;
    List<SpriteRenderer> waterRenderers = new List<SpriteRenderer>();


    // Use this for initialization
    void Start () {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                SpriteRenderer newRend = CreateRender(Water);

                if (i != 1)
                    newRend.flipX = !newRend.flipX;
                if(j != 1)
                    newRend.flipY = !newRend.flipY;

                newRend.transform.localPosition = new Vector3(
                    (i-1) * TileOffSet.x,
                    (j-1) * TileOffSet.y,
                    0);

                waterRenderers.Add(newRend);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
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
}
