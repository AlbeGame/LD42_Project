using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class LilypodController : MonoBehaviour {
    public Texture2D origin;
    private Texture2D source;
    int pixel_eat;
    SpriteRenderer s_renderer;
    private int height;
    private int widht;
	void Start () {
        if(source)
            return;
        source = new Texture2D(origin.width, origin.height);
        for(int x = 0; x < origin.width; x++)
            for(int y = 0; y < origin.height; y++)
                source.SetPixel(x,y,origin.GetPixel(x,y));
        source.Apply();
        s_renderer = GetComponent<SpriteRenderer>();
        s_renderer.sprite = Sprite.Create(source, new Rect(0, 0, origin.width, origin.height), new Vector2(0.5f, 0.5f));
	}
	
	void Update () {
        if(source == null)
            Start();


        if(Input.GetKeyDown(KeyCode.Mouse0)){
            Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            EatLilypod(mousepos,30);
        }

        if(Input.GetKeyDown(KeyCode.T)){
            Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
		
	}

    public void EatLilypod(Vector2 world_point, float range){
        Vector3 pos = world_point;
        pos.z = transform.position.z;
        pos = transform.InverseTransformPoint(pos);

        int xPixel = Mathf.RoundToInt(pos.x * s_renderer.sprite.pixelsPerUnit);
        int yPixel = Mathf.RoundToInt(pos.y * s_renderer.sprite.pixelsPerUnit);

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

    //return true if you have space to 
    public bool CanLand(Vector2 w_pos,float radious){
        if(Vector2.Distance(transform.position, w_pos) > radious/10f)
            return false;
        
        Vector3 pos = w_pos;
        pos.z = transform.position.z;
        pos = transform.InverseTransformPoint(pos);

        int xPixel = Mathf.RoundToInt(pos.x * s_renderer.sprite.pixelsPerUnit);
        int yPixel = Mathf.RoundToInt(pos.y * s_renderer.sprite.pixelsPerUnit);

        int good_pixel = 0;
        int bad_pixel = PixelsInRange(radious);

        for(int x = 0; x < origin.width; x++) {
            for(int y = 0; y < origin.height; y++) {

                Vector2 pixels = new Vector2(x - (origin.width / 2f), y - (origin.height / 2f));

                if(Vector2.Distance(pixels, new Vector2(xPixel, yPixel)) < radious) {
                    if(source.GetPixel(x, y) != Color.clear) {
                        good_pixel++;
                        bad_pixel--;
                    }
                }
            }
        }

        if(bad_pixel > good_pixel)
            return false;
        else
            return true;
    }

    //how many pixels can we count in a speecific range
    int PixelsInRange(float range){
        int n = 0;
        for(int x = 0; x < origin.width; x++) {
            for(int y = 0; y < origin.height; y++) {
                Vector2 pixels = new Vector2(x - (origin.width / 2f), y - (origin.height / 2f));
                if(Vector2.Distance(pixels, new Vector2(0,0)) < range)
                    n++;
            }
        }
        return n;
    }

}
