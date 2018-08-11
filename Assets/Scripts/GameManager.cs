using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager I;

    private void Awake()
    {
        if (I != null)
            DestroyImmediate(this.gameObject);
        else
        {
            I = this;
        }
    }
}
