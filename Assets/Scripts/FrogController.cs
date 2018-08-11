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
    void Start () {
        mid_fade_pos = Vector2.zero;
        fade_position = transform.position;
        position_reached = true;
	}
	

    void Update () {
        CalculateJumpScale();
        if(!position_reached)
            FadeToPosition();
	
    }

    private void LateUpdate() {
        GetControls();
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
        Debug.Log("Jump done");
    }
    void GetControls(){
        if(Input.GetKey(KeyCode.Mouse0))
            key_hold_time += Time.deltaTime;
        if(Input.GetKeyUp(KeyCode.Mouse0)) {
            IdentifyAction();
            key_hold_time = 0;
        }
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
    

    public void Jump(Vector2 direction,float force,Vector2 MaxScaleOnAir){
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
}
