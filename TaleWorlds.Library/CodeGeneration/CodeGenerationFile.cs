using System;
using System.Collections.Generic;
using System.Text;

namespace TaleWorlds.Library.CodeGeneration
{
	// Token: 0x020000BE RID: 190
	public class CodeGenerationFile
	{
		// Token: 0x06000716 RID: 1814 RVA: 0x00017C94 File Offset: 0x00015E94
		public CodeGenerationFile(List<string> usingDefinitions = null)
		{
			this._lines = new List<string>();
			if (usingDefinitions != null && usingDefinitions.Count > 0)
			{
				foreach (string text in usingDefinitions)
				{
					this.AddLine("using " + text + ";");
				}
			}
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x00017D10 File Offset: 0x00015F10
		public void AddLine(string line)
		{
			this._lines.Add(line);
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x00017D20 File Offset: 0x00015F20
		public string GenerateText()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (string text in this._lines)
			{
				if (text == "}" || text == "};")
				{
					num--;
				}
				string text2 = "";
				for (int i = 0; i < num; i++)
				{
					text2 += "\t";
				}
				text2 = text2 + text + "\n";
				if (text == "{")
				{
					num++;
				}
				stringBuilder.Append(text2);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400022E RID: 558
		private List<string> _lines;
	}
}
