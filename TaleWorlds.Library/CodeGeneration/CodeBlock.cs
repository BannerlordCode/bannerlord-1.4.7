using System;
using System.Collections.Generic;

namespace TaleWorlds.Library.CodeGeneration
{
	// Token: 0x020000C2 RID: 194
	public class CodeBlock
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x0600073C RID: 1852 RVA: 0x0001829A File Offset: 0x0001649A
		public List<string> Lines
		{
			get
			{
				return this._lines;
			}
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x000182A2 File Offset: 0x000164A2
		public CodeBlock()
		{
			this._lines = new List<string>();
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x000182B5 File Offset: 0x000164B5
		public void AddLine(string line)
		{
			this._lines.Add(line);
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x000182C4 File Offset: 0x000164C4
		public void AddLines(IEnumerable<string> lines)
		{
			foreach (string text in lines)
			{
				this._lines.Add(text);
			}
		}

		// Token: 0x0400023E RID: 574
		private List<string> _lines;
	}
}
