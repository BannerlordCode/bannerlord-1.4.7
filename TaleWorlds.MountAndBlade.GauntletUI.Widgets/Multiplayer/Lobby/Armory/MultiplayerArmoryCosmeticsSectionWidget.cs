using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Armory
{
	// Token: 0x020000B5 RID: 181
	public class MultiplayerArmoryCosmeticsSectionWidget : Widget
	{
		// Token: 0x06000964 RID: 2404 RVA: 0x0001A6E6 File Offset: 0x000188E6
		public MultiplayerArmoryCosmeticsSectionWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0001A6F0 File Offset: 0x000188F0
		private void AnimateTauntAssignmentStates(float dt)
		{
			this._tauntAssignmentStateTimer += dt;
			float num;
			if (this._tauntAssignmentStateTimer < this.TauntAssignmentStateAnimationDuration)
			{
				num = this._tauntAssignmentStateTimer / this.TauntAssignmentStateAnimationDuration;
				base.EventManager.AddLateUpdateAction(this, new Action<float>(this.AnimateTauntAssignmentStates), 1);
			}
			else
			{
				num = 1f;
			}
			float num2 = (this.IsTauntAssignmentActive ? 1f : this.TauntAssignmentStateAlpha);
			float num3 = (this.IsTauntAssignmentActive ? this.TauntAssignmentStateAlpha : 1f);
			float num4 = MathF.Lerp(num2, num3, num, 1E-05f);
			this.SetWidgetAlpha(this.TopSectionParent, num4);
			this.SetWidgetAlpha(this.BottomSectionParent, num4);
			this.SetWidgetAlpha(this.SortControlsParent, num4);
			this.SetWidgetAlpha(this.CategorySeparatorWidget, num4);
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x0001A7B4 File Offset: 0x000189B4
		private void SetWidgetAlpha(Widget widget, float alpha)
		{
			if (widget != null)
			{
				widget.IsVisible = alpha != 0f;
				widget.SetGlobalAlphaRecursively(alpha);
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000967 RID: 2407 RVA: 0x0001A7D1 File Offset: 0x000189D1
		// (set) Token: 0x06000968 RID: 2408 RVA: 0x0001A7DC File Offset: 0x000189DC
		public bool IsTauntAssignmentActive
		{
			get
			{
				return this._isTauntAssignmentActive;
			}
			set
			{
				if (value != this._isTauntAssignmentActive)
				{
					this._isTauntAssignmentActive = value;
					base.OnPropertyChanged(value, "IsTauntAssignmentActive");
					this._tauntAssignmentStateTimer = 0f;
					base.EventManager.AddLateUpdateAction(this, new Action<float>(this.AnimateTauntAssignmentStates), 1);
				}
			}
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000969 RID: 2409 RVA: 0x0001A829 File Offset: 0x00018A29
		// (set) Token: 0x0600096A RID: 2410 RVA: 0x0001A831 File Offset: 0x00018A31
		public float TauntAssignmentStateAnimationDuration
		{
			get
			{
				return this._tauntAssignmentStateAnimationDuration;
			}
			set
			{
				if (value != this._tauntAssignmentStateAnimationDuration)
				{
					this._tauntAssignmentStateAnimationDuration = value;
					base.OnPropertyChanged(value, "TauntAssignmentStateAnimationDuration");
				}
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x0600096B RID: 2411 RVA: 0x0001A84F File Offset: 0x00018A4F
		// (set) Token: 0x0600096C RID: 2412 RVA: 0x0001A857 File Offset: 0x00018A57
		public float TauntAssignmentStateAlpha
		{
			get
			{
				return this._tauntAssignmentStateAlpha;
			}
			set
			{
				if (value != this._tauntAssignmentStateAlpha)
				{
					this._tauntAssignmentStateAlpha = value;
					base.OnPropertyChanged(value, "TauntAssignmentStateAlpha");
				}
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x0600096D RID: 2413 RVA: 0x0001A875 File Offset: 0x00018A75
		// (set) Token: 0x0600096E RID: 2414 RVA: 0x0001A87D File Offset: 0x00018A7D
		public Widget TopSectionParent
		{
			get
			{
				return this._topSectionParent;
			}
			set
			{
				if (value != this._topSectionParent)
				{
					this._topSectionParent = value;
					base.OnPropertyChanged<Widget>(value, "TopSectionParent");
				}
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x0600096F RID: 2415 RVA: 0x0001A89B File Offset: 0x00018A9B
		// (set) Token: 0x06000970 RID: 2416 RVA: 0x0001A8A3 File Offset: 0x00018AA3
		public Widget BottomSectionParent
		{
			get
			{
				return this._bottomSectionParent;
			}
			set
			{
				if (value != this._bottomSectionParent)
				{
					this._bottomSectionParent = value;
					base.OnPropertyChanged<Widget>(value, "BottomSectionParent");
				}
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000971 RID: 2417 RVA: 0x0001A8C1 File Offset: 0x00018AC1
		// (set) Token: 0x06000972 RID: 2418 RVA: 0x0001A8C9 File Offset: 0x00018AC9
		public Widget SortControlsParent
		{
			get
			{
				return this._sortControlsParent;
			}
			set
			{
				if (value != this._sortControlsParent)
				{
					this._sortControlsParent = value;
					base.OnPropertyChanged<Widget>(value, "SortControlsParent");
				}
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000973 RID: 2419 RVA: 0x0001A8E7 File Offset: 0x00018AE7
		// (set) Token: 0x06000974 RID: 2420 RVA: 0x0001A8EF File Offset: 0x00018AEF
		public Widget CategorySeparatorWidget
		{
			get
			{
				return this._categorySeparatorWidget;
			}
			set
			{
				if (value != this._categorySeparatorWidget)
				{
					this._categorySeparatorWidget = value;
					base.OnPropertyChanged<Widget>(value, "CategorySeparatorWidget");
				}
			}
		}

		// Token: 0x0400043D RID: 1085
		private float _tauntAssignmentStateTimer;

		// Token: 0x0400043E RID: 1086
		private bool _isTauntAssignmentActive;

		// Token: 0x0400043F RID: 1087
		private float _tauntAssignmentStateAnimationDuration;

		// Token: 0x04000440 RID: 1088
		private float _tauntAssignmentStateAlpha;

		// Token: 0x04000441 RID: 1089
		private Widget _topSectionParent;

		// Token: 0x04000442 RID: 1090
		private Widget _bottomSectionParent;

		// Token: 0x04000443 RID: 1091
		private Widget _sortControlsParent;

		// Token: 0x04000444 RID: 1092
		private Widget _categorySeparatorWidget;
	}
}
