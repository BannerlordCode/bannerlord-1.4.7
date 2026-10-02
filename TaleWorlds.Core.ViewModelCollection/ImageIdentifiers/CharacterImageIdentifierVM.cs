using System;
using TaleWorlds.Core.ImageIdentifiers;

namespace TaleWorlds.Core.ViewModelCollection.ImageIdentifiers
{
	// Token: 0x0200001E RID: 30
	public class CharacterImageIdentifierVM : ImageIdentifierVM
	{
		// Token: 0x0600019D RID: 413 RVA: 0x00005946 File Offset: 0x00003B46
		public CharacterImageIdentifierVM(CharacterCode characterCode)
		{
			base.ImageIdentifier = new CharacterImageIdentifier(characterCode);
		}
	}
}
