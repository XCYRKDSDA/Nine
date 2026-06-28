using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nine.Graphics;

public class TextureRegion
{
    /// <summary>
    /// 原始纹理对象
    /// </summary>
    public Texture2D Texture { get; }

    /// <summary>
    /// 在原始纹理坐标系中的像素区域
    /// </summary>
    public Rectangle Bounds { get; }

    /// <summary>
    /// Bounds 局部坐标系下的变换锚点
    /// </summary>
    public Vector2 Anchor { get; }

    /// <summary>
    /// Bounds 局部坐标系下的虚拟方框，描述纹理内实际有效的部分
    /// </summary>
    public RectangleF VirtualFrame { get; }

    public TextureRegion(
        Texture2D texture,
        Rectangle? region = null,
        Vector2? anchor = null,
        RectangleF? virtualFrame = null
    )
    {
        Texture = texture;
        Bounds = region ?? texture.Bounds;

        Anchor = anchor ?? Vector2.Zero;
        VirtualFrame = virtualFrame ?? new RectangleF(0, 0, Bounds.Width, Bounds.Height);
    }
}
