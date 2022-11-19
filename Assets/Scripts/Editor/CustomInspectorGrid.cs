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
            int spaceBetween = 25; //TODO: is it used???
            DrawDefaultInspector();
            GUILayout.Space(25);
        
            Gridsystem gridsystem = (Gridsystem)target;
            if (!GUILayout.Button("Create new Grid")) return;
            //Delete actual Gridbox
            for (int x = 0; x < Gridsystem.Instance.TileArray.GetLength(0); x++)
            {
                for (int y = 0; y < Gridsystem.Instance.TileArray.GetLength(1); y++)
                {
                    for (int z = 0; z < Gridsystem.Instance.TileArray.GetLength(2); z++)
                    {
                        GridTile tile = Gridsystem.Instance.TileArray[x, y, z];
                        Destroy(tile.transform.parent.gameObject);
                    }
                }
            }

            //Init new
            gridsystem.ReInitialize();
        }
    }
}
