namespace tuleeeeee.StateMachine
{
    public class StateManager
    {
        public PlayerState CurrentPlayerState { get; private set; }

        public void Initialize(PlayerState stratingState)
        {
            CurrentPlayerState = stratingState;
            CurrentPlayerState.Enter();
        }
        public void ChangeState(PlayerState newState)
        {
            CurrentPlayerState.Exit();
            CurrentPlayerState = newState;
            CurrentPlayerState.Enter();
        }

        public EnemyState CurrentEnemyState { get; private set; }

        public void Initialize(EnemyState stratingState)
        {
            CurrentEnemyState = stratingState;
            CurrentEnemyState.Enter();
        }
        public void ChangeState(EnemyState newState)
        {
            CurrentEnemyState.Exit();
            CurrentEnemyState = newState;
            CurrentEnemyState.Enter();
        }
    }
}
