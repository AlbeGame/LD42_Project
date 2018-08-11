using System.Collections;
using System.Collections.Generic;
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

    public bool flag;
    private bool position_reached;

    LilypadController parentLily;
    CircleCollider2D coll;

    void Start () {

        coll = GetComponent<CircleCollider2D>();
        mid_fade_pos = Vector2.zero;
        fade_position = transform.position;
        position_reached = true;
	}
	
    void Update () {
        CalculateJumpScale();
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

        if (!parentLily.CanLand(transform.position, coll.radius / 2))
            Debug.Log("Dead");
        else
            transform.parent = parentLily.transform;
    }

    public void OnInputHold()
    {
        key_hold_time += Time.deltaTime;
    }

    public void OnInputRelease()
    {
        IdentifyAction();
        key_hold_time = 0;
    }

    void IdentifyAction(){
        if(key_hold_time > jump_hold_time_threshold){
            float force = 1 + (key_hold_time - jump_hold_time_threshold);
            Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = ((Vector2)transform.position - mousepos).normalized;

            Jump(direction,force,Vector2.one * 1.4f);
        }
        else
            Eat();
        
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

        max_scale = MaxScaleOnAir;
        flag = false;
        position_reached = false;
        float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90);

        fade_position = transform.position - (Vector3)direction*(force*2);
        start_dist = Vector2.Distance(transform.position, fade_position);
        mid_fade_pos = Vector2.Lerp(transform.position, fade_position, 0.5f);
        FlipScale();
    }

    public void Eat(){
        Debug.Log("Eating");
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
