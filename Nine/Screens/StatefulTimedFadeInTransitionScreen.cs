using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nine.Screens;

public abstract class StatefulTimedFadeInTransitionScreen<TSourceState, TTargetState>(
    GraphicsDevice graphicsDevice,
    IVisualConfigurableScreen<TSourceState> prevScreen,
    IVisualConfigurableScreen<TTargetState> nextScreen,
    TimeSpan duration
) : StatefulTimedTransitionScreenBase<TSourceState, TTargetState>(prevScreen, nextScreen, duration)
{
    private readonly RenderTarget2D _backgroundRenderTarget = new(
        graphicsDevice,
        graphicsDevice.PresentationParameters.BackBufferWidth,
        graphicsDevice.PresentationParameters.BackBufferHeight,
        false,
        SurfaceFormat.Color,
        DepthFormat.None,
        0,
        RenderTargetUsage.PreserveContents
    );

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

    protected virtual (float, float) CalculateAlpha() => (1 - Progress, Progress);

    protected virtual void DrawBackground() { }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        // 缓存当前的绘制目标
        var renderTargetsCache = graphicsDevice.GetRenderTargets();

        // 绘制过渡的背景
        graphicsDevice.SetRenderTarget(_backgroundRenderTarget);
        graphicsDevice.Clear(Color.Black);
        DrawBackground();

        // 绘制前一个界面
        graphicsDevice.SetRenderTarget(_prevRenderTarget);
        graphicsDevice.Clear(Color.Black);
        PrevScreen.Draw(gameTime);

        // 绘制后一个界面
        graphicsDevice.SetRenderTarget(_nextRenderTarget);
        graphicsDevice.Clear(Color.Black);
        NextScreen.Draw(gameTime);

        // 计算混合
        var (prevAlpha, nextAlpha) = CalculateAlpha();
        var bgAlpha = 1 - prevAlpha - nextAlpha;
        // Debug.Assert(prevAlpha >= 0 && prevAlpha <= 1);
        // Debug.Assert(nextAlpha >= 0 && nextAlpha <= 1);
        // Debug.Assert(bgAlpha >= 0 && bgAlpha <= 1);

        // 混合三个界面
        graphicsDevice.SetRenderTargets(renderTargetsCache);
        graphicsDevice.Clear(Color.Black);
        _spriteBatch.Begin(
            SpriteSortMode.Immediate,
            new BlendState()
            {
                ColorSourceBlend = Blend.One,
                AlphaSourceBlend = Blend.One,
                ColorDestinationBlend = Blend.One,
                AlphaDestinationBlend = Blend.One,
            }
        );
        _spriteBatch.Draw(_backgroundRenderTarget, Vector2.Zero, Color.White * bgAlpha);
        _spriteBatch.Draw(_prevRenderTarget, Vector2.Zero, Color.White * prevAlpha);
        _spriteBatch.Draw(_nextRenderTarget, Vector2.Zero, Color.White * nextAlpha);
        _spriteBatch.End();
    }
}
