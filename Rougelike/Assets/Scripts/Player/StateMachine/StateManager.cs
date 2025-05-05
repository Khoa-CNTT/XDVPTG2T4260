namespace tuleeeeee.StateMachine
{
    public class StateManager
    {
        public PlayerState CurrentPlayerState { get; private set; }

        public void InitializePlayer(PlayerState stratingState)
        {
            CurrentPlayerState = stratingState;
            CurrentPlayerState.Enter();
        }
        public void ChangePlayerState(PlayerState newState)
        {
            CurrentPlayerState.Exit();
            CurrentPlayerState = newState;
            CurrentPlayerState.Enter();
        }

        public EnemyState CurrentEnemyState { get; private set; }

        public void InitializeEnemy(EnemyState stratingState)
        {
            CurrentEnemyState = stratingState;
            CurrentEnemyState.Enter();
        }
        public void ChangeEnemyState(EnemyState newState)
        {
            CurrentEnemyState.Exit();
            CurrentEnemyState = newState;
            CurrentEnemyState.Enter();
        }
    }
}
