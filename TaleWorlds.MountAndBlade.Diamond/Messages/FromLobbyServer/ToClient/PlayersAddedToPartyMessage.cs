using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200005E RID: 94
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class PlayersAddedToPartyMessage : Message
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001DC RID: 476 RVA: 0x00003500 File Offset: 0x00001700
		// (set) Token: 0x060001DD RID: 477 RVA: 0x00003508 File Offset: 0x00001708
		[TupleElementNames(new string[] { "PlayerId", "PlayerName", "IsPartyLeader" })]
		[JsonProperty]
		public List<ValueTuple<PlayerId, string, bool>> Players
		{
			[return: TupleElementNames(new string[] { "PlayerId", "PlayerName", "IsPartyLeader" })]
			get;
			[param: TupleElementNames(new string[] { "PlayerId", "PlayerName", "IsPartyLeader" })]
			private set;
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001DE RID: 478 RVA: 0x00003511 File Offset: 0x00001711
		// (set) Token: 0x060001DF RID: 479 RVA: 0x00003519 File Offset: 0x00001719
		[TupleElementNames(new string[] { "PlayerId", "PlayerName" })]
		[JsonProperty]
		public List<ValueTuple<PlayerId, string>> InvitedPlayers
		{
			[return: TupleElementNames(new string[] { "PlayerId", "PlayerName" })]
			get;
			[param: TupleElementNames(new string[] { "PlayerId", "PlayerName" })]
			private set;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00003522 File Offset: 0x00001722
		public PlayersAddedToPartyMessage()
		{
			this.Players = new List<ValueTuple<PlayerId, string, bool>>();
			this.InvitedPlayers = new List<ValueTuple<PlayerId, string>>();
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00003540 File Offset: 0x00001740
		public PlayersAddedToPartyMessage(PlayerId playerId, string playerName, bool isPartyLeader)
			: this()
		{
			this.AddPlayer(playerId, playerName, isPartyLeader);
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00003551 File Offset: 0x00001751
		public void AddPlayer(PlayerId playerId, string playerName, bool isPartyLeader)
		{
			this.Players.Add(new ValueTuple<PlayerId, string, bool>(playerId, playerName, isPartyLeader));
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00003566 File Offset: 0x00001766
		public void AddInvitedPlayer(PlayerId playerId, string playerName)
		{
			this.InvitedPlayers.Add(new ValueTuple<PlayerId, string>(playerId, playerName));
		}
	}
}
