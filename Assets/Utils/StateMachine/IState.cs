namespace Utils.State
{
    public interface IState
    {
        void Enter();
        void Exit();
        void Update();
        bool IsStateMachine();
        string GetName();
    }
}