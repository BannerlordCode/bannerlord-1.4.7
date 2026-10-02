using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Friend
{
	// Token: 0x020000B2 RID: 178
	public class MultiplayerLobbyFriendsPanelWidget : Widget
	{
		// Token: 0x0600094B RID: 2379 RVA: 0x0001A394 File Offset: 0x00018594
		public MultiplayerLobbyFriendsPanelWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0001A39D File Offset: 0x0001859D
		private void OnShowListTogglePropertyChanged(PropertyOwnerObject owner, string propertyName, bool value)
		{
			if (propertyName == "IsSelected")
			{
				this.FriendsListPanel.IsVisible = this.ShowListToggle.IsSelected;
			}
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x0001A3C2 File Offset: 0x000185C2
		private void IsForcedOpenUpdated()
		{
			this.FriendsListPanel.IsVisible = this.IsForcedOpen;
			this.ShowListToggle.IsSelected = this.IsForcedOpen;
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x0600094E RID: 2382 RVA: 0x0001A3E6 File Offset: 0x000185E6
		// (set) Token: 0x0600094F RID: 2383 RVA: 0x0001A3EE File Offset: 0x000185EE
		[Editor(false)]
		public bool IsForcedOpen
		{
			get
			{
				return this._isForcedOpen;
			}
			set
			{
				if (this._isForcedOpen != value)
				{
					this._isForcedOpen = value;
					base.OnPropertyChanged(value, "IsForcedOpen");
					this.IsForcedOpenUpdated();
				}
			}
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000950 RID: 2384 RVA: 0x0001A412 File Offset: 0x00018612
		// (set) Token: 0x06000951 RID: 2385 RVA: 0x0001A41A File Offset: 0x0001861A
		[Editor(false)]
		public Widget FriendsListPanel
		{
			get
			{
				return this._friendsListPanel;
			}
			set
			{
				if (this._friendsListPanel != value)
				{
					this._friendsListPanel = value;
					base.OnPropertyChanged<Widget>(value, "FriendsListPanel");
				}
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000952 RID: 2386 RVA: 0x0001A438 File Offset: 0x00018638
		// (set) Token: 0x06000953 RID: 2387 RVA: 0x0001A440 File Offset: 0x00018640
		[Editor(false)]
		public ToggleStateButtonWidget ShowListToggle
		{
			get
			{
				return this._showListToggle;
			}
			set
			{
				if (this._showListToggle != value)
				{
					if (this._showListToggle != null)
					{
						this._showListToggle.boolPropertyChanged -= this.OnShowListTogglePropertyChanged;
					}
					this._showListToggle = value;
					if (this._showListToggle != null)
					{
						this._showListToggle.boolPropertyChanged += this.OnShowListTogglePropertyChanged;
					}
					base.OnPropertyChanged<ToggleStateButtonWidget>(value, "ShowListToggle");
				}
			}
		}

		// Token: 0x04000433 RID: 1075
		private bool _isForcedOpen;

		// Token: 0x04000434 RID: 1076
		private Widget _friendsListPanel;

		// Token: 0x04000435 RID: 1077
		private ToggleStateButtonWidget _showListToggle;
	}
}
