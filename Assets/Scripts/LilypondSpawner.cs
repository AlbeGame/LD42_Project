using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilypondSpawner : MonoBehaviour {
    public GameObject lilypond;
    public Transform camera_pos;
    List<Vector2> closed_position = new List<Vector2>();

    int last_x = 0;
	void Start () {
		
	}
	
	void Update () {
        if((int)camera_pos.position.x % 20 == 0 && last_x != (int)camera_pos.position.x){
            SpawnPonds();
            last_x = (int)camera_pos.position.x;
        }
	}

    void SpawnPonds(){
        float x_start = camera_pos.position.x;
        for(int i = 0; i < 3; i++) {
            Vector2 newpos;
            bool exit = true;
            do {
                exit = true;
                newpos = new Vector2(x_start + Random.Range(0, 20f), Random.Range(-10f, 10f));
                foreach(Vector2 v in closed_position){
                    if(Vector2.Distance(newpos, v) < 5) {
                        exit = false;
                        break;
                    }
                }
            } while(!exit);
            closed_position.Add(newpos);
            Instantiate(lilypond,newpos,Quaternion.identity);
        }
    }
}
