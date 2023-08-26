using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nine.Graphics;

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
    }

    public TextureAtlas Load(AssetsContext context, string asset)
    {
        using var fileStream = context.Open(asset);
        var serializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        var jsonTextureAtlas = JsonSerializer.Deserialize<JsonTextureAtlas>(fileStream, serializerOptions) ?? throw new JsonException();

        var sourceTexture = context.Load<Texture2D>(jsonTextureAtlas.Image);
        var textureAtlas = new TextureAtlas(sourceTexture);

        foreach (var (key, jsonSubTexture) in jsonTextureAtlas.Regions)
        {
            var sourceRegion = new Rectangle(jsonSubTexture.X, jsonSubTexture.Y, jsonSubTexture.W, jsonSubTexture.H);
            textureAtlas.Add(key, sourceRegion);
        }

        return textureAtlas;
    }
}
