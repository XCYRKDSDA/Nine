using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nine.Assets.Serialization;
using Nine.Graphics;
using Zio;

namespace Nine.Assets;

public class TextureAtlasLoader : IAssetLoader<TextureAtlas>
{
    private class JsonTextureAtlas
    {
        public string Image { get; set; } = string.Empty;

        public Dictionary<string, JsonSubTexture> Regions { get; set; } = new();
    }

    private class JsonSubTexture
    {
        public int X { get; set; }

        public int Y { get; set; }

        public int W { get; set; }

        public int H { get; set; }

        public Vector2? Anchor { get; set; }

        public Vector2? Size { get; set; }

        public NinePatchPadding? Padding { get; set; }
    }

    public TextureAtlas Load(IFileSystem fs, IAssetsManager assets, in UPath path)
    {
        using var fileStream = fs.OpenFile(path, FileMode.Open, FileAccess.Read);

        var serializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        serializerOptions.Converters.Add(new Vector2JsonConverter());
        serializerOptions.Converters.Add(new NinePatchPaddingJsonConverter());
        var jsonTextureAtlas = JsonSerializer.Deserialize<JsonTextureAtlas>(fileStream, serializerOptions) ??
                               throw new JsonException();

        var sourceTexture = assets.Load<Texture2D>(UPath.Combine(path.GetDirectory(), jsonTextureAtlas.Image));
        var textureAtlas = new TextureAtlas(sourceTexture);

        foreach (var (key, jsonSubTexture) in jsonTextureAtlas.Regions)
        {
            var sourceRegion = new Rectangle(jsonSubTexture.X, jsonSubTexture.Y, jsonSubTexture.W, jsonSubTexture.H);
            if (jsonSubTexture.Padding is { } padding)
                textureAtlas.Add(key, sourceRegion, padding, jsonSubTexture.Anchor, jsonSubTexture.Size);
            else
                textureAtlas.Add(key, sourceRegion, jsonSubTexture.Anchor, jsonSubTexture.Size);
        }

        return textureAtlas;
    }
}
