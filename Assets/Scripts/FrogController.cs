using DG.Tweening;
using UnityEngine;

public class FrogController : MonoBehaviour {
    //time the user hold the mouse btn
    public float key_hold_time;
    //min time to recognise the action as jump
    float jump_hold_time_threshold = 0.4f;

    //new position to fade on
    Vector2 fade_position;
    //mid point of position to fade on
    Vector2 mid_fade_pos;
    //new scale to fade on
    Vector2 scale_fade;
    //max scale to reach
    Vector2 max_scale;
    //starting distance between start->end
    float start_dist;

    private bool flag;
    private bool position_reached;

    [HideInInspector]
    public LilypadController parentLily;
    AudioController audioCtrl;

    Animator animCtrl;
    public Transform marker;
    void Start () {
        audioCtrl = GetComponentInChildren<AudioController>();
        eatingCtrl = GetComponentInChildren<FrogEatingController>();
        eatingCtrl.SetFrog(this);
        animCtrl = GetComponentInChildren<Animator>();
        mid_fade_pos = Vector2.zero;
        fade_position = transform.position;
        position_reached = true;
	}
	
    void Update () {
        //CalculateJumpScale();
        if(!position_reached)
            FadeToPosition();
	
    }

    float progress = 0;
    void CalculateJumpScale(){

        progress = 0;
        if(mid_fade_pos != Vector2.zero) {
            progress = 1 - Vector2.Distance(transform.position, mid_fade_pos) / (start_dist / 2f);
            progress = Mathf.Clamp01(progress);
            progress = Mathf.Abs((flag ? -1 : +0) + progress);
        }

        if(Vector2.Distance(transform.position, mid_fade_pos) < 0.3f && flag)
            FlipScale();

    }

    void FadeToPosition(){
        if(Vector2.Distance(transform.position, fade_position) > 0.01f) {
            transform.localScale = Vector2.Lerp(transform.localScale, scale_fade, progress);
            transform.position = Vector2.LerpUnclamped(transform.position, fade_position, Time.deltaTime * 7);
        } 
        else
            OnJumpCompleted();
    }

    void OnJumpCompleted(){
        position_reached = true;

        if (Vector2.Distance(parentLily.transform.position, transform.position)> 2)
            Debug.Log("Dead");
        else
            transform.parent = parentLily.transform;
    }

    public void OnInputHold(){
        key_hold_time += Time.deltaTime;
        if (key_hold_time > jump_hold_time_threshold)
            ShowJumpMarker();
    }

    public void OnInputRelease()
    {
        IdentifyAction();
        key_hold_time = 0;
        HideJumpMarker();
    }

    void ShowJumpMarker(){
        Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = ((Vector2)transform.position - mousepos);
        //float force = 1 + (key_hold_time - jump_hold_time_threshold) * 2.2f;
        //force = Mathf.Clamp(force, 0, 1);
        marker.transform.localPosition = transform.position - new Vector3(direction.x, direction.y,0);
        marker.GetComponent<LineRenderer>().SetPosition(1, (Vector3)direction);
    }

    public void JumpTo(Transform _objective, float _flyTime)
    {
        transform.DOJump(_objective.position, 1, 1, _flyTime).OnComplete(()=>OnJumpCompleted());
    }

    void HideJumpMarker(){
        marker.transform.localPosition = new Vector2(-9999, 9999);
    }

    void IdentifyAction(){
        if(key_hold_time > jump_hold_time_threshold) {
            float force = 1 + (key_hold_time - jump_hold_time_threshold);
            force = Mathf.Clamp(force, 0, 3);
            Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = ((Vector2)transform.position - mousepos);

            Jump(direction, force, Vector2.one * 1.4f);
        } 
        else {
            Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Eat(mousepos);
        }
    }

    void FlipScale() {
        flag = !flag;
        if(!flag)
            scale_fade = Vector2.one;
        else
            scale_fade = max_scale;
    }
    
    public void Jump(Vector2 direction,float force,Vector2 MaxScaleOnAir)
    {
        transform.parent = null;
        animCtrl.SetTrigger("Jump");
        audioCtrl.Play();

        max_scale = MaxScaleOnAir;
        flag = false;
        position_reached = false;
        float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90);

        fade_position = transform.position - (Vector3)direction/**(force*2)*/;
        start_dist = Vector2.Distance(transform.position, fade_position);
        mid_fade_pos = Vector2.Lerp(transform.position, fade_position, 0.5f);
        //FlipScale();
    }

    FrogEatingController eatingCtrl;
    public void Eat(Vector2 mousepos){
        mousepos = (mousepos - (Vector2)transform.position).normalized;
        float rot_z = Mathf.Atan2(mousepos.y, mousepos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        eatingCtrl.audioCtrl.Play();
        animCtrl.SetTrigger("Eat");
    }

    public void EatFeedback()
    {
        transform.DOPunchScale(Vector3.one * 0.1f, 1).OnComplete(()=>transform.localScale = Vector3.one);
    }

    public void SetParentLily(LilypadController _parentLily)
    {
        parentLily = _parentLily;
        transform.parent = parentLily.transform;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (position_reached)
            return;

        LilypadController lily = other.GetComponent<LilypadController>();
        if (lily != null)
            parentLily = lily;
    }
}
