using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGoToMainMenu : MonoBehaviour
{
    public void GoToMainMenu()
    {
        GameController.instance.GoToMainMenu();
    }
}
