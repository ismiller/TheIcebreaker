namespace Scaramouche.Game {
    public interface IAcceptVisitor {

        void AcceptRayCast(IVisitor _visitor);
        void AcceptOnTrigger(IVisitor _visitor);

    }
}
