using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000143 RID: 323
	[Serializable]
	public struct PlayerSessionId
	{
		// Token: 0x170002AE RID: 686
		// (get) Token: 0x060008A4 RID: 2212 RVA: 0x0000CBEA File Offset: 0x0000ADEA
		// (set) Token: 0x060008A5 RID: 2213 RVA: 0x0000CBF2 File Offset: 0x0000ADF2
		[JsonProperty]
		public Guid Guid
		{
			get
			{
				return this._guid;
			}
			private set
			{
				this._guid = value;
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x060008A6 RID: 2214 RVA: 0x0000CBFB File Offset: 0x0000ADFB
		public SessionKey SessionKey
		{
			get
			{
				return new SessionKey(this._guid);
			}
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0000CC08 File Offset: 0x0000AE08
		public PlayerSessionId(Guid guid)
		{
			this._guid = guid;
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x0000CC11 File Offset: 0x0000AE11
		public PlayerSessionId(SessionKey sessionKey)
		{
			this._guid = sessionKey.Guid;
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x0000CC20 File Offset: 0x0000AE20
		public static PlayerSessionId NewGuid()
		{
			return new PlayerSessionId(Guid.NewGuid());
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0000CC2C File Offset: 0x0000AE2C
		public override string ToString()
		{
			return this._guid.ToString();
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x0000CC3F File Offset: 0x0000AE3F
		public byte[] ToByteArray()
		{
			return this._guid.ToByteArray();
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x0000CC4C File Offset: 0x0000AE4C
		public static bool operator ==(PlayerSessionId a, PlayerSessionId b)
		{
			return a._guid == b._guid;
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x0000CC5F File Offset: 0x0000AE5F
		public static bool operator !=(PlayerSessionId a, PlayerSessionId b)
		{
			return a._guid != b._guid;
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0000CC74 File Offset: 0x0000AE74
		public override bool Equals(object o)
		{
			return o != null && o is PlayerSessionId && this._guid.Equals(((PlayerSessionId)o).Guid);
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x0000CCA7 File Offset: 0x0000AEA7
		public override int GetHashCode()
		{
			return this._guid.GetHashCode();
		}

		// Token: 0x040003AE RID: 942
		private Guid _guid;
	}
}
