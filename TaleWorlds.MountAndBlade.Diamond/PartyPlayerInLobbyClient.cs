using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000126 RID: 294
	public class PartyPlayerInLobbyClient
	{
		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000790 RID: 1936 RVA: 0x0000B7D8 File Offset: 0x000099D8
		// (set) Token: 0x06000791 RID: 1937 RVA: 0x0000B7E0 File Offset: 0x000099E0
		public PlayerId PlayerId { get; private set; }

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000792 RID: 1938 RVA: 0x0000B7E9 File Offset: 0x000099E9
		// (set) Token: 0x06000793 RID: 1939 RVA: 0x0000B7F1 File Offset: 0x000099F1
		public string Name { get; private set; }

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000794 RID: 1940 RVA: 0x0000B7FA File Offset: 0x000099FA
		// (set) Token: 0x06000795 RID: 1941 RVA: 0x0000B802 File Offset: 0x00009A02
		public bool WaitingInvitation { get; private set; }

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000796 RID: 1942 RVA: 0x0000B80B File Offset: 0x00009A0B
		// (set) Token: 0x06000797 RID: 1943 RVA: 0x0000B813 File Offset: 0x00009A13
		public bool IsPartyLeader { get; private set; }

		// Token: 0x06000798 RID: 1944 RVA: 0x0000B81C File Offset: 0x00009A1C
		public PartyPlayerInLobbyClient(PlayerId playerId, string name, bool isPartyLeader = false)
		{
			this.PlayerId = playerId;
			this.Name = name;
			this.IsPartyLeader = isPartyLeader;
			this.WaitingInvitation = true;
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0000B840 File Offset: 0x00009A40
		public void SetAtParty()
		{
			this.WaitingInvitation = false;
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0000B849 File Offset: 0x00009A49
		public void SetLeader()
		{
			this.IsPartyLeader = true;
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x0000B852 File Offset: 0x00009A52
		public void SetMember()
		{
			this.IsPartyLeader = false;
		}
	}
}
