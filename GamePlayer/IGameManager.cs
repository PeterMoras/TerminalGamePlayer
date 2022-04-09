
namespace GamePlayer
{
    public interface IGameManager
    {
        /// <summary>
        /// Starts the game process and everything about it.
        /// </summary>
        /// <returns>Only returns when the game finishes. Make sure to surround this in a try catch if the game exits unexpectedly</returns>
        Task StartWork();

        /// <summary>
        /// A function that immediately triggers the game to end and exit, causing StartWork to complete.
        /// </summary>
        void ExitGame();
    }
}