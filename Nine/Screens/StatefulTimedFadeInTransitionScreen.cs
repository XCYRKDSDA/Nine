using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nine.Animations;

namespace Nine.Screens;

public class StatefulTimedFadeInTransitionScreen<TState>(
    GraphicsDevice graphicsDevice,
    ScreenManager screenManager,
    IConfigurableScreen<TState> prevScreen,
    IConfigurableScreen<TState> nextScreen,
    TimeSpan duration,
    ICurve<TState> transitionCurve,
    ICurve<float>? alphaCurve = null
) : StatefulTimedTransitionScreenBase<TState>(screenManager, prevScreen, nextScreen, duration)
    where TState : struct
{
    private readonly RenderTarget2D _prevRenderTarget = new(
        graphicsDevice,
        graphicsDevice.PresentationParameters.BackBufferWidth,
        graphicsDevice.PresentationParameters.BackBufferHeight,
        false,
        SurfaceFormat.Color,
        DepthFormat.None,
        0,
        RenderTargetUsage.PreserveContents
    );

    private readonly RenderTarget2D _nextRenderTarget = new(
        graphicsDevice,
        graphicsDevice.PresentationParameters.BackBufferWidth,
        graphicsDevice.PresentationParameters.BackBufferHeight,
        false,
        SurfaceFormat.Color,
        DepthFormat.None,
        0,
        RenderTargetUsage.PreserveContents
    );

    private readonly SpriteBatch _spriteBatch = new(graphicsDevice, 1);

    protected override TState TransitionState(float progress) => transitionCurve.Evaluate(progress);

    public override void Draw(GameTime gameTime)
    {
        // 缓存当前的绘制目标
        var renderTargetsCache = graphicsDevice.GetRenderTargets();

        // 绘制前一个界面
        graphicsDevice.SetRenderTarget(_prevRenderTarget);
        graphicsDevice.Clear(Color.Black);
        PrevScreen.Draw(gameTime);

        // 绘制后一个界面
        graphicsDevice.SetRenderTarget(_nextRenderTarget);
        graphicsDevice.Clear(Color.Transparent);
        NextScreen.Draw(gameTime);

        // 混合两个界面
        var alpha = alphaCurve is null ? Progress : alphaCurve.Evaluate(Progress);
        graphicsDevice.SetRenderTargets(renderTargetsCache);
        _spriteBatch.Begin();
        _spriteBatch.Draw(_prevRenderTarget, Vector2.Zero, Color.White);
        _spriteBatch.Draw(_nextRenderTarget, Vector2.Zero, Color.White * alpha);
        _spriteBatch.End();
    }
}
