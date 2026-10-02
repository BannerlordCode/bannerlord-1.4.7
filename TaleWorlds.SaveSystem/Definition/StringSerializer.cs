using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000058 RID: 88
	internal class StringSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002F7 RID: 759 RVA: 0x0000D0CE File Offset: 0x0000B2CE
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000D0D0 File Offset: 0x0000B2D0
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return null;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000D0D3 File Offset: 0x0000B2D3
		public int GetSizeInBytes()
		{
			return 0;
		}
	}
}
