using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugAI : MonoBehaviour {
    public LilypodController pad;

    float speed = 1;
    Vector2 direction;

    //time he needs to wait before eating another chunk
    float cooldown = 0.4f;
    float temp_cool_down;

    void Start () {
        temp_cool_down = cooldown;
        direction = (transform.position - pad.transform.position).normalized;
	}
	void Update () {
        transform.position -= ((Vector3)direction * Time.deltaTime/2) * speed;

        if(cooldown <= 0)
            AttemptBite();
        else
            cooldown -= Time.deltaTime;
	}

    void AttemptBite(){
        if(pad.CanLand(transform.position,10)){
            speed = Random.Range(0.3f,0.65f);
            pad.EatLilypod(transform.position,Random.Range(9f,20f));
            cooldown = temp_cool_down;
        }
    }
}
