using System;
using System.Collections;
using System.Collections.Generic;
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

        //TODO: (Robin) load als event oder nacheinander aufrufen wegen Order of execution

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
            Debug.Log("Load Start");
            
            // Start loading the scene
            AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
            
            Debug.Log(asyncLoadLevel.progress);

            // Wait until the level finish loading
            while (asyncLoadLevel.isDone)
            {
                yield return null;
            }
            
            Debug.Log("Load While End");

            // Wait a frame so every Awake and Start method is called
            yield return new WaitForEndOfFrame();

            Debug.Log("Load Before");

            Load.OnLoadButtonClick?.Invoke(loadName);
            
            Debug.Log("Load End");

        }
    }
}
