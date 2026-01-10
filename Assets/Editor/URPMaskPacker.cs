using UnityEngine;
using UnityEditor;
using System.IO;

public class URPMaskPacker : EditorWindow {
    public Texture2D roughnessTex;
    public string outputPath = "Textures/MaskMap.png";  // Relative to Assets

    [MenuItem("Tools/Polycam: Pack URP Mask Map")]
    static void Init() => GetWindow<URPMaskPacker>("URP Mask Packer").Show();

    void OnGUI() {
        roughnessTex = (Texture2D)EditorGUILayout.ObjectField("Roughness Texture", roughnessTex, typeof(Texture2D), false);
        outputPath = EditorGUILayout.TextField("Output Path (in Assets/)", outputPath);
        if (GUILayout.Button("Pack Mask Map")) PackMask();
    }

    void PackMask() {
        if (roughnessTex == null) return;
        Texture2D mask = new(roughnessTex.width, roughnessTex.height, TextureFormat.RGBA32, false);
        Color[] pixels = roughnessTex.GetPixels();
        for (int i = 0; i < pixels.Length; i++) {
            float rough = pixels[i].grayscale;
            float smooth = 1f - rough;  // Invert to Smoothness
            pixels[i] = new Color(0f, 1f, 1f, smooth);  // R=0 Metallic, G=1 AO, B=1 Detail, A=Smoothness
        }
        mask.SetPixels(pixels);
        mask.Apply();
        byte[] bytes = mask.EncodeToPNG();
        File.WriteAllBytes(Path.Combine(Application.dataPath, outputPath), bytes);
        AssetDatabase.Refresh();
        Debug.Log($"URP Mask Map saved: Assets/{outputPath}");
        DestroyImmediate(mask);
    }
}
