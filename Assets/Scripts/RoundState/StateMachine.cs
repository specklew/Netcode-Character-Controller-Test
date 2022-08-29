using UnityEngine;

namespace RoundState
{
    public class StateMachine : MonoBehaviour
    {
        private State _state;

        protected State GetCurrentState()
        {
            return _state;
        }

        protected void SetState(State state)
        {
            _state = state;
            StartCoroutine(state.Start());
        }
    }
}