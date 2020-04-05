namespace Scaramouche.Game {
    public interface IStateLoad {

        IStateLoad Enter();
        void LoadScene(string _nameScene);

    }
}
