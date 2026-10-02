using System;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection
{
	// Token: 0x02000013 RID: 19
	public static class MultiplayerPlayerContextMenuHelper
	{
		// Token: 0x0600010A RID: 266 RVA: 0x00005848 File Offset: 0x00003A48
		public static void AddLobbyViewProfileOptions(MPLobbyPlayerBaseVM player, MBBindingList<StringPairItemWithActionVM> contextMenuOptions)
		{
			contextMenuOptions.Add(new StringPairItemWithActionVM(new Action<object>(MultiplayerPlayerContextMenuHelper.ExecuteViewProfile), new TextObject("{=bjJkW9dO}View Profile", null).ToString(), "ViewProfile", player));
			MultiplayerPlayerContextMenuHelper.AddPlatformProfileCardOption(new Action<object>(MultiplayerPlayerContextMenuHelper.ExecuteViewPlatformProfileCardLobby), player, player.ProvidedID, contextMenuOptions);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0000589B File Offset: 0x00003A9B
		public static void AddMissionViewProfileOptions(MPPlayerVM player, MBBindingList<StringPairItemWithActionVM> contextMenuOptions)
		{
			MultiplayerPlayerContextMenuHelper.AddPlatformProfileCardOption(new Action<object>(MultiplayerPlayerContextMenuHelper.ExecuteViewPlatformProfileCardMission), player, player.Peer.Peer.Id, contextMenuOptions);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000058C0 File Offset: 0x00003AC0
		private static void AddPlatformProfileCardOption(Action<object> onExecuted, object target, PlayerId playerId, MBBindingList<StringPairItemWithActionVM> contextMenuOptions)
		{
			if (!PlatformServices.Instance.IsPlayerProfileCardAvailable(NetworkMain.GameClient.PlayerID))
			{
				return;
			}
			if (!PlatformServices.Instance.IsPlayerProfileCardAvailable(playerId))
			{
				return;
			}
			if (!playerId.ProvidedType.SupportsPlayerCard())
			{
				return;
			}
			TextObject empty = TextObject.GetEmpty();
			Debug.FailedAssert("Platform profile is supported but \"Show Profile\" text is not defined!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\MultiplayerPlayerContextMenuHelper.cs", "AddPlatformProfileCardOption", 51);
			if (!empty.IsEmpty())
			{
				contextMenuOptions.Add(new StringPairItemWithActionVM(onExecuted, empty.ToString(), "ViewProfile", target));
			}
		}

		// Token: 0x0600010D RID: 269 RVA: 0x0000593D File Offset: 0x00003B3D
		private static void ExecuteViewProfile(object playerObj)
		{
			(playerObj as MPLobbyPlayerBaseVM).ExecuteShowProfile();
		}

		// Token: 0x0600010E RID: 270 RVA: 0x0000594A File Offset: 0x00003B4A
		private static void ExecuteViewPlatformProfileCardLobby(object playerObj)
		{
			PlatformServices.Instance.ShowPlayerProfileCard((playerObj as MPLobbyPlayerBaseVM).ProvidedID);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005961 File Offset: 0x00003B61
		private static void ExecuteViewPlatformProfileCardMission(object playerObj)
		{
			PlatformServices.Instance.ShowPlayerProfileCard((playerObj as MPPlayerVM).Peer.Peer.Id);
		}
	}
}
