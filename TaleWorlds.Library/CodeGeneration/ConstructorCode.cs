using System;
using System.Collections.Generic;

namespace TaleWorlds.Library.CodeGeneration
{
	// Token: 0x020000C0 RID: 192
	public class ConstructorCode
	{
		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600071C RID: 1820 RVA: 0x00017E6C File Offset: 0x0001606C
		// (set) Token: 0x0600071D RID: 1821 RVA: 0x00017E74 File Offset: 0x00016074
		public string Name { get; set; }

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600071E RID: 1822 RVA: 0x00017E7D File Offset: 0x0001607D
		// (set) Token: 0x0600071F RID: 1823 RVA: 0x00017E85 File Offset: 0x00016085
		public string MethodSignature { get; set; }

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000720 RID: 1824 RVA: 0x00017E8E File Offset: 0x0001608E
		// (set) Token: 0x06000721 RID: 1825 RVA: 0x00017E96 File Offset: 0x00016096
		public string BaseCall { get; set; }

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000722 RID: 1826 RVA: 0x00017E9F File Offset: 0x0001609F
		// (set) Token: 0x06000723 RID: 1827 RVA: 0x00017EA7 File Offset: 0x000160A7
		public bool IsStatic { get; set; }

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000724 RID: 1828 RVA: 0x00017EB0 File Offset: 0x000160B0
		// (set) Token: 0x06000725 RID: 1829 RVA: 0x00017EB8 File Offset: 0x000160B8
		public MethodCodeAccessModifier AccessModifier { get; set; }

		// Token: 0x06000726 RID: 1830 RVA: 0x00017EC1 File Offset: 0x000160C1
		public ConstructorCode()
		{
			this.Name = "UnassignedConstructorName";
			this.MethodSignature = "()";
			this.BaseCall = "";
			this._lines = new List<string>();
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x00017EF8 File Offset: 0x000160F8
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
			text = text + this.Name + this.MethodSignature;
			if (!string.IsNullOrEmpty(this.BaseCall))
			{
				text = text + " : base" + this.BaseCall;
			}
			codeGenerationFile.AddLine(text);
			codeGenerationFile.AddLine("{");
			foreach (string text2 in this._lines)
			{
				codeGenerationFile.AddLine(text2);
			}
			codeGenerationFile.AddLine("}");
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x00018010 File Offset: 0x00016210
		public void AddLine(string line)
		{
			this._lines.Add(line);
		}

		// Token: 0x04000235 RID: 565
		private List<string> _lines;
	}
}
