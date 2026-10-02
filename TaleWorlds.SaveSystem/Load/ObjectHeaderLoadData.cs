using System;
using System.Runtime.Serialization;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Load
{
	// Token: 0x0200003E RID: 62
	public class ObjectHeaderLoadData
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000267 RID: 615 RVA: 0x0000BFB3 File Offset: 0x0000A1B3
		// (set) Token: 0x06000268 RID: 616 RVA: 0x0000BFBB File Offset: 0x0000A1BB
		public int Id { get; private set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000269 RID: 617 RVA: 0x0000BFC4 File Offset: 0x0000A1C4
		// (set) Token: 0x0600026A RID: 618 RVA: 0x0000BFCC File Offset: 0x0000A1CC
		public object LoadedObject { get; private set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600026B RID: 619 RVA: 0x0000BFD5 File Offset: 0x0000A1D5
		// (set) Token: 0x0600026C RID: 620 RVA: 0x0000BFDD File Offset: 0x0000A1DD
		public object Target { get; private set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600026D RID: 621 RVA: 0x0000BFE6 File Offset: 0x0000A1E6
		// (set) Token: 0x0600026E RID: 622 RVA: 0x0000BFEE File Offset: 0x0000A1EE
		public short PropertyCount { get; private set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600026F RID: 623 RVA: 0x0000BFF7 File Offset: 0x0000A1F7
		// (set) Token: 0x06000270 RID: 624 RVA: 0x0000BFFF File Offset: 0x0000A1FF
		public short ChildStructCount { get; private set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000271 RID: 625 RVA: 0x0000C008 File Offset: 0x0000A208
		// (set) Token: 0x06000272 RID: 626 RVA: 0x0000C010 File Offset: 0x0000A210
		public TypeDefinition TypeDefinition { get; private set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000273 RID: 627 RVA: 0x0000C019 File Offset: 0x0000A219
		// (set) Token: 0x06000274 RID: 628 RVA: 0x0000C021 File Offset: 0x0000A221
		public LoadContext Context { get; private set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000275 RID: 629 RVA: 0x0000C02A File Offset: 0x0000A22A
		// (set) Token: 0x06000276 RID: 630 RVA: 0x0000C032 File Offset: 0x0000A232
		public SaveId SaveId { get; private set; }

		// Token: 0x06000277 RID: 631 RVA: 0x0000C03B File Offset: 0x0000A23B
		public ObjectHeaderLoadData(LoadContext context, int id)
		{
			this.Context = context;
			this.Id = id;
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000C054 File Offset: 0x0000A254
		public void InitialieReaders(SaveEntryFolder saveEntryFolder)
		{
			BinaryReader binaryReader = saveEntryFolder.GetEntry(new EntryId(-1, SaveEntryExtension.Basics)).GetBinaryReader();
			this.SaveId = SaveId.ReadSaveIdFrom(binaryReader);
			this.PropertyCount = binaryReader.ReadShort();
			this.ChildStructCount = binaryReader.ReadShort();
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000C098 File Offset: 0x0000A298
		public void CreateObject()
		{
			this.TypeDefinition = this.Context.DefinitionContext.TryGetTypeDefinition(this.SaveId) as TypeDefinition;
			if (this.TypeDefinition != null)
			{
				Type type = this.TypeDefinition.Type;
				this.LoadedObject = FormatterServices.GetUninitializedObject(type);
				this.Target = this.LoadedObject;
			}
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000C0F2 File Offset: 0x0000A2F2
		public void AdvancedResolveObject(MetaData metaData, ObjectLoadData objectLoadData)
		{
			this.Target = this.TypeDefinition.AdvancedResolveObject(this.LoadedObject, metaData, objectLoadData);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000C10D File Offset: 0x0000A30D
		public void ResolveObject()
		{
			this.Target = this.TypeDefinition.ResolveObject(this.LoadedObject);
		}
	}
}
