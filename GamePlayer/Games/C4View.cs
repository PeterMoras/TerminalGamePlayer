namespace GamePlayer.Games.Connect4
{
    public interface IC4View
    {
        public void StartShowing();
        void StopShowing();

        void SetAfterMessage(string message);
    }





    public class C4View : IC4View, IDisposable
    {
        private IC4Model _model;
        bool _visible;
        private string _afterMessage;

        public C4View(IC4Model model)
        {
            _model = model;
            model.OnDataChange += OnDataChange;
        }

        private void OnDataChange(C4ModelEventArgs e)
        {
            if (_visible)
                showView(e.Data);
        }

        public void StartShowing()
        {
            _visible = true;
            showView(_model.Data);
        }
        public void StopShowing()
        {
            _visible = false;
        }

        // 7 columns, 6 rows
        void showView(C4PieceType[,] pieces)
        {
            Console.Clear();
            drawLines(pieces);
            if (!String.IsNullOrEmpty(_afterMessage))
                Console.WriteLine(_afterMessage);
        }
        void drawLines(C4PieceType[,] pieces)
        {
            var p = pieces;
            var cols = pieces.GetLength(0);
            var rows = pieces.GetLength(1);

            for (int r = rows - 1; r >= 0; r--)
            {
                //Console.WriteLine($"7 ${p[0, r]}|${p[1, r]}|${p[2, r]}|${p[3, r]}|${p[4, r]}|${p[5, r]}");
                Console.Write($"{r + 1} ");
                for (int c = 0; c < cols; c++)
                {
                    if (c != 0)
                        Console.Write("|");
                    Console.Write($"{p[c, r]}");
                }
                Console.WriteLine();
            }
            Console.Write("  ");
            //write numbers underneath to know which column each is
            for (int c = 0; c < cols; c++)
            {
                if (c != 0)
                    Console.Write(" ");
                Console.Write($"{c}");
            }
            Console.WriteLine();


        }

        public void SetAfterMessage(string message)
        {
            _afterMessage = message;
            StartShowing();
        }

        public void Dispose()
        {
            _visible = false;
        }
    }
}