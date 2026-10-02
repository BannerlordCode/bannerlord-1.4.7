using System;
using System.Threading.Tasks;
using TaleWorlds.Library.Http;

namespace TaleWorlds.Library
{
	// Token: 0x02000036 RID: 54
	public static class HttpHelper
	{
		// Token: 0x060001BF RID: 447 RVA: 0x000070BB File Offset: 0x000052BB
		public static Task<string> DownloadStringTaskAsync(string url)
		{
			return HttpDriverManager.GetDefaultHttpDriver().HttpGetString(url, false);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x000070C9 File Offset: 0x000052C9
		public static Task<byte[]> DownloadDataTaskAsync(string url)
		{
			return HttpDriverManager.GetDefaultHttpDriver().HttpDownloadData(url);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x000070D6 File Offset: 0x000052D6
		public static Task<string> PostStringAsync(string url, string postData, string mediaType = "application/json")
		{
			return HttpDriverManager.GetDefaultHttpDriver().HttpPostString(url, postData, mediaType, false);
		}
	}
}
