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

        
    }
}
