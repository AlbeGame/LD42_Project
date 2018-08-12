using System.Collections.Generic;
using UnityEngine;

public class LilypadSpawner : MonoBehaviour {

    public Spawner2D SpawnInfo = new Spawner2D();
    public float LilysSpeed = 2;
    List<LilypadController> lilypadsPull = new List<LilypadController>();
    List<LilypadController> lilypadsActive = new List<LilypadController>();
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
            if(lilypadsActive.Count < SpawnInfo.MaxAmunt)
                SpawnLilypad();
            currentTimer = 0 - Random.Range(0, SpawnInfo.DelayBetweenSpawns);
        }
        else
            currentTimer += Time.deltaTime;
    }

    public void SpawnLilypad()
    {
        LilypadController newlily;
        if (lilypadsPull.Count <= 0)
        {
            newlily = Instantiate(SpawnInfo.GetRandomSpawnable(), transform).GetComponent<LilypadController>();
            lilypadsActive.Add(newlily);
        }
        else
        {
            newlily = lilypadsPull[0];
            lilypadsActive.Add(newlily);
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
            newlily = Instantiate(SpawnInfo.GetRandomSpawnable(), transform).GetComponent<LilypadController>();
            lilypadsActive.Add(newlily);
        }
        else
        {
            newlily = lilypadsPull[0];
            lilypadsActive.Add(newlily);
            lilypadsPull.RemoveAt(0);
        }

        newlily.Init();
        newlily.SetLilySpawner(this);
        newlily.transform.position = _position;
        newlily.SetSpeedVector(_speed);
        newlily.gameObject.SetActive(true);
    }

    public List<LilypadController> GetLilipads()
    {
        return lilypadsActive;
    }

    public void ReturnLilyToPull(LilypadController _lily)
    {
        lilypadsPull.Add(_lily);
        lilypadsActive.Remove(_lily);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(SpawnInfo.Center, SpawnInfo.Size);
    }

}
[System.Serializable]
public struct Spawner2D
{
    public Vector2 Center;
    public Vector2 Size;

    public int MaxAmunt;
    public float DelayBetweenSpawns;

    public List<GameObject> SpawnableItems;

    public GameObject GetRandomSpawnable()
    {
        int index = Random.Range(0, SpawnableItems.Count);
        return SpawnableItems[index];
    }
}
