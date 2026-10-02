using System;
using System.Runtime.Serialization;

namespace TaleWorlds.Diamond.Rest
{
	// Token: 0x02000041 RID: 65
	[DataContract]
	[Serializable]
	public abstract class RestRequestMessage : RestData
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00004D3B File Offset: 0x00002F3B
		// (set) Token: 0x06000173 RID: 371 RVA: 0x00004D43 File Offset: 0x00002F43
		[DataMember]
		public byte[] UserCertificate { get; set; }
	}
}
