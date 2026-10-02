using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Barter
{
	// Token: 0x02000190 RID: 400
	public class BarterItemVisualBrushWidget : BrushWidget
	{
		// Token: 0x06001495 RID: 5269 RVA: 0x00037F97 File Offset: 0x00036197
		public BarterItemVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001496 RID: 5270 RVA: 0x00037FAC File Offset: 0x000361AC
		protected override void OnParallelUpdate(float dt)
		{
			base.OnParallelUpdate(dt);
			if (!this._imageDetermined)
			{
				this.RegisterStatesOfWidgetFromBrush(this.SpriteWidget);
				this.UpdateVisual();
				this._imageDetermined = true;
			}
			if (this._imageDetermined && this.Type == "fief_barterable")
			{
				this.SpriteClipWidget.ClipContents = true;
				this.SpriteWidget.WidthSizePolicy = SizePolicy.Fixed;
				this.SpriteWidget.HeightSizePolicy = SizePolicy.Fixed;
				this.SpriteWidget.ScaledSuggestedHeight = this.SpriteClipWidget.Size.X;
				this.SpriteWidget.ScaledSuggestedWidth = this.SpriteClipWidget.Size.X;
				this.SpriteWidget.PositionYOffset = 18f;
				this.SpriteWidget.VerticalAlignment = VerticalAlignment.Center;
			}
		}

		// Token: 0x06001497 RID: 5271 RVA: 0x00038074 File Offset: 0x00036274
		private void RegisterStatesOfWidgetFromBrush(BrushWidget widget)
		{
			if (widget != null)
			{
				foreach (BrushLayer brushLayer in widget.ReadOnlyBrush.Layers)
				{
					widget.AddState(brushLayer.Name);
				}
			}
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x000380D4 File Offset: 0x000362D4
		private void UpdateVisual()
		{
			Sprite sprite = null;
			this.SpriteWidget.IsVisible = false;
			this.MaskedTextureWidget.IsVisible = false;
			this.ImageIdentifierWidget.IsVisible = false;
			string type = this.Type;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(type);
			if (num <= 1654682144U)
			{
				if (num <= 403518212U)
				{
					if (num != 189982571U)
					{
						if (num != 284088421U)
						{
							if (num != 403518212U)
							{
								goto IL_02C1;
							}
							if (!(type == "fief_barterable"))
							{
								goto IL_02C1;
							}
							sprite = base.EventManager.Context.SpriteData.GetSprite(this.FiefImagePath + "_t");
							this.SpriteWidget.Brush = base.EventManager.Context.DefaultBrush;
							this.SpriteWidget.IsVisible = true;
							goto IL_02CD;
						}
						else
						{
							if (!(type == "lift_siege_barterable"))
							{
								goto IL_02C1;
							}
							goto IL_02C1;
						}
					}
					else if (!(type == "join_faction_barterable"))
					{
						goto IL_02C1;
					}
				}
				else if (num <= 806661062U)
				{
					if (num != 535704800U)
					{
						if (num != 806661062U)
						{
							goto IL_02C1;
						}
						if (!(type == "war_barterable"))
						{
							goto IL_02C1;
						}
						goto IL_02C1;
					}
					else
					{
						if (!(type == "no_attack_barterable"))
						{
							goto IL_02C1;
						}
						goto IL_02C1;
					}
				}
				else if (num != 1289251258U)
				{
					if (num != 1654682144U)
					{
						goto IL_02C1;
					}
					if (!(type == "start_siege_barterable"))
					{
						goto IL_02C1;
					}
					goto IL_02C1;
				}
				else
				{
					if (!(type == "marriage_barterable"))
					{
						goto IL_02C1;
					}
					goto IL_02B3;
				}
			}
			else if (num <= 2639715379U)
			{
				if (num <= 2166136261U)
				{
					if (num != 2080743372U)
					{
						if (num != 2166136261U)
						{
							goto IL_02C1;
						}
						if (type != null && type.Length != 0)
						{
							goto IL_02C1;
						}
						goto IL_02C1;
					}
					else if (!(type == "leave_faction_barterable"))
					{
						goto IL_02C1;
					}
				}
				else if (num != 2342284176U)
				{
					if (num != 2639715379U)
					{
						goto IL_02C1;
					}
					if (!(type == "item_barterable"))
					{
						goto IL_02C1;
					}
					goto IL_02B3;
				}
				else
				{
					if (!(type == "set_prisoner_free_barterable"))
					{
						goto IL_02C1;
					}
					goto IL_02B3;
				}
			}
			else if (num <= 3787227692U)
			{
				if (num != 3249789840U)
				{
					if (num != 3787227692U)
					{
						goto IL_02C1;
					}
					if (!(type == "safe_passage_barterable"))
					{
						goto IL_02C1;
					}
					goto IL_02C1;
				}
				else if (!(type == "mercenary_join_faction_barterable"))
				{
					goto IL_02C1;
				}
			}
			else if (num != 3835993774U)
			{
				if (num != 3957684540U)
				{
					goto IL_02C1;
				}
				if (!(type == "gold_barterable"))
				{
					goto IL_02C1;
				}
				goto IL_02C1;
			}
			else
			{
				if (!(type == "peace_barterable"))
				{
					goto IL_02C1;
				}
				goto IL_02C1;
			}
			this.MaskedTextureWidget.IsVisible = true;
			goto IL_02CD;
			IL_02B3:
			this.ImageIdentifierWidget.IsVisible = true;
			goto IL_02CD;
			IL_02C1:
			this.SpriteWidget.IsVisible = true;
			IL_02CD:
			if (this.SpriteWidget.ContainsState(this.Type))
			{
				this.SpriteWidget.SetState(this.Type);
			}
			if (sprite != null)
			{
				this.SetWidgetSpriteForAllStyles(this.SpriteWidget, sprite);
			}
			this.SpriteClipWidget.IsVisible = this.SpriteWidget.IsVisible;
		}

		// Token: 0x06001499 RID: 5273 RVA: 0x000383F8 File Offset: 0x000365F8
		private void SetWidgetSpriteForAllStyles(BrushWidget widget, Sprite sprite)
		{
			widget.Sprite = sprite;
			foreach (Style style in widget.Brush.Styles)
			{
				StyleLayer[] layers = style.GetLayers();
				for (int i = 0; i < layers.Length; i++)
				{
					layers[i].Sprite = sprite;
				}
			}
		}

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x0600149A RID: 5274 RVA: 0x0003846C File Offset: 0x0003666C
		// (set) Token: 0x0600149B RID: 5275 RVA: 0x00038474 File Offset: 0x00036674
		[Editor(false)]
		public BrushWidget SpriteWidget
		{
			get
			{
				return this._spriteWidget;
			}
			set
			{
				if (this._spriteWidget != value)
				{
					this._spriteWidget = value;
					base.OnPropertyChanged<BrushWidget>(value, "SpriteWidget");
				}
			}
		}

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x0600149C RID: 5276 RVA: 0x00038492 File Offset: 0x00036692
		// (set) Token: 0x0600149D RID: 5277 RVA: 0x0003849A File Offset: 0x0003669A
		[Editor(false)]
		public Widget SpriteClipWidget
		{
			get
			{
				return this._spriteClipWidget;
			}
			set
			{
				if (this._spriteClipWidget != value)
				{
					this._spriteClipWidget = value;
					base.OnPropertyChanged<Widget>(value, "SpriteClipWidget");
				}
			}
		}

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x0600149E RID: 5278 RVA: 0x000384B8 File Offset: 0x000366B8
		// (set) Token: 0x0600149F RID: 5279 RVA: 0x000384C0 File Offset: 0x000366C0
		[Editor(false)]
		public ImageIdentifierWidget ImageIdentifierWidget
		{
			get
			{
				return this._imageIdentifierWidget;
			}
			set
			{
				if (this._imageIdentifierWidget != value)
				{
					this._imageIdentifierWidget = value;
					base.OnPropertyChanged<ImageIdentifierWidget>(value, "ImageIdentifierWidget");
				}
			}
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x060014A0 RID: 5280 RVA: 0x000384DE File Offset: 0x000366DE
		// (set) Token: 0x060014A1 RID: 5281 RVA: 0x000384E6 File Offset: 0x000366E6
		[Editor(false)]
		public MaskedTextureWidget MaskedTextureWidget
		{
			get
			{
				return this._maskedTextureWidget;
			}
			set
			{
				if (this._maskedTextureWidget != value)
				{
					this._maskedTextureWidget = value;
					base.OnPropertyChanged<MaskedTextureWidget>(value, "MaskedTextureWidget");
				}
			}
		}

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x060014A2 RID: 5282 RVA: 0x00038504 File Offset: 0x00036704
		// (set) Token: 0x060014A3 RID: 5283 RVA: 0x0003850C File Offset: 0x0003670C
		[Editor(false)]
		public bool HasVisualIdentifier
		{
			get
			{
				return this._hasVisualIdentifier;
			}
			set
			{
				if (this._hasVisualIdentifier != value)
				{
					this._hasVisualIdentifier = value;
					base.OnPropertyChanged(value, "HasVisualIdentifier");
				}
			}
		}

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x060014A4 RID: 5284 RVA: 0x0003852A File Offset: 0x0003672A
		// (set) Token: 0x060014A5 RID: 5285 RVA: 0x00038532 File Offset: 0x00036732
		[Editor(false)]
		public string Type
		{
			get
			{
				return this._type;
			}
			set
			{
				if (this._type != value)
				{
					this._type = value;
					base.OnPropertyChanged<string>(value, "Type");
				}
			}
		}

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x060014A6 RID: 5286 RVA: 0x00038555 File Offset: 0x00036755
		// (set) Token: 0x060014A7 RID: 5287 RVA: 0x0003855D File Offset: 0x0003675D
		[Editor(false)]
		public string FiefImagePath
		{
			get
			{
				return this._fiefImagePath;
			}
			set
			{
				if (this._fiefImagePath != value)
				{
					this._fiefImagePath = value;
					base.OnPropertyChanged<string>(value, "FiefImagePath");
				}
			}
		}

		// Token: 0x04000956 RID: 2390
		private bool _imageDetermined;

		// Token: 0x04000957 RID: 2391
		private string _type = "";

		// Token: 0x04000958 RID: 2392
		private string _fiefImagePath;

		// Token: 0x04000959 RID: 2393
		private bool _hasVisualIdentifier;

		// Token: 0x0400095A RID: 2394
		private BrushWidget _spriteWidget;

		// Token: 0x0400095B RID: 2395
		private MaskedTextureWidget _maskedTextureWidget;

		// Token: 0x0400095C RID: 2396
		private ImageIdentifierWidget _imageIdentifierWidget;

		// Token: 0x0400095D RID: 2397
		private Widget _spriteClipWidget;
	}
}
