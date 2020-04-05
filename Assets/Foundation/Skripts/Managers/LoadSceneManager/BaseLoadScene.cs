using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Scaramouche.Game {
    public class BaseLoadScene {

        protected Dictionary<string, SceneConfig> scenes;
        protected bool isScenes => scenes.Count > 0;

        protected string GetActiveScene() {
            return SceneManager.GetActiveScene().name;
        }
    }
}
