using System.Collections.Generic;
using UnityEngine;

public class BugsSpawner : MonoBehaviour {

    public GameObject BugPrefab;
    public float SpawnDelay = 1;
    public float BugsSpeed = 2;
    public int MaxBugsAmount = 50;
    List<BugAI> bugsPull = new List<BugAI>();
    int bugsAmount;

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
            if(bugsAmount < MaxBugsAmount)
                SpawnBug();
            currentTimer = 0 - Random.Range(0, SpawnDelay);
        }
        else
            currentTimer += Time.deltaTime;
    }

    public void SpawnBug()
    {
        BugAI newBug;
        if (bugsPull.Count <= 0)
        {
            newBug = Instantiate(BugPrefab, transform).GetComponent<BugAI>();
            bugsAmount++;
        }
        else
        {
            newBug = bugsPull[0];
            bugsPull.RemoveAt(0);
        }

        newBug.SetBugSpawner(this);
        newBug.transform.position = GetBugOriginPosition();
        newBug.SetSpeed((Camera.main.transform.position - newBug.transform.position + (Vector3)randomSpaceVector/100).normalized * BugsSpeed);
        newBug.gameObject.SetActive(true);
    }

    public void ReturnBugToPull(BugAI _lily)
    {
        bugsPull.Add(_lily);
        bugsAmount--;
        _lily.gameObject.SetActive(false);
    }

    Vector3 GetBugOriginPosition()
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
