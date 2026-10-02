using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer
{
	// Token: 0x0200008E RID: 142
	public class MultiplayerPollProgressionWidget : Widget
	{
		// Token: 0x060007C2 RID: 1986 RVA: 0x000167D8 File Offset: 0x000149D8
		public MultiplayerPollProgressionWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x000167E1 File Offset: 0x000149E1
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x060007C4 RID: 1988 RVA: 0x000167EA File Offset: 0x000149EA
		// (set) Token: 0x060007C5 RID: 1989 RVA: 0x000167F2 File Offset: 0x000149F2
		public bool HasOngoingPoll
		{
			get
			{
				return this._hasOngoingPoll;
			}
			set
			{
				if (value != this._hasOngoingPoll)
				{
					this._hasOngoingPoll = value;
					base.OnPropertyChanged(value, "HasOngoingPoll");
					ListPanel pollExtension = this.PollExtension;
					if (pollExtension == null)
					{
						return;
					}
					pollExtension.SetState(value ? "Active" : "Inactive");
				}
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x060007C6 RID: 1990 RVA: 0x0001682F File Offset: 0x00014A2F
		// (set) Token: 0x060007C7 RID: 1991 RVA: 0x00016837 File Offset: 0x00014A37
		[Editor(false)]
		public ListPanel PollExtension
		{
			get
			{
				return this._pollExtension;
			}
			set
			{
				if (value != this._pollExtension)
				{
					this._pollExtension = value;
					base.OnPropertyChanged<ListPanel>(value, "PollExtension");
					this._pollExtension.SetState("Inactive");
				}
			}
		}

		// Token: 0x04000368 RID: 872
		private const string _activeState = "Active";

		// Token: 0x04000369 RID: 873
		private const string _inactiveState = "Inactive";

		// Token: 0x0400036A RID: 874
		private bool _hasOngoingPoll;

		// Token: 0x0400036B RID: 875
		private ListPanel _pollExtension;
	}
}
