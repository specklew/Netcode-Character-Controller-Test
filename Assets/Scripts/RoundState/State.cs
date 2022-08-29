using System.Collections;

namespace RoundState
{
    public abstract class State
    {
        protected readonly GameplayManager GameplayManager;

        protected State(GameplayManager gameplayManager)
        {
            GameplayManager = gameplayManager;
        }
        
        public virtual IEnumerator Start()
        {
            yield break;
        }
        
    }
}