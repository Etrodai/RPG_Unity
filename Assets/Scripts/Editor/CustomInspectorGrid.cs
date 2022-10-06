using UnityEngine;
using UnityEditor;

[CustomEditor((typeof(Gridsystem)))]
public class CustomInspectorGrid : Editor
{
    public override void OnInspectorGUI()
    {
        int spaceBetween = 25;
        DrawDefaultInspector();
        GUILayout.Space(25);
        
        Gridsystem gridsystem = (Gridsystem)target;
        if (GUILayout.Button("Create new Grid"))
        {
            //Delete actual Gridbox
            foreach (var tile in gridsystem.tileArray)
            {
                Destroy(tile.transform.parent.gameObject);
            }
            
            //Init new
            gridsystem.ReInitialize();
        }
    }
}
