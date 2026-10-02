using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000068 RID: 104
	public class SaveCodeGenerationContext
	{
		// Token: 0x06000375 RID: 885 RVA: 0x0000EC15 File Offset: 0x0000CE15
		public SaveCodeGenerationContext(DefinitionContext definitionContext)
		{
			this._definitionContext = definitionContext;
			this._assemblies = new Dictionary<Assembly, SaveCodeGenerationContextAssembly>();
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000EC30 File Offset: 0x0000CE30
		public void AddAssembly(Assembly assembly, string defaultNamespace, string location, string fileName)
		{
			SaveCodeGenerationContextAssembly saveCodeGenerationContextAssembly = new SaveCodeGenerationContextAssembly(this._definitionContext, assembly, defaultNamespace, location, fileName);
			this._assemblies.Add(assembly, saveCodeGenerationContextAssembly);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000EC5C File Offset: 0x0000CE5C
		internal SaveCodeGenerationContextAssembly FindAssemblyInformation(Assembly assembly)
		{
			SaveCodeGenerationContextAssembly saveCodeGenerationContextAssembly;
			this._assemblies.TryGetValue(assembly, out saveCodeGenerationContextAssembly);
			return saveCodeGenerationContextAssembly;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000EC7C File Offset: 0x0000CE7C
		internal void FillFiles()
		{
			List<Tuple<string, string>> list = new List<Tuple<string, string>>();
			foreach (SaveCodeGenerationContextAssembly saveCodeGenerationContextAssembly in this._assemblies.Values)
			{
				saveCodeGenerationContextAssembly.Generate();
				string text = saveCodeGenerationContextAssembly.GenerateText();
				list.Add(new Tuple<string, string>(saveCodeGenerationContextAssembly.Location + saveCodeGenerationContextAssembly.FileName, text));
			}
			foreach (Tuple<string, string> tuple in list)
			{
				File.WriteAllText(tuple.Item1, tuple.Item2, Encoding.UTF8);
			}
		}

		// Token: 0x04000106 RID: 262
		private Dictionary<Assembly, SaveCodeGenerationContextAssembly> _assemblies;

		// Token: 0x04000107 RID: 263
		private DefinitionContext _definitionContext;
	}
}
