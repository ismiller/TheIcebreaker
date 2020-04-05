using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Scaramouche.Game {
    public class TestLoad : BaseLoadScene, IStateLoad {

        private AsyncOperation operation;
        private bool isOperation => operation != null;

        public static IStateLoad Initialize(SceneConfig[] _scenes) {
            return new TestLoad(_scenes);
        }

        public TestLoad(SceneConfig[] _scenes) {
            base.scenes = new Dictionary<string, SceneConfig>();
            foreach(var scene in _scenes) {
                if(scene.loadTest) {
                    base.scenes.Add(scene.sceneName, scene);
                }
            }
        }

        public IStateLoad Enter() {
            if(base.isScenes) {
                foreach(var scene in scenes.Values) {
                    if(!LoadSceneManager.CheckSceneIsLoad(scene.sceneName)) {
                        TaskManager.AddTask(Load(scene.sceneName));
                    }
                }
            }
            return this;
        }

        public void LoadScene(string _nameScene) {
            Debug.LogWarning("<color=red>Выбран тестовый режим!</color>");
            Debug.LogWarning("<color=black>Загрузка другой сцены в этом режиме не доступна!</color>");
        }

        protected IEnumerator Load(string _nameScene) {
            operation = SceneManager.LoadSceneAsync(_nameScene, LoadSceneMode.Additive);
            while(!operation.isDone) {
                yield return null;
            }
        }
    }
}
