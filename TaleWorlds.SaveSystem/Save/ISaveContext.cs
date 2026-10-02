using System;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Save
{
	// Token: 0x02000029 RID: 41
	internal interface ISaveContext
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600017F RID: 383
		DefinitionContext DefinitionContext { get; }

		// Token: 0x06000180 RID: 384
		int AddOrGetStringId(string text);

		// Token: 0x06000181 RID: 385
		int GetObjectId(object target);

		// Token: 0x06000182 RID: 386
		int GetContainerId(object target);

		// Token: 0x06000183 RID: 387
		int GetStringId(string target);

		// Token: 0x06000184 RID: 388
		bool Save(object target, MetaData metaData, out string errorMessage);

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000185 RID: 389
		GameData SaveData { get; }
	}
}
