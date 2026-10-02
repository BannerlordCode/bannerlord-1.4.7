using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends
{
	// Token: 0x02000052 RID: 82
	public class MPLobbyFriendGroupVM : ViewModel
	{
		// Token: 0x06000720 RID: 1824 RVA: 0x000169FF File Offset: 0x00014BFF
		public MPLobbyFriendGroupVM(MPLobbyFriendGroupVM.FriendGroupType groupType)
		{
			this.GroupType = groupType;
			this._friendOperationQueue = new List<MPLobbyFriendGroupVM.FriendOperation>();
			this.FriendList = new MBBindingList<MPLobbyFriendItemVM>();
			this.RefreshValues();
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x00016A2C File Offset: 0x00014C2C
		public override void RefreshValues()
		{
			base.RefreshValues();
			switch (this.GroupType)
			{
			case MPLobbyFriendGroupVM.FriendGroupType.InGame:
				this.Title = new TextObject("{=uUoSmCBS}In Bannerlord", null).ToString();
				break;
			case MPLobbyFriendGroupVM.FriendGroupType.Online:
				this.Title = new TextObject("{=V305MaOP}Online", null).ToString();
				break;
			case MPLobbyFriendGroupVM.FriendGroupType.Offline:
				this.Title = new TextObject("{=Zv1lg272}Offline", null).ToString();
				break;
			case MPLobbyFriendGroupVM.FriendGroupType.FriendRequests:
				this.Title = new TextObject("{=K8CGzQYL}Received Requests", null).ToString();
				break;
			case MPLobbyFriendGroupVM.FriendGroupType.PendingRequests:
				this.Title = new TextObject("{=QwbVdMLi}Sent Requests", null).ToString();
				break;
			}
			this.FriendList.ApplyActionOnAllItems(delegate(MPLobbyFriendItemVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x00016B04 File Offset: 0x00014D04
		public void Tick()
		{
			for (int i = 0; i < this._friendOperationQueue.Count; i++)
			{
				this.HandleFriendOperationAux(this._friendOperationQueue[i]);
			}
			this._friendOperationQueue.Clear();
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x00016B44 File Offset: 0x00014D44
		private void HandleFriendOperationAux(MPLobbyFriendGroupVM.FriendOperation operation)
		{
			List<MPLobbyFriendGroupVM.FriendOperation> friendOperationQueue = this._friendOperationQueue;
			lock (friendOperationQueue)
			{
				switch (operation.Type)
				{
				case MPLobbyFriendGroupVM.FriendOperation.OperationTypes.Add:
					this.FriendList.Add(operation.Friend);
					operation.Friend.UpdateNameAndAvatar(false);
					break;
				case MPLobbyFriendGroupVM.FriendOperation.OperationTypes.Remove:
					this.FriendList.Remove(operation.Friend);
					break;
				case MPLobbyFriendGroupVM.FriendOperation.OperationTypes.Clear:
					this.FriendList.Clear();
					break;
				}
			}
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x00016BD8 File Offset: 0x00014DD8
		public void ClearFriends()
		{
			List<MPLobbyFriendGroupVM.FriendOperation> friendOperationQueue = this._friendOperationQueue;
			lock (friendOperationQueue)
			{
				this._friendOperationQueue.Add(new MPLobbyFriendGroupVM.FriendOperation(MPLobbyFriendGroupVM.FriendOperation.OperationTypes.Clear, null));
			}
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00016C24 File Offset: 0x00014E24
		public void AddFriend(MPLobbyFriendItemVM player)
		{
			if (player.ProvidedID != NetworkMain.GameClient.PlayerID)
			{
				List<MPLobbyFriendGroupVM.FriendOperation> friendOperationQueue = this._friendOperationQueue;
				lock (friendOperationQueue)
				{
					this._friendOperationQueue.Add(new MPLobbyFriendGroupVM.FriendOperation(MPLobbyFriendGroupVM.FriendOperation.OperationTypes.Add, player));
				}
			}
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x00016C88 File Offset: 0x00014E88
		public void RemoveFriend(MPLobbyFriendItemVM player)
		{
			if (player.ProvidedID != NetworkMain.GameClient.PlayerID)
			{
				List<MPLobbyFriendGroupVM.FriendOperation> friendOperationQueue = this._friendOperationQueue;
				lock (friendOperationQueue)
				{
					this._friendOperationQueue.Add(new MPLobbyFriendGroupVM.FriendOperation(MPLobbyFriendGroupVM.FriendOperation.OperationTypes.Remove, player));
				}
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000727 RID: 1831 RVA: 0x00016CEC File Offset: 0x00014EEC
		// (set) Token: 0x06000728 RID: 1832 RVA: 0x00016CF4 File Offset: 0x00014EF4
		[DataSourceProperty]
		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				if (value != this._title)
				{
					this._title = value;
					base.OnPropertyChangedWithValue<string>(value, "Title");
				}
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000729 RID: 1833 RVA: 0x00016D17 File Offset: 0x00014F17
		// (set) Token: 0x0600072A RID: 1834 RVA: 0x00016D1F File Offset: 0x00014F1F
		[DataSourceProperty]
		public MPLobbyFriendGroupVM.FriendGroupType GroupType
		{
			get
			{
				return this._groupType;
			}
			set
			{
				if (value != this._groupType)
				{
					this._groupType = value;
					base.OnPropertyChanged("GroupType");
				}
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x0600072B RID: 1835 RVA: 0x00016D3C File Offset: 0x00014F3C
		// (set) Token: 0x0600072C RID: 1836 RVA: 0x00016D44 File Offset: 0x00014F44
		[DataSourceProperty]
		public MBBindingList<MPLobbyFriendItemVM> FriendList
		{
			get
			{
				return this._friendList;
			}
			set
			{
				if (value != this._friendList)
				{
					this._friendList = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyFriendItemVM>>(value, "FriendList");
				}
			}
		}

		// Token: 0x04000353 RID: 851
		private List<MPLobbyFriendGroupVM.FriendOperation> _friendOperationQueue;

		// Token: 0x04000354 RID: 852
		private string _title;

		// Token: 0x04000355 RID: 853
		private MPLobbyFriendGroupVM.FriendGroupType _groupType;

		// Token: 0x04000356 RID: 854
		private MBBindingList<MPLobbyFriendItemVM> _friendList;

		// Token: 0x02000100 RID: 256
		private readonly struct FriendOperation
		{
			// Token: 0x06001192 RID: 4498 RVA: 0x00036C3C File Offset: 0x00034E3C
			public FriendOperation(MPLobbyFriendGroupVM.FriendOperation.OperationTypes type, MPLobbyFriendItemVM friend)
			{
				this.Type = type;
				this.Friend = friend;
			}

			// Token: 0x040008B8 RID: 2232
			public readonly MPLobbyFriendItemVM Friend;

			// Token: 0x040008B9 RID: 2233
			public readonly MPLobbyFriendGroupVM.FriendOperation.OperationTypes Type;

			// Token: 0x02000196 RID: 406
			public enum OperationTypes
			{
				// Token: 0x04000A61 RID: 2657
				Add,
				// Token: 0x04000A62 RID: 2658
				Remove,
				// Token: 0x04000A63 RID: 2659
				Clear
			}
		}

		// Token: 0x02000101 RID: 257
		public enum FriendGroupType
		{
			// Token: 0x040008BB RID: 2235
			InGame,
			// Token: 0x040008BC RID: 2236
			Online,
			// Token: 0x040008BD RID: 2237
			Offline,
			// Token: 0x040008BE RID: 2238
			FriendRequests,
			// Token: 0x040008BF RID: 2239
			PendingRequests
		}
	}
}
