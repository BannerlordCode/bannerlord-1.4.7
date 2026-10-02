using System;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI
{
	// Token: 0x02000003 RID: 3
	public class MultiplayerAdminInformationScreen : GlobalLayer
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000009 RID: 9 RVA: 0x0000212C File Offset: 0x0000032C
		// (set) Token: 0x0600000A RID: 10 RVA: 0x00002133 File Offset: 0x00000333
		public static MultiplayerAdminInformationScreen Current { get; private set; }

		// Token: 0x0600000B RID: 11 RVA: 0x0000213C File Offset: 0x0000033C
		public MultiplayerAdminInformationScreen()
		{
			this._dataSource = new MultiplayerAdminInformationVM();
			GauntletLayer gauntletLayer = new GauntletLayer("MultiplayerAdminInformation", 15300, false);
			this._movie = gauntletLayer.LoadMovie("MultiplayerAdminInformation", this._dataSource);
			base.Layer = gauntletLayer;
			InformationManager.OnAddSystemNotification += this.OnSystemNotificationReceived;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000219A File Offset: 0x0000039A
		public static void OnInitialize()
		{
			if (MultiplayerAdminInformationScreen.Current == null)
			{
				MultiplayerAdminInformationScreen.Current = new MultiplayerAdminInformationScreen();
				ScreenManager.AddGlobalLayer(MultiplayerAdminInformationScreen.Current, false);
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000021B8 File Offset: 0x000003B8
		public void OnFinalize()
		{
			InformationManager.OnAddSystemNotification -= this.OnSystemNotificationReceived;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000021CB File Offset: 0x000003CB
		private void OnSystemNotificationReceived(string obj)
		{
			this._dataSource.OnNewMessageReceived(obj);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000021DC File Offset: 0x000003DC
		public static void OnRemove()
		{
			if (MultiplayerAdminInformationScreen.Current != null)
			{
				MultiplayerAdminInformationScreen.Current.OnFinalize();
				ScreenManager.RemoveGlobalLayer(MultiplayerAdminInformationScreen.Current);
				(MultiplayerAdminInformationScreen.Current.Layer as GauntletLayer).ReleaseMovie(MultiplayerAdminInformationScreen.Current._movie);
				MultiplayerAdminInformationScreen.Current._dataSource = null;
				MultiplayerAdminInformationScreen.Current = null;
			}
		}

		// Token: 0x04000004 RID: 4
		private MultiplayerAdminInformationVM _dataSource;

		// Token: 0x04000005 RID: 5
		private GauntletMovieIdentifier _movie;
	}
}
