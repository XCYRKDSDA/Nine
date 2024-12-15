using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public int[]? Padding { get; set; }
    }

    public TextureAtlas Load(IFileSystem fs, IAssetsManager assets, in UPath path)
    {
        using var fileStream = fs.OpenFile(path, FileMode.Open, FileAccess.Read);

        var serializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        var jsonTextureAtlas = JsonSerializer.Deserialize<JsonTextureAtlas>(fileStream, serializerOptions) ??
                               throw new JsonException();

        var sourceTexture = assets.Load<Texture2D>(UPath.Combine(path.GetDirectory(), jsonTextureAtlas.Image));
        var textureAtlas = new TextureAtlas(sourceTexture);

        foreach (var (key, jsonSubTexture) in jsonTextureAtlas.Regions)
        {
            var sourceRegion = new Rectangle(jsonSubTexture.X, jsonSubTexture.Y, jsonSubTexture.W, jsonSubTexture.H);
            if (jsonSubTexture.Padding is null)
                textureAtlas.Add(key, sourceRegion);
            else
            {
                var padding = jsonSubTexture.Padding.Length switch
                {
                    1 => new NinePatchPadding(jsonSubTexture.Padding[0]),
                    2 => new NinePatchPadding(jsonSubTexture.Padding[0], jsonSubTexture.Padding[1]),
                    4 => new NinePatchPadding(jsonSubTexture.Padding[0], jsonSubTexture.Padding[1],
                                              jsonSubTexture.Padding[2], jsonSubTexture.Padding[3]),
                    _ => throw new NotImplementedException()
                };
                textureAtlas.Add(key, sourceRegion, padding);
            }
        }

        return textureAtlas;
    }
}
