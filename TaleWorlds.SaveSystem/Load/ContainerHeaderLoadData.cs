using System;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Load
{
	// Token: 0x02000035 RID: 53
	public class ContainerHeaderLoadData
	{
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000217 RID: 535 RVA: 0x0000A9A6 File Offset: 0x00008BA6
		// (set) Token: 0x06000218 RID: 536 RVA: 0x0000A9AE File Offset: 0x00008BAE
		public int Id { get; private set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000219 RID: 537 RVA: 0x0000A9B7 File Offset: 0x00008BB7
		// (set) Token: 0x0600021A RID: 538 RVA: 0x0000A9BF File Offset: 0x00008BBF
		public object Target { get; private set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600021B RID: 539 RVA: 0x0000A9C8 File Offset: 0x00008BC8
		// (set) Token: 0x0600021C RID: 540 RVA: 0x0000A9D0 File Offset: 0x00008BD0
		public LoadContext Context { get; private set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600021D RID: 541 RVA: 0x0000A9D9 File Offset: 0x00008BD9
		// (set) Token: 0x0600021E RID: 542 RVA: 0x0000A9E1 File Offset: 0x00008BE1
		public ContainerDefinition TypeDefinition { get; private set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600021F RID: 543 RVA: 0x0000A9EA File Offset: 0x00008BEA
		// (set) Token: 0x06000220 RID: 544 RVA: 0x0000A9F2 File Offset: 0x00008BF2
		public SaveId SaveId { get; private set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000221 RID: 545 RVA: 0x0000A9FB File Offset: 0x00008BFB
		// (set) Token: 0x06000222 RID: 546 RVA: 0x0000AA03 File Offset: 0x00008C03
		public int ElementCount { get; private set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000223 RID: 547 RVA: 0x0000AA0C File Offset: 0x00008C0C
		// (set) Token: 0x06000224 RID: 548 RVA: 0x0000AA14 File Offset: 0x00008C14
		public ContainerType ContainerType { get; private set; }

		// Token: 0x06000225 RID: 549 RVA: 0x0000AA1D File Offset: 0x00008C1D
		public ContainerHeaderLoadData(LoadContext context, int id)
		{
			this.Context = context;
			this.Id = id;
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000AA33 File Offset: 0x00008C33
		public bool GetObjectTypeDefinition()
		{
			this.TypeDefinition = this.Context.DefinitionContext.TryGetTypeDefinition(this.SaveId) as ContainerDefinition;
			return this.TypeDefinition != null;
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000AA60 File Offset: 0x00008C60
		public void CreateObject()
		{
			Type type = this.TypeDefinition.Type;
			if (this.ContainerType == ContainerType.Array)
			{
				this.Target = Activator.CreateInstance(type, new object[] { this.ElementCount });
				return;
			}
			if (this.ContainerType == ContainerType.List)
			{
				this.Target = Activator.CreateInstance(typeof(MBList<>).MakeGenericType(type.GetGenericArguments()));
				return;
			}
			this.Target = Activator.CreateInstance(type);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000AADC File Offset: 0x00008CDC
		public void InitialieReaders(SaveEntryFolder saveEntryFolder)
		{
			BinaryReader binaryReader = saveEntryFolder.GetEntry(new EntryId(-1, SaveEntryExtension.Object)).GetBinaryReader();
			this.SaveId = SaveId.ReadSaveIdFrom(binaryReader);
			this.ContainerType = (ContainerType)binaryReader.ReadByte();
			this.ElementCount = binaryReader.ReadInt();
		}
	}
}
