using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LilypodController : MonoBehaviour {
    public Texture2D origin;
    private Texture2D source;
    int pixel_eat;

	// Update is called once per frame
	void Update () {
        Move();
	}

    void EatLilypod(Vector2 world_point, float range){
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

    //return true if you have space to 
    bool CanLand(Vector2 w_pos,float radious){
        Vector3 pos = w_pos;
        pos.z = transform.position.z;
        pos = transform.InverseTransformPoint(pos);

        int xPixel = Mathf.RoundToInt(pos.x * GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);
        int yPixel = Mathf.RoundToInt(pos.y * GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);

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

    LilypondSpawner lilySpawner;
    SpriteRenderer lilyRenderer;
    public void Init()
    {
        source = new Texture2D(origin.width, origin.height);
        for (int x = 0; x < origin.width; x++)
            for (int y = 0; y < origin.height; y++)
                source.SetPixel(x, y, origin.GetPixel(x, y));
        source.Apply();

        lilyRenderer = GetComponent<SpriteRenderer>();
        lilyRenderer.sprite = Sprite.Create(source, new Rect(0, 0, origin.width, origin.height), new Vector2(0.5f, 0.5f));
    }

    Vector2 speed;
    public void SetSpeedVector(Vector2 _speed)
    {
        speed = _speed;
    }

    public void SetLilySpawner(LilypondSpawner _spawner)
    {
        lilySpawner = _spawner;
    }

    bool hasBeenRendered = false;
    private void Move()
    {
        transform.localPosition += (Vector3)speed;

        if (!hasBeenRendered && lilyRenderer.isVisible)
            hasBeenRendered = true;
        else if (hasBeenRendered && !lilyRenderer.isVisible)
            lilySpawner.ReturnLilyToPull(this);
    }
}
