using Infrastructure.GameStates.Interfaces;
using UnityEngine;


namespace Infrastructure.GameStates
{
    public class GameLoopState : IGameState
    {
        public GameLoopState() { }


        public void Enter()
        {
            InitGameLoopServices();
        }


        private void InitGameLoopServices() { }
    }
}



