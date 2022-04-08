
namespace GamePlayer
{
    public interface ITurnManager
    {
        Task StartTakingTurns(CancellationToken token);
        void SetPlayers(List<IPlayer> playerList);

        event Action<IPlayer>? OnTurnComplete;
        event Action? OnRoundComplete;
    }

    public class TurnManager : ITurnManager
    {
        List<IPlayer> players = new();

        public event Action<IPlayer>? OnTurnComplete;
        public event Action? OnRoundComplete;

        public void SetPlayers(List<IPlayer> playerList)
        {
            players = playerList;
        }

        public async Task StartTakingTurns(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                foreach (var player in players)
                {
                    //player.GetActionData();
                    await player.TakeTurn(token);
                    OnTurnComplete?.Invoke(player);
                }
                OnRoundComplete?.Invoke();
            }

        }
    }
}