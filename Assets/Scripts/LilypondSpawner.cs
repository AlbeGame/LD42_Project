using System.Collections.Generic;
using UnityEngine;

public class LilypondSpawner : MonoBehaviour {

    public GameObject Lilypad;
    public float SpawnRatio = 1;
    List<LilypodController> lilypadsPull = new List<LilypodController>();


    private void Update()
    {
        
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
    }

    public void ReturnLilyToPull(LilypodController _lily)
    {
        lilypadsPull.Add(_lily);
    }
}
