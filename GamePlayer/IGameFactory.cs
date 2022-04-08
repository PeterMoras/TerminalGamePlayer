using GamePlayer.Games.Connect4;
using Microsoft.Extensions.DependencyInjection;

namespace GamePlayer
{
    public interface IGameFactory
    {
        public IEnumerable<GameList> GetGameList();
        IGameManager? CreateGame(GameList game);
    }
    public class GameFactory : IGameFactory
    {
        private IServiceProvider _services;

        public GameFactory(IServiceProvider services)
        {
            _services = services;
        }

        public IEnumerable<GameList> GetGameList()
        {
            return GameList.GetAll<GameList>();
        }

        public IGameManager? CreateGame(GameList game)
        {
            if (game == GameList.Connect4)
            {
                return _services.GetService<C4Manager>();
            }

            return null;
        }
    }
}