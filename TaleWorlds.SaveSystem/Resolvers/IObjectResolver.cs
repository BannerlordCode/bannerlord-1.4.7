using System;
using TaleWorlds.SaveSystem.Load;

namespace TaleWorlds.SaveSystem.Resolvers
{
	// Token: 0x02000034 RID: 52
	public interface IObjectResolver
	{
		// Token: 0x06000214 RID: 532
		bool CheckIfRequiresAdvancedResolving(object originalObject);

		// Token: 0x06000215 RID: 533
		object ResolveObject(object originalObject);

		// Token: 0x06000216 RID: 534
		object AdvancedResolveObject(object originalObject, MetaData metaData, ObjectLoadData objectLoadData);
	}
}
