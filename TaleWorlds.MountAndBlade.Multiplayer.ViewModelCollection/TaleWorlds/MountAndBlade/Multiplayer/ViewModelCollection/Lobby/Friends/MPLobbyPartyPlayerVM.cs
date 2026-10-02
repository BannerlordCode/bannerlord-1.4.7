using System;
using TaleWorlds.Library;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends
{
	// Token: 0x02000057 RID: 87
	public class MPLobbyPartyPlayerVM : MPLobbyPlayerBaseVM
	{
		// Token: 0x060007C6 RID: 1990 RVA: 0x00019401 File Offset: 0x00017601
		public MPLobbyPartyPlayerVM(PlayerId id, Action<MPLobbyPartyPlayerVM> onActivatePlayerActions)
			: base(id, "", null, null)
		{
			this._onActivatePlayerActions = onActivatePlayerActions;
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x00019418 File Offset: 0x00017618
		private void ExecuteActivatePlayerActions()
		{
			this._onActivatePlayerActions(this);
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x060007C8 RID: 1992 RVA: 0x00019426 File Offset: 0x00017626
		// (set) Token: 0x060007C9 RID: 1993 RVA: 0x0001942E File Offset: 0x0001762E
		[DataSourceProperty]
		public bool IsWaitingConfirmation
		{
			get
			{
				return this._isWaitingConfirmation;
			}
			set
			{
				if (value != this._isWaitingConfirmation)
				{
					this._isWaitingConfirmation = value;
					base.OnPropertyChangedWithValue(value, "IsWaitingConfirmation");
				}
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x060007CA RID: 1994 RVA: 0x0001944C File Offset: 0x0001764C
		// (set) Token: 0x060007CB RID: 1995 RVA: 0x00019454 File Offset: 0x00017654
		[DataSourceProperty]
		public bool IsPartyLeader
		{
			get
			{
				return this._isPartyLeader;
			}
			set
			{
				if (value != this._isPartyLeader)
				{
					this._isPartyLeader = value;
					base.OnPropertyChangedWithValue(value, "IsPartyLeader");
				}
			}
		}

		// Token: 0x04000391 RID: 913
		private Action<MPLobbyPartyPlayerVM> _onActivatePlayerActions;

		// Token: 0x04000392 RID: 914
		private bool _isWaitingConfirmation;

		// Token: 0x04000393 RID: 915
		private bool _isPartyLeader;
	}
}
