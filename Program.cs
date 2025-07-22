using SadConsole.Configuration;
using Sigmarch;

Settings.WindowTitle = "Sigmarch";

Builder gameStartup = new Builder()
    .SetScreenSize(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT)
    .SetStartingScreen<RootScreen>()
    .ShowMonoGameFPS()
    .IsStartingScreenFocused(true)
    // .ConfigureFonts(true)
    ;

Game.Create(gameStartup);
Game.Instance.Run();
Game.Instance.Dispose();