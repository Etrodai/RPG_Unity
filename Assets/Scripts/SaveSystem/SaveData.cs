using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaveSystem
{
    public class SaveData : MonoBehaviour
    {
        public static SaveData Instance { get; private set; }
        public GameSave GameSave { get; private set; } = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
            
            DontDestroyOnLoad(gameObject);
        }

        public void LoadData(string loadName)
        {
            StartCoroutine(LoadScene(loadName));
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

            Debug.Log("Wait for end of frame");
            
            // Wait a frame so every Awake and Start method is called
            yield return new WaitForEndOfFrame();
            
            Debug.Log("Waited for end of frame");

            GameSave = Load.LoadData(loadName);
            
            Load.OnLoadButtonClick?.Invoke(GameSave);
        }
    }
}
