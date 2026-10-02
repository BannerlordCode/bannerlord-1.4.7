using System;
using System.Threading.Tasks;

namespace TaleWorlds.Library.Http
{
	// Token: 0x020000B3 RID: 179
	public interface IHttpDriver
	{
		// Token: 0x060006BC RID: 1724
		Task<string> HttpGetString(string url, bool withUserToken);

		// Token: 0x060006BD RID: 1725
		Task<string> HttpPostString(string url, string postData, string mediaType, bool withUserToken);

		// Token: 0x060006BE RID: 1726
		Task<byte[]> HttpDownloadData(string url);

		// Token: 0x060006BF RID: 1727
		IHttpRequestTask CreateHttpPostRequestTask(string address, string postData, bool withUserToken);

		// Token: 0x060006C0 RID: 1728
		IHttpRequestTask CreateHttpGetRequestTask(string address, bool withUserToken);
	}
}
