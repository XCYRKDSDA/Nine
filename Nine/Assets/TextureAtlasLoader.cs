using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nine.Graphics;

namespace Nine.Assets;

public class TextureAtlasLoader : IAssetLoader<TextureAtlas>
{
    private class JsonTextureAtlas
    {
        public string ImagePath { get; set; } = string.Empty;

        public Dictionary<string, JsonSubTexture> SubTextures { get; set; } = new();
    }

    private class JsonSubTexture
    {
        public int X { get; set; }

        public int Y { get; set; }

        public int W { get; set; }

        public int H { get; set; }

        public int? FrameX { get; set; }

        public int? FrameY { get; set; }

        public int? FrameW { get; set; }

        public int? FrameH { get; set; }
    }

    public TextureAtlas Load(AssetsContext context, string asset)
    {
        using var fileStream = context.Open(asset);
        var serializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        var jsonTextureAtlas = JsonSerializer.Deserialize<JsonTextureAtlas>(fileStream, serializerOptions) ?? throw new JsonException();

        var sourceTexture = context.Load<Texture2D>(jsonTextureAtlas.ImagePath);
        var textureAtlas = new TextureAtlas(sourceTexture);

        foreach (var (key, jsonSubTexture) in jsonTextureAtlas.SubTextures)
        {
            var sourceRegion = new Rectangle(jsonSubTexture.X, jsonSubTexture.Y, jsonSubTexture.W, jsonSubTexture.H);
            Rectangle? virtualFrame = null;
            if (jsonSubTexture.FrameX is int frameX && jsonSubTexture.FrameY is int frameY
                && jsonSubTexture.FrameW is int frameW && jsonSubTexture.FrameH is int frameH)
                virtualFrame = new Rectangle(frameX, frameY, frameW, frameH);
            textureAtlas.Add(key, sourceRegion, virtualFrame);
        }

        return textureAtlas;
    }
}
