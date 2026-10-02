using System;
using System.Reflection;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001C8 RID: 456
	public class MissionInfo
	{
		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06001B65 RID: 7013 RVA: 0x0005F93D File Offset: 0x0005DB3D
		// (set) Token: 0x06001B66 RID: 7014 RVA: 0x0005F945 File Offset: 0x0005DB45
		public string Name { get; set; }

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06001B67 RID: 7015 RVA: 0x0005F94E File Offset: 0x0005DB4E
		// (set) Token: 0x06001B68 RID: 7016 RVA: 0x0005F956 File Offset: 0x0005DB56
		public MethodInfo Creator { get; set; }

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06001B69 RID: 7017 RVA: 0x0005F95F File Offset: 0x0005DB5F
		// (set) Token: 0x06001B6A RID: 7018 RVA: 0x0005F967 File Offset: 0x0005DB67
		public Type Manager { get; set; }

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06001B6B RID: 7019 RVA: 0x0005F970 File Offset: 0x0005DB70
		// (set) Token: 0x06001B6C RID: 7020 RVA: 0x0005F978 File Offset: 0x0005DB78
		public bool UsableByEditor { get; set; }
	}
}
