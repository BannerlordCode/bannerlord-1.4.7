using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends
{
	// Token: 0x02000053 RID: 83
	public class MPLobbyFriendItemVM : MPLobbyPlayerBaseVM
	{
		// Token: 0x0600072D RID: 1837 RVA: 0x00016D62 File Offset: 0x00014F62
		public MPLobbyFriendItemVM(PlayerId ID, Action<MPLobbyPlayerBaseVM> onActivatePlayerActions, Action<PlayerId> onInviteToClan = null, Action<PlayerId> onFriendRequestAnswered = null)
			: base(ID, string.Empty, onInviteToClan, onFriendRequestAnswered)
		{
			this._onActivatePlayerActions = onActivatePlayerActions;
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00016D7A File Offset: 0x00014F7A
		private void ExecuteActivatePlayerActions()
		{
			this._onActivatePlayerActions(this);
		}

		// Token: 0x04000357 RID: 855
		private Action<MPLobbyPlayerBaseVM> _onActivatePlayerActions;
	}
}
