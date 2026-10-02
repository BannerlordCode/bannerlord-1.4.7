using System;
using System.Collections;

namespace TaleWorlds.Library
{
	// Token: 0x02000038 RID: 56
	public interface IMBBindingList : IList, ICollection, IEnumerable
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060001C5 RID: 453
		// (remove) Token: 0x060001C6 RID: 454
		event ListChangedEventHandler ListChanged;
	}
}
