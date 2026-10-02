using System;
using System.Collections.Generic;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000321 RID: 801
	public class PlayerConnectionInfo
	{
		// Token: 0x06002D8C RID: 11660 RVA: 0x000AFF75 File Offset: 0x000AE175
		public PlayerConnectionInfo(PlayerId playerID)
		{
			this.PlayerID = playerID;
			this._parameters = new Dictionary<string, object>();
		}

		// Token: 0x06002D8D RID: 11661 RVA: 0x000AFF8F File Offset: 0x000AE18F
		public void AddParameter(string name, object parameter)
		{
			if (!this._parameters.ContainsKey(name))
			{
				this._parameters.Add(name, parameter);
			}
		}

		// Token: 0x06002D8E RID: 11662 RVA: 0x000AFFAC File Offset: 0x000AE1AC
		public T GetParameter<T>(string name) where T : class
		{
			if (this._parameters.ContainsKey(name))
			{
				return this._parameters[name] as T;
			}
			return default(T);
		}

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x06002D8F RID: 11663 RVA: 0x000AFFE7 File Offset: 0x000AE1E7
		// (set) Token: 0x06002D90 RID: 11664 RVA: 0x000AFFEF File Offset: 0x000AE1EF
		public int SessionKey { get; set; }

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x06002D91 RID: 11665 RVA: 0x000AFFF8 File Offset: 0x000AE1F8
		// (set) Token: 0x06002D92 RID: 11666 RVA: 0x000B0000 File Offset: 0x000AE200
		public string Name { get; set; }

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x06002D93 RID: 11667 RVA: 0x000B0009 File Offset: 0x000AE209
		// (set) Token: 0x06002D94 RID: 11668 RVA: 0x000B0011 File Offset: 0x000AE211
		public NetworkCommunicator NetworkPeer { get; set; }

		// Token: 0x040011EE RID: 4590
		private Dictionary<string, object> _parameters;

		// Token: 0x040011F2 RID: 4594
		public readonly PlayerId PlayerID;
	}
}
