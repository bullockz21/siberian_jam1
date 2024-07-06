﻿using UnityEditor;
using UnityEngine;

[System.Obsolete]
[CustomEditor (typeof (NoiseGenerator))]
public class NoiseGenEditor : Editor
{
    NoiseGenerator noise;
    Editor noiseSettingsEditor;

    public override void OnInspectorGUI ()
    {
        DrawDefaultInspector ();

        if (GUILayout.Button ("Update"))
        {
            noise.ManualUpdate ();
            EditorApplication.QueuePlayerLoopUpdate ();
        }

        if (GUILayout.Button ("Save")) Save();

        if (GUILayout.Button ("Load")) Load();

        if (noise.ActiveSettings != null)
            DrawSettingsEditor(noise.ActiveSettings, ref noise.showSettingsEditor, ref noiseSettingsEditor);
    }

    void Save ()
    {
        FindObjectOfType<Save3D>().Save(noise.shapeTexture, NoiseGenerator.shapeNoiseName);
        Debug.Log($"Texture saved: {noise.shapeTexture.name}");

        FindObjectOfType<Save3D>().Save(noise.detailTexture, NoiseGenerator.detailNoiseName);
        Debug.Log($"Texture saved: {noise.detailTexture.name}");
    }

    void Load ()
    {
        noise.Load(NoiseGenerator.shapeNoiseName, noise.shapeTexture);
        noise.Load(NoiseGenerator.detailNoiseName, noise.detailTexture);
    }

    void DrawSettingsEditor (Object settings, ref bool foldout, ref Editor editor)
    {
        if (settings != null)
        {
            foldout = EditorGUILayout.InspectorTitlebar (foldout, settings);
            using (var check = new EditorGUI.ChangeCheckScope ())
            {
                if (foldout)
                {
                    CreateCachedEditor (settings, null, ref editor);
                    editor.OnInspectorGUI ();
                }

                if (check.changed)
                    noise.ActiveNoiseSettingsChanged();
            }
        }
    }

    void OnEnable ()
    {
        noise = (NoiseGenerator) target;
    }
}