namespace Scaramouche.Game {
    public interface IVisitor {
        
        void Visit(NullableObstacleActor _actor);
        void Visit(ObstacleActor _actor);
        void Visit(SlopeSurfaceActor _actor);
        void Visit(DefoltSurfaceActor _actor);
        void Visit(MovePlatformActor _actor);
    }
}
