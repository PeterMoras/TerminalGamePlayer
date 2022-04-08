using System.Text;

namespace GamePlayer
{
    public interface IInputListener
    {

        event Action<string>? OnSubmit;
        event Action<ConsoleKeyInfo>? OnInput;

        Task<ConsoleKeyInfo> GetNextInput();
        void StartListening();
        bool CanType { get; set; }
    }
    public class InputListener : IInputListener
    {
        bool _showTyped = false;
        StringBuilder _textLine = new StringBuilder();

        public event Action<string>? OnSubmit;
        public event Action<ConsoleKeyInfo>? OnInput;
        public bool CanType { get => _showTyped; set => _showTyped = value; }

        public void StartListening()
        {
            var task = Task.Run(GetInputs);

        }
        public async Task<ConsoleKeyInfo> GetNextInput()
        {
            TaskCompletionSource<ConsoleKeyInfo> complete = new();
            OnInput += (key) => { complete.TrySetResult(key); };
            return await complete.Task;
        }

        void GetInputs()
        {
            try
            {
                while (true)
                {
                    var key = Console.ReadKey(!CanType);
                    OnInput?.Invoke(key);

                    if (!CanType) continue;

                    if (key.Key == ConsoleKey.Enter)
                    {
                        OnSubmit?.Invoke(_textLine.ToString());
                        clearLine();
                        continue;
                    }
                    else if (key.Key == ConsoleKey.Backspace)
                    {
                        removeCharacter();

                    }
                    else
                    {
                        _textLine.Append(key.KeyChar);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
            }



        }
        void removeCharacter()
        {
            if (_textLine.Length == 0) return;
            _textLine.Remove(_textLine.Length - 1, 1);
            Console.Write(' ');
            Console.SetCursorPosition(_textLine.Length, Console.CursorTop);
        }
        void clearLine()
        {
            _textLine.Clear();
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }


    }
}