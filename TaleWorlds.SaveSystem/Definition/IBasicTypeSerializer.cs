using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000043 RID: 67
	public interface IBasicTypeSerializer
	{
		// Token: 0x060002A4 RID: 676
		void Serialize(IWriter writer, object value);

		// Token: 0x060002A5 RID: 677
		object Deserialize(IReader reader);

		// Token: 0x060002A6 RID: 678
		int GetSizeInBytes();
	}
}
