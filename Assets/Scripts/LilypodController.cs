using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilypodController : MonoBehaviour {
    public Texture2D origin;
    private Texture2D source;
    int pixel_eat;
	// Use this for initialization
	void Start () {
        source = new Texture2D(origin.width, origin.height);
        for(int x = 0; x < origin.width; x++)
            for(int y = 0; y < origin.height; y++)
                source.SetPixel(x,y,origin.GetPixel(x,y));
        source.Apply();
        GetComponent<SpriteRenderer>().sprite = Sprite.Create(source, new Rect(0, 0, origin.width, origin.height), new Vector2(0.5f, 0.5f));
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            DeformPixels(mousepos,30);
        }

		
	}

    void DeformPixels(Vector2 world_point, float range){
        Vector3 pos = world_point;
        pos.z = transform.position.z;
        pos = transform.InverseTransformPoint(pos);

        int xPixel = Mathf.RoundToInt(pos.x * GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);
        int yPixel = Mathf.RoundToInt(pos.y * GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);

        for(int x = 0; x < origin.width; x++) {
            for(int y = 0; y < origin.height; y++) {
                
                Vector2 pixels = new Vector2(x - (origin.width / 2f), y - (origin.height / 2f));

                if(Vector2.Distance(pixels,new Vector2(xPixel,yPixel)) < range){
                    source.SetPixel(x, y, Color.clear);
                    pixel_eat++;
                }
            }
        }
        source.Apply();
        
    }
}
