using System.Collections;

namespace RoundState.States
{
    public class Round : State
    {

        public Round(GameplayManager gameplayManager) : base(gameplayManager)
        {
        }
        
        public override IEnumerator Start()
        {
            yield break;
        }
    }
}