using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nine.Screens.Transitions;

public class CustomFadeInTransition(
    GraphicsDevice graphicsDevice, ScreenManager screenManager, IScreen prevScreen, IScreen nextScreen,
    TimeSpan duration)
    : TransitionBase(screenManager, prevScreen, nextScreen)
{
    private readonly RenderTarget2D _prevRenderTarget = new(
        graphicsDevice, graphicsDevice.PresentationParameters.BackBufferWidth,
        graphicsDevice.PresentationParameters.BackBufferHeight
    );

    private readonly RenderTarget2D _nextRenderTarget = new(
        graphicsDevice, graphicsDevice.PresentationParameters.BackBufferWidth,
        graphicsDevice.PresentationParameters.BackBufferHeight
    );

    private readonly SpriteBatch _spriteBatch = new(graphicsDevice, 1);

    private TimeSpan _duration = TimeSpan.Zero;

    public float Progress => (float)(_duration / duration);

    public override void Update(GameTime gameTime)
    {
        _duration += gameTime.ElapsedGameTime;
        var progress = (float)(_duration / duration);
        prevScreen.OnTransitOut(progress);
        nextScreen.OnTransitIn(progress);

        if (_duration >= duration)
            ScreenManager.ActiveScreen = nextScreen;
    }

    public override void Draw(GameTime gameTime)
    {
        var renderTargetsCache = graphicsDevice.GetRenderTargets();

        graphicsDevice.SetRenderTarget(_prevRenderTarget);
        graphicsDevice.Clear(Color.Transparent);
        prevScreen.Draw(gameTime);

        graphicsDevice.SetRenderTarget(_nextRenderTarget);
        graphicsDevice.Clear(Color.Transparent);
        nextScreen.Draw(gameTime);

        graphicsDevice.SetRenderTargets(renderTargetsCache);

        _spriteBatch.Begin();
        _spriteBatch.Draw(_prevRenderTarget, Vector2.Zero, Color.White);
        _spriteBatch.Draw(_nextRenderTarget, Vector2.Zero, Color.White * Progress);
        _spriteBatch.End();
    }
}
