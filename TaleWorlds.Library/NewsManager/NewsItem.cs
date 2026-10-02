using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TaleWorlds.Library.NewsManager
{
	// Token: 0x020000AB RID: 171
	public struct NewsItem
	{
		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000682 RID: 1666 RVA: 0x00016BA5 File Offset: 0x00014DA5
		// (set) Token: 0x06000683 RID: 1667 RVA: 0x00016BAD File Offset: 0x00014DAD
		public string Title { get; set; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000684 RID: 1668 RVA: 0x00016BB6 File Offset: 0x00014DB6
		// (set) Token: 0x06000685 RID: 1669 RVA: 0x00016BBE File Offset: 0x00014DBE
		public string Description { get; set; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000686 RID: 1670 RVA: 0x00016BC7 File Offset: 0x00014DC7
		// (set) Token: 0x06000687 RID: 1671 RVA: 0x00016BCF File Offset: 0x00014DCF
		public string ImageSourcePath { get; set; }

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000688 RID: 1672 RVA: 0x00016BD8 File Offset: 0x00014DD8
		// (set) Token: 0x06000689 RID: 1673 RVA: 0x00016BE0 File Offset: 0x00014DE0
		public List<NewsType> Feeds { get; set; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600068A RID: 1674 RVA: 0x00016BE9 File Offset: 0x00014DE9
		// (set) Token: 0x0600068B RID: 1675 RVA: 0x00016BF1 File Offset: 0x00014DF1
		public string NewsLink { get; set; }

		// Token: 0x020000F5 RID: 245
		[JsonConverter(typeof(StringEnumConverter))]
		public enum NewsTypes
		{
			// Token: 0x04000316 RID: 790
			LauncherSingleplayer,
			// Token: 0x04000317 RID: 791
			LauncherMultiplayer,
			// Token: 0x04000318 RID: 792
			MultiplayerLobby
		}
	}
}
