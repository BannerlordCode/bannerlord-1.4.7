using System;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Save
{
	// Token: 0x0200002B RID: 43
	internal abstract class MemberSaveData : VariableSaveData
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600019C RID: 412 RVA: 0x00008878 File Offset: 0x00006A78
		// (set) Token: 0x0600019D RID: 413 RVA: 0x00008880 File Offset: 0x00006A80
		public ObjectSaveData ObjectSaveData { get; private set; }

		// Token: 0x0600019E RID: 414 RVA: 0x00008889 File Offset: 0x00006A89
		protected MemberSaveData(ObjectSaveData objectSaveData)
			: base(objectSaveData.Context)
		{
			this.ObjectSaveData = objectSaveData;
		}

		// Token: 0x0600019F RID: 415
		public abstract void Initialize(TypeDefinitionBase typeDefinition);

		// Token: 0x060001A0 RID: 416
		public abstract void InitializeAsCustomStruct(int structId);
	}
}
