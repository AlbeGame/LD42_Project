using System.Collections.Generic;
using UnityEngine;

public class BugsSpawner : MonoBehaviour {

    public Spawner2D SpawnInfo;
    public float BugsSpeed = 2;
    List<BugAI> bugsPull = new List<BugAI>();
    List<BugAI> bugsActive = new List<BugAI>();

    Vector2 randomSpaceVector {
        get
        {
            return new Vector2(Random.Range(-Screen.currentResolution.width, Screen.currentResolution.width), Random.Range(-Screen.currentResolution.height, Screen.currentResolution.height));
        }
    }

    float currentTimer = 0;
    private void Update()
    {
        if (currentTimer > SpawnInfo.DelayBetweenSpawns)
        {
            if(bugsActive.Count < SpawnInfo.MaxAmunt)
                SpawnBug();
            currentTimer = 0 - Random.Range(0, SpawnInfo.DelayBetweenSpawns);
        }
        else
            currentTimer += Time.deltaTime;
    }

    public void SpawnBug()
    {
        BugAI newBug;
        if (bugsPull.Count <= 0)
        {
            newBug = Instantiate(SpawnInfo.GetRandomSpawnable(), transform).GetComponent<BugAI>();
            bugsActive.Add(newBug);
        }
        else
        {
            newBug = bugsPull[0];
            bugsActive.Add(newBug);
            bugsPull.RemoveAt(0);
        }

        newBug.SetBugSpawner(this);
        newBug.transform.position = GetBugOriginPosition();
        newBug.SetSpeed((Camera.main.transform.position - newBug.transform.position + (Vector3)randomSpaceVector/100).normalized * BugsSpeed);
        newBug.gameObject.SetActive(true);
    }

    public void ReturnBugToPull(BugAI _bug)
    {
        bugsPull.Add(_bug);
        bugsActive.Remove(_bug);
        _bug.gameObject.SetActive(false);
    }

    public List<BugAI> GetBugs()
    {
        return bugsActive;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(SpawnInfo.Center, SpawnInfo.Size);
    }
}
