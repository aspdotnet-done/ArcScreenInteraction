using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(ImageFitter))]
public class ImageFitterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ImageFitter imageFitter = (ImageFitter)target;
        if (GUILayout.Button("Fit"))
        {
            imageFitter.Fit();
        }
    }
}