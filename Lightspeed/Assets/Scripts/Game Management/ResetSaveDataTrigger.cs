using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetSaveDataTrigger : MonoBehaviour
{
    public void ResetSaveData()
    {
        GameController.instance.ClearSaveData();
    }
}
