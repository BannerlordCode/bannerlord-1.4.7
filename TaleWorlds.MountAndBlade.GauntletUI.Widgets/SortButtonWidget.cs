using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000042 RID: 66
	public class SortButtonWidget : ButtonWidget
	{
		// Token: 0x060003C5 RID: 965 RVA: 0x0000C024 File Offset: 0x0000A224
		public SortButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000C030 File Offset: 0x0000A230
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.SortVisualWidget != null)
			{
				if (base.IsSelected)
				{
					switch (this.SortState)
					{
					case 0:
						this.SortVisualWidget.SetState("Default");
						return;
					case 1:
						this.SortVisualWidget.SetState("Ascending");
						return;
					case 2:
						this.SortVisualWidget.SetState("Descending");
						return;
					default:
						return;
					}
				}
				else
				{
					this.SortVisualWidget.SetState("Default");
				}
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060003C7 RID: 967 RVA: 0x0000C0B1 File Offset: 0x0000A2B1
		// (set) Token: 0x060003C8 RID: 968 RVA: 0x0000C0B9 File Offset: 0x0000A2B9
		[Editor(false)]
		public int SortState
		{
			get
			{
				return this._sortState;
			}
			set
			{
				if (this._sortState != value)
				{
					this._sortState = value;
					base.OnPropertyChanged(value, "SortState");
				}
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x0000C0D7 File Offset: 0x0000A2D7
		// (set) Token: 0x060003CA RID: 970 RVA: 0x0000C0DF File Offset: 0x0000A2DF
		[Editor(false)]
		public BrushWidget SortVisualWidget
		{
			get
			{
				return this._sortVisualWidget;
			}
			set
			{
				if (this._sortVisualWidget != value)
				{
					this._sortVisualWidget = value;
					base.OnPropertyChanged<BrushWidget>(value, "SortVisualWidget");
				}
			}
		}

		// Token: 0x04000196 RID: 406
		private int _sortState;

		// Token: 0x04000197 RID: 407
		private BrushWidget _sortVisualWidget;
	}
}
