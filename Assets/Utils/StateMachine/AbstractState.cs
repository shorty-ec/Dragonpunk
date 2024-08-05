namespace Utils.State
{
    public abstract class AbstractState: IState
    {
        public virtual void Enter()
        {
        }

        public virtual void Exit()
        {
        }

        public virtual void Update()
        {
        }

        public virtual bool IsStateMachine() => false;
        public string GetName() => GetType().Name;
    }
}