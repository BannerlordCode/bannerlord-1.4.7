using System;
using System.Runtime.Serialization;

namespace TaleWorlds.Diamond.Rest
{
	// Token: 0x0200003F RID: 63
	[DataContract]
	[Serializable]
	public class RestObjectRequestMessage : RestRequestMessage
	{
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00004CE3 File Offset: 0x00002EE3
		// (set) Token: 0x0600016B RID: 363 RVA: 0x00004CEB File Offset: 0x00002EEB
		[DataMember]
		public MessageType MessageType { get; private set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00004CF4 File Offset: 0x00002EF4
		// (set) Token: 0x0600016D RID: 365 RVA: 0x00004CFC File Offset: 0x00002EFC
		[DataMember]
		public SessionCredentials SessionCredentials { get; private set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600016E RID: 366 RVA: 0x00004D05 File Offset: 0x00002F05
		// (set) Token: 0x0600016F RID: 367 RVA: 0x00004D0D File Offset: 0x00002F0D
		[DataMember]
		public Message Message { get; private set; }

		// Token: 0x06000170 RID: 368 RVA: 0x00004D16 File Offset: 0x00002F16
		public RestObjectRequestMessage()
		{
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00004D1E File Offset: 0x00002F1E
		public RestObjectRequestMessage(SessionCredentials sessionCredentials, Message message, MessageType messageType)
		{
			this.Message = message;
			this.MessageType = messageType;
			this.SessionCredentials = sessionCredentials;
		}
	}
}
