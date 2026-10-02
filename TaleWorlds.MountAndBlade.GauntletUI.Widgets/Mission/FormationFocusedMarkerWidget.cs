using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000DE RID: 222
	public class FormationFocusedMarkerWidget : BrushWidget
	{
		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06000B5F RID: 2911 RVA: 0x0001FAB5 File Offset: 0x0001DCB5
		// (set) Token: 0x06000B60 RID: 2912 RVA: 0x0001FABD File Offset: 0x0001DCBD
		public int NormalSize { get; set; } = 55;

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06000B61 RID: 2913 RVA: 0x0001FAC6 File Offset: 0x0001DCC6
		// (set) Token: 0x06000B62 RID: 2914 RVA: 0x0001FACE File Offset: 0x0001DCCE
		public int FocusedSize { get; set; } = 60;

		// Token: 0x06000B63 RID: 2915 RVA: 0x0001FAD7 File Offset: 0x0001DCD7
		public FormationFocusedMarkerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x0001FAF0 File Offset: 0x0001DCF0
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			this.UpdateVisibility();
			if (base.IsVisible)
			{
				this.UpdateSize();
			}
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x0001FB0D File Offset: 0x0001DD0D
		private void UpdateVisibility()
		{
			base.IsVisible = this.IsTargetingAFormation || (this.IsFormationTargetRelevant && this.IsCenterOfFocus);
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x0001FB34 File Offset: 0x0001DD34
		private void UpdateSize()
		{
			float num4;
			if (this.IsCenterOfFocus)
			{
				float num = (float)(this.IsTargetingAFormation ? (this.FocusedSize + 3) : this.FocusedSize);
				float num2 = MathF.Sin(base.EventManager.Time * 5f);
				num2 = (num2 + 1f) / 2f;
				float num3 = (num - (float)this.NormalSize) * num2;
				num4 = (float)this.NormalSize + num3;
			}
			else
			{
				num4 = (float)this.NormalSize;
			}
			base.ScaledSuggestedHeight = num4 * base._scaleToUse;
			base.ScaledSuggestedWidth = num4 * base._scaleToUse;
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x0001FBC1 File Offset: 0x0001DDC1
		private void UpdateState()
		{
			this.SetState(this.IsTargetingAFormation ? "Targeting" : "Default");
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06000B68 RID: 2920 RVA: 0x0001FBDD File Offset: 0x0001DDDD
		// (set) Token: 0x06000B69 RID: 2921 RVA: 0x0001FBE5 File Offset: 0x0001DDE5
		public bool IsCenterOfFocus
		{
			get
			{
				return this._isCenterOfFocus;
			}
			set
			{
				if (this._isCenterOfFocus != value)
				{
					this._isCenterOfFocus = value;
					base.OnPropertyChanged(value, "IsCenterOfFocus");
				}
			}
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06000B6A RID: 2922 RVA: 0x0001FC03 File Offset: 0x0001DE03
		// (set) Token: 0x06000B6B RID: 2923 RVA: 0x0001FC0B File Offset: 0x0001DE0B
		public bool IsFormationTargetRelevant
		{
			get
			{
				return this._isFormationTargetRelevant;
			}
			set
			{
				if (this._isFormationTargetRelevant != value)
				{
					this._isFormationTargetRelevant = value;
					base.OnPropertyChanged(value, "IsFormationTargetRelevant");
				}
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06000B6C RID: 2924 RVA: 0x0001FC29 File Offset: 0x0001DE29
		// (set) Token: 0x06000B6D RID: 2925 RVA: 0x0001FC31 File Offset: 0x0001DE31
		public bool IsTargetingAFormation
		{
			get
			{
				return this._isTargetingAFormation;
			}
			set
			{
				if (this._isTargetingAFormation != value)
				{
					this._isTargetingAFormation = value;
					base.OnPropertyChanged(value, "IsTargetingAFormation");
					this.UpdateState();
				}
			}
		}

		// Token: 0x04000524 RID: 1316
		private bool _isCenterOfFocus;

		// Token: 0x04000525 RID: 1317
		private bool _isFormationTargetRelevant;

		// Token: 0x04000526 RID: 1318
		private bool _isTargetingAFormation;
	}
}
