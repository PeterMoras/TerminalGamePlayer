using GamePlayer.Games.Connect4;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GamePlayer.Tests;

public class UnitTest1
{
    [Fact]
    public void AssertSameC4Manager()
    {
        using var provider = new ServiceCollection()
            .AddSingleton<IC4Model, C4Model>()
            .AddSingleton<IGameManager, C4Manager>()
            .AddSingleton<IInputListener, InputListener>()
            .AddSingleton<IC4View, C4View>()
            .AddSingleton<ITurnManager, C4TurnManager>()
            .BuildServiceProvider();
        Assert.Equal(1, 1);
        var gm1 = provider.GetService<C4Manager>();
        var gm2 = provider.GetService<IGameManager>();
        Assert.Null(gm1);
        Assert.NotNull(gm2);
    }
}