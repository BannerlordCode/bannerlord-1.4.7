using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TaleWorlds.Library.Http
{
	// Token: 0x020000AE RID: 174
	public class DotNetHttpDriver : IHttpDriver
	{
		// Token: 0x06000693 RID: 1683 RVA: 0x00016C4C File Offset: 0x00014E4C
		public DotNetHttpDriver()
		{
			ServicePointManager.DefaultConnectionLimit = 5;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			this._httpClient = new HttpClient();
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x00016C6F File Offset: 0x00014E6F
		IHttpRequestTask IHttpDriver.CreateHttpPostRequestTask(string address, string postData, bool withUserToken)
		{
			return new HttpPostRequest(this._httpClient, address, postData);
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00016C7E File Offset: 0x00014E7E
		IHttpRequestTask IHttpDriver.CreateHttpGetRequestTask(string address, bool withUserToken)
		{
			return new HttpGetRequest(this._httpClient, address);
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x00016C8C File Offset: 0x00014E8C
		async Task<string> IHttpDriver.HttpGetString(string url, bool withUserToken)
		{
			HttpResponseMessage httpResponseMessage = await this._httpClient.GetAsync(url);
			HttpResponseMessage responseMessage = httpResponseMessage;
			string text = await responseMessage.Content.ReadAsStringAsync();
			if (!responseMessage.IsSuccessStatusCode)
			{
				throw new Exception(text);
			}
			return text;
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x00016CDC File Offset: 0x00014EDC
		async Task<string> IHttpDriver.HttpPostString(string url, string postData, string mediaType, bool withUserToken)
		{
			HttpResponseMessage httpResponseMessage = await this._httpClient.PostAsync(url, new StringContent(postData, Encoding.Unicode, mediaType));
			string text;
			using (HttpResponseMessage response = httpResponseMessage)
			{
				using (HttpContent content = response.Content)
				{
					text = await content.ReadAsStringAsync();
				}
			}
			return text;
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x00016D3C File Offset: 0x00014F3C
		async Task<byte[]> IHttpDriver.HttpDownloadData(string url)
		{
			return await this._httpClient.GetByteArrayAsync(url);
		}

		// Token: 0x040001F7 RID: 503
		private HttpClient _httpClient;
	}
}
