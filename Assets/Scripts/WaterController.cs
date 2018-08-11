using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour {

    public Sprite WaterShade;
    SpriteRenderer wShadeRend1;
    SpriteRenderer wShadeRend2;
    public List<Sprite> WaterStains = new List<Sprite>();
    List<SpriteRenderer> wStainsRends1 = new List<SpriteRenderer>();
    List<SpriteRenderer> wStainsRends2 = new List<SpriteRenderer>();

    public Sprite WaterWaves;
    SpriteRenderer wWavesRend1;
    SpriteRenderer wWavesRend2;

    // Use this for initialization
    void Start () {

        wShadeRend1 = CreateRender(WaterShade, false);
        wShadeRend1 = CreateRender(WaterShade, true);

        for (int i = 0; i < WaterStains.Count; i++)
        {
            wStainsRends1.Add(CreateRender(WaterStains[i], false));
            wStainsRends1.Add(CreateRender(WaterStains[i], true));
        }

        wWavesRend1 = CreateRender(WaterWaves, false);
        wWavesRend2 = CreateRender(WaterWaves, true);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Create a child GameObject with a spriteRenderer (that renders _image)
    /// </summary>
    /// <param name="_image"></param>
    /// <returns></returns>
    SpriteRenderer CreateRender(Sprite _image, bool _flipAndDisplaced)
    {
        GameObject newGameObject = new GameObject(_image.name + " Water Renderer");
        newGameObject.transform.parent = transform;

        SpriteRenderer newRenderer = newGameObject.AddComponent<SpriteRenderer>();
        newRenderer.sprite = _image;

        if (_flipAndDisplaced)
        {
            newRenderer.flipX = true;
            newGameObject.transform.localPosition += Vector3.right * Screen.currentResolution.width / 100;
        }

        return newRenderer;
    }
}
