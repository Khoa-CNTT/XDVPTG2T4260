namespace tuleeeeee.StateMachine
{
    public class StateManager
    {
        #region PLAYERSTATE

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
        #endregion

        #region ENEMYSTATE
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
        #endregion

        #region CHESTSTATE
        public ChestState CurrentChestState { get; private set; }

        public void Initialize(ChestState stratingState)
        {
            CurrentChestState = stratingState;
            CurrentChestState.Enter();
        }
        public void ChangeState(ChestState newState)
        {
            CurrentChestState.Exit();
            CurrentChestState = newState;
            CurrentChestState.Enter();
        } 
        #endregion
    }
}
