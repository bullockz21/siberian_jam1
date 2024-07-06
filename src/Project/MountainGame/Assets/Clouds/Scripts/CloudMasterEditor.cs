using System.IO;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR

[System.Obsolete]
[CustomEditor(typeof(CloudMaster))]
public class CloudMasterEditor : Editor
{
    CloudMaster master;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }

    void OnEnable()
    {
        master = (CloudMaster)target;
    }
}

#endif
