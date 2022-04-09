using GamePlayer;

namespace GamePlayer.Games.Connect4
{
    public interface IC4Player : IPlayer
    {
        public C4PieceType Type { get; }
    }

    public class C4ConsolePlayer : IC4Player
    {
        private IInputListener _input;
        private IC4Model _model;
        public C4PieceType Type { get; }

        public C4ConsolePlayer(IInputListener input, IC4Model model, C4PieceType type)
        {
            _input = input;
            _model = model;
            Type = type;
            // _input
        }


        public async Task TakeTurn()
        {
            int col = -1;
            while (true)
            {
                var input = await _input.GetNextInput();
                //if input is an int, returns that int
                if (!int.TryParse("" + input.KeyChar, out col)) continue;
                if (col < 0 || col >= _model.Cols) continue;
                bool added = _model.TryAddPiece(col, Type, this);
                if (added) return;
            }
        }
    }
    public class C4EasyAIPlayer : IC4Player
    {
        readonly IC4Model _model;
        public C4PieceType Type { get; }

        public C4EasyAIPlayer(IC4Model model, C4PieceType type)
        {
            Type = type;
            _model = model;
        }


        public Task TakeTurn()
        {
            while (true)
            {
                var rand = new Random();
                var selection = (int)rand.NextInt64(0, _model.Cols);
                bool success = _model.TryAddPiece(selection, Type, this);
                if (success) return Task.Run(() => { });
            }

        }
    }
}