using System;
using System.Text;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x0200003B RID: 59
	public struct BannerDebugInfo
	{
		// Token: 0x06000218 RID: 536 RVA: 0x0000EC04 File Offset: 0x0000CE04
		public static BannerDebugInfo CreateManual(string sourceName)
		{
			return new BannerDebugInfo
			{
				SourceName = sourceName,
				SourceType = BannerDebugInfo.SourceTypes.Manual
			};
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000EC2C File Offset: 0x0000CE2C
		public static BannerDebugInfo CreateWidget(string sourceName)
		{
			return new BannerDebugInfo
			{
				SourceType = BannerDebugInfo.SourceTypes.Widget,
				SourceName = sourceName
			};
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000EC54 File Offset: 0x0000CE54
		public string CreateName()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("type:");
			stringBuilder.Append(BannerDebugInfo.GetSourceTypeName(this.SourceType));
			stringBuilder.Append("name:");
			stringBuilder.Append(this.SourceName);
			return stringBuilder.ToString();
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000ECA2 File Offset: 0x0000CEA2
		private static string GetSourceTypeName(BannerDebugInfo.SourceTypes type)
		{
			switch (type)
			{
			case BannerDebugInfo.SourceTypes.Widget:
				return "Wi";
			case BannerDebugInfo.SourceTypes.Manual:
				return "Mn";
			}
			return "Un";
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000ECCC File Offset: 0x0000CECC
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Format("type: {0}_", this.SourceType));
			stringBuilder.Append("name: " + this.SourceName + "_");
			return stringBuilder.ToString();
		}

		// Token: 0x04000134 RID: 308
		public BannerDebugInfo.SourceTypes SourceType;

		// Token: 0x04000135 RID: 309
		public string SourceName;

		// Token: 0x020000C4 RID: 196
		public enum SourceTypes
		{
			// Token: 0x0400037D RID: 893
			Undefined,
			// Token: 0x0400037E RID: 894
			Widget,
			// Token: 0x0400037F RID: 895
			Manual
		}
	}
}
