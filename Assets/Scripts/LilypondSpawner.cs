﻿using System.Collections.Generic;
using UnityEngine;

public class LilypondSpawner : MonoBehaviour {

    public GameObject Lilypad;
    public float SpawnDelay = 1;
    List<LilypodController> lilypadsPull = new List<LilypodController>();

    float currentTimer = 0;
    private void Update()
    {
        if (currentTimer > SpawnDelay)
        {
            SpawnLilypad();
            currentTimer = 0 - Random.Range(0, SpawnDelay);
        }
        else
            currentTimer += Time.deltaTime;
    }

    public void SpawnLilypad()
    {
        LilypodController newlily;
        if (lilypadsPull.Count <= 0)
            newlily = Instantiate(Lilypad, transform).GetComponent<LilypodController>();
        else
        {
            newlily = lilypadsPull[0];
            lilypadsPull.RemoveAt(0);
        }

        newlily.Init();
        newlily.SetLilySpawner(this);
        newlily.transform.position = GetLilyOriginPosition();
        newlily.SetSpeedVector((Camera.main.transform.position - newlily.transform.position).normalized * 0.01f);
        newlily.gameObject.SetActive(true);
    }

    public void ReturnLilyToPull(LilypodController _lily)
    {
        lilypadsPull.Add(_lily);
        _lily.gameObject.SetActive(false);
    }

    Vector3 GetLilyOriginPosition()
    {
        Vector3 niceOrigin = new Vector3();

        niceOrigin.x = (Camera.main.transform.position.x + Random.Range(Screen.currentResolution.width / 2, Screen.currentResolution.width))/100;
        niceOrigin.y = (Camera.main.transform.position.y + Random.Range(Screen.currentResolution.height / 2, Screen.currentResolution.height))/ 100;

        if (Random.Range(0, 1f) < .5f)
            niceOrigin.x = -niceOrigin.x;

        if (Random.Range(0, 1f) < .5f)
            niceOrigin.y = -niceOrigin.y;

        return niceOrigin;
    }
}
