using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nine.Screens.Transitions;

/// <summary>
/// 淡入淡出过渡画面.
/// 上一个画面逐渐淡出, 画面变为纯色, 然后再淡入到新画面
/// </summary>
public class FadeTransition : TransitionBase
{
    private enum Stage
    {
        FadingOut,
        Waiting,
        FadingIn,
    }

    /// <summary>
    /// 淡入淡出的背景色
    /// </summary>
    public Color Background { get; set; } = Color.Black;

    /// <summary>
    /// 淡出的时长
    /// </summary>
    public TimeSpan FadeOutDuration { get; set; } = TimeSpan.FromSeconds(0.4);

    /// <summary>
    /// 淡出后进入淡入过程前的最小延迟. 实际的延迟由下一个画面的加载速度决定
    /// </summary>
    public TimeSpan MinimumDelayBeforeFadeIn { get; set; } = TimeSpan.FromSeconds(0.1);

    /// <summary>
    /// 淡入的时长
    /// </summary>
    public TimeSpan FadeInDuration { get; set; } = TimeSpan.FromSeconds(0.6);

    private TimeSpan _duration = TimeSpan.Zero;

    private Stage _stage = Stage.FadingOut;

    private float _fadeRatio = 0;

    private static readonly VertexPositionColor[] _vertices = new VertexPositionColor[]
    {
        new(new(-1, 1, 0), Color.Black),
        new(new(1, 1, 0), Color.Black),
        new(new(-1, -1, 0), Color.Black),
        new(new(1, -1, 0), Color.Black)
    };

    private readonly GraphicsDevice _graphicsDevice;
    private readonly BasicEffect _effect;

    public FadeTransition(GraphicsDevice graphicsDevice, ScreenManager screenManager, IScreen prevScreen,
                          IScreen nextScreen)
        : base(screenManager, prevScreen, nextScreen)
    {
        _graphicsDevice = graphicsDevice;
        _effect = new(graphicsDevice) { VertexColorEnabled = true };
    }

    public FadeTransition(GraphicsDevice graphicsDevice, ScreenManager screenManager, IScreen prevScreen,
                          Task<IScreen> nextScreenLoader)
        : base(screenManager, prevScreen, nextScreenLoader)
    {
        _graphicsDevice = graphicsDevice;
        _effect = new(graphicsDevice) { VertexColorEnabled = true };
    }

    public override void Update(GameTime gameTime)
    {
        _duration += gameTime.ElapsedGameTime;

        // 根据时间和新画面加载状况更新时间和状态
        if (_stage == Stage.FadingOut && _duration >= FadeOutDuration)
        {
            _stage = Stage.Waiting;
            _duration = TimeSpan.Zero;
        }
        else if (_stage == Stage.Waiting && _duration >= MinimumDelayBeforeFadeIn && NextScreen is not null)
        {
            _stage = Stage.FadingIn;
            _duration = TimeSpan.Zero;
        }
        else if (_stage == Stage.FadingIn && _duration >= FadeInDuration)
            ScreenManager.ActiveScreen = NextScreen;

        // 根据时间和状态计算淡化比例
        _fadeRatio = _stage switch
        {
            Stage.FadingOut => (float)(_duration.TotalSeconds / FadeOutDuration.TotalSeconds),
            Stage.Waiting => 1f,
            Stage.FadingIn => 1 - (float)(_duration.TotalSeconds / FadeInDuration.TotalSeconds),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public override void Draw(GameTime gameTime)
    {
        if (_stage == Stage.FadingOut)
            PrevScreen.Draw(gameTime);
        else if (_stage == Stage.FadingIn)
            NextScreen!.Draw(gameTime);

        _effect.Alpha = _fadeRatio;
        _vertices[0].Color = _vertices[1].Color = _vertices[2].Color = _vertices[3].Color = Background;
        foreach (var pass in _effect.CurrentTechnique.Passes)
        {
            pass.Apply();
            _graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, _vertices, 0, 2);
        }
    }
}
