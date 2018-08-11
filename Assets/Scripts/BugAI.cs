using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugAI : MonoBehaviour {
    private LilypadController pad;

    SpriteRenderer bugRenderer;
    BugsSpawner bugSpawner;

    float speed = 1;
    Vector2 direction;
    bool can_bite = false;
    //time he needs to wait before eating another chunk
    float cooldown = 0.4f;
    float temp_cool_down;

    void Start () {
        temp_cool_down = cooldown;
        bugRenderer = GetComponent<SpriteRenderer>();
        bugSpawner = GetComponent<BugsSpawner>();

	}

    public IEnumerator StartBiting(){
        while(true){
            transform.position -= ((Vector3)direction * Time.deltaTime / 2) * speed;

            if(cooldown <= 0)
                AttemptBite();
            else
                cooldown -= Time.deltaTime;
            
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator CheckForEnd(){
        while(can_bite){
            can_bite = pad.CanLand(transform.position, 10);
            if(!can_bite) {
                StopAllCoroutines();
                pad = null;
                GetComponent<Rigidbody2D>().simulated = true;
                transform.SetParent(null);
            }
            yield return new WaitForSeconds(4);
        }
    }
    void AttemptBite() {
        if(!can_bite)
            can_bite = pad.CanLand(transform.position, 10);
        if(can_bite){
            speed = Random.Range(0.3f, 0.65f);
            pad.EatLilypod(transform.position, Random.Range(7f, 17f));
            cooldown = temp_cool_down;
        }
    }

    public void SetSpeed(Vector2 _speed)
    {
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
                if (bugRenderer == null)
                    Destroy(this.gameObject);
                else
                    bugSpawner.ReturnBugToPull(this);
            }
            else
                currentTimer += Time.deltaTime;
        }
        else if (hasBeenRendered && !bugRenderer.isVisible)
        {
            if (bugRenderer == null)
                Destroy(this.gameObject);
            else
                bugSpawner.ReturnBugToPull(this);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        
        if(!pad && collision.gameObject.GetComponent<LilypadController>()){
            GetComponent<Rigidbody2D>().simulated = false;
            pad = collision.gameObject.GetComponent<LilypadController>();
            direction = (transform.position - pad.transform.position).normalized;
            StartCoroutine(StartBiting());
            StartCoroutine(CheckForEnd());
            transform.SetParent(collision.transform);
        }
    }
}
