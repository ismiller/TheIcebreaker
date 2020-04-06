namespace Scaramouche.Game {
    public interface IVisitorEnvironment {
        void Visit(ObstacleActor _actor);
        void Visit(SlopeSurfaceActor _actor);
        void Visit(DefoltSurfaceActor _actor);
        void Visit(SlipperySurfaceActor _actor);
    }
}
