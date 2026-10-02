using System;
using System.Collections.Generic;

namespace TaleWorlds.Library.CodeGeneration
{
	// Token: 0x020000BD RID: 189
	public class CodeGenerationContext
	{
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x00017B96 File Offset: 0x00015D96
		// (set) Token: 0x06000712 RID: 1810 RVA: 0x00017B9E File Offset: 0x00015D9E
		public List<NamespaceCode> Namespaces { get; private set; }

		// Token: 0x06000713 RID: 1811 RVA: 0x00017BA7 File Offset: 0x00015DA7
		public CodeGenerationContext()
		{
			this.Namespaces = new List<NamespaceCode>();
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x00017BBC File Offset: 0x00015DBC
		public NamespaceCode FindOrCreateNamespace(string name)
		{
			foreach (NamespaceCode namespaceCode in this.Namespaces)
			{
				if (namespaceCode.Name == name)
				{
					return namespaceCode;
				}
			}
			NamespaceCode namespaceCode2 = new NamespaceCode();
			namespaceCode2.Name = name;
			this.Namespaces.Add(namespaceCode2);
			return namespaceCode2;
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x00017C38 File Offset: 0x00015E38
		public void GenerateInto(CodeGenerationFile codeGenerationFile)
		{
			foreach (NamespaceCode namespaceCode in this.Namespaces)
			{
				namespaceCode.GenerateInto(codeGenerationFile);
				codeGenerationFile.AddLine("");
			}
		}
	}
}
