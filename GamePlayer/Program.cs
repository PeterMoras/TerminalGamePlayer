using GamePlayer;
using GamePlayer.Games.Connect4;
using Microsoft.Extensions.DependencyInjection;
// See https://aka.ms/new-console-template for more information
Console.Clear();
Console.WriteLine("Hello, World!");


await using var provider = new ServiceCollection()
            .AddSingleton<IInputListener, InputListener>()
            .AddScoped<IGameFactory, GameFactory>()
            .AddSingleton<IGameSelectManager, GameSelectManager>()
            .AddScoped<ITurnManager, TurnManager>()

            //  Connect4 Game Services 
            .AddScoped<C4Manager>()
            .AddScoped<IGameManager, C4Manager>()
            .AddScoped<IC4View, C4View>()
            .AddScoped<IC4Model, C4Model>()
            .AddScoped<IPlayerFactory, C4PlayerFactory>()
            .AddScoped<C4ConsolePlayer>()
            .AddScoped<C4EasyAIPlayer>()


            .BuildServiceProvider();

//input listener is started in the background to listen for events that can be consumed by other servides.
//start the game select manager. When the manager stops the program completes. (The manager stops if Esc is pressed while in game select menu)
var gamePlayingTask = provider.GetRequiredService<IGameSelectManager>().StartManager();
provider.GetRequiredService<IInputListener>().StartListening();

await gamePlayingTask;
Console.WriteLine("Completed program");