using System;
using System.Reflection;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000059 RID: 89
	public class ContainerDefinition : TypeDefinitionBase
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060002FB RID: 763 RVA: 0x0000D0DE File Offset: 0x0000B2DE
		// (set) Token: 0x060002FC RID: 764 RVA: 0x0000D0E6 File Offset: 0x0000B2E6
		public Assembly DefinedAssembly { get; private set; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060002FD RID: 765 RVA: 0x0000D0EF File Offset: 0x0000B2EF
		// (set) Token: 0x060002FE RID: 766 RVA: 0x0000D0F7 File Offset: 0x0000B2F7
		public CollectObjectsDelegate CollectObjectsMethod { get; private set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060002FF RID: 767 RVA: 0x0000D100 File Offset: 0x0000B300
		// (set) Token: 0x06000300 RID: 768 RVA: 0x0000D108 File Offset: 0x0000B308
		public bool HasNoChildObject { get; private set; }

		// Token: 0x06000301 RID: 769 RVA: 0x0000D111 File Offset: 0x0000B311
		public ContainerDefinition(Type type, ContainerSaveId saveId, Assembly definedAssembly)
			: base(type, saveId)
		{
			this.DefinedAssembly = definedAssembly;
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000D122 File Offset: 0x0000B322
		public void InitializeForAutoGeneration(CollectObjectsDelegate collectObjectsDelegate, bool hasNoChildObject)
		{
			this.CollectObjectsMethod = collectObjectsDelegate;
			this.HasNoChildObject = hasNoChildObject;
		}
	}
}
