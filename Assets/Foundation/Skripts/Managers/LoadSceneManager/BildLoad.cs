using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Scaramouche.Game {
    public class BildLoad : BaseLoadScene, IStateLoad {

        private SceneConfig loadingScene;
        private SceneConfig uiScene;

        private AsyncOperation operation;
        private bool isOperation => operation != null;
        private bool isUIScene => LoadSceneManager.CheckSceneIsLoad(uiScene.sceneName);
        private delegate bool IsSceneLoad(string _name); 
        IsSceneLoad isSceneLoad = (string _name) => LoadSceneManager.CheckSceneIsLoad(_name);

        public static IStateLoad Initialize(SceneConfig[] _scenes) {
            return new BildLoad(_scenes);
        }

        public BildLoad(SceneConfig[] _scenes) {
            base.scenes = new Dictionary<string, SceneConfig>();
            foreach(var scene in _scenes) {
                base.scenes.Add(scene.sceneName, scene);
                if(scene.isIntermediate) {
                    loadingScene = scene;
                } else if(scene.isAuxiliary) {
                    uiScene = scene;
                }
            }
        }

        public IStateLoad Enter() {
            if(base.isScenes) {
                foreach(var scene in scenes.Values) {
                    if(scene.isIndependent && !isSceneLoad(scene.sceneName)) {
                        TaskManager.AddTask(Load(scene.sceneName));
                    }
                }
            }
            return this;
        }

        public void LoadScene(string _nameScene) {
            string nameActiveScene = base.GetActiveScene();
            if(!isSceneLoad(_nameScene)) {
                TaskManager.AddTask(Load(loadingScene.sceneName));
                TaskManager.AddTask(Unload(nameActiveScene));
                TaskManager.AddTask(Load(_nameScene));

                if(scenes.ContainsKey(_nameScene) && isUIScene) {
                    TaskManager.AddTask(Unload(uiScene.sceneName));
                } else if(!isUIScene) {
                    TaskManager.AddTask(Load(uiScene.sceneName));
                }

                TaskManager.AddTask(Unload(loadingScene.sceneName));
            }
        }

        protected IEnumerator Load(string _nameScene) {
            operation = SceneManager.LoadSceneAsync(_nameScene, LoadSceneMode.Additive);
            while(!operation.isDone) {
                yield return null;
            }
        }

        protected IEnumerator Unload(string _nameScene) {
            operation = SceneManager.UnloadSceneAsync(_nameScene);
            while(isOperation && !operation.isDone) {
                yield return null;
            }
        }
    }
}
