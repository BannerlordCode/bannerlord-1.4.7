using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.FaceGenerator
{
	// Token: 0x0200007D RID: 125
	public class FaceGenPropertyVM : ViewModel
	{
		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x060009CE RID: 2510 RVA: 0x000221B2 File Offset: 0x000203B2
		public int KeyTimePoint { get; }

		// Token: 0x060009CF RID: 2511 RVA: 0x000221BC File Offset: 0x000203BC
		public FaceGenPropertyVM(int keyNo, double min, double max, TextObject name, int keyTimePoint, int tabId, double value, float initialValue, Action<int, float, bool, bool> updateFace, Action addCommand, Action resetSliderPrevValuesCommand, bool isEnabled = true, bool isDiscrete = false, bool addCommandOnValueChange = true)
		{
			this._calledFromInit = true;
			this._updateFace = updateFace;
			this._addCommand = addCommand;
			this._nameObj = name;
			this._resetSliderPrevValuesCommand = resetSliderPrevValuesCommand;
			this.KeyNo = keyNo;
			this.Min = (float)min;
			this.Max = (float)max;
			this.KeyTimePoint = keyTimePoint;
			this.TabID = tabId;
			this._initialValue = initialValue;
			this.Value = (float)value;
			this.PrevValue = -1.0;
			this.IsEnabled = isEnabled;
			this.IsDiscrete = isDiscrete;
			this._calledFromInit = false;
			this._addCommandOnValueChange = addCommandOnValueChange;
			this.RefreshValues();
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x0002227F File Offset: 0x0002047F
		public void Reset()
		{
			this._updateOnValueChange = false;
			this.Value = this._initialValue;
			this._updateOnValueChange = true;
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x0002229C File Offset: 0x0002049C
		public void Randomize()
		{
			this._updateOnValueChange = false;
			float num = 0.5f * (MBRandom.RandomFloat + MBRandom.RandomFloat);
			this.Value = num * (this.Max - this.Min) + this.Min;
			this._updateOnValueChange = true;
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x000222E5 File Offset: 0x000204E5
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this._nameObj.ToString();
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x000222FE File Offset: 0x000204FE
		public void AddCommand()
		{
			this._addCommand();
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x060009D4 RID: 2516 RVA: 0x0002230B File Offset: 0x0002050B
		// (set) Token: 0x060009D5 RID: 2517 RVA: 0x00022313 File Offset: 0x00020513
		[DataSourceProperty]
		public float Min
		{
			get
			{
				return this._min;
			}
			set
			{
				if (value != this._min)
				{
					this._min = value;
					base.OnPropertyChangedWithValue(value, "Min");
				}
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x060009D6 RID: 2518 RVA: 0x00022331 File Offset: 0x00020531
		// (set) Token: 0x060009D7 RID: 2519 RVA: 0x00022339 File Offset: 0x00020539
		[DataSourceProperty]
		public int TabID
		{
			get
			{
				return this._tabID;
			}
			set
			{
				if (value != this._tabID)
				{
					this._tabID = value;
					base.OnPropertyChangedWithValue(value, "TabID");
				}
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x060009D8 RID: 2520 RVA: 0x00022357 File Offset: 0x00020557
		// (set) Token: 0x060009D9 RID: 2521 RVA: 0x0002235F File Offset: 0x0002055F
		[DataSourceProperty]
		public float Max
		{
			get
			{
				return this._max;
			}
			set
			{
				if (value != this._max)
				{
					this._max = value;
					base.OnPropertyChangedWithValue(value, "Max");
				}
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x060009DA RID: 2522 RVA: 0x0002237D File Offset: 0x0002057D
		// (set) Token: 0x060009DB RID: 2523 RVA: 0x00022388 File Offset: 0x00020588
		[DataSourceProperty]
		public float Value
		{
			get
			{
				return this._value;
			}
			set
			{
				if (MathF.Abs(value - this._value) > 0.01f)
				{
					if (!this._calledFromInit && this.PrevValue < 0.0 && this._updateOnValueChange && this._addCommandOnValueChange)
					{
						this._addCommand();
					}
					this._resetSliderPrevValuesCommand();
					if (this.KeyNo >= 0)
					{
						this.PrevValue = (double)this._value;
					}
					this._value = value;
					base.OnPropertyChangedWithValue(value, "Value");
					Action<int, float, bool, bool> updateFace = this._updateFace;
					if (updateFace == null)
					{
						return;
					}
					updateFace(this.KeyNo, value, this._calledFromInit, this._updateOnValueChange);
				}
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x060009DC RID: 2524 RVA: 0x00022437 File Offset: 0x00020637
		// (set) Token: 0x060009DD RID: 2525 RVA: 0x0002243F File Offset: 0x0002063F
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x060009DE RID: 2526 RVA: 0x00022462 File Offset: 0x00020662
		// (set) Token: 0x060009DF RID: 2527 RVA: 0x0002246A File Offset: 0x0002066A
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x060009E0 RID: 2528 RVA: 0x00022488 File Offset: 0x00020688
		// (set) Token: 0x060009E1 RID: 2529 RVA: 0x00022490 File Offset: 0x00020690
		[DataSourceProperty]
		public bool IsDiscrete
		{
			get
			{
				return this._isDiscrete;
			}
			set
			{
				if (value != this._isDiscrete)
				{
					this._isDiscrete = value;
					base.OnPropertyChangedWithValue(value, "IsDiscrete");
				}
			}
		}

		// Token: 0x04000456 RID: 1110
		public int KeyNo;

		// Token: 0x04000457 RID: 1111
		public double PrevValue = -1.0;

		// Token: 0x04000458 RID: 1112
		private bool _updateOnValueChange = true;

		// Token: 0x04000459 RID: 1113
		private readonly TextObject _nameObj;

		// Token: 0x0400045A RID: 1114
		private readonly Action<int, float, bool, bool> _updateFace;

		// Token: 0x0400045B RID: 1115
		private readonly Action _resetSliderPrevValuesCommand;

		// Token: 0x0400045C RID: 1116
		private readonly Action _addCommand;

		// Token: 0x0400045D RID: 1117
		private readonly bool _addCommandOnValueChange;

		// Token: 0x0400045E RID: 1118
		private readonly bool _calledFromInit;

		// Token: 0x0400045F RID: 1119
		private readonly float _initialValue;

		// Token: 0x04000460 RID: 1120
		private int _tabID = -1;

		// Token: 0x04000461 RID: 1121
		private string _name;

		// Token: 0x04000462 RID: 1122
		private float _value;

		// Token: 0x04000463 RID: 1123
		private float _max;

		// Token: 0x04000464 RID: 1124
		private float _min;

		// Token: 0x04000465 RID: 1125
		private bool _isEnabled;

		// Token: 0x04000466 RID: 1126
		private bool _isDiscrete;
	}
}
