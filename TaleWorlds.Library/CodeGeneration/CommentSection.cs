using System;
using System.Collections.Generic;

namespace TaleWorlds.Library.CodeGeneration
{
	// Token: 0x020000BF RID: 191
	public class CommentSection
	{
		// Token: 0x06000719 RID: 1817 RVA: 0x00017DE8 File Offset: 0x00015FE8
		public CommentSection()
		{
			this._lines = new List<string>();
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x00017DFB File Offset: 0x00015FFB
		public void AddCommentLine(string line)
		{
			this._lines.Add(line);
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x00017E0C File Offset: 0x0001600C
		public void GenerateInto(CodeGenerationFile codeGenerationFile)
		{
			foreach (string text in this._lines)
			{
				codeGenerationFile.AddLine("//" + text);
			}
		}

		// Token: 0x0400022F RID: 559
		private List<string> _lines;
	}
}
