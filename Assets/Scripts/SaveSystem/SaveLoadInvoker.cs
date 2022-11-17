using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        public GameSave GameSave { get; private set; } = new();

        //TODO: (Robin) load als event oder nacheinander aufrufen wegen Order of execution

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


        private void SaveGameData(SaveLoadInvoker invoker)
        {
            StartCoroutine(SaveGameDataAfterFrame());
        }

        private IEnumerator SaveGameDataAfterFrame()
        {
            yield return new WaitForEndOfFrame();
            Save.AutoSaveData(GameSave, "GameData");
        }

        private void SaveGameDataAs(string savePlace, SaveLoadInvoker invoker)
        {
            StartCoroutine(SaveGameDataAsAfterFrame(savePlace));
        }

        private IEnumerator SaveGameDataAsAfterFrame(string savePlace)
        {
            yield return new WaitForEndOfFrame();
            Save.SaveDataAs(savePlace, GameSave, "GameData");
        }

        public void OnSaveValueChanged()
        {
            saveAsButton.interactable = !string.IsNullOrWhiteSpace(saveName.text);
        }
        
        public void OnSaveButtonClick()
        {
            Save.OnSaveButtonClick?.Invoke(this);
        }

        public void OnSaveAsButtonClick()
        {
            Save.OnSaveAsButtonClick?.Invoke(saveName.text, this);
        }

        public void OnResumeButtonClick()
        {
            //change scene, than wait 1 frame, than load
            string loadName = Path.Combine(Application.persistentDataPath, $@"Data\\Autosafe");
            // Load.OnLoadButtonClick?.Invoke(loadName);
            StartCoroutine(LoadScene(loadName));
        }

        public void OnLoadAsButtonClick(TextMeshProUGUI buttonName)
        {
            //change scene, than wait 1 frame, than load
            Debug.Log("Load");
            string loadName = Path.Combine(Application.persistentDataPath, $@"Data\\{buttonName.text}");
            // Load.OnLoadButtonClick?.Invoke(loadName);
            StartCoroutine(LoadScene(loadName));
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
        
        /// <summary>
        /// https://stackoverflow.com/questions/52722160/in-unity-after-loadscene-is-there-common-way-to-wait-all-monobehaviourstart-t
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadScene(string loadName)
        {
            loadName = Path.Combine(loadName, $@"GameData");

            // Start loading the scene
            AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);

            // Wait until the level finish loading
            while (!asyncLoadLevel.isDone)
            {
                Debug.Log("is not done");
                yield return null;
            }

            // Wait a frame so every Awake and Start method is called
            yield return new WaitForEndOfFrame();

            GameSave = Load.LoadData(loadName);
            
            Load.OnLoadButtonClick?.Invoke(GameSave);
        }
    }
}
