using System;
using System.Numerics;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x0200005F RID: 95
	public class MaskedTextureWidget : TextureWidget
	{
		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000668 RID: 1640 RVA: 0x0001B883 File Offset: 0x00019A83
		// (set) Token: 0x06000669 RID: 1641 RVA: 0x0001B88B File Offset: 0x00019A8B
		[Editor(false)]
		public float OverlayTextureScale { get; set; }

		// Token: 0x0600066A RID: 1642 RVA: 0x0001B894 File Offset: 0x00019A94
		public MaskedTextureWidget(UIContext context)
			: base(context)
		{
			base.TextureProviderName = "";
			this.OverlayTextureScale = 1f;
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x0001B8B3 File Offset: 0x00019AB3
		public override void OnClearTextureProvider()
		{
			this._textureCache = null;
			base.SetTextureProviderProperty("IsReleased", true);
			base.OnClearTextureProvider();
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x0001B8D4 File Offset: 0x00019AD4
		protected internal override void OnContextActivated()
		{
			base.OnContextActivated();
			string imageId = this.ImageId;
			this.ImageId = string.Empty;
			this.ImageId = imageId;
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0001B900 File Offset: 0x00019B00
		protected internal override void OnContextDeactivated()
		{
			base.OnContextDeactivated();
			base.SetTextureProviderProperty("IsReleased", true);
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x0600066E RID: 1646 RVA: 0x0001B919 File Offset: 0x00019B19
		// (set) Token: 0x0600066F RID: 1647 RVA: 0x0001B924 File Offset: 0x00019B24
		[Editor(false)]
		public string ImageId
		{
			get
			{
				return this._imageId;
			}
			set
			{
				if (this._imageId != value)
				{
					if (!string.IsNullOrEmpty(this._imageId))
					{
						base.SetTextureProviderProperty("IsReleased", true);
					}
					this._imageId = value;
					base.OnPropertyChanged<string>(value, "ImageId");
					base.SetTextureProviderProperty("ImageId", value);
					if (!string.IsNullOrEmpty(this._imageId))
					{
						base.SetTextureProviderProperty("IsReleased", false);
					}
				}
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000670 RID: 1648 RVA: 0x0001B99A File Offset: 0x00019B9A
		// (set) Token: 0x06000671 RID: 1649 RVA: 0x0001B9A4 File Offset: 0x00019BA4
		[Editor(false)]
		public string AdditionalArgs
		{
			get
			{
				return this._additionalArgs;
			}
			set
			{
				if (this._additionalArgs != value)
				{
					if (!string.IsNullOrEmpty(this._additionalArgs))
					{
						base.SetTextureProviderProperty("IsReleased", true);
					}
					this._additionalArgs = value;
					base.OnPropertyChanged<string>(value, "AdditionalArgs");
					base.SetTextureProviderProperty("AdditionalArgs", value);
					if (!string.IsNullOrEmpty(this._additionalArgs))
					{
						base.SetTextureProviderProperty("IsReleased", false);
					}
				}
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000672 RID: 1650 RVA: 0x0001BA1A File Offset: 0x00019C1A
		// (set) Token: 0x06000673 RID: 1651 RVA: 0x0001BA24 File Offset: 0x00019C24
		[Editor(false)]
		public bool IsBig
		{
			get
			{
				return this._isBig;
			}
			set
			{
				if (this._isBig != value)
				{
					base.SetTextureProviderProperty("IsReleased", true);
					base.SetTextureProviderProperty("IsReleased", false);
					this._isBig = value;
					base.OnPropertyChanged(value, "IsBig");
					base.SetTextureProviderProperty("IsBig", value);
				}
			}
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x0001BA80 File Offset: 0x00019C80
		protected override void OnRender(TwoDimensionContext twoDimensionContext, TwoDimensionDrawContext drawContext)
		{
			this._isRenderRequestedPreviousFrame = true;
			if (base.TextureProvider == null)
			{
				return;
			}
			Texture textureForRender = base.TextureProvider.GetTextureForRender(twoDimensionContext, null);
			if (textureForRender == null || !textureForRender.IsValid)
			{
				return;
			}
			bool flag = false;
			if (textureForRender != this._textureCache)
			{
				base.Brush.DefaultLayer.OverlayMethod = BrushOverlayMethod.CoverWithTexture;
				this._textureCache = textureForRender;
				flag = true;
				base.UpdateBrushRendererInternal(base.EventManager.CachedDt);
			}
			if (this._textureCache != null)
			{
				bool flag2 = base.TextureProviderName == "BannerImageTextureProvider";
				int num = (flag2 ? ((int)(((base.Size.X > base.Size.Y) ? base.Size.Y : base.Size.X) * 2.5f * this.OverlayTextureScale * base._inverseScaleToUse)) : ((int)(((base.Size.X > base.Size.Y) ? base.Size.X : base.Size.Y) * this.OverlayTextureScale * base._inverseScaleToUse)));
				Vector2 vector = default(Vector2);
				if (flag2)
				{
					float num2 = ((float)num - base.Size.X * base._inverseScaleToUse) * 0.5f - base.Brush.DefaultLayer.OverlayXOffset;
					float num3 = ((float)num - base.Size.Y * base._inverseScaleToUse) * 0.5f - base.Brush.DefaultLayer.OverlayYOffset;
					vector = new Vector2(num2, num3);
				}
				if (this._overlaySpriteCache == null || flag || this._overlaySpriteSizeCache != num)
				{
					this._overlaySpriteSizeCache = num;
					this._overlaySpriteCache = new SpriteFromTexture(this._textureCache, this._overlaySpriteSizeCache, this._overlaySpriteSizeCache);
				}
				base.Brush.DefaultLayer.OverlaySprite = this._overlaySpriteCache;
				base.BrushRenderer.Render(drawContext, in this.AreaRect, base._scaleToUse, base.Context.ContextAlpha, vector, default(Vector2));
			}
		}

		// Token: 0x04000300 RID: 768
		private Texture _textureCache;

		// Token: 0x04000301 RID: 769
		private SpriteFromTexture _overlaySpriteCache;

		// Token: 0x04000302 RID: 770
		private int _overlaySpriteSizeCache;

		// Token: 0x04000303 RID: 771
		private string _imageId;

		// Token: 0x04000304 RID: 772
		private string _additionalArgs;

		// Token: 0x04000305 RID: 773
		private bool _isBig;
	}
}
