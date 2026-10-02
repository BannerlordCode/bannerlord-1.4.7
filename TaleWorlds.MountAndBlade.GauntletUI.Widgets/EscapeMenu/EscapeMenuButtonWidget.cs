using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.EscapeMenu
{
	// Token: 0x02000155 RID: 341
	public class EscapeMenuButtonWidget : ButtonWidget
	{
		// Token: 0x06001239 RID: 4665 RVA: 0x000324F3 File Offset: 0x000306F3
		public EscapeMenuButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x000324FC File Offset: 0x000306FC
		private void PositiveBehavioredStateUpdated()
		{
			if (this.IsPositiveBehaviored)
			{
				base.Brush = this.PositiveBehaviorBrush;
			}
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x0600123B RID: 4667 RVA: 0x00032512 File Offset: 0x00030712
		// (set) Token: 0x0600123C RID: 4668 RVA: 0x0003251A File Offset: 0x0003071A
		[Editor(false)]
		public bool IsPositiveBehaviored
		{
			get
			{
				return this._isPositiveBehaviored;
			}
			set
			{
				if (this._isPositiveBehaviored != value)
				{
					this._isPositiveBehaviored = value;
					base.OnPropertyChanged(value, "IsPositiveBehaviored");
					this.PositiveBehavioredStateUpdated();
				}
			}
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x0600123D RID: 4669 RVA: 0x0003253E File Offset: 0x0003073E
		// (set) Token: 0x0600123E RID: 4670 RVA: 0x00032546 File Offset: 0x00030746
		[Editor(false)]
		public Brush PositiveBehaviorBrush
		{
			get
			{
				return this._positiveBehaviorBrush;
			}
			set
			{
				if (this._positiveBehaviorBrush != value)
				{
					this._positiveBehaviorBrush = value;
					base.OnPropertyChanged<Brush>(value, "PositiveBehaviorBrush");
				}
			}
		}

		// Token: 0x0400084D RID: 2125
		private bool _isPositiveBehaviored;

		// Token: 0x0400084E RID: 2126
		private Brush _positiveBehaviorBrush;
	}
}
