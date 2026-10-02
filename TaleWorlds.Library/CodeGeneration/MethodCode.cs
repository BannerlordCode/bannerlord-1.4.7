using System;
using System.Collections.Generic;

namespace TaleWorlds.Library.CodeGeneration
{
	// Token: 0x020000C1 RID: 193
	public class MethodCode
	{
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000729 RID: 1833 RVA: 0x0001801E File Offset: 0x0001621E
		// (set) Token: 0x0600072A RID: 1834 RVA: 0x00018026 File Offset: 0x00016226
		public string Comment { get; set; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x0600072B RID: 1835 RVA: 0x0001802F File Offset: 0x0001622F
		// (set) Token: 0x0600072C RID: 1836 RVA: 0x00018037 File Offset: 0x00016237
		public string Name { get; set; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600072D RID: 1837 RVA: 0x00018040 File Offset: 0x00016240
		// (set) Token: 0x0600072E RID: 1838 RVA: 0x00018048 File Offset: 0x00016248
		public string MethodSignature { get; set; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x00018051 File Offset: 0x00016251
		// (set) Token: 0x06000730 RID: 1840 RVA: 0x00018059 File Offset: 0x00016259
		public string ReturnParameter { get; set; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x00018062 File Offset: 0x00016262
		// (set) Token: 0x06000732 RID: 1842 RVA: 0x0001806A File Offset: 0x0001626A
		public bool IsStatic { get; set; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x00018073 File Offset: 0x00016273
		// (set) Token: 0x06000734 RID: 1844 RVA: 0x0001807B File Offset: 0x0001627B
		public MethodCodeAccessModifier AccessModifier { get; set; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x00018084 File Offset: 0x00016284
		// (set) Token: 0x06000736 RID: 1846 RVA: 0x0001808C File Offset: 0x0001628C
		public MethodCodePolymorphismInfo PolymorphismInfo { get; set; }

		// Token: 0x06000737 RID: 1847 RVA: 0x00018095 File Offset: 0x00016295
		public MethodCode()
		{
			this.Name = "UnnamedMethod";
			this.MethodSignature = "()";
			this.PolymorphismInfo = MethodCodePolymorphismInfo.None;
			this.ReturnParameter = "void";
			this._lines = new List<string>();
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x000180D0 File Offset: 0x000162D0
		public void GenerateInto(CodeGenerationFile codeGenerationFile)
		{
			string text = "";
			if (this.AccessModifier == MethodCodeAccessModifier.Public)
			{
				text += "public ";
			}
			else if (this.AccessModifier == MethodCodeAccessModifier.Protected)
			{
				text += "protected ";
			}
			else if (this.AccessModifier == MethodCodeAccessModifier.Private)
			{
				text += "private ";
			}
			else if (this.AccessModifier == MethodCodeAccessModifier.Internal)
			{
				text += "internal ";
			}
			if (this.IsStatic)
			{
				text += "static ";
			}
			if (this.PolymorphismInfo == MethodCodePolymorphismInfo.Virtual)
			{
				text += "virtual ";
			}
			else if (this.PolymorphismInfo == MethodCodePolymorphismInfo.Override)
			{
				text += "override ";
			}
			text = string.Concat(new string[] { text, this.ReturnParameter, " ", this.Name, this.MethodSignature });
			if (!string.IsNullOrEmpty(this.Comment))
			{
				codeGenerationFile.AddLine(this.Comment);
			}
			codeGenerationFile.AddLine(text);
			codeGenerationFile.AddLine("{");
			foreach (string text2 in this._lines)
			{
				codeGenerationFile.AddLine(text2);
			}
			codeGenerationFile.AddLine("}");
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0001822C File Offset: 0x0001642C
		public void AddLine(string line)
		{
			this._lines.Add(line);
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0001823C File Offset: 0x0001643C
		public void AddLines(IEnumerable<string> lines)
		{
			foreach (string text in lines)
			{
				this._lines.Add(text);
			}
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0001828C File Offset: 0x0001648C
		public void AddCodeBlock(CodeBlock codeBlock)
		{
			this.AddLines(codeBlock.Lines);
		}

		// Token: 0x0400023D RID: 573
		private List<string> _lines;
	}
}
