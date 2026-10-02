using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TaleWorlds.Diamond.Rest
{
	// Token: 0x02000042 RID: 66
	[DataContract]
	[Serializable]
	public sealed class RestResponse : RestData
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000175 RID: 373 RVA: 0x00004D54 File Offset: 0x00002F54
		// (set) Token: 0x06000176 RID: 374 RVA: 0x00004D5C File Offset: 0x00002F5C
		[DataMember]
		public bool Successful { get; private set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00004D65 File Offset: 0x00002F65
		// (set) Token: 0x06000178 RID: 376 RVA: 0x00004D6D File Offset: 0x00002F6D
		[DataMember]
		public string SuccessfulReason { get; private set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00004D76 File Offset: 0x00002F76
		// (set) Token: 0x0600017A RID: 378 RVA: 0x00004D7E File Offset: 0x00002F7E
		[DataMember]
		public RestFunctionResult FunctionResult { get; set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00004D87 File Offset: 0x00002F87
		// (set) Token: 0x0600017C RID: 380 RVA: 0x00004D8F File Offset: 0x00002F8F
		[DataMember]
		public byte[] UserCertificate { get; set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00004D98 File Offset: 0x00002F98
		public int RemainingMessageCount
		{
			get
			{
				if (this._responseMessages != null)
				{
					return this._responseMessages.Count;
				}
				return 0;
			}
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00004DAF File Offset: 0x00002FAF
		public RestResponse()
		{
			this._responseMessages = new List<RestResponseMessage>();
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00004DC2 File Offset: 0x00002FC2
		public void SetSuccessful(bool successful, string successfulReason)
		{
			this.Successful = successful;
			this.SuccessfulReason = successfulReason;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00004DD2 File Offset: 0x00002FD2
		public static RestResponse Create(bool successful, string successfulReason)
		{
			RestResponse restResponse = new RestResponse();
			restResponse.SetSuccessful(successful, successfulReason);
			return restResponse;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00004DE1 File Offset: 0x00002FE1
		public RestResponseMessage TryDequeueMessage()
		{
			if (this._responseMessages != null && this._responseMessages.Count > 0)
			{
				RestResponseMessage restResponseMessage = this._responseMessages[0];
				this._responseMessages.RemoveAt(0);
				return restResponseMessage;
			}
			return null;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00004E13 File Offset: 0x00003013
		public void ClearMessageQueue()
		{
			this._responseMessages.Clear();
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00004E20 File Offset: 0x00003020
		public void EnqueueMessage(RestResponseMessage message)
		{
			this._responseMessages.Add(message);
		}

		// Token: 0x04000088 RID: 136
		[DataMember]
		private List<RestResponseMessage> _responseMessages;
	}
}
