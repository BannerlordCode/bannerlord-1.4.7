using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x0200006B RID: 107
	public class TextureWidget : ImageWidget
	{
		// Token: 0x17000213 RID: 531
		// (get) Token: 0x0600074E RID: 1870 RVA: 0x0001F56E File Offset: 0x0001D76E
		// (set) Token: 0x0600074F RID: 1871 RVA: 0x0001F576 File Offset: 0x0001D776
		public Widget LoadingIconWidget { get; set; }

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000750 RID: 1872 RVA: 0x0001F57F File Offset: 0x0001D77F
		// (set) Token: 0x06000751 RID: 1873 RVA: 0x0001F587 File Offset: 0x0001D787
		public TextureProvider TextureProvider { get; private set; }

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000752 RID: 1874 RVA: 0x0001F590 File Offset: 0x0001D790
		// (set) Token: 0x06000753 RID: 1875 RVA: 0x0001F598 File Offset: 0x0001D798
		public bool SetForClearNextFrame { get; protected set; }

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000754 RID: 1876 RVA: 0x0001F5A1 File Offset: 0x0001D7A1
		// (set) Token: 0x06000755 RID: 1877 RVA: 0x0001F5A9 File Offset: 0x0001D7A9
		[Editor(false)]
		public string TextureProviderName
		{
			get
			{
				return this._textureProviderName;
			}
			set
			{
				if (this._textureProviderName != value)
				{
					this._textureProviderName = value;
					base.OnPropertyChanged<string>(value, "TextureProviderName");
				}
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000756 RID: 1878 RVA: 0x0001F5CC File Offset: 0x0001D7CC
		// (set) Token: 0x06000757 RID: 1879 RVA: 0x0001F5D4 File Offset: 0x0001D7D4
		public Texture Texture
		{
			get
			{
				return this._texture;
			}
			protected set
			{
				if (value != this._texture)
				{
					this._texture = value;
					this.OnTextureUpdated();
				}
			}
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0001F5EC File Offset: 0x0001D7EC
		public TextureWidget(UIContext context)
			: base(context)
		{
			this.TextureProviderName = "ResourceTextureProvider";
			this.TextureProvider = null;
			this._textureProviderProperties = new Dictionary<string, object>();
			this.SetTextureProviderProperty("SourceInfo", base.Context.Name ?? this.ToString());
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x0001F63D File Offset: 0x0001D83D
		public virtual void OnClearTextureProvider()
		{
			TextureProvider textureProvider = this.TextureProvider;
			if (textureProvider != null)
			{
				textureProvider.Clear(true);
			}
			this.TextureProvider = null;
			this.SetForClearNextFrame = true;
			this._lastWidth = 0f;
			this._lastHeight = 0f;
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x0001F675 File Offset: 0x0001D875
		protected override void OnDisconnectedFromRoot()
		{
			base.OnDisconnectedFromRoot();
			this.OnClearTextureProvider();
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x0001F684 File Offset: 0x0001D884
		private void SetTextureProviderProperties()
		{
			if (this.TextureProvider != null)
			{
				foreach (KeyValuePair<string, object> keyValuePair in this._textureProviderProperties)
				{
					this.TextureProvider.SetProperty(keyValuePair.Key, keyValuePair.Value);
				}
			}
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x0001F6F4 File Offset: 0x0001D8F4
		protected void SetTextureProviderProperty(string name, object value)
		{
			this._textureProviderProperties[name] = value;
			if (this.TextureProvider != null)
			{
				this.TextureProvider.SetProperty(name, value);
			}
			this.Texture = null;
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x0001F71F File Offset: 0x0001D91F
		protected object GetTextureProviderProperty(string propertyName)
		{
			TextureProvider textureProvider = this.TextureProvider;
			if (textureProvider == null)
			{
				return null;
			}
			return textureProvider.GetProperty(propertyName);
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x0001F734 File Offset: 0x0001D934
		protected TObject? GetTextureProviderProperty<TObject>(string propertyName) where TObject : struct
		{
			TextureProvider textureProvider = this.TextureProvider;
			object obj;
			if ((obj = ((textureProvider != null) ? textureProvider.GetProperty(propertyName) : null)) is TObject)
			{
				TObject tobject = (TObject)((object)obj);
				return new TObject?(tobject);
			}
			return null;
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x0001F774 File Offset: 0x0001D974
		protected void UpdateTextureWidget()
		{
			if (this._isRenderRequestedPreviousFrame && base.IsRecursivelyVisible())
			{
				if (this.TextureProvider != null)
				{
					if (this._lastWidth != base.Size.X || this._lastHeight != base.Size.Y || this._isTargetSizeDirty)
					{
						int num = MathF.Round(base.Size.X);
						int num2 = MathF.Round(base.Size.Y);
						this.TextureProvider.SetTargetSize(num, num2);
						this._lastWidth = base.Size.X;
						this._lastHeight = base.Size.Y;
						this._isTargetSizeDirty = false;
						return;
					}
				}
				else if (!string.IsNullOrEmpty(this.TextureProviderName))
				{
					this.TextureProvider = TextureProviderFactory.CreateInstance(this.TextureProviderName);
					this.SetTextureProviderProperties();
					this.SetForClearNextFrame = false;
					this._isTargetSizeDirty = true;
				}
			}
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x0001F860 File Offset: 0x0001DA60
		protected virtual void OnTextureUpdated()
		{
			TextureWidget.<>c__DisplayClass33_0 CS$<>8__locals1 = new TextureWidget.<>c__DisplayClass33_0();
			TextureWidget.<>c__DisplayClass33_0 CS$<>8__locals2 = CS$<>8__locals1;
			Texture texture = this.Texture;
			CS$<>8__locals2.isTextureValid = texture != null && texture.IsValid;
			if (this.LoadingIconWidget != null)
			{
				this.LoadingIconWidget.IsVisible = !CS$<>8__locals1.isTextureValid;
				this.LoadingIconWidget.ApplyActionToAllChildrenRecursive(delegate(Widget w)
				{
					w.IsVisible = !CS$<>8__locals1.isTextureValid;
				});
			}
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x0001F8BE File Offset: 0x0001DABE
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			this.UpdateTextureWidget();
			if (this._isRenderRequestedPreviousFrame)
			{
				TextureProvider textureProvider = this.TextureProvider;
				if (textureProvider != null)
				{
					textureProvider.Tick(dt);
				}
			}
			this._isRenderRequestedPreviousFrame = false;
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x0001F8F0 File Offset: 0x0001DAF0
		protected override void OnRender(TwoDimensionContext twoDimensionContext, TwoDimensionDrawContext drawContext)
		{
			this._isRenderRequestedPreviousFrame = base.IsRecursivelyVisible();
			if (this.TextureProvider == null)
			{
				return;
			}
			this.Texture = this.TextureProvider.GetTextureForRender(twoDimensionContext, null);
			Texture texture = this.Texture;
			if (texture == null || !texture.IsValid)
			{
				return;
			}
			SimpleMaterial simpleMaterial = drawContext.CreateSimpleMaterial();
			StyleLayer[] layers = base.ReadOnlyBrush.GetStyleOrDefault(base.CurrentState).GetLayers();
			simpleMaterial.OverlayEnabled = false;
			simpleMaterial.CircularMaskingEnabled = false;
			simpleMaterial.Texture = this.Texture;
			simpleMaterial.NinePatchParameters = SpriteNinePatchParameters.Empty;
			if (layers != null && layers.Length != 0)
			{
				StyleLayer styleLayer = layers[0];
				simpleMaterial.AlphaFactor = styleLayer.AlphaFactor * base.ReadOnlyBrush.GlobalAlphaFactor * base.Context.ContextAlpha;
				simpleMaterial.ColorFactor = styleLayer.ColorFactor * base.ReadOnlyBrush.GlobalColorFactor;
				simpleMaterial.HueFactor = styleLayer.HueFactor;
				simpleMaterial.SaturationFactor = styleLayer.SaturationFactor;
				simpleMaterial.ValueFactor = styleLayer.ValueFactor;
				simpleMaterial.Color = styleLayer.Color * base.ReadOnlyBrush.GlobalColor;
			}
			else
			{
				simpleMaterial.AlphaFactor = base.ReadOnlyBrush.GlobalAlphaFactor * base.Context.ContextAlpha;
				simpleMaterial.ColorFactor = base.ReadOnlyBrush.GlobalColorFactor;
				simpleMaterial.HueFactor = 0f;
				simpleMaterial.SaturationFactor = 0f;
				simpleMaterial.ValueFactor = 0f;
				simpleMaterial.Color = Color.White * base.ReadOnlyBrush.GlobalColor;
			}
			ImageDrawObject imageDrawObject = ImageDrawObject.Create(in this.AreaRect, in Vec2.Zero, in Vec2.One);
			imageDrawObject.Scale = base._scaleToUse;
			if (drawContext.CircularMaskEnabled)
			{
				simpleMaterial.CircularMaskingEnabled = true;
				simpleMaterial.CircularMaskingCenter = drawContext.CircularMaskCenter;
				simpleMaterial.CircularMaskingRadius = drawContext.CircularMaskRadius;
				simpleMaterial.CircularMaskingSmoothingRadius = drawContext.CircularMaskSmoothingRadius;
			}
			drawContext.Draw(simpleMaterial, in imageDrawObject);
		}

		// Token: 0x04000368 RID: 872
		private string _textureProviderName;

		// Token: 0x04000369 RID: 873
		private Texture _texture;

		// Token: 0x0400036A RID: 874
		private float _lastWidth;

		// Token: 0x0400036B RID: 875
		private float _lastHeight;

		// Token: 0x0400036C RID: 876
		protected bool _isTargetSizeDirty;

		// Token: 0x0400036D RID: 877
		private Dictionary<string, object> _textureProviderProperties;

		// Token: 0x0400036E RID: 878
		protected bool _isRenderRequestedPreviousFrame;
	}
}
