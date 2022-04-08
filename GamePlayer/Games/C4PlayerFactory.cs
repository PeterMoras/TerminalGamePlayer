using Microsoft.Extensions.DependencyInjection;

namespace GamePlayer.Games.Connect4
{
    public class C4PlayerFactory : IPlayerFactory
    {
        private readonly IServiceProvider _provider;

        public C4PlayerFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IPlayer CreatePlayer(PlayerType type)
        {
            if (type == PlayerType.Human)
                return _provider.GetRequiredService<C4ConsolePlayer>();

            return _provider.GetRequiredService<C4EasyAIPlayer>();
        }
    }
}