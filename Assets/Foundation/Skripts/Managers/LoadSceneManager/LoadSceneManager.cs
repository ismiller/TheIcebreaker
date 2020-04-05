using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using System.IO;

namespace Scaramouche.Game {
    [CreateAssetMenu(fileName = "SceneLoadManagers", menuName = "Managers/SceneManagers")]
    public class LoadSceneManager : ManagerBase, IAwake {

        [SerializeField] private mode operatingMode = mode.Test;
        private enum mode { Test, Bild }

        private Dictionary<string, SceneConfig> requiredScenes;
        private static LoadSceneManager sceneMng;
        
        private Scene activeScene;
        private IStateLoad currentStateLoad;

        public void OnAwake() {
            SceneManager.sceneLoaded += OnLoadScene;
            sceneMng = Toolbox.GetManager<LoadSceneManager>();
            LoadScenConfig();
            StateInitialize();           
        }

        public static void OnLoadScene(Scene _scene, LoadSceneMode _mode) {
            if(sceneMng) {
                sceneMng.SetActiveScene(_scene);
            }
        }

        private void LoadScenConfig() {
            requiredScenes = new Dictionary<string, SceneConfig>();
            var config = File.ReadAllText(Application.dataPath + "/Resources/scene_config.json");
            foreach(var scene in JsonConvert.DeserializeObject<SceneConfig[]>(config)) {
                requiredScenes.Add(scene.sceneName, scene);
            }
        }

        private void StateInitialize() {
            SceneConfig[] scenes = new SceneConfig[requiredScenes.Keys.Count];
            requiredScenes.Values.CopyTo(scenes, 0); 
            if(operatingMode == mode.Test) {
                currentStateLoad = TestLoad.Initialize(scenes).Enter();
            } else {
                currentStateLoad = BildLoad.Initialize(scenes).Enter();
            }
        }

        public static void LoadScene(string _nameScene) {
            if(sceneMng) {
                sceneMng.currentStateLoad.LoadScene(_nameScene);
            }
        }

        private void SetActiveScene(Scene _scene) {
            if(requiredScenes.TryGetValue(_scene.name, out SceneConfig scene)) {
                if(scene.isActive) {
                    SceneManager.SetActiveScene(_scene);
                } else return;
            } else {
                SceneManager.SetActiveScene(_scene);
            }
        }

        public static bool CheckSceneIsLoad(string _nameScene) {
            for(int i = 0; i < SceneManager.sceneCount; i++) {
                Scene currentScene = SceneManager.GetSceneAt(i);
                if(_nameScene.Equals(currentScene.name)) {
                    return true;
                }
            } 
            return false;
        }
    }
}