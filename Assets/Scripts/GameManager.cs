using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager I;

    public FrogController Frog;
    public LilypadSpawner LilySpawner;
    public BugsSpawner BugsSpawner;
    public UIController UICtrl;

    InputController inputCtrl;

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
        MainMenu();
    }

    public void StartGame()
    {
        LilySpawner.gameObject.SetActive(true);
        BugsSpawner.gameObject.SetActive(true);
        LilySpawner.SpawnLilypad();
        Frog.gameObject.SetActive(true);
        LilypadController spawnedLily = GetCloseLilyPad(Frog.transform.position, 100);
        Frog.transform.position = spawnedLily.transform.position;
        Frog.SetParentLily(spawnedLily);
        UICtrl.ToggleMainMenu(false);

        inputCtrl.enabled = true;
    }

    public void MainMenu()
    {
        inputCtrl = GetComponent<InputController>();
        inputCtrl.enabled = false;
        UICtrl.ToggleMainMenu(true);
        Frog.gameObject.SetActive(false);
        LilySpawner.gameObject.SetActive(false);
        BugsSpawner.gameObject.SetActive(false);
    }

    public LilypadController GetCloseLilyPad(Vector2 from,float min_dist){
        foreach(LilypadController l in LilySpawner.GetLilipads()) {
            if(Vector2.Distance(l.transform.position, from) < min_dist)
                return l;
        }
        return null;
    }
}
