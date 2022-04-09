


namespace GamePlayer.Games.Connect4
{
    public class C4Manager : IGameManager, IDisposable
    {
        readonly TaskCompletionSource complete;
        private readonly IC4Model _model;
        private readonly ITurnManager _turnManager;
        private readonly IInputListener _input;
        private readonly IC4View _view;
        private readonly IPlayerFactory _playerFactory;
        private bool _canType;



        public C4Manager(IInputListener input, IC4View view, ITurnManager turns, IPlayerFactory playerFactory, IC4Model model)
        {
            _model = model;
            _turnManager = turns;
            _input = input;
            _view = view;
            _playerFactory = playerFactory;
            complete = new TaskCompletionSource();
            Console.WriteLine("New Connect Game Manager Loaded");
        }


        public async Task StartWork()
        {
            Console.Clear();
            Console.WriteLine("Starting Connect4!");

            _canType = _input.CanType;
            _input.CanType = false;
            _input.OnInput += GetInput;
            _model.OnWinningPiecePlaced += WinningPiecePlaced;

            var task = SetUpInitialGameState();
            await complete.Task;
        }

        private async void WinningPiecePlaced(C4WinArgs e)
        {
            var winningPlayer = e.WinningPlayer;
            if (winningPlayer is C4ConsolePlayer)
                _view.SetAfterMessage("You Won!");
            if (winningPlayer is C4EasyAIPlayer)
                _view.SetAfterMessage("You lost to an EASY AI? Wow.");

            await _input.GetNextInput();
            //game is complete, exit back to main menu by completing work
            complete.TrySetResult();

        }

        async Task SetUpInitialGameState()
        {
            try
            {
                _model.Clear();
                var playerList = new List<IPlayer>();
                playerList.Add(_playerFactory.CreatePlayer(PlayerType.Human, C4PieceType.O));
                playerList.Add(_playerFactory.CreatePlayer(PlayerType.EasyAI, C4PieceType.X));
                _view.StartShowing();
                _turnManager.SetPlayers(playerList);
                await _turnManager.StartTakingTurns();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
            }

        }

        private void GetInput(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Escape)
            {
                //escape key pressed at game select. Exit!
                Dispose();
                //this ends the task that keeps the manager alive. When the manager dies the program completes
            }

        }

        public void Dispose()
        {
            complete.TrySetResult();
            _input.OnInput -= GetInput;
            _input.CanType = _canType;
            _view.StopShowing();
            //Console.WriteLine("Exiting Connect4");
            //Task.Delay(500).Wait();
            //GC.SuppressFinalize(this);
        }

        public void ExitGame()
        {
            complete.TrySetResult();
        }
    }
}