using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nine.Screens;
using Nine.Screens.Transitions;

namespace Nine.Samples;

internal abstract class SampleScreen : ScreenBase
{
    public SampleScreen(Game game, ScreenManager screenManager,
                        Texture2D background, Func<IScreen> nextScreenBuilder)
        : base(game, screenManager)
    {
        _spriteBatch = new(game.GraphicsDevice, 1);
        _background = background;
        _nextScreenConstructor = nextScreenBuilder;
    }

    private readonly SpriteBatch _spriteBatch;

    private readonly Texture2D _background;
    private readonly Func<IScreen> _nextScreenConstructor;

    public sealed override void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.Space))
            ScreenManager.ActiveScreen = new FadeTransition(
                Game, ScreenManager, ScreenManager.ActiveScreen, Task.Run(_nextScreenConstructor));
    }

    public sealed override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();
        _spriteBatch.Draw(_background, Game.GraphicsDevice.Viewport.Bounds, Color.White);
        _spriteBatch.End();
    }
}

internal class SampleScreen1 : SampleScreen
{
    public SampleScreen1(Game game, ScreenManager screenManager)
        : base(game, screenManager, game.Content.Load<Texture2D>("bars"), () => new SampleScreen2(game, screenManager))
    { }
}

internal class SampleScreen2 : SampleScreen
{
    public SampleScreen2(Game game, ScreenManager screenManager)
        : base(game, screenManager, game.Content.Load<Texture2D>("circles"), () => new SampleScreen3(game, screenManager))
    { }
}

internal class SampleScreen3 : SampleScreen
{
    public SampleScreen3(Game game, ScreenManager screenManager)
        : base(game, screenManager, game.Content.Load<Texture2D>("polygons"), () => new SampleScreen4(game, screenManager))
    { }
}

internal class SampleScreen4 : SampleScreen
{
    public SampleScreen4(Game game, ScreenManager screenManager)
        : base(game, screenManager, game.Content.Load<Texture2D>("triangles"), () => new SampleScreen1(game, screenManager))
    { }
}
