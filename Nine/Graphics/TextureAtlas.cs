using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nine.Graphics;

public class TextureAtlas : IReadOnlyDictionary<string, TextureRegion>
{
    private readonly Texture2D _texture;
    private readonly Dictionary<string, TextureRegion> _regions = new();

    public Texture2D Texture => _texture;

    #region IReadOnlyDictionary

    public IEnumerable<string> Keys => _regions.Keys;

    public IEnumerable<TextureRegion> Values => _regions.Values;

    public int Count => _regions.Count;

    public TextureRegion this[string key] => _regions[key];

    public bool ContainsKey(string key) => _regions.ContainsKey(key);

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out TextureRegion value) => _regions.TryGetValue(key, out value);

    public IEnumerator<KeyValuePair<string, TextureRegion>> GetEnumerator() => _regions.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _regions.GetEnumerator();

    #endregion

    public TextureAtlas(Texture2D texture)
    {
        _texture = texture;
    }

    public void Add(string key, Rectangle region)
    {
        _regions[key] = new(_texture, region);
    }

    public void Add(string key, Rectangle region, NinePatchPadding padding)
    {
        _regions[key] = new NinePatchRegion(_texture, padding, region);
    }
}
