using System;

namespace TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData
{
	// Token: 0x02000171 RID: 369
	public class FavoriteServerData : MultiplayerLocalData
	{
		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000A3F RID: 2623 RVA: 0x0001087F File Offset: 0x0000EA7F
		// (set) Token: 0x06000A40 RID: 2624 RVA: 0x00010887 File Offset: 0x0000EA87
		public string Address { get; set; }

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000A41 RID: 2625 RVA: 0x00010890 File Offset: 0x0000EA90
		// (set) Token: 0x06000A42 RID: 2626 RVA: 0x00010898 File Offset: 0x0000EA98
		public int Port { get; set; }

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000A43 RID: 2627 RVA: 0x000108A1 File Offset: 0x0000EAA1
		// (set) Token: 0x06000A44 RID: 2628 RVA: 0x000108A9 File Offset: 0x0000EAA9
		public string GameType { get; set; }

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000A45 RID: 2629 RVA: 0x000108B2 File Offset: 0x0000EAB2
		// (set) Token: 0x06000A46 RID: 2630 RVA: 0x000108BA File Offset: 0x0000EABA
		public string Name { get; set; }

		// Token: 0x06000A47 RID: 2631 RVA: 0x000108C3 File Offset: 0x0000EAC3
		private FavoriteServerData()
		{
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x000108CB File Offset: 0x0000EACB
		public static FavoriteServerData CreateFrom(GameServerEntry serverEntry)
		{
			if (serverEntry == null)
			{
				return null;
			}
			return new FavoriteServerData
			{
				Address = serverEntry.Address,
				Port = serverEntry.Port,
				GameType = serverEntry.GameType,
				Name = serverEntry.ServerName
			};
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x00010908 File Offset: 0x0000EB08
		public override bool HasSameContentWith(MultiplayerLocalData other)
		{
			FavoriteServerData favoriteServerData;
			return (favoriteServerData = other as FavoriteServerData) != null && (this.Address == favoriteServerData.Address && this.Port == favoriteServerData.Port && this.GameType == favoriteServerData.GameType) && this.Name == favoriteServerData.Name;
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x00010968 File Offset: 0x0000EB68
		public bool HasSameContentWith(GameServerEntry serverEntry)
		{
			return this.Address == serverEntry.Address && this.Port == serverEntry.Port && this.GameType == serverEntry.GameType && this.Name == serverEntry.ServerName;
		}
	}
}
