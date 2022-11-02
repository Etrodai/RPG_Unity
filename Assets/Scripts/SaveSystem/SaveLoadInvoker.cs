using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SaveSystem
{
    public class SaveLoadInvoker : MonoBehaviour
    {
        [SerializeField] private TMP_InputField saveName;
        private List<Button> loadButton = new();
    
        public void OnSaveButtonClick()
        {
            Save.OnSaveButtonClick?.Invoke();
        }

        public void OnSaveAsButtonClick()
        {
            Save.OnSaveAsButtonClick?.Invoke(saveName.text);
        }

        public void OnResumeButtonClick()
        {
            string loadName = Path.Combine(Application.persistentDataPath, $@"Data\\Autosafe");
            Load.OnLoadButtonClick?.Invoke(loadName);
        }
    
        public void OnLoadButtonClick(string name)
        {
            string loadName = Path.Combine(Application.persistentDataPath, $@"Data\\{name}");
            Load.OnLoadButtonClick?.Invoke(loadName);
        }

        public string[] LoadSaveNames()
        {
            var directories = Directory.GetDirectories(Path.Combine(Application.persistentDataPath, $@"Data)"));
            List<string> saveNames = new();
            foreach (string directory in directories)
            {
                saveNames.Add(Path.GetDirectoryName(directory));
            }

            return saveNames.ToArray();
        
            //TODO: (Robin) add Buttons and add it to loadButtonList
        }
    }
}
