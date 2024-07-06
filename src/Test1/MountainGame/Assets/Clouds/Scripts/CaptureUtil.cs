using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TLab.EditorTool
{
#if UNITY_EDITOR
    public static class CaptureUtil
    {
        [MenuItem("Tools/Capture/MainCamera")]
        public static void CaptureMainCamera()
        {
            var renderCamera = Camera.main;
            if (renderCamera == null)
            {
                Debug.LogError("RenderingCamera was not found");

                return;
            }

            var size = new Vector2Int(renderCamera.pixelWidth, renderCamera.pixelHeight);
            var render = new RenderTexture(size.x, size.y, 24);
            var texture = new Texture2D(size.x, size.y, TextureFormat.RGB24, false);

            try
            {
                renderCamera.targetTexture = render;
                renderCamera.Render();

                RenderTexture.active = render;
                texture.ReadPixels(new Rect(0, 0, size.x, size.y), 0, 0);
                texture.Apply();
            }
            finally
            {
                renderCamera.targetTexture = null;
                RenderTexture.active = null;
            }

            string dt = System.DateTime.Now.ToString("yyyy-MM-ddTHHmmss"); // ISO-8601
            string fn = $"{Application.dataPath}/SceneViewScreenshot-{dt}-{renderCamera.transform.forward}.png";
            File.WriteAllBytes(fn, texture.EncodeToPNG());

            AssetDatabase.Refresh();
        }
    }
#endif
}