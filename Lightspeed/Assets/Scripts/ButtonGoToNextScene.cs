using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGoToNextScene : MonoBehaviour {

	public void GoToNextScene()
    {
        GameController.instance.GoToNextScene();
    }
}
