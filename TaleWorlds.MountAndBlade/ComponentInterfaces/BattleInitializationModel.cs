using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.ComponentInterfaces
{
	// Token: 0x020003FC RID: 1020
	public abstract class BattleInitializationModel : MBGameModel<BattleInitializationModel>
	{
		// Token: 0x17000A0C RID: 2572
		// (get) Token: 0x06003791 RID: 14225 RVA: 0x000E4E42 File Offset: 0x000E3042
		// (set) Token: 0x06003792 RID: 14226 RVA: 0x000E4E49 File Offset: 0x000E3049
		public static bool BypassPlayerDeployment { get; private set; }

		// Token: 0x06003793 RID: 14227
		public abstract List<FormationClass> GetAllAvailableTroopTypes();

		// Token: 0x06003794 RID: 14228
		protected abstract bool CanPlayerSideDeployWithOrderOfBattleAux();

		// Token: 0x06003795 RID: 14229 RVA: 0x000E4E51 File Offset: 0x000E3051
		public bool CanPlayerSideDeployWithOrderOfBattle()
		{
			if (!this._isCanPlayerSideDeployWithOOBCached)
			{
				this._canPlayerSideDeployWithOOB = !BattleInitializationModel.BypassPlayerDeployment && this.CanPlayerSideDeployWithOrderOfBattleAux();
				this._isCanPlayerSideDeployWithOOBCached = true;
			}
			return this._canPlayerSideDeployWithOOB;
		}

		// Token: 0x06003796 RID: 14230 RVA: 0x000E4E7E File Offset: 0x000E307E
		public void InitializeModel()
		{
			this._isCanPlayerSideDeployWithOOBCached = false;
			this._isInitialized = true;
		}

		// Token: 0x06003797 RID: 14231 RVA: 0x000E4E8E File Offset: 0x000E308E
		public void FinalizeModel()
		{
			this._isInitialized = false;
		}

		// Token: 0x06003798 RID: 14232 RVA: 0x000E4E98 File Offset: 0x000E3098
		public static void SetBypassPlayerDeployment(bool value)
		{
			MissionGameModels missionGameModels = MissionGameModels.Current;
			BattleInitializationModel battleInitializationModel = ((missionGameModels != null) ? missionGameModels.BattleInitializationModel : null);
			if (battleInitializationModel != null && BattleInitializationModel.BypassPlayerDeployment != value)
			{
				battleInitializationModel._isCanPlayerSideDeployWithOOBCached = false;
			}
			BattleInitializationModel.BypassPlayerDeployment = value;
		}

		// Token: 0x040017D4 RID: 6100
		public const int MinimumTroopCountForPlayerDeployment = 20;

		// Token: 0x040017D6 RID: 6102
		private bool _canPlayerSideDeployWithOOB;

		// Token: 0x040017D7 RID: 6103
		private bool _isCanPlayerSideDeployWithOOBCached;

		// Token: 0x040017D8 RID: 6104
		private bool _isInitialized;
	}
}
