namespace Infrastructure.GameStates.Interfaces
{
    public interface IGameState
    {
        public void Enter();
    }

    public interface IExitableState: IGameState
    {
        public void Exit();
    }
}
