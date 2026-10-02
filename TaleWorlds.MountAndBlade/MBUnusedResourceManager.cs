using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001E1 RID: 481
	public class MBUnusedResourceManager
	{
		// Token: 0x06001C54 RID: 7252 RVA: 0x00061193 File Offset: 0x0005F393
		public static void SetMeshUsed(string meshName)
		{
			MBAPI.IMBWorld.SetMeshUsed(meshName);
		}

		// Token: 0x06001C55 RID: 7253 RVA: 0x000611A0 File Offset: 0x0005F3A0
		public static void SetMaterialUsed(string meshName)
		{
			MBAPI.IMBWorld.SetMaterialUsed(meshName);
		}

		// Token: 0x06001C56 RID: 7254 RVA: 0x000611AD File Offset: 0x0005F3AD
		public static void SetBodyUsed(string bodyName)
		{
			MBAPI.IMBWorld.SetBodyUsed(bodyName);
		}
	}
}
