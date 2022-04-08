namespace GamePlayer
{

    public interface IPlayer
    {
        //public new Task<T> GetActionData();
        Task TakeTurn(CancellationToken token);
    }
}