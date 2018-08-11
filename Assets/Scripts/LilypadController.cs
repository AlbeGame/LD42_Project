using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[ExecuteInEditMode]
public class LilypadController : MonoBehaviour {
    public Texture2D origin;
    private Texture2D source;
    int pixel_eat;
    int width;
    int height;

    private void OnEnable()
    {
        Init();
    }

    // Update is called once per frame
    void Update () {
        PoolCheck();
	}

    public async void EatLilypod(Vector2 world_point, float range) {
        Vector3 pos = world_point;
        pos.z = transform.position.z;
        pos = transform.InverseTransformPoint(pos);

        int xPixel = Mathf.RoundToInt(pos.x * lilyRenderer.sprite.pixelsPerUnit);
        int yPixel = Mathf.RoundToInt(pos.y * lilyRenderer.sprite.pixelsPerUnit);

        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                Vector2 pixels = new Vector2(x - (width / 2f), y - (height / 2f));
                if(ManatthanDistance(pixels, new Vector2(xPixel, yPixel)) < range) {
                    if(pixel_eat % 30 == 0)
                        await Task.Delay(1);
                    source.SetPixel(x, y, Color.clear);
                    pixel_eat++;
                }
            }
        }
        source.Apply();

    }

    //return true if you have space to 
    public bool CanLand(Vector2 w_pos, float radious) {
        Vector3 pos = w_pos;
        pos.z = transform.position.z;
        pos = transform.InverseTransformPoint(pos);

        int xPixel = Mathf.RoundToInt(pos.x * lilyRenderer.sprite.pixelsPerUnit);
        int yPixel = Mathf.RoundToInt(pos.y * lilyRenderer.sprite.pixelsPerUnit);
        if(ManatthanDistance(new Vector2(xPixel, yPixel), Vector2.zero) > width / 1.9f)
            return false;
        int good_pixel = 0;
        int bad_pixel = PixelsInRange(radious);

        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {

                Vector2 pixels = new Vector2(x - (width / 2f), y - (height / 2f));

                if(ManatthanDistance(pixels, new Vector2(xPixel, yPixel)) < radious) {
                    if(source.GetPixel(x, y) != Color.clear) {
                        good_pixel++;
                        bad_pixel--;
                        if(good_pixel > bad_pixel)
                            return true;
                    }
                }
            }
        }

        if(bad_pixel > good_pixel)
            return false;
        else
            return true;
    }
    float ManatthanDistance(Vector2 a, Vector2 b) {
        return Abs(a.x - b.x) + Abs(a.y - b.y);
    }
    float Abs(float n) {
        if(n < 0)
            return -n;
        else
            return n;
    }
    //how many pixels can we count in a speecific range
    int PixelsInRange(float range) {
        int n = 0;
        for(int x = width / 2; x < width; x++) {
            for(int y = height / 2; y < height; y++) {
                Vector2 pixels = new Vector2(x - (width / 2f), y - (height / 2f));
                if(ManatthanDistance(pixels, new Vector2(0, 0)) < range)
                    n++;
            }
        }
        return n;
    }

    LilypadSpawner lilySpawner;
    SpriteRenderer lilyRenderer;
    public void Init()
    {
        width = origin.width;
        height = origin.height;
        source = new Texture2D(width, height);
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                source.SetPixel(x, y, origin.GetPixel(x, y));
        source.Apply();

        lilyRenderer = GetComponent<SpriteRenderer>();
        lilyRenderer.sprite = Sprite.Create(source, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
    }

    Vector2 speed;
    public void SetSpeedVector(Vector2 _speed)
    {
        speed = _speed;
        GetComponent<Rigidbody2D>().velocity = speed;
    }

    public void SetLilySpawner(LilypadSpawner _spawner)
    {
        lilySpawner = _spawner;
    }

    bool hasBeenRendered = false;
    float currentTimer;
    private void PoolCheck()
    {
        if (!hasBeenRendered)
        {
            if (lilyRenderer.isVisible)
                hasBeenRendered = true;
            else if (currentTimer > 20)
            {
                if (lilySpawner == null)
                    Destroy(this.gameObject);
                else
                    lilySpawner.ReturnLilyToPull(this);
            }
            else
                currentTimer += Time.deltaTime;
        }
        else if (hasBeenRendered && !lilyRenderer.isVisible)
        {
            if (lilySpawner == null)
                Destroy(this.gameObject);
            else
                lilySpawner.ReturnLilyToPull(this);
        }
    }
}
