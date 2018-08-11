using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilypodController : MonoBehaviour {
    public Texture2D origin;
    private Texture2D source;
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
            DeformPixels(mousepos,1);
        }

		
	}

    void DeformPixels(Vector2 world_point, float range){
        Vector2 position = transform.position;
        //distanze w_p e position 0 1
        // 0 1


        for(int x = 0; x < origin.width; x++) {
            Debug.Log(position.x + x - (origin.width / 2f));
            for(int y = 0; y < origin.height; y++) {
                
                Vector2 pixe_to_world = new Vector2(
                    (position.x + x - (origin.width / 2f)) / ((float)origin.width / 2),
                    (position.y + y - (origin.height / 2f)) / ((float)origin.height / 2));
                pixe_to_world.x = Mathf.Abs(pixe_to_world.x);
                pixe_to_world.y = Mathf.Abs(pixe_to_world.y);

                if(Vector2.Distance(pixe_to_world, world_point) < range)
                    source.SetPixel(x, y, Color.clear);
            }
        }
        source.Apply();
        
    }
}
