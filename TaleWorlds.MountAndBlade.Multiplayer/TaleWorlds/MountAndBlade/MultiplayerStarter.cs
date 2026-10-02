using System;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000022 RID: 34
	public class MultiplayerStarter
	{
		// Token: 0x060001B1 RID: 433 RVA: 0x00007C62 File Offset: 0x00005E62
		public MultiplayerStarter(MBObjectManager objectManager)
		{
			this._objectManager = objectManager;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00007C71 File Offset: 0x00005E71
		public void LoadXMLFromFile(string xmlPath, string xsdPath)
		{
			this._objectManager.LoadOneXmlFromFile(xmlPath, xsdPath, false);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00007C81 File Offset: 0x00005E81
		public void ClearEmptyObjects()
		{
			this._objectManager.UnregisterNonReadyObjects();
		}

		// Token: 0x04000065 RID: 101
		private readonly MBObjectManager _objectManager;
	}
}
