using System;
using TaleWorlds.Core.ImageIdentifiers;

namespace TaleWorlds.Core.ViewModelCollection.ImageIdentifiers
{
	// Token: 0x02000020 RID: 32
	public class GenericImageIdentifierVM : ImageIdentifierVM
	{
		// Token: 0x0600019F RID: 415 RVA: 0x0000596F File Offset: 0x00003B6F
		public GenericImageIdentifierVM(ImageIdentifier imageIdentifier)
		{
			if (imageIdentifier == null)
			{
				base.ImageIdentifier = new EmptyImageIdentifier();
				return;
			}
			base.ImageIdentifier = imageIdentifier;
		}
	}
}
