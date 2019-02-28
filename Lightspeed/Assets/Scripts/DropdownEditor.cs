using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


// base on: https://www.youtube.com/watch?v=T-IhjR7ktPQ
[CustomEditor(typeof(PhysicsPlatformMovementController))]
public class DropdownEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PhysicsPlatformMovementController script = (PhysicsPlatformMovementController)target;

        GUIContent arrayLabel = new GUIContent("follow X or Y?");
        script.dropDownIndex = EditorGUILayout.Popup(arrayLabel, script.dropDownIndex, script.followXorY);
        script.followX = (0 == script.dropDownIndex);
    }
}
