using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.OrderOfBattle
{
	// Token: 0x020000EB RID: 235
	public class OrderOfBattleFormationClassLockBrushWidget : BrushWidget
	{
		// Token: 0x06000C12 RID: 3090 RVA: 0x0002125C File Offset: 0x0001F45C
		public OrderOfBattleFormationClassLockBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x00021265 File Offset: 0x0001F465
		private void OnLockStateSet()
		{
			if (this.IsLocked)
			{
				base.Brush = this.LockedBrush;
			}
			else
			{
				base.Brush = this.UnlockedBrush;
			}
			this._isInitialStateSet = true;
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06000C14 RID: 3092 RVA: 0x00021290 File Offset: 0x0001F490
		// (set) Token: 0x06000C15 RID: 3093 RVA: 0x00021298 File Offset: 0x0001F498
		[Editor(false)]
		public bool IsLocked
		{
			get
			{
				return this._isLocked;
			}
			set
			{
				if (value != this._isLocked || !this._isInitialStateSet)
				{
					this._isLocked = value;
					base.OnPropertyChanged(value, "IsLocked");
					this.OnLockStateSet();
				}
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06000C16 RID: 3094 RVA: 0x000212C4 File Offset: 0x0001F4C4
		// (set) Token: 0x06000C17 RID: 3095 RVA: 0x000212CC File Offset: 0x0001F4CC
		[Editor(false)]
		public Brush LockedBrush
		{
			get
			{
				return this._lockedBrush;
			}
			set
			{
				if (value != this._lockedBrush)
				{
					this._lockedBrush = value;
					base.OnPropertyChanged<Brush>(value, "LockedBrush");
				}
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06000C18 RID: 3096 RVA: 0x000212EA File Offset: 0x0001F4EA
		// (set) Token: 0x06000C19 RID: 3097 RVA: 0x000212F2 File Offset: 0x0001F4F2
		[Editor(false)]
		public Brush UnlockedBrush
		{
			get
			{
				return this._unlockedBrush;
			}
			set
			{
				if (value != this._unlockedBrush)
				{
					this._unlockedBrush = value;
					base.OnPropertyChanged<Brush>(value, "UnlockedBrush");
				}
			}
		}

		// Token: 0x04000572 RID: 1394
		private bool _isInitialStateSet;

		// Token: 0x04000573 RID: 1395
		private bool _isLocked;

		// Token: 0x04000574 RID: 1396
		private Brush _lockedBrush;

		// Token: 0x04000575 RID: 1397
		private Brush _unlockedBrush;
	}
}
