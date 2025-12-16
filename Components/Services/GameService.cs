//using tictactoe_blazor.Components.GameLogic;

//namespace tictactoe_blazor.Components.Services
//{
//    public class GameService
//    {
//        public TicTacToe? CurrentGame { get; private set; }

//        public void StartGame(int n, int m)
//        {
//            CurrentGame = new TicTacToe(n, m);
//        }

//        public void MakeMove(int index)
//        {
//            if (CurrentGame != null && !CurrentGame.GameEnded)
//            {
//                CurrentGame.MakeMove(index);
//            }
//        }

//        public void EndGame()
//        {
//            CurrentGame?.EndGameEarly();
//        }

//        public void Restart()
//        {
//            CurrentGame = null;
//        }
//    }
//}

// вариант с хабом
using Microsoft.AspNetCore.SignalR;
using tictactoe_blazor.Components.GameLogic;
using tictactoe_blazor.Components.Hubs; 

namespace tictactoe_blazor.Components.Services
{
    public class GameService
    {
        private readonly IHubContext<GameHub> _hubContext;
        public TicTacToe? CurrentGame { get; private set; }

        public GameService(IHubContext<GameHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public void StartGame(int n, int m)
        {
            CurrentGame = new TicTacToe(n, m);
            NotifyClients();
        }

        public void MakeMove(int index)
        {
            if (CurrentGame != null && CurrentGame.MakeMove(index))
            {
                NotifyClients();
            }
        }

        public void EndGame()
        {
            if (CurrentGame != null)
            {
                CurrentGame.EndGameEarly();
                NotifyClients();
            }
        }

        public void Restart()
        {
            CurrentGame = null;
            NotifyClients();
        }

        private void NotifyClients()
        {
            // fire-and-forget — отправляем всем клиентам сообщение об обновлении
            _ = _hubContext.Clients.All.SendAsync("BoardUpdated");
        }
    }
}
