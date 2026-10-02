using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001DE RID: 478
	public abstract class MBSubModuleBase
	{
		// Token: 0x06001C21 RID: 7201 RVA: 0x00061026 File Offset: 0x0005F226
		protected internal virtual void OnSubModuleLoad()
		{
		}

		// Token: 0x06001C22 RID: 7202 RVA: 0x00061028 File Offset: 0x0005F228
		protected internal virtual void OnSubModuleUnloaded()
		{
		}

		// Token: 0x06001C23 RID: 7203 RVA: 0x0006102A File Offset: 0x0005F22A
		protected internal virtual void OnBeforeInitialModuleScreenSetAsRoot()
		{
		}

		// Token: 0x06001C24 RID: 7204 RVA: 0x0006102C File Offset: 0x0005F22C
		protected internal virtual void RegisterSubModuleTypes()
		{
		}

		// Token: 0x06001C25 RID: 7205 RVA: 0x0006102E File Offset: 0x0005F22E
		protected internal virtual void OnNewModuleLoad()
		{
		}

		// Token: 0x06001C26 RID: 7206 RVA: 0x00061030 File Offset: 0x0005F230
		public virtual void OnConfigChanged()
		{
		}

		// Token: 0x06001C27 RID: 7207 RVA: 0x00061032 File Offset: 0x0005F232
		protected internal virtual void OnBeforeGameStart(MBGameManager mbGameManager, List<string> disabledModules)
		{
		}

		// Token: 0x06001C28 RID: 7208 RVA: 0x00061034 File Offset: 0x0005F234
		protected internal virtual void OnGameStart(Game game, IGameStarter gameStarterObject)
		{
		}

		// Token: 0x06001C29 RID: 7209 RVA: 0x00061036 File Offset: 0x0005F236
		protected internal virtual void OnApplicationTick(float dt)
		{
		}

		// Token: 0x06001C2A RID: 7210 RVA: 0x00061038 File Offset: 0x0005F238
		protected internal virtual void AfterAsyncTickTick(float dt)
		{
		}

		// Token: 0x06001C2B RID: 7211 RVA: 0x0006103A File Offset: 0x0005F23A
		protected internal virtual void InitializeGameStarter(Game game, IGameStarter starterObject)
		{
		}

		// Token: 0x06001C2C RID: 7212 RVA: 0x0006103C File Offset: 0x0005F23C
		public virtual void OnGameLoaded(Game game, object initializerObject)
		{
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x0006103E File Offset: 0x0005F23E
		public virtual void OnAfterGameLoaded(Game game)
		{
		}

		// Token: 0x06001C2E RID: 7214 RVA: 0x00061040 File Offset: 0x0005F240
		public virtual void OnNewGameCreated(Game game, object initializerObject)
		{
		}

		// Token: 0x06001C2F RID: 7215 RVA: 0x00061042 File Offset: 0x0005F242
		public virtual void BeginGameStart(Game game)
		{
		}

		// Token: 0x06001C30 RID: 7216 RVA: 0x00061044 File Offset: 0x0005F244
		public virtual void OnCampaignStart(Game game, object starterObject)
		{
		}

		// Token: 0x06001C31 RID: 7217 RVA: 0x00061046 File Offset: 0x0005F246
		public virtual void RegisterSubModuleObjects(bool isSavedCampaign)
		{
		}

		// Token: 0x06001C32 RID: 7218 RVA: 0x00061048 File Offset: 0x0005F248
		public virtual void AfterRegisterSubModuleObjects(bool isSavedCampaign)
		{
		}

		// Token: 0x06001C33 RID: 7219 RVA: 0x0006104A File Offset: 0x0005F24A
		public virtual void OnMultiplayerGameStart(Game game, object starterObject)
		{
		}

		// Token: 0x06001C34 RID: 7220 RVA: 0x0006104C File Offset: 0x0005F24C
		public virtual void OnGameInitializationFinished(Game game)
		{
		}

		// Token: 0x06001C35 RID: 7221 RVA: 0x0006104E File Offset: 0x0005F24E
		public virtual void OnAfterGameInitializationFinished(Game game, object starterObject)
		{
		}

		// Token: 0x06001C36 RID: 7222 RVA: 0x00061050 File Offset: 0x0005F250
		public virtual bool DoLoading(Game game)
		{
			return true;
		}

		// Token: 0x06001C37 RID: 7223 RVA: 0x00061053 File Offset: 0x0005F253
		public virtual void OnGameEnd(Game game)
		{
		}

		// Token: 0x06001C38 RID: 7224 RVA: 0x00061055 File Offset: 0x0005F255
		public virtual void OnMissionBehaviorInitialize(Mission mission)
		{
		}

		// Token: 0x06001C39 RID: 7225 RVA: 0x00061057 File Offset: 0x0005F257
		public virtual void OnBeforeMissionBehaviorInitialize(Mission mission)
		{
		}

		// Token: 0x06001C3A RID: 7226 RVA: 0x00061059 File Offset: 0x0005F259
		public virtual void OnInitialState()
		{
		}

		// Token: 0x06001C3B RID: 7227 RVA: 0x0006105B File Offset: 0x0005F25B
		protected internal virtual void OnNetworkTick(float dt)
		{
		}

		// Token: 0x06001C3C RID: 7228 RVA: 0x0006105D File Offset: 0x0005F25D
		public virtual void OnSubModuleActivated()
		{
		}

		// Token: 0x06001C3D RID: 7229 RVA: 0x0006105F File Offset: 0x0005F25F
		public virtual void OnSubModuleDeactivated()
		{
		}

		// Token: 0x06001C3E RID: 7230 RVA: 0x00061061 File Offset: 0x0005F261
		public virtual void InitializeSubModuleGameObjects(Game game)
		{
		}
	}
}
