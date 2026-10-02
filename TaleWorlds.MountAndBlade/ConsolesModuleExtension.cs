using System;
using System.Collections.Generic;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000206 RID: 518
	public class ConsolesModuleExtension : IPlatformModuleExtension
	{
		// Token: 0x06001E11 RID: 7697 RVA: 0x00067A5C File Offset: 0x00065C5C
		public ConsolesModuleExtension()
		{
			this._modulePaths = new List<string>();
		}

		// Token: 0x06001E12 RID: 7698 RVA: 0x00067A70 File Offset: 0x00065C70
		public void Initialize(List<string> args)
		{
			string platformModulePaths = Utilities.GetPlatformModulePaths();
			Debug.Print("ConsolesModuleExtension::Initialize::" + platformModulePaths + "\n", 0, Debug.DebugColor.White, 17592186044416UL);
			if (platformModulePaths.Length > 0)
			{
				this._modulePaths = new List<string>(platformModulePaths.Split(new char[] { '$' }));
				return;
			}
			this._modulePaths = new List<string>();
		}

		// Token: 0x06001E13 RID: 7699 RVA: 0x00067AD5 File Offset: 0x00065CD5
		public string[] GetModulePaths()
		{
			return this._modulePaths.ToArray();
		}

		// Token: 0x06001E14 RID: 7700 RVA: 0x00067AE2 File Offset: 0x00065CE2
		public void Destroy()
		{
		}

		// Token: 0x06001E15 RID: 7701 RVA: 0x00067AE4 File Offset: 0x00065CE4
		public void SetLauncherMode(bool isLauncherModeActive)
		{
		}

		// Token: 0x06001E16 RID: 7702 RVA: 0x00067AE6 File Offset: 0x00065CE6
		public bool CheckEntitlement(string title)
		{
			return true;
		}

		// Token: 0x04000A46 RID: 2630
		private List<string> _modulePaths;
	}
}
