


namespace GamePlayer.Games.Connect4
{
    public class C4Manager : IGameManager
    {
        CancellationTokenSource tokenSource;
        private TaskCompletionSource keepGameAliveTask;
        private IC4Model _model;
        private ITurnManager _turnManager;
        private IInputListener _input;
        private IC4View _view;
        private IPlayerFactory _playerFactory;
        private bool _canType;



        public C4Manager(IInputListener input, IC4View view, ITurnManager turns, IPlayerFactory playerFactory, IC4Model model)
        {
            _model = model;
            _turnManager = turns;
            _input = input;
            _view = view;
            _playerFactory = playerFactory;
            keepGameAliveTask = new TaskCompletionSource();
        }


        public async Task StartGame()
        {
            Console.Clear();
            Console.WriteLine("Starting Connect4!");

            keepGameAliveTask = new TaskCompletionSource();
            _canType = _input.CanType;
            _input.CanType = false;
            _input.OnInput += GetInput;

            await setUpInitialGameState();
            await keepGameAliveTask.Task;
        }


        async Task setUpInitialGameState()
        {
            try
            {
                tokenSource = new CancellationTokenSource();
                _model.Clear();
                var playerList = new List<IPlayer>();
                playerList.Add(_playerFactory.CreatePlayer(PlayerType.Human));
                playerList.Add(_playerFactory.CreatePlayer(PlayerType.EasyAI));
                _view.StartShowing();
                _turnManager.SetPlayers(playerList);
                await _turnManager.StartTakingTurns(tokenSource.Token);
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
            keepGameAliveTask.TrySetResult();
            tokenSource.Cancel();
            _input.OnInput -= GetInput;
            _input.CanType = _canType;
            _view.StopShowing();
            Console.WriteLine("Exiting Connect4");
            Task.Delay(500).Wait();
        }
    }
}