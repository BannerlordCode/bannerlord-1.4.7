using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000129 RID: 297
	[Serializable]
	public class PlayerJoinGameData
	{
		// Token: 0x17000266 RID: 614
		// (get) Token: 0x060007A2 RID: 1954 RVA: 0x0000B89B File Offset: 0x00009A9B
		// (set) Token: 0x060007A3 RID: 1955 RVA: 0x0000B8A3 File Offset: 0x00009AA3
		public PlayerData PlayerData { get; set; }

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x060007A4 RID: 1956 RVA: 0x0000B8AC File Offset: 0x00009AAC
		public PlayerId PlayerId
		{
			get
			{
				return this.PlayerData.PlayerId;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x060007A5 RID: 1957 RVA: 0x0000B8B9 File Offset: 0x00009AB9
		// (set) Token: 0x060007A6 RID: 1958 RVA: 0x0000B8C1 File Offset: 0x00009AC1
		public string Name { get; set; }

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x060007A7 RID: 1959 RVA: 0x0000B8CA File Offset: 0x00009ACA
		// (set) Token: 0x060007A8 RID: 1960 RVA: 0x0000B8D2 File Offset: 0x00009AD2
		public Guid? PartyId { get; set; }

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x060007A9 RID: 1961 RVA: 0x0000B8DB File Offset: 0x00009ADB
		// (set) Token: 0x060007AA RID: 1962 RVA: 0x0000B8E3 File Offset: 0x00009AE3
		public Dictionary<string, List<string>> UsedCosmetics { get; set; }

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x060007AB RID: 1963 RVA: 0x0000B8EC File Offset: 0x00009AEC
		// (set) Token: 0x060007AC RID: 1964 RVA: 0x0000B8F4 File Offset: 0x00009AF4
		[JsonProperty]
		public string IpAddress { get; private set; }

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x060007AD RID: 1965 RVA: 0x0000B8FD File Offset: 0x00009AFD
		// (set) Token: 0x060007AE RID: 1966 RVA: 0x0000B905 File Offset: 0x00009B05
		[JsonProperty]
		public bool IsAdmin { get; private set; }

		// Token: 0x060007AF RID: 1967 RVA: 0x0000B90E File Offset: 0x00009B0E
		public PlayerJoinGameData()
		{
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x0000B916 File Offset: 0x00009B16
		public PlayerJoinGameData(PlayerData playerData, string name, Guid? partyId, Dictionary<string, List<string>> usedCosmetics, string ipAddress, bool isAdmin)
		{
			this.PlayerData = playerData;
			this.Name = name;
			this.PartyId = partyId;
			this.UsedCosmetics = usedCosmetics;
			this.IpAddress = ipAddress;
			this.IsAdmin = isAdmin;
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x0000B94C File Offset: 0x00009B4C
		public override string ToString()
		{
			return string.Format("Player Join Game Data: {0}, name={1}, party={2}, cosmetics={3}, ip={4}, isAdmin={5}", new object[]
			{
				this.PlayerId,
				this.Name,
				this.PartyId,
				this.UsedCosmetics.Count,
				this.IpAddress,
				this.IsAdmin
			});
		}
	}
}
