using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200005D RID: 93
	public readonly struct UndoRedoKey
	{
		// Token: 0x06000732 RID: 1842 RVA: 0x00018E34 File Offset: 0x00017034
		public UndoRedoKey(int gender, int race, BodyProperties bodyProperties)
		{
			this.Gender = gender;
			this.Race = race;
			this.BodyProperties = bodyProperties;
		}

		// Token: 0x0400039C RID: 924
		public readonly int Gender;

		// Token: 0x0400039D RID: 925
		public readonly int Race;

		// Token: 0x0400039E RID: 926
		public readonly BodyProperties BodyProperties;
	}
}
