using Microsoft.Xna.Framework;

namespace Nine.Screens;

public interface IScreen
{
    /// <summary>
    /// 当该画面被设置为活动画面时调用
    /// </summary>
    void OnActivated();

    /// <summary>
    /// 当该画面被取消设置为活动画面时调用
    /// </summary>
    void OnDeactivated();

    void Update(GameTime gameTime);

    void Draw(GameTime gameTime);

    #region Transition

    void OnStartTransitIn(object? context);

    void OnTransitIn(object? context, float progress);

    void OnStartTransitOut(object? context);

    void OnTransitOut(object? context, float progress);

    #endregion
}
