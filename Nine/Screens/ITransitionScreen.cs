using Microsoft.Xna.Framework;

namespace Nine.Screens;

public enum TransitionState
{
    Pending,
    InProgress,
    Completed,
}

public interface ITransitionScreen : IScreen
{
    /// <summary>
    /// 反向更新, 使得过渡界面可以逆转
    /// </summary>
    void UpdateBackward(GameTime gameTime);

    TransitionState TransitionState { get; }
}
