namespace Scaramouche.Game {
    public class SceneConfig {

        public string sceneName;
        public SceneType sceneType;
        public bool loadTest;
        public bool isActive;

        public enum SceneType {
            independent,    //самостоятельные сцены
            intermediate,   //промежуточные
            auxiliary,      //дополняющие
        }

        public bool isIndependent => (sceneType == SceneType.independent);
        public bool isIntermediate => (sceneType == SceneType.intermediate);
        public bool isAuxiliary => (sceneType == SceneType.auxiliary);
    }
}
