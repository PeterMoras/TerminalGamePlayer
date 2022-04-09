
namespace GamePlayer
{
    public interface ITurnManager
    {
        Task StartTakingTurns();
        void SetPlayers(List<IPlayer> playerList);

        event Action<IPlayer>? OnTurnComplete;
        event Action? OnRoundComplete;

        void EndTurnTaking();

        public IPlayer? ActivePlayer { get; }
    }

    public class TurnManager : ITurnManager, IDisposable
    {
        List<IPlayer> players = new();

        IPlayer? _activePlayer;

        public IPlayer? ActivePlayer => _activePlayer;

        public event Action<IPlayer>? OnTurnComplete;
        public event Action? OnRoundComplete;
        public bool _continueTurns = true;

        public void SetPlayers(List<IPlayer> playerList)
        {
            players = playerList;
            _activePlayer = playerList.FirstOrDefault();
        }

        public void EndTurnTaking()
        {
            _continueTurns = false;
        }

        public async Task StartTakingTurns()
        {
            while (true)
            {
                foreach (var player in players)
                {
                    if (!_continueTurns) return;
                    _activePlayer = player;
                    //player.GetActionData();
                    await player.TakeTurn();
                    OnTurnComplete?.Invoke(player);
                }
                OnRoundComplete?.Invoke();
            }

        }

        public void Dispose()
        {
            _continueTurns = false;
        }
    }
}