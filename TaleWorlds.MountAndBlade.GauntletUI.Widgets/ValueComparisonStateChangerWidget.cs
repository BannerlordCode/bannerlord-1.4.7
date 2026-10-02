using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000046 RID: 70
	public class ValueComparisonStateChangerWidget : BrushWidget
	{
		// Token: 0x060003E7 RID: 999 RVA: 0x0000C5D3 File Offset: 0x0000A7D3
		public ValueComparisonStateChangerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000C5DC File Offset: 0x0000A7DC
		private void UpdateState(float dt)
		{
			bool flag = false;
			switch (this.WatchType)
			{
			case ValueComparisonStateChangerWidget.WatchTypes.Equals:
				flag = this.FirstValueFloat == this.SecondValueFloat;
				break;
			case ValueComparisonStateChangerWidget.WatchTypes.NotEquals:
				flag = this.FirstValueFloat != this.SecondValueFloat;
				break;
			case ValueComparisonStateChangerWidget.WatchTypes.GreaterThan:
				flag = this.FirstValueFloat > this.SecondValueFloat;
				break;
			case ValueComparisonStateChangerWidget.WatchTypes.LessThan:
				flag = this.FirstValueFloat < this.SecondValueFloat;
				break;
			}
			(this.TargetWidget ?? this).SetState(flag ? this.TrueState : this.FalseState);
			this._isScheduledForUpdate = false;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000C677 File Offset: 0x0000A877
		private void SetDirty()
		{
			if (!this._isScheduledForUpdate)
			{
				base.EventManager.AddLateUpdateAction(this, new Action<float>(this.UpdateState), 1);
				this._isScheduledForUpdate = true;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x0000C6A1 File Offset: 0x0000A8A1
		// (set) Token: 0x060003EB RID: 1003 RVA: 0x0000C6A9 File Offset: 0x0000A8A9
		public Widget TargetWidget
		{
			get
			{
				return this._targetWidget;
			}
			set
			{
				if (value != this._targetWidget)
				{
					this._targetWidget = value;
					base.OnPropertyChanged<Widget>(value, "TargetWidget");
					this.SetDirty();
				}
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x0000C6CD File Offset: 0x0000A8CD
		// (set) Token: 0x060003ED RID: 1005 RVA: 0x0000C6D5 File Offset: 0x0000A8D5
		public ValueComparisonStateChangerWidget.WatchTypes WatchType
		{
			get
			{
				return this._watchType;
			}
			set
			{
				if (value != this._watchType)
				{
					this._watchType = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x0000C6ED File Offset: 0x0000A8ED
		// (set) Token: 0x060003EF RID: 1007 RVA: 0x0000C6F6 File Offset: 0x0000A8F6
		public int FirstValueInt
		{
			get
			{
				return (int)this._firstValueFloat;
			}
			set
			{
				if (value != (int)this._firstValueFloat)
				{
					this._firstValueFloat = (float)value;
					base.OnPropertyChanged(value, "FirstValueInt");
					this.SetDirty();
				}
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060003F0 RID: 1008 RVA: 0x0000C71C File Offset: 0x0000A91C
		// (set) Token: 0x060003F1 RID: 1009 RVA: 0x0000C725 File Offset: 0x0000A925
		public int SecondValueInt
		{
			get
			{
				return (int)this._secondValueFloat;
			}
			set
			{
				if (value != (int)this._secondValueFloat)
				{
					this._secondValueFloat = (float)value;
					base.OnPropertyChanged(value, "SecondValueInt");
					this.SetDirty();
				}
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x0000C74B File Offset: 0x0000A94B
		// (set) Token: 0x060003F3 RID: 1011 RVA: 0x0000C753 File Offset: 0x0000A953
		public float FirstValueFloat
		{
			get
			{
				return this._firstValueFloat;
			}
			set
			{
				if (value != this._firstValueFloat)
				{
					this._firstValueFloat = value;
					base.OnPropertyChanged(value, "FirstValueFloat");
					this.SetDirty();
				}
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x0000C777 File Offset: 0x0000A977
		// (set) Token: 0x060003F5 RID: 1013 RVA: 0x0000C77F File Offset: 0x0000A97F
		public float SecondValueFloat
		{
			get
			{
				return this._secondValueFloat;
			}
			set
			{
				if (value != this._secondValueFloat)
				{
					this._secondValueFloat = value;
					base.OnPropertyChanged(value, "SecondValueFloat");
					this.SetDirty();
				}
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x0000C7A3 File Offset: 0x0000A9A3
		// (set) Token: 0x060003F7 RID: 1015 RVA: 0x0000C7AB File Offset: 0x0000A9AB
		public string TrueState
		{
			get
			{
				return this._trueState;
			}
			set
			{
				if (value != this._trueState)
				{
					this._trueState = value;
					base.OnPropertyChanged<string>(value, "TrueState");
					this.SetDirty();
				}
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x0000C7D4 File Offset: 0x0000A9D4
		// (set) Token: 0x060003F9 RID: 1017 RVA: 0x0000C7DC File Offset: 0x0000A9DC
		public string FalseState
		{
			get
			{
				return this._falseState;
			}
			set
			{
				if (value != this._falseState)
				{
					this._falseState = value;
					base.OnPropertyChanged<string>(value, "FalseState");
					this.SetDirty();
				}
			}
		}

		// Token: 0x040001A0 RID: 416
		private bool _isScheduledForUpdate;

		// Token: 0x040001A1 RID: 417
		private Widget _targetWidget;

		// Token: 0x040001A2 RID: 418
		private ValueComparisonStateChangerWidget.WatchTypes _watchType;

		// Token: 0x040001A3 RID: 419
		private float _firstValueFloat;

		// Token: 0x040001A4 RID: 420
		private float _secondValueFloat;

		// Token: 0x040001A5 RID: 421
		private string _trueState;

		// Token: 0x040001A6 RID: 422
		private string _falseState;

		// Token: 0x020001A3 RID: 419
		public enum WatchTypes
		{
			// Token: 0x040009B7 RID: 2487
			Equals,
			// Token: 0x040009B8 RID: 2488
			NotEquals,
			// Token: 0x040009B9 RID: 2489
			GreaterThan,
			// Token: 0x040009BA RID: 2490
			LessThan
		}
	}
}
