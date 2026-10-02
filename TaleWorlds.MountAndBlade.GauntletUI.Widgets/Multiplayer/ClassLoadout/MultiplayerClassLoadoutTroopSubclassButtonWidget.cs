using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.ClassLoadout
{
	// Token: 0x020000CF RID: 207
	public class MultiplayerClassLoadoutTroopSubclassButtonWidget : ButtonWidget
	{
		// Token: 0x06000AB8 RID: 2744 RVA: 0x0001E035 File Offset: 0x0001C235
		public MultiplayerClassLoadoutTroopSubclassButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x0001E040 File Offset: 0x0001C240
		private void UpdateIcon()
		{
			if (string.IsNullOrEmpty(this.TroopType) || this._iconWidget == null)
			{
				return;
			}
			Brush iconBrush = this.IconBrush;
			Sprite sprite;
			if (iconBrush == null)
			{
				sprite = null;
			}
			else
			{
				BrushLayer layer = iconBrush.GetLayer(this.TroopType);
				sprite = ((layer != null) ? layer.Sprite : null);
			}
			Sprite sprite2 = sprite;
			foreach (Style style in this.IconWidget.Brush.Styles)
			{
				StyleLayer[] layers = style.GetLayers();
				for (int i = 0; i < layers.Length; i++)
				{
					layers[i].Sprite = sprite2;
				}
			}
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x0001E0F0 File Offset: 0x0001C2F0
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			Widget parentWidget = base.ParentWidget;
			if (parentWidget == null)
			{
				return;
			}
			parentWidget.SetState(base.CurrentState);
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x0001E10F File Offset: 0x0001C30F
		public override void SetState(string stateName)
		{
			base.SetState(stateName);
			if (this.PerksNavigationScopeTargeter != null)
			{
				this.PerksNavigationScopeTargeter.IsScopeEnabled = stateName == "Selected";
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06000ABC RID: 2748 RVA: 0x0001E136 File Offset: 0x0001C336
		// (set) Token: 0x06000ABD RID: 2749 RVA: 0x0001E13E File Offset: 0x0001C33E
		[DataSourceProperty]
		public string TroopType
		{
			get
			{
				return this._troopType;
			}
			set
			{
				if (value != this._troopType)
				{
					this._troopType = value;
					base.OnPropertyChanged<string>(value, "TroopType");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06000ABE RID: 2750 RVA: 0x0001E167 File Offset: 0x0001C367
		// (set) Token: 0x06000ABF RID: 2751 RVA: 0x0001E16F File Offset: 0x0001C36F
		[DataSourceProperty]
		public Brush IconBrush
		{
			get
			{
				return this._iconBrush;
			}
			set
			{
				if (value != this._iconBrush)
				{
					this._iconBrush = value;
					base.OnPropertyChanged<Brush>(value, "IconBrush");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06000AC0 RID: 2752 RVA: 0x0001E193 File Offset: 0x0001C393
		// (set) Token: 0x06000AC1 RID: 2753 RVA: 0x0001E19B File Offset: 0x0001C39B
		[DataSourceProperty]
		public BrushWidget IconWidget
		{
			get
			{
				return this._iconWidget;
			}
			set
			{
				if (value != this._iconWidget)
				{
					this._iconWidget = value;
					base.OnPropertyChanged<BrushWidget>(value, "IconWidget");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06000AC2 RID: 2754 RVA: 0x0001E1BF File Offset: 0x0001C3BF
		// (set) Token: 0x06000AC3 RID: 2755 RVA: 0x0001E1C7 File Offset: 0x0001C3C7
		public NavigationScopeTargeter PerksNavigationScopeTargeter
		{
			get
			{
				return this._perksNavigationScopeTargeter;
			}
			set
			{
				if (value != this._perksNavigationScopeTargeter)
				{
					this._perksNavigationScopeTargeter = value;
					base.OnPropertyChanged<NavigationScopeTargeter>(value, "PerksNavigationScopeTargeter");
					if (this._perksNavigationScopeTargeter != null)
					{
						this._perksNavigationScopeTargeter.IsScopeEnabled = false;
					}
				}
			}
		}

		// Token: 0x040004E1 RID: 1249
		private string _troopType;

		// Token: 0x040004E2 RID: 1250
		private Brush _iconBrush;

		// Token: 0x040004E3 RID: 1251
		private BrushWidget _iconWidget;

		// Token: 0x040004E4 RID: 1252
		private NavigationScopeTargeter _perksNavigationScopeTargeter;
	}
}
