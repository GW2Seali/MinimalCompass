using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace MinimalCompass.Services;

public sealed class TextureService(ContentsManager contentsManager)
{
	public Texture2D WindowBackground { get; } = contentsManager.GetTexture("window/background.png");
}