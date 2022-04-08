namespace GamePlayer
{

    public interface IGameSelectManager
    {
        Task StartManager();
        void GotoGameSelect();

        IGameManager? ActiveGame { get; }
    }

    public class GameSelectManager : IGameSelectManager
    {
        TaskCompletionSource keepManagerAliveTask;

        private readonly IGameFactory _gameFactory;
        private readonly IInputListener _input;
        private IGameManager? _activeGame;

        private bool textShowState;

        public IGameManager? ActiveGame => _activeGame;

        public GameSelectManager(IInputListener input, IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _input = input;
            keepManagerAliveTask = new TaskCompletionSource();
        }
        public async Task StartManager()
        {
            keepManagerAliveTask = new TaskCompletionSource();

            GotoGameSelect();
            await keepManagerAliveTask.Task;

            Dispose();
        }

        private void Dispose()
        {
            _input.OnInput -= ManagerListener;
            Console.WriteLine("Exiting Game Manager");
        }

        public void GotoGameSelect()
        {
            Console.Clear();
            Console.WriteLine("Choose a game to play:");
            var games = _gameFactory.GetGameList();
            int index = 0;
            foreach (var game in games)
            {
                Console.WriteLine($"{index}: {game.Name}");
                index++;
            }

            //Manager now listens for input 
            textShowState = _input.CanType;
            _input.CanType = false;
            _input.OnInput += ManagerListener;

        }

        void GameComplete()
        {
            _activeGame = null;
            GotoGameSelect();
        }

        async Task RunGameTask(GameList gameToStart)
        {
            var gameManager = _gameFactory.CreateGame(gameToStart);
            if (gameManager == null)
            {
                Console.WriteLine("Unable to find the game: " + gameToStart.Name);
            }
            else
            {
                _activeGame = gameManager;
                try
                {
                    await gameManager.StartGame();
                }
                catch (Exception e)
                {
                    Console.WriteLine();
                    Console.WriteLine("The Game had an Exception and exited abruptly");
                    Console.Error.WriteLine(e);
                    Console.ReadLine();

                }

            }


            GameComplete();
        }

        private void ManagerListener(ConsoleKeyInfo key)
        {
            int selection = -1;
            if (key.Key == ConsoleKey.Escape)
            {
                //escape key pressed at game select. Exit!
                keepManagerAliveTask.TrySetResult(); //this ends the task that keeps the manager alive. When the manager dies the program completes
                return;
            }
            //is key press not an int? then exit
            else if (!int.TryParse("" + key.KeyChar, out selection)) return;

            //set to list because IEnumerable would be slower due to multiple gets
            var games = _gameFactory.GetGameList().ToList();

            if (selection >= games.Count)
                return; //out of range, just keep listening...


            GameList selectedGame = games[selection];
            //dont listen to inputs here anymore
            _input.OnInput -= ManagerListener;
            _input.CanType = textShowState;

            var task = Task.Run(() => RunGameTask(selectedGame));
        }
    }
}