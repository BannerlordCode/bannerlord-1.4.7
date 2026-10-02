using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.Admin;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.AdminPanel
{
	// Token: 0x020000AD RID: 173
	public class MultiplayerAdminPanelNumericOptionVM : MultiplayerAdminPanelOptionBaseVM
	{
		// Token: 0x06001063 RID: 4195 RVA: 0x00033208 File Offset: 0x00031408
		public MultiplayerAdminPanelNumericOptionVM(IAdminPanelNumericOption option)
			: base(option)
		{
			this._option = option;
			this._minValue = this._option.GetMinimumValue();
			this._maxValue = this._option.GetMaximumValue();
			this.IntValue = this._option.GetValue();
			this.IsNumericOption = true;
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x0003325D File Offset: 0x0003145D
		public override void UpdateValues()
		{
			base.UpdateValues();
			this.IntValue = this._option.GetValue();
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x00033278 File Offset: 0x00031478
		private int GetClampedInt(int value)
		{
			if (this._minValue != null)
			{
				value = MathF.Max(value, this._minValue.Value);
			}
			if (this._maxValue != null)
			{
				value = MathF.Min(value, this._maxValue.Value);
			}
			return value;
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06001066 RID: 4198 RVA: 0x000332C6 File Offset: 0x000314C6
		// (set) Token: 0x06001067 RID: 4199 RVA: 0x000332CE File Offset: 0x000314CE
		[DataSourceProperty]
		public bool IsNumericOption
		{
			get
			{
				return this._isNumericOption;
			}
			set
			{
				if (value != this._isNumericOption)
				{
					this._isNumericOption = value;
					base.OnPropertyChangedWithValue(value, "IsNumericOption");
				}
			}
		}

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06001068 RID: 4200 RVA: 0x000332EC File Offset: 0x000314EC
		// (set) Token: 0x06001069 RID: 4201 RVA: 0x000332F4 File Offset: 0x000314F4
		[DataSourceProperty]
		public int IntValue
		{
			get
			{
				return this._intValue;
			}
			set
			{
				if (value != this._intValue)
				{
					value = this.GetClampedInt(value);
					this._intValue = value;
					base.OnPropertyChangedWithValue(value, "IntValue");
					this._option.SetValue(value);
				}
			}
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x0600106A RID: 4202 RVA: 0x00033327 File Offset: 0x00031527
		// (set) Token: 0x0600106B RID: 4203 RVA: 0x00033348 File Offset: 0x00031548
		public int MinValueInt
		{
			get
			{
				if (this._minValue == null)
				{
					return int.MinValue;
				}
				return this._minValue.Value;
			}
			set
			{
				int? minValue = this._minValue;
				if (!((value == minValue.GetValueOrDefault()) & (minValue != null)))
				{
					this._minValue = new int?(value);
					base.OnPropertyChangedWithValue(value, "MinValueInt");
				}
			}
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x0600106C RID: 4204 RVA: 0x00033388 File Offset: 0x00031588
		// (set) Token: 0x0600106D RID: 4205 RVA: 0x000333A8 File Offset: 0x000315A8
		public int MaxValueInt
		{
			get
			{
				if (this._maxValue == null)
				{
					return int.MaxValue;
				}
				return this._maxValue.Value;
			}
			set
			{
				int? maxValue = this._maxValue;
				if (!((value == maxValue.GetValueOrDefault()) & (maxValue != null)))
				{
					this._maxValue = new int?(value);
					base.OnPropertyChangedWithValue(value, "MaxValueInt");
				}
			}
		}

		// Token: 0x040007A2 RID: 1954
		private int? _minValue;

		// Token: 0x040007A3 RID: 1955
		private int? _maxValue;

		// Token: 0x040007A4 RID: 1956
		private new readonly IAdminPanelNumericOption _option;

		// Token: 0x040007A5 RID: 1957
		private bool _isNumericOption;

		// Token: 0x040007A6 RID: 1958
		private int _intValue;
	}
}
