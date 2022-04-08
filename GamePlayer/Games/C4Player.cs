using GamePlayer;

namespace GamePlayer.Games.Connect4
{
    public class C4ConsolePlayer : IPlayer
    {
        private IInputListener _input;
        private IC4Model _model;

        public C4ConsolePlayer(IInputListener input, IC4Model model)
        {
            _input = input;
            _model = model;
            // _input
        }

        public async Task TakeTurn(CancellationToken token)
        {
            int col = -1;
            while (!token.IsCancellationRequested)
            {
                var input = await _input.GetNextInput();
                //if input is an int, returns that int
                if (!int.TryParse("" + input.KeyChar, out col)) continue;
                if (col < 0 || col >= _model.Cols) continue;
                bool added = _model.TryAddPiece(col, C4Piece.O);
                if (added) return;
            }
        }
    }
    public class C4EasyAIPlayer : IPlayer
    {
        readonly IC4Model _model;
        public C4EasyAIPlayer(IC4Model model)
        {
            _model = model;
        }


        public Task TakeTurn(CancellationToken token)
        {
            while (true)
            {
                var rand = new Random();
                var selection = (int)rand.NextInt64(0, _model.Cols);
                bool success = _model.TryAddPiece(selection, C4Piece.X);
                if (success) return Task.Run(() => { });
            }

        }
    }
}