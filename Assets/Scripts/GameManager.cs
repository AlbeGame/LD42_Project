using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager I;

    public FrogController Frog;
    public LilypadSpawner LilySpawner;
    public WaterController WaterCtrl;

    private void Awake()
    {
        if (I != null)
            DestroyImmediate(this.gameObject);
        else
        {
            I = this;
        }
    }

    private void Start()
    {
        Vector3 startLilyPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
        LilySpawner.SpawnLilypad(startLilyPos, Vector3.zero);
        Frog.SetParentLily(FindObjectOfType<LilypadController>());
    }
    public LilypadController GetCloseLilyPad(Vector2 from,float min_dist){
        foreach(LilypadController l in LilySpawner.GetLilipads()) {
            if(Vector2.Distance(l.transform.position, from) < min_dist)
                return l;
        }
        return null;
    }
}
