using System;
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
        [SerializeField] private Button saveAsButton;
        [SerializeField] private GameObject loadPanel;
        [SerializeField] private Transform buttonList;
        [SerializeField] private GameObject loadButtonPrefab;
        // private List<Button> loadButton = new();

        //TODO: (Robin) save als event oder nacheinander aufrufen wegen Order of execution, selbe Frage bei load

        private void Start()
        {
            OnSaveValueChanged();
            loadPanel.SetActive(false);
        }

        public void OnSaveValueChanged()
        {
            saveAsButton.interactable = !string.IsNullOrWhiteSpace(saveName.text);
        }
        
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
            //change scene, than wait 1 frame, than load
            string loadName = Path.Combine(Application.persistentDataPath, $@"Data\\Autosafe");
            Load.OnLoadButtonClick?.Invoke(loadName);
        }

        public void OnLoadAsButtonClick(TextMeshProUGUI buttonName)
        {
            //change scene, than wait 1 frame, than load
            Debug.Log("Load");
            string loadName = Path.Combine(Application.persistentDataPath, $@"Data\\{buttonName.text}");
            Load.OnLoadButtonClick?.Invoke(loadName);
        }

        public void OnLoadButtonClick()
        {
            string[] directories = Directory.GetDirectories(Path.Combine(Application.persistentDataPath, @"Data"));
            foreach (string directory in directories)
            {
                GameObject loadButton = Instantiate(loadButtonPrefab, buttonList, true);
                TextMeshProUGUI text = loadButton.GetComponentInChildren<TextMeshProUGUI>();
                string fileName = Path.GetFileName(directory);
                text.text = fileName;
                Button button = loadButton.GetComponent<Button>();
                button.onClick.AddListener(delegate { OnLoadAsButtonClick(text); });
            }
            loadPanel.SetActive(true);
        }
    }
}
