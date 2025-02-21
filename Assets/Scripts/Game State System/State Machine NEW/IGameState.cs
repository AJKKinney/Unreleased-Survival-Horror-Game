namespace AustenKinney.GameState
{
    public interface IGameState
    {
        void EnterState(GameStateManager gameStateManager);
        void UpdateState(GameStateManager gameStateManager);
        void ExitState(GameStateManager gameStateManager);
    }
}
