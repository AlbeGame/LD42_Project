using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.AdvancedPolygonCollider;

public class BugAI : MonoBehaviour {
    private LilypadController pad;

    SpriteRenderer bugRenderer;
    BugsSpawner bugSpawner;

    float speed = 1;
    Vector2 velocity;
    Vector2 direction;

    //time he needs to wait before eating another chunk
    float cooldown = 0.4f;
    float temp_cool_down;

    void Start () {
        temp_cool_down = cooldown;
        bugRenderer = GetComponentInChildren<SpriteRenderer>();
        bugSpawner = GetComponent<BugsSpawner>();
        InvokeRepeating("UpdateCollider",0,5);
	}

    public void UpdateCollider(){
        if(pad && GameManager.I.Frog.parentLily != null && GameManager.I.Frog.parentLily == pad) {
            pad.GetComponent<AdvancedPolygonCollider>().RecalculatePolygon();
        }

    }

    private void Update() {
        if(!pad){
            //pad = GameManager.getcloserliliypad(myposition,mid_dist)
        }else if(Vector2.Distance(transform.position,pad.transform.position)>1.4f){
            Outside();
        }
    }

    public IEnumerator StartBiting(){
        while (true){
            transform.position -= ((Vector3)direction * Time.deltaTime / 2) * speed;

            if(cooldown <= 0)
                AttemptBite();
            else
                cooldown -= Time.deltaTime;
            
            yield return new WaitForEndOfFrame();
        }
    }

    void AttemptBite() {
        if(pad){
            speed = Random.Range(0.3f, 0.65f);
            pad.EatLilypod(transform.position, Random.Range(7f, 17f));
            cooldown = temp_cool_down;
        }
    }

    public void SetSpeed(Vector2 _speed)
    {
        velocity = _speed;
        GetComponent<Rigidbody2D>().velocity = _speed;
    }

    public void SetBugSpawner(BugsSpawner _spawner)
    {
        bugSpawner = _spawner;
    }

    bool hasBeenRendered;
    float currentTimer;
    private void PoolCheck()
    {
        if (!hasBeenRendered)
        {
            if (bugRenderer.isVisible)
                hasBeenRendered = true;
            else if (currentTimer > 20)
            {
                Kill();
            }
            else
                currentTimer += Time.deltaTime;
        }
        else if (hasBeenRendered && !bugRenderer.isVisible)
        {
            Kill();
        }
    }

    public void Kill()
    {
        if (bugRenderer == null)
            Destroy(this.gameObject);
        else
            bugSpawner.ReturnBugToPull(this);
    }

    private void Inside(LilypadController ctrl) {
        if(!pad || pad != ctrl && bugRenderer.isVisible) {
            pad = ctrl;
            GetComponent<Collider2D>().isTrigger = true;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            direction = (transform.position - pad.transform.position).normalized;
            StartCoroutine(StartBiting());
            transform.SetParent(ctrl.transform);
        } 
    }

    private void Outside() {
        if(pad) {
            pad = null;
            GetComponent<Collider2D>().isTrigger = true;
            StopAllCoroutines();
            transform.SetParent(null);
            SetSpeed(velocity);
        }
    }
}
