using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : MonoBehaviour {

    public float key_hold_time;
    float jump_hold_time_threshold = 0.4f;

    Vector2 fade_position;
    Vector2 mid_fade_pos;
    float start_dist;
    Vector2 scale_fade;

    public bool flag;
	void Start () {
        mid_fade_pos = Vector2.zero;
        fade_position = transform.position;

	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKey(KeyCode.Mouse0)){
            key_hold_time += Time.deltaTime; 
        }
        if(Input.GetKeyUp(KeyCode.Mouse0)){
            IdentifyAction();
            key_hold_time = 0;
        }
        float progress = 0;
        if(mid_fade_pos != Vector2.zero) {
            progress = 1 - Vector2.Distance(transform.position, mid_fade_pos) / (start_dist / 2f);
            progress = Mathf.Clamp01(progress);
            progress = Mathf.Abs((flag ? -1 : +0) + progress);
        }

        if(Vector2.Distance(transform.position, mid_fade_pos) < 0.3f && flag)
            FlipScale();
        
        transform.localScale = Vector2.Lerp(transform.localScale, scale_fade,progress);

        transform.position = Vector2.LerpUnclamped(transform.position,fade_position,Time.deltaTime*7);
	}

    void IdentifyAction(){
        if(key_hold_time > jump_hold_time_threshold)
            Jump();
        else
            Eat();
    }

    void FlipScale() {
        flag = !flag;
        if(!flag)
            scale_fade = Vector2.one;
        else
            scale_fade = Vector2.one * 1.2f;
    }
    

    void Jump(){
        flag = false;
        float force = 1 + (key_hold_time - jump_hold_time_threshold);

        Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = ((Vector2)transform.position - mousepos).normalized;

        float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90);

        fade_position = transform.position - (Vector3)direction*(force*2);
        start_dist = Vector2.Distance(transform.position, fade_position);
        mid_fade_pos = Vector2.Lerp(transform.position, fade_position, 0.5f);
        FlipScale();

    }

    void Eat(){
        Debug.Log("Eating");
    }
}
