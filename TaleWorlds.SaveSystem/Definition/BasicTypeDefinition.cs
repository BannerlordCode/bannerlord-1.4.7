using System;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000042 RID: 66
	internal class BasicTypeDefinition : TypeDefinitionBase
	{
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x0000CBB6 File Offset: 0x0000ADB6
		// (set) Token: 0x060002A2 RID: 674 RVA: 0x0000CBBE File Offset: 0x0000ADBE
		public IBasicTypeSerializer Serializer { get; private set; }

		// Token: 0x060002A3 RID: 675 RVA: 0x0000CBC7 File Offset: 0x0000ADC7
		public BasicTypeDefinition(Type type, int saveId, IBasicTypeSerializer serializer)
			: base(type, new TypeSaveId(saveId))
		{
			this.Serializer = serializer;
		}
	}
}
