using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Tournament
{
	// Token: 0x02000051 RID: 81
	public class TournamentMatchWidget : Widget
	{
		// Token: 0x0600046F RID: 1135 RVA: 0x0000E2DA File Offset: 0x0000C4DA
		public TournamentMatchWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000470 RID: 1136 RVA: 0x0000E2E3 File Offset: 0x0000C4E3
		// (set) Token: 0x06000471 RID: 1137 RVA: 0x0000E2EC File Offset: 0x0000C4EC
		[Editor(false)]
		public int State
		{
			get
			{
				return this._state;
			}
			set
			{
				if (this._state != value)
				{
					this._state = value;
					List<Widget> allChildrenRecursive = base.GetAllChildrenRecursive(null);
					for (int i = 0; i < allChildrenRecursive.Count; i++)
					{
						TournamentParticipantBrushWidget tournamentParticipantBrushWidget;
						if ((tournamentParticipantBrushWidget = allChildrenRecursive[i] as TournamentParticipantBrushWidget) != null)
						{
							tournamentParticipantBrushWidget.MatchState = this.State;
						}
					}
				}
			}
		}

		// Token: 0x040001E5 RID: 485
		private int _state;
	}
}
