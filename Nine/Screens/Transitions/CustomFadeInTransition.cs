using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nine.Screens.Transitions;

public class CustomFadeInTransition(
    GraphicsDevice graphicsDevice, ScreenManager screenManager, IScreen prevScreen, IScreen nextScreen,
    TimeSpan duration, object? context = null)
    : TransitionBase(screenManager, prevScreen, nextScreen, context)
{
    private readonly RenderTarget2D _prevRenderTarget = new(
        graphicsDevice, graphicsDevice.PresentationParameters.BackBufferWidth,
        graphicsDevice.PresentationParameters.BackBufferHeight,
        false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents
    );

    private readonly RenderTarget2D _nextRenderTarget = new(
        graphicsDevice, graphicsDevice.PresentationParameters.BackBufferWidth,
        graphicsDevice.PresentationParameters.BackBufferHeight,
        false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents
    );

    private readonly SpriteBatch _spriteBatch = new(graphicsDevice, 1);

    private TimeSpan _duration = TimeSpan.Zero;

    public float Progress => (float)(_duration / duration);

    public override void Update(GameTime gameTime)
    {
        _duration += gameTime.ElapsedGameTime;
        var progress = (float)(_duration / duration);
        base.UpdateTransition(progress);

        if (_duration >= duration)
            ScreenManager.ActiveScreen = NextScreen;
    }

    public override void Draw(GameTime gameTime)
    {
        var renderTargetsCache = graphicsDevice.GetRenderTargets();

        graphicsDevice.SetRenderTarget(_prevRenderTarget);
        graphicsDevice.Clear(Color.Black);
        PrevScreen.Draw(gameTime);

        graphicsDevice.SetRenderTarget(_nextRenderTarget);
        graphicsDevice.Clear(Color.Transparent);
        NextScreen.Draw(gameTime);

        graphicsDevice.SetRenderTargets(renderTargetsCache);

        _spriteBatch.Begin();
        _spriteBatch.Draw(_prevRenderTarget, Vector2.Zero, Color.White);
        _spriteBatch.Draw(_nextRenderTarget, Vector2.Zero, Color.White * Progress);
        _spriteBatch.End();
    }
}
