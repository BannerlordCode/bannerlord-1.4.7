using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterCreation.Culture
{
	// Token: 0x0200018B RID: 395
	public class CharacterCreationCultureVisualBrushWidget : BrushWidget
	{
		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x06001471 RID: 5233 RVA: 0x00037A5A File Offset: 0x00035C5A
		// (set) Token: 0x06001472 RID: 5234 RVA: 0x00037A62 File Offset: 0x00035C62
		public bool UseSmallVisuals { get; set; } = true;

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x06001473 RID: 5235 RVA: 0x00037A6B File Offset: 0x00035C6B
		// (set) Token: 0x06001474 RID: 5236 RVA: 0x00037A73 File Offset: 0x00035C73
		public ParallaxItemBrushWidget Layer1Widget { get; set; }

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x06001475 RID: 5237 RVA: 0x00037A7C File Offset: 0x00035C7C
		// (set) Token: 0x06001476 RID: 5238 RVA: 0x00037A84 File Offset: 0x00035C84
		public ParallaxItemBrushWidget Layer2Widget { get; set; }

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06001477 RID: 5239 RVA: 0x00037A8D File Offset: 0x00035C8D
		// (set) Token: 0x06001478 RID: 5240 RVA: 0x00037A95 File Offset: 0x00035C95
		public ParallaxItemBrushWidget Layer3Widget { get; set; }

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06001479 RID: 5241 RVA: 0x00037A9E File Offset: 0x00035C9E
		// (set) Token: 0x0600147A RID: 5242 RVA: 0x00037AA6 File Offset: 0x00035CA6
		public ParallaxItemBrushWidget Layer4Widget { get; set; }

		// Token: 0x0600147B RID: 5243 RVA: 0x00037AAF File Offset: 0x00035CAF
		public CharacterCreationCultureVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x00037AC8 File Offset: 0x00035CC8
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._isFirstFrame)
			{
				this._alphaTarget = (float)(string.IsNullOrEmpty(this.CurrentCultureId) ? 0 : 1);
				this.SetGlobalAlphaRecursively(this._alphaTarget);
				ParallaxItemBrushWidget layer1Widget = this.Layer1Widget;
				if (layer1Widget != null)
				{
					layer1Widget.RegisterBrushStatesOfWidget();
				}
				ParallaxItemBrushWidget layer2Widget = this.Layer2Widget;
				if (layer2Widget != null)
				{
					layer2Widget.RegisterBrushStatesOfWidget();
				}
				ParallaxItemBrushWidget layer3Widget = this.Layer3Widget;
				if (layer3Widget != null)
				{
					layer3Widget.RegisterBrushStatesOfWidget();
				}
				ParallaxItemBrushWidget layer4Widget = this.Layer4Widget;
				if (layer4Widget != null)
				{
					layer4Widget.RegisterBrushStatesOfWidget();
				}
				this._isFirstFrame = false;
			}
			this.SetGlobalAlphaRecursively(Mathf.Lerp(base.ReadOnlyBrush.GlobalAlphaFactor, this._alphaTarget, dt * 10f));
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x00037B78 File Offset: 0x00035D78
		private void SetCultureVisual(string newCultureId)
		{
			if (string.IsNullOrEmpty(newCultureId))
			{
				this._alphaTarget = 0f;
				return;
			}
			if (this.UseSmallVisuals)
			{
				Sprite sprite = base.Context.SpriteData.GetSprite("CharacterCreation\\Culture\\" + newCultureId);
				if (sprite == null)
				{
					sprite = base.Context.SpriteData.GetSprite("CharacterCreation\\Culture\\blank_culture");
				}
				using (Dictionary<string, Style>.ValueCollection.Enumerator enumerator = base.Brush.Styles.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Style style = enumerator.Current;
						StyleLayer[] layers = style.GetLayers();
						for (int i = 0; i < layers.Length; i++)
						{
							layers[i].Sprite = sprite;
						}
					}
					goto IL_00EC;
				}
			}
			ParallaxItemBrushWidget layer1Widget = this.Layer1Widget;
			if (layer1Widget != null)
			{
				layer1Widget.SetState(newCultureId);
			}
			ParallaxItemBrushWidget layer2Widget = this.Layer2Widget;
			if (layer2Widget != null)
			{
				layer2Widget.SetState(newCultureId);
			}
			ParallaxItemBrushWidget layer3Widget = this.Layer3Widget;
			if (layer3Widget != null)
			{
				layer3Widget.SetState(newCultureId);
			}
			ParallaxItemBrushWidget layer4Widget = this.Layer4Widget;
			if (layer4Widget != null)
			{
				layer4Widget.SetState(newCultureId);
			}
			IL_00EC:
			this._alphaTarget = 1f;
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x0600147E RID: 5246 RVA: 0x00037C8C File Offset: 0x00035E8C
		// (set) Token: 0x0600147F RID: 5247 RVA: 0x00037C94 File Offset: 0x00035E94
		[Editor(false)]
		public string CurrentCultureId
		{
			get
			{
				return this._currentCultureId;
			}
			set
			{
				if (this._currentCultureId != value)
				{
					this._currentCultureId = value;
					base.OnPropertyChanged<string>(value, "CurrentCultureId");
					this.SetCultureVisual(value);
					this.SetGlobalAlphaRecursively(1f);
				}
			}
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06001480 RID: 5248 RVA: 0x00037CC9 File Offset: 0x00035EC9
		// (set) Token: 0x06001481 RID: 5249 RVA: 0x00037CD1 File Offset: 0x00035ED1
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
					this._isBig = value;
					base.OnPropertyChanged(value, "IsBig");
				}
			}
		}

		// Token: 0x04000949 RID: 2377
		private float _alphaTarget;

		// Token: 0x0400094A RID: 2378
		private bool _isFirstFrame = true;

		// Token: 0x0400094B RID: 2379
		private string _currentCultureId;

		// Token: 0x0400094C RID: 2380
		private bool _isBig;
	}
}
