using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Gw2Sharp.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace MinimalCompass.Controls
{
	public sealed class CompassControl : Control
	{
		public float DistortionFactor { get; private set; } = 0.02f;

		private float _locationFactorX = 0.5f;
		public float LocationFactorX
		{
			get => _locationFactorX;
			set => SetProperty(ref _locationFactorX, Math.Min(Math.Max(value, 0f), 1f), true);
		}

		private float _locationFactorY = 0.1f;
		public float LocationFactorY
		{
			get => _locationFactorY;
			set => SetProperty(ref _locationFactorY, Math.Min(Math.Max(value, 0f), 1f), true);
		}
		
		private const float ViewAngle = 90f;
		private const int BaseTickHeight = 15;
		
		// private static readonly Logger Logger = Logger.GetLogger<CompassControl>();

		public CompassControl()
		{
			Parent = GameService.Graphics.SpriteScreen;
			_size = new Point(300, 100);
			Location = new Point(
				(int)((GameService.Graphics.SpriteScreen.Width - Width) * LocationFactorX), 
				(int)((GameService.Graphics.SpriteScreen.Height - Height) * LocationFactorY)
				);
		}

		/// <inheritdoc />
		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			if (!GameService.Gw2Mumble.IsAvailable) return;
			if (!GameService.GameIntegration.Gw2Instance.IsInGame) return;
			if (GameService.Gw2Mumble.UI.IsMapOpen) return;
			if (!Enabled) return;
			
			Coordinates3 heading = GameService.Gw2Mumble.RawClient.CameraFront;
			double angle = Math.Atan2(heading.X, heading.Z);
			double centerDegree = angle * (180 / Math.PI);

			float centerY = bounds.Height / 2f;
			float centerX = bounds.Width / 2f;

			double startDegree = centerDegree - ViewAngle;
			double endDegree = centerDegree + ViewAngle;

			double firstTick = Math.Ceiling(startDegree / 5.0) * 5;

			for (double degree = firstTick; degree <= endDegree; degree += 5)
			{
				double angleFromCenter = degree - centerDegree;

				double normalizedAngle = angleFromCenter / (ViewAngle / 2.0);

				if (Math.Abs(normalizedAngle) > 1.2) continue;

				double distortedAngle = Math.Sign(normalizedAngle) * Math.Pow(Math.Abs(normalizedAngle), 1.0 - DistortionFactor);

				float x = centerX + (float)(distortedAngle * centerX);

				double scale = 1.0 / (1.0 + Math.Abs(distortedAngle));

				double normalizedDegree = (degree % 360 + 360) % 360;

				double currentTickHeight = BaseTickHeight * scale;
				string? label = null;

				if (normalizedDegree % 90 == 0)
				{
					currentTickHeight = BaseTickHeight * 2 * scale;
					label = normalizedDegree switch
					{
						0 => "N",
						90 => "E",
						180 => "S",
						270 => "W",
						_ => null
					};
				}
				else if (normalizedDegree % 45 == 0)
				{
					currentTickHeight = BaseTickHeight * 1.5 * scale;
					label = normalizedDegree switch
					{
						45 => "NE",
						135 => "SE",
						225 => "SW",
						315 => "NW",
						_ => null
					};
				}
				else if (normalizedDegree % 15 == 0)
				{
					currentTickHeight = BaseTickHeight * 1.2 * scale;
				}

				var tickWidth = (float)(2 * scale);
				spriteBatch.DrawLine(new Vector2(Location.X +  x, Location.Y + centerY - (float)currentTickHeight / 2), 
					new Vector2(Location.X + x, Location.Y + centerY + (float)currentTickHeight / 2),
					Color.White, 
					tickWidth);

				if (label == null) continue;
				BitmapFont font = GameService.Content.DefaultFont14;
				Size2 textSize = font.MeasureString(label);
				float textY = centerY - (float)currentTickHeight / 2 - 5;
				var fontScale = (float)(1.0f * scale);
				Vector2 textPosition = new(x - (textSize.Width * fontScale) / 2, textY - (textSize.Height * fontScale));
				spriteBatch.DrawStringOnCtrl(this,
					label, 
					font, 
					new Rectangle((int)textPosition.X, (int)textPosition.Y, 
						(int)(textSize.Width * fontScale), 
						(int)(textSize.Height * fontScale)), 
					Color.White);
			}

			const float centerIndicatorHeight = BaseTickHeight * 2.5f;
			spriteBatch.DrawLine(new Vector2(Location.X + centerX, Location.Y +  centerY - centerIndicatorHeight / 2), 
				new Vector2(Location.X + centerX, Location.Y + centerY + centerIndicatorHeight / 2), 
				Color.Red, 
				2);
		}

		/// <inheritdoc />
		public override void RecalculateLayout()
		{
			Location = new Point(
				(int)((GameService.Graphics.SpriteScreen.Width - Width) * LocationFactorX), 
				(int)((GameService.Graphics.SpriteScreen.Height - Height) * LocationFactorY)
				);
		}
		
		public void SetLocationFactor(float xFactor, float yFactor)
		{
			LocationFactorX = Math.Min(Math.Max(xFactor, 0f), 1f);
			LocationFactorY = Math.Min(Math.Max(yFactor, 0f), 1f);
		}

		/// <inheritdoc />
		protected override CaptureType CapturesInput()
		{
			return CaptureType.DoNotBlock;
		}
	}
}