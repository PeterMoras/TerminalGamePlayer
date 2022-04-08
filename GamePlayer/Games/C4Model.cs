namespace GamePlayer.Games.Connect4
{
    public interface IC4Model
    {
        C4Piece[,] Data { get; }

        public long Rows { get; }
        public long Cols { get; }

        event Action<C4ModelEventArgs>? OnDataChange;

        bool TryAddPiece(int col, C4Piece piece);
        void Clear();
    }
    public class C4Model : IC4Model
    {

        public static C4Piece[,] DefaultBoard => FromEmpty(6, 7);
        private C4Piece[,] _data = DefaultBoard;

        public C4Piece[,] Data => _data;

        public long Rows => Data.GetLongLength(1);
        public long Cols => Data.GetLongLength(0);

        public event Action<C4ModelEventArgs>? OnDataChange;

        public void Clear()
        {
            _data = DefaultBoard;
            OnDataChange?.Invoke(new C4ModelEventArgs(_data));
        }

        public bool TryAddPiece(int col, C4Piece piece)
        {
            for (int r = 0; r < Rows; r++)
            {
                if (Data[col, r] == C4Piece.None)
                {
                    Data[col, r] = piece;
                    OnDataChange?.Invoke(new C4ModelEventArgs(_data));
                    return true;
                }
            }
            return false;
        }


        public void SetDataAtIndex(int col, int row, C4Piece value)
        {
            var oldValue = _data[col, row];

            if (oldValue != value)
            {
                _data[col, row] = value;
                OnDataChange?.Invoke(new C4ModelEventArgs(_data));
            }

        }
        public static C4Piece[,] FromEmpty(int cols, int rows)
        {
            C4Piece[,] data = new C4Piece[cols, rows];
            var length = data.Length;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    data[c, r] = C4Piece.None;
                }
            }
            return data;
        }

    }
    public record C4ModelEventArgs(C4Piece[,] Data);
}