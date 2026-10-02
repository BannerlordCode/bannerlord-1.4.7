using System;
using System.Collections.Generic;

namespace TaleWorlds.Library.CodeGeneration
{
	// Token: 0x020000C5 RID: 197
	public class NamespaceCode
	{
		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000740 RID: 1856 RVA: 0x00018314 File Offset: 0x00016514
		// (set) Token: 0x06000741 RID: 1857 RVA: 0x0001831C File Offset: 0x0001651C
		public string Name { get; set; }

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000742 RID: 1858 RVA: 0x00018325 File Offset: 0x00016525
		// (set) Token: 0x06000743 RID: 1859 RVA: 0x0001832D File Offset: 0x0001652D
		public List<ClassCode> Classes { get; private set; }

		// Token: 0x06000744 RID: 1860 RVA: 0x00018336 File Offset: 0x00016536
		public NamespaceCode()
		{
			this.Classes = new List<ClassCode>();
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x0001834C File Offset: 0x0001654C
		public void GenerateInto(CodeGenerationFile codeGenerationFile)
		{
			codeGenerationFile.AddLine("namespace " + this.Name);
			codeGenerationFile.AddLine("{");
			foreach (ClassCode classCode in this.Classes)
			{
				classCode.GenerateInto(codeGenerationFile);
				codeGenerationFile.AddLine("");
			}
			codeGenerationFile.AddLine("}");
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x000183D4 File Offset: 0x000165D4
		public void AddClass(ClassCode clasCode)
		{
			this.Classes.Add(clasCode);
		}
	}
}
