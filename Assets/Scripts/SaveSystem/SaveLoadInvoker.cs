using System.Collections;
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

        private SaveData saveData;
        //TODO: (Robin) load als event oder nacheinander aufrufen wegen Order of execution

        private void Awake()
        {
            saveData = SaveData.Instance;
        }

        private void Start()
        {
            OnSaveValueChanged();
            loadPanel.SetActive(false);
            Save.OnSaveButtonClick.AddListener(SaveGameData);
            Save.OnSaveAsButtonClick.AddListener(SaveGameDataAs);
        }
        
        private void OnDestroy()
        {
            Save.OnSaveButtonClick.RemoveListener(SaveGameData);
            Save.OnSaveAsButtonClick.RemoveListener(SaveGameDataAs);
        }


        private void SaveGameData()
        {
            StartCoroutine(SaveGameDataAfterFrame());
        }

        private IEnumerator SaveGameDataAfterFrame()
        {
            yield return new WaitForEndOfFrame();
            Save.AutoSaveData(saveData.GameSave, "GameData");
        }

        private void SaveGameDataAs(string savePlace)
        {
            StartCoroutine(SaveGameDataAsAfterFrame(savePlace));
        }

        private IEnumerator SaveGameDataAsAfterFrame(string savePlace)
        {
            yield return new WaitForEndOfFrame();
            Save.SaveDataAs(savePlace, saveData.GameSave, "GameData");
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
            // Load.OnLoadButtonClick?.Invoke(loadName);
            saveData.LoadData(loadName);
            // StartCoroutine(saveData.LoadScene(loadName));
        }

        public void OnLoadAsButtonClick(TextMeshProUGUI buttonName)
        {
            //change scene, than wait 1 frame, than load
            Debug.Log("Load");
            string loadName = Path.Combine(Application.persistentDataPath, $@"Data\\{buttonName.text}");
            // Load.OnLoadButtonClick?.Invoke(loadName);
            saveData.LoadData(loadName);
            // StartCoroutine(saveData.LoadScene(loadName));
        }

        public void OnLoadButtonClick()
        {
            string[] directories = Directory.GetDirectories(Path.Combine(Application.persistentDataPath, @"Data"));
            for (int i = 0; i < directories.Length; i++)
            {
                string directory = directories[i];
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
