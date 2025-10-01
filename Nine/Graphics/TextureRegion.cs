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
    /// 在像素区域坐标系下的逻辑原点
    /// </summary>
    public Vector2 LogicalOrigin { get; }

    /// <summary>
    /// 逻辑尺寸
    /// </summary>
    public Vector2 LogicalSize { get; }

    public TextureRegion(Texture2D texture, Rectangle? region = null,
                         Vector2? logicalOrigin = null, Vector2? logicalSize = null)
    {
        Texture = texture;
        Bounds = region ?? texture.Bounds;

        LogicalOrigin = logicalOrigin ?? Vector2.Zero;
        LogicalSize = logicalSize ?? Bounds.Size.ToVector2();
    }
}
