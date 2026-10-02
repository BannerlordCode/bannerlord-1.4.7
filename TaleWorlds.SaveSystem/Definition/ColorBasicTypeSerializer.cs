using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000056 RID: 86
	internal class ColorBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002EF RID: 751 RVA: 0x0000D074 File Offset: 0x0000B274
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			Color color = (Color)value;
			writer.WriteColor(color);
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000D08F File Offset: 0x0000B28F
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return reader.ReadColor();
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000D09C File Offset: 0x0000B29C
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 16;
		}
	}
}
