using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000EA RID: 234
	[Flags]
	public enum ImplicitUseKindFlags
	{
		// Token: 0x04000224 RID: 548
		Default = 7,
		// Token: 0x04000225 RID: 549
		Access = 1,
		// Token: 0x04000226 RID: 550
		Assign = 2,
		// Token: 0x04000227 RID: 551
		InstantiatedWithFixedConstructorSignature = 4,
		// Token: 0x04000228 RID: 552
		InstantiatedNoFixedConstructorSignature = 8
	}
}
