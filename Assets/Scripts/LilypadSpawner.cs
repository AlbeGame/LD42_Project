using System.Collections.Generic;
using UnityEngine;

public class LilypadSpawner : MonoBehaviour {

    public GameObject Lilypad;
    public float SpawnDelay = 1;
    public float LilysSpeed = 2;
    public int MaxLilyAmount = 10;
    int lilyAmount;
    List<LilypadController> lilypadsPull = new List<LilypadController>();
    
    Vector2 randomSpaceVector {
        get
        {
            return new Vector2(Random.Range(-Screen.currentResolution.width, Screen.currentResolution.width), Random.Range(-Screen.currentResolution.height, Screen.currentResolution.height));
        }
    }

    float currentTimer = 0;
    private void Update()
    {
        if (currentTimer > SpawnDelay)
        {
            if(lilyAmount < MaxLilyAmount)
                SpawnLilypad();
            currentTimer = 0 - Random.Range(0, SpawnDelay);
        }
        else
            currentTimer += Time.deltaTime;
    }

    public void SpawnLilypad()
    {
        LilypadController newlily;
        if (lilypadsPull.Count <= 0)
        {
            lilyAmount++;
            newlily = Instantiate(Lilypad, transform).GetComponent<LilypadController>();
        }
        else
        {
            newlily = lilypadsPull[0];
            lilypadsPull.RemoveAt(0);
        }

        newlily.Init();
        newlily.SetLilySpawner(this);
        newlily.transform.position = GetLilyOriginPosition();
        newlily.SetSpeedVector((Camera.main.transform.position - newlily.transform.position + (Vector3)randomSpaceVector/100).normalized * 0.01f * LilysSpeed);
        newlily.gameObject.SetActive(true);
    }

    public void SpawnLilypad(Vector3 _position, Vector3 _speed)
    {
        LilypadController newlily;
        if (lilypadsPull.Count <= 0)
        {
            lilyAmount++;
            newlily = Instantiate(Lilypad, transform).GetComponent<LilypadController>();
        }
        else
        {
            newlily = lilypadsPull[0];
            lilypadsPull.RemoveAt(0);
        }

        newlily.Init();
        newlily.SetLilySpawner(this);
        newlily.transform.position = _position;
        newlily.SetSpeedVector(_speed);
        newlily.gameObject.SetActive(true);
    }

    public void ReturnLilyToPull(LilypadController _lily)
    {
        lilypadsPull.Add(_lily);
        _lily.gameObject.SetActive(false);
    }

    Vector3 GetLilyOriginPosition()
    {
        Vector3 niceOrigin = new Vector3();

        niceOrigin.x = (Camera.main.transform.position.x + Screen.currentResolution.width * Random.Range(.6f,1.1f))/100;
        niceOrigin.y = (Camera.main.transform.position.y + Screen.currentResolution.height * Random.Range(.6f,1.1f)) / 100;

        if (Random.Range(0, 1f) < .5f)
            niceOrigin.x = -niceOrigin.x;

        if (Random.Range(0, 1f) < .5f)
            niceOrigin.y = -niceOrigin.y;

        return niceOrigin;
    }
}
