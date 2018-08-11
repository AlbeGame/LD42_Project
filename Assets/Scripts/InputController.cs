using UnityEngine;

public class InputController : MonoBehaviour {

	// Update is called once per frame
	void Update () {

        if(Input.GetMouseButton(0))
            GameManager.I.Frog.OnInputHold();
        if (Input.GetMouseButtonUp(0))
            GameManager.I.Frog.OnInputRelease();

    }
}
