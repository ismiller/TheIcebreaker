namespace Scaramouche.Game {
    public interface IAcceptVisitor {
        void AcceptRayCast(IVisitorEnvironment _visitor);
        void AcceptOnTrigger(IVisitorEnvironment _visitor);
    }
}
