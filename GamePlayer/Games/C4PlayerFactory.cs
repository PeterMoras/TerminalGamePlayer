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

        public IPlayer CreatePlayer(PlayerType type, C4PieceType pieceType)
        {
            var inputService = _provider.GetRequiredService<IInputListener>();
            var model = _provider.GetRequiredService<IC4Model>();
            IC4Player player;

            switch (type)
            {
                case PlayerType.Human:
                    return new C4ConsolePlayer(inputService, model, pieceType);
                default:
                    return new C4EasyAIPlayer(model, pieceType);

            }


        }
    }
}