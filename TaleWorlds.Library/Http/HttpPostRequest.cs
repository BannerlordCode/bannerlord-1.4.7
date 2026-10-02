using System;
using System.Net.Http;
using System.Text;

namespace TaleWorlds.Library.Http
{
	// Token: 0x020000B1 RID: 177
	public class HttpPostRequest : IHttpRequestTask
	{
		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060006AE RID: 1710 RVA: 0x00016F49 File Offset: 0x00015149
		// (set) Token: 0x060006AF RID: 1711 RVA: 0x00016F51 File Offset: 0x00015151
		public HttpRequestTaskState State { get; private set; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060006B0 RID: 1712 RVA: 0x00016F5A File Offset: 0x0001515A
		// (set) Token: 0x060006B1 RID: 1713 RVA: 0x00016F62 File Offset: 0x00015162
		public bool Successful { get; private set; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060006B2 RID: 1714 RVA: 0x00016F6B File Offset: 0x0001516B
		// (set) Token: 0x060006B3 RID: 1715 RVA: 0x00016F73 File Offset: 0x00015173
		public string ResponseData { get; private set; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060006B4 RID: 1716 RVA: 0x00016F7C File Offset: 0x0001517C
		// (set) Token: 0x060006B5 RID: 1717 RVA: 0x00016F84 File Offset: 0x00015184
		public Exception Exception { get; private set; }

		// Token: 0x060006B6 RID: 1718 RVA: 0x00016F8D File Offset: 0x0001518D
		public HttpPostRequest(HttpClient httpClient, string address, string postData)
			: this(httpClient, address, postData, new Version("1.1"))
		{
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x00016FA2 File Offset: 0x000151A2
		public HttpPostRequest(HttpClient httpClient, string address, string postData, Version version)
		{
			this._httpClient = httpClient;
			this._postData = postData;
			this._address = address;
			this.State = HttpRequestTaskState.NotStarted;
			this.ResponseData = "";
			this._versionToUse = version;
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x00016FD9 File Offset: 0x000151D9
		private void SetFinishedAsSuccessful(string responseData)
		{
			this.Successful = true;
			this.ResponseData = responseData;
			this.State = HttpRequestTaskState.Finished;
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x00016FF0 File Offset: 0x000151F0
		private void SetFinishedAsUnsuccessful(Exception e)
		{
			this.Successful = false;
			this.Exception = e;
			this.State = HttpRequestTaskState.Finished;
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x00017007 File Offset: 0x00015207
		public void Start()
		{
			this.DoTask();
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x00017010 File Offset: 0x00015210
		private async void DoTask()
		{
			this.State = HttpRequestTaskState.Working;
			try
			{
				Debug.Print("Http Post Request to " + this._address, 0, Debug.DebugColor.White, 17592186044416UL);
				using (HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, this._address))
				{
					requestMessage.Version = this._versionToUse;
					requestMessage.Headers.Add("Accept", "application/json");
					requestMessage.Headers.Add("UserAgent", "TaleWorlds Client");
					requestMessage.Content = new StringContent(this._postData, Encoding.Unicode, "application/json");
					HttpResponseMessage httpResponseMessage = await this._httpClient.SendAsync(requestMessage);
					using (HttpResponseMessage response = httpResponseMessage)
					{
						bool isSuccessStatusCode = response.IsSuccessStatusCode;
						response.EnsureSuccessStatusCode();
						Debug.Print(string.Concat(new object[] { "Protocol version used for post request to ", this._address, " is: ", response.Version }), 0, Debug.DebugColor.White, 17592186044416UL);
						using (HttpContent content = response.Content)
						{
							this.SetFinishedAsSuccessful(await content.ReadAsStringAsync());
						}
						HttpContent content = null;
					}
					HttpResponseMessage response = null;
				}
				HttpRequestMessage requestMessage = null;
			}
			catch (Exception ex)
			{
				this.SetFinishedAsUnsuccessful(ex);
			}
		}

		// Token: 0x04000203 RID: 515
		private HttpClient _httpClient;

		// Token: 0x04000204 RID: 516
		private readonly string _address;

		// Token: 0x04000205 RID: 517
		private string _postData;

		// Token: 0x0400020A RID: 522
		private Version _versionToUse;
	}
}
