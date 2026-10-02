using System;

namespace SandBox.View.CharacterCreation
{
	// Token: 0x0200007C RID: 124
	public sealed class CharacterCreationStageViewAttribute : Attribute
	{
		// Token: 0x06000559 RID: 1369 RVA: 0x0002834C File Offset: 0x0002654C
		public CharacterCreationStageViewAttribute(Type stageType)
		{
			this.StageType = stageType;
		}

		// Token: 0x04000278 RID: 632
		public readonly Type StageType;
	}
}
