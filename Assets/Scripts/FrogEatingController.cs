using UnityEngine;
using System.Collections.Generic;

public class FrogEatingController : MonoBehaviour {

    public int StomachCapacity = 50;
    public int EatenBugs;

    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BugAI bug = collision.GetComponent<BugAI>();

        if (bug != null)
        {
            Sprite imageToUse = bug.GetComponentInChildren<SpriteRenderer>().sprite;
            bug.Kill();
            EatenBugs++;
            float stomachIndex = EatenBugs/StomachCapacity;
            GameManager.I.UICtrl.StomachAdd(imageToUse, stomachIndex);
        }
    }
}
