using UI.Gridsystem;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor((typeof(Gridsystem)))]
    public class CustomInspectorGrid : UnityEditor.Editor
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
                foreach (var tile in Gridsystem.Instance.TileArray)
                {
                    Destroy(tile.transform.parent.gameObject);
                }
            
                //Init new
                gridsystem.ReInitialize();
            }
        }
    }
}
