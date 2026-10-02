using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Kingdom
{
	// Token: 0x02000134 RID: 308
	public class KingdomDecisionOptionWidget : Widget
	{
		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06000FFB RID: 4091 RVA: 0x0002BFD9 File Offset: 0x0002A1D9
		// (set) Token: 0x06000FFC RID: 4092 RVA: 0x0002BFE1 File Offset: 0x0002A1E1
		public Widget SealVisualWidget { get; set; }

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06000FFD RID: 4093 RVA: 0x0002BFEA File Offset: 0x0002A1EA
		// (set) Token: 0x06000FFE RID: 4094 RVA: 0x0002BFF2 File Offset: 0x0002A1F2
		public DecisionSupportStrengthListPanel StrengthWidget { get; set; }

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x06000FFF RID: 4095 RVA: 0x0002BFFB File Offset: 0x0002A1FB
		// (set) Token: 0x06001000 RID: 4096 RVA: 0x0002C003 File Offset: 0x0002A203
		public bool IsPlayerSupporter { get; set; }

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06001001 RID: 4097 RVA: 0x0002C00C File Offset: 0x0002A20C
		// (set) Token: 0x06001002 RID: 4098 RVA: 0x0002C014 File Offset: 0x0002A214
		public bool IsAbstain { get; set; }

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06001003 RID: 4099 RVA: 0x0002C01D File Offset: 0x0002A21D
		// (set) Token: 0x06001004 RID: 4100 RVA: 0x0002C025 File Offset: 0x0002A225
		public float SealStartWidth { get; set; } = 232f;

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06001005 RID: 4101 RVA: 0x0002C02E File Offset: 0x0002A22E
		// (set) Token: 0x06001006 RID: 4102 RVA: 0x0002C036 File Offset: 0x0002A236
		public float SealStartHeight { get; set; } = 232f;

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06001007 RID: 4103 RVA: 0x0002C03F File Offset: 0x0002A23F
		// (set) Token: 0x06001008 RID: 4104 RVA: 0x0002C047 File Offset: 0x0002A247
		public float SealEndWidth { get; set; } = 140f;

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06001009 RID: 4105 RVA: 0x0002C050 File Offset: 0x0002A250
		// (set) Token: 0x0600100A RID: 4106 RVA: 0x0002C058 File Offset: 0x0002A258
		public float SealEndHeight { get; set; } = 140f;

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x0600100B RID: 4107 RVA: 0x0002C061 File Offset: 0x0002A261
		// (set) Token: 0x0600100C RID: 4108 RVA: 0x0002C069 File Offset: 0x0002A269
		public float SealAnimLength { get; set; } = 0.2f;

		// Token: 0x0600100D RID: 4109 RVA: 0x0002C074 File Offset: 0x0002A274
		public KingdomDecisionOptionWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x0002C0CC File Offset: 0x0002A2CC
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			this.StrengthWidget.IsVisible = !this.IsAbstain && this.IsPlayerSupporter && this.IsOptionSelected && !this.IsKingsOption && !this._isKingsDecisionDone;
			if (this._animStartTime != -1f && base.EventManager.Time - this._animStartTime < this.SealAnimLength)
			{
				this.SealVisualWidget.IsVisible = true;
				float num = (base.EventManager.Time - this._animStartTime) / this.SealAnimLength;
				this.SealVisualWidget.SuggestedWidth = Mathf.Lerp(this.SealStartWidth, this.SealEndWidth, num);
				this.SealVisualWidget.SuggestedHeight = Mathf.Lerp(this.SealStartHeight, this.SealEndHeight, num);
				this.SealVisualWidget.SetGlobalAlphaRecursively(Mathf.Lerp(0f, 1f, num));
			}
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x0002C1BC File Offset: 0x0002A3BC
		internal void OnKingsDecisionDone()
		{
			this._isKingsDecisionDone = true;
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x0002C1C5 File Offset: 0x0002A3C5
		internal void OnFinalDone()
		{
			this._isKingsDecisionDone = false;
			this._animStartTime = -1f;
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x0002C1D9 File Offset: 0x0002A3D9
		private void OnSelectionChange(bool value)
		{
			if (!this.IsPlayerSupporter)
			{
				this.SealVisualWidget.IsVisible = value;
				this.SealVisualWidget.SetGlobalAlphaRecursively(0.2f);
				return;
			}
			this.SealVisualWidget.IsVisible = false;
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x0002C20C File Offset: 0x0002A40C
		private void HandleKingsOption()
		{
			this._animStartTime = base.EventManager.Time;
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06001013 RID: 4115 RVA: 0x0002C21F File Offset: 0x0002A41F
		// (set) Token: 0x06001014 RID: 4116 RVA: 0x0002C227 File Offset: 0x0002A427
		[Editor(false)]
		public bool IsOptionSelected
		{
			get
			{
				return this._isOptionSelected;
			}
			set
			{
				if (this._isOptionSelected != value)
				{
					this._isOptionSelected = value;
					base.OnPropertyChanged(value, "IsOptionSelected");
					this.OnSelectionChange(value);
					base.GamepadNavigationIndex = (value ? (-1) : 0);
				}
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06001015 RID: 4117 RVA: 0x0002C259 File Offset: 0x0002A459
		// (set) Token: 0x06001016 RID: 4118 RVA: 0x0002C261 File Offset: 0x0002A461
		[Editor(false)]
		public bool IsKingsOption
		{
			get
			{
				return this._isKingsOption;
			}
			set
			{
				if (this._isKingsOption != value)
				{
					this._isKingsOption = value;
					base.OnPropertyChanged(value, "IsKingsOption");
					this.HandleKingsOption();
				}
			}
		}

		// Token: 0x04000749 RID: 1865
		private float _animStartTime = -1f;

		// Token: 0x0400074A RID: 1866
		private bool _isKingsDecisionDone;

		// Token: 0x0400074B RID: 1867
		private bool _isOptionSelected;

		// Token: 0x0400074C RID: 1868
		public bool _isKingsOption;
	}
}
