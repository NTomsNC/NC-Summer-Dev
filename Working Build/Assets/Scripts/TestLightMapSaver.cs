using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestLightMapSaver : MonoBehaviour
{
    [Space(50.0f)]
    public int z = 0;

    public void SaveLightmaps()
    {
        Mesh meshToModify = GetComponent<MeshFilter>().sharedMesh;
        Vector4 lightmapOffsetAndScale = GetComponent<Renderer>().lightmapScaleOffset;

        Vector2[] modifiedUV2s = meshToModify.uv2;
        for (int i = 0; i < meshToModify.uv2.Length; i++)
        {
            modifiedUV2s[i] = new Vector2(meshToModify.uv2[i].x * lightmapOffsetAndScale.x +
            lightmapOffsetAndScale.z, meshToModify.uv2[i].y * lightmapOffsetAndScale.y +
            lightmapOffsetAndScale.w);
        }
        meshToModify.uv2 = modifiedUV2s;
    }
}

[CustomEditor(typeof(TestLightMapSaver))]
public class TestLightMapSaverEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUI.Button(new Rect(0, 20, 400, 30), "Save LightMaps into New Prefabs"))
        {
            TestLightMapSaver lightmapper = (TestLightMapSaver)target;

            lightmapper.SaveLightmaps();
        }
    }
}
