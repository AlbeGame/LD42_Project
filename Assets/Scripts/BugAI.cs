using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.AdvancedPolygonCollider;

public class BugAI : MonoBehaviour
{
    public float EatingRadius;
    private LilypadController pad;

    SpriteRenderer bugRenderer;
    AudioController audioCtrl;
    BugsSpawner bugSpawner;

    public float SpeedOnLily = 1;
    Vector2 velocity;

    //time he needs to wait before eating another chunk
    public float BiteDelay = 0.4f;
    float temp_cool_down;

    void Start()
    {
        audioCtrl = GetComponent<AudioController>();
        audioCtrl.SetVolume(.3f, 1);
        temp_cool_down = BiteDelay;
        bugRenderer = GetComponentInChildren<SpriteRenderer>();
        InvokeRepeating("UpdateCollider", 0, 5);
    }

    public void UpdateCollider()
    {
        if (pad && GameManager.I.Frog.parentLily != null && GameManager.I.Frog.parentLily == pad)
        {
            pad.GetComponent<AdvancedPolygonCollider>().RecalculatePolygon();
        }

    }

    private void Update()
    {

        if (!pad && bugRenderer.isVisible)
        {
            LilypadController _pad = GameManager.I.GetCloseLilyPad(transform.position, EatingRadius);
            if (_pad)
                Inside(_pad);
        }
        else if (pad && Vector2.Distance(transform.position, pad.transform.position) > EatingRadius + 0.2f)
        {
            Outside();
        }
    }

    public IEnumerator StartBiting(Vector2 direction)
    {
        Vector2 dir = direction;
        while (true)
        {
            transform.position -= ((Vector3)dir * Time.deltaTime / 2) * SpeedOnLily;

            if (BiteDelay <= 0)
                AttemptBite();
            else
                BiteDelay -= Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    int audioBitesToGive = 3;
    void AttemptBite()
    {
        if (pad)
        {
            if (audioBitesToGive > 0)
            {
                audioCtrl.SetVolume(Mathf.Clamp01(audioBitesToGive));
                audioCtrl.Play();
                audioBitesToGive--;
            }
            pad.EatLilypod(transform.position, Random.Range(7f, 17f));
            BiteDelay = temp_cool_down;
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
        audioBitesToGive = 3;
        if (bugSpawner == null)
            Destroy(this.gameObject);
        else
            bugSpawner.ReturnBugToPull(this);
    }

    private void Inside(LilypadController ctrl)
    {
        pad = ctrl;
        GetComponent<Collider2D>().isTrigger = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().isKinematic = true;
        Vector2 direction = (transform.position - pad.transform.position).normalized;
        StartCoroutine(StartBiting(direction));
        transform.SetParent(ctrl.transform);
    }

    private void Outside()
    {
        pad = null;
        GetComponent<Collider2D>().isTrigger = false;
        GetComponent<Rigidbody2D>().useFullKinematicContacts = true;
        StopAllCoroutines();
        transform.SetParent(null);
        SetSpeed(velocity);
    }
}
