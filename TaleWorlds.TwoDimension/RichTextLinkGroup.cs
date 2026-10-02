using System;
using System.Collections.Generic;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x0200000B RID: 11
	public class RichTextLinkGroup
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600007B RID: 123 RVA: 0x000048C7 File Offset: 0x00002AC7
		// (set) Token: 0x0600007C RID: 124 RVA: 0x000048CF File Offset: 0x00002ACF
		public string Href { get; private set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600007D RID: 125 RVA: 0x000048D8 File Offset: 0x00002AD8
		// (set) Token: 0x0600007E RID: 126 RVA: 0x000048E0 File Offset: 0x00002AE0
		internal int StartIndex { get; private set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600007F RID: 127 RVA: 0x000048E9 File Offset: 0x00002AE9
		internal int EndIndex
		{
			get
			{
				return this.StartIndex + this._tokens.Count;
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000048FD File Offset: 0x00002AFD
		internal RichTextLinkGroup(int startIndex, string href)
		{
			this.Href = href;
			this.StartIndex = startIndex;
			this._tokens = new List<TextToken>();
		}

		// Token: 0x06000081 RID: 129 RVA: 0x0000491E File Offset: 0x00002B1E
		internal void AddToken(TextToken textToken)
		{
			this._tokens.Add(textToken);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0000492C File Offset: 0x00002B2C
		internal bool Contains(TextToken textToken)
		{
			return this._tokens.Contains(textToken);
		}

		// Token: 0x04000051 RID: 81
		private List<TextToken> _tokens;
	}
}
