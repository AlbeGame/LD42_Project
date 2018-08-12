using System.Collections.Generic;
using UnityEngine;

public class BugsSpawner : MonoBehaviour {

    public Spawner2D SpawnInfo;
    public float BugsSpeed = 2;
    List<BugAI> bugsPull = new List<BugAI>();
    List<BugAI> bugsActive = new List<BugAI>();

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
        newBug.SetSpeed((Camera.main.transform.position - newBug.transform.position).normalized * BugsSpeed);
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

        niceOrigin.x = Random.Range(SpawnInfo.Center.x - SpawnInfo.Size.x / 2, SpawnInfo.Center.x + SpawnInfo.Size.x / 2);
        niceOrigin.y = Random.Range(SpawnInfo.Center.y - SpawnInfo.Size.y/2, SpawnInfo.Center.y + SpawnInfo.Size.y / 2);

        return niceOrigin;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(SpawnInfo.Center, SpawnInfo.Size);
    }
}
