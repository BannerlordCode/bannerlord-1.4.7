using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Friend
{
	// Token: 0x020000B1 RID: 177
	public class MultiplayerLobbyFriendGroupWidget : Widget
	{
		// Token: 0x06000944 RID: 2372 RVA: 0x0001A276 File Offset: 0x00018476
		public MultiplayerLobbyFriendGroupWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x0001A27F File Offset: 0x0001847F
		private void FriendCountChanged(Widget widget)
		{
			this.Toggle.PlayerCount = this.List.ChildCount;
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x0001A297 File Offset: 0x00018497
		private void FriendCountChanged(Widget parentWidget, Widget addedWidget)
		{
			this.Toggle.PlayerCount = this.List.ChildCount;
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000947 RID: 2375 RVA: 0x0001A2AF File Offset: 0x000184AF
		// (set) Token: 0x06000948 RID: 2376 RVA: 0x0001A2B8 File Offset: 0x000184B8
		[Editor(false)]
		public ListPanel List
		{
			get
			{
				return this._list;
			}
			set
			{
				if (this._list != value)
				{
					ListPanel list = this._list;
					if (list != null)
					{
						list.ItemAddEventHandlers.Remove(new Action<Widget, Widget>(this.FriendCountChanged));
					}
					ListPanel list2 = this._list;
					if (list2 != null)
					{
						list2.ItemAfterRemoveEventHandlers.Remove(new Action<Widget>(this.FriendCountChanged));
					}
					this._list = value;
					ListPanel list3 = this._list;
					if (list3 != null)
					{
						list3.ItemAddEventHandlers.Add(new Action<Widget, Widget>(this.FriendCountChanged));
					}
					ListPanel list4 = this._list;
					if (list4 != null)
					{
						list4.ItemAfterRemoveEventHandlers.Add(new Action<Widget>(this.FriendCountChanged));
					}
					base.OnPropertyChanged<ListPanel>(value, "List");
				}
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000949 RID: 2377 RVA: 0x0001A36E File Offset: 0x0001856E
		// (set) Token: 0x0600094A RID: 2378 RVA: 0x0001A376 File Offset: 0x00018576
		[Editor(false)]
		public MultiplayerLobbyFriendGroupToggleWidget Toggle
		{
			get
			{
				return this._toggle;
			}
			set
			{
				if (this._toggle != value)
				{
					this._toggle = value;
					base.OnPropertyChanged<MultiplayerLobbyFriendGroupToggleWidget>(value, "Toggle");
				}
			}
		}

		// Token: 0x04000431 RID: 1073
		private ListPanel _list;

		// Token: 0x04000432 RID: 1074
		private MultiplayerLobbyFriendGroupToggleWidget _toggle;
	}
}
