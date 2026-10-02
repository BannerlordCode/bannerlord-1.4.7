using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.BannerEditor
{
	// Token: 0x0200004F RID: 79
	[GameStateScreen(typeof(BannerEditorState))]
	public class GauntletBannerEditorScreen : ScreenBase, IGameStateListener
	{
		// Token: 0x060003CC RID: 972 RVA: 0x00017CE4 File Offset: 0x00015EE4
		public GauntletBannerEditorScreen(BannerEditorState bannerEditorState)
		{
			LoadingWindow.EnableGlobalLoadingWindow();
			this._clan = bannerEditorState.GetClan();
			this._bannerEditorLayer = new BannerEditorView(bannerEditorState.GetCharacter(), bannerEditorState.GetClan().Banner, new ControlCharacterCreationStage(this.OnDone), new TextObject("{=WiNRdfsm}Done", null), new ControlCharacterCreationStage(this.OnCancel), new TextObject("{=3CpNUnVl}Cancel", null), null, null, null, null, null);
			this._bannerEditorLayer.DataSource.SetClanRelatedRules(bannerEditorState.GetClan().Kingdom == null);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00017D75 File Offset: 0x00015F75
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			this._bannerEditorLayer.OnTick(dt);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00017D8C File Offset: 0x00015F8C
		public void OnDone()
		{
			uint primaryColor = this._bannerEditorLayer.DataSource.BannerVM.Banner.GetPrimaryColor();
			uint firstIconColor = this._bannerEditorLayer.DataSource.BannerVM.Banner.GetFirstIconColor();
			this._clan.Color2 = firstIconColor;
			if (this._bannerEditorLayer.DataSource.CanChangeBackgroundColor)
			{
				this._clan.Color = primaryColor;
				this._clan.UpdateBannerColor(primaryColor, firstIconColor);
			}
			else
			{
				this._clan.UpdateBannerColor(this._clan.Color, firstIconColor);
			}
			Game.Current.GameStateManager.PopState(0);
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00017E2F File Offset: 0x0001602F
		public void OnCancel()
		{
			Game.Current.GameStateManager.PopState(0);
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00017E41 File Offset: 0x00016041
		protected override void OnInitialize()
		{
			base.OnInitialize();
			Game.Current.GameStateManager.RegisterActiveStateDisableRequest(this);
			InformationManager.HideAllMessages();
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00017E5E File Offset: 0x0001605E
		protected override void OnFinalize()
		{
			base.OnFinalize();
			this._bannerEditorLayer.OnFinalize();
			if (LoadingWindow.IsLoadingWindowActive)
			{
				LoadingWindow.DisableGlobalLoadingWindow();
			}
			Game.Current.GameStateManager.UnregisterActiveStateDisableRequest(this);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00017E8D File Offset: 0x0001608D
		protected override void OnActivate()
		{
			base.OnActivate();
			base.AddLayer(this._bannerEditorLayer.GauntletLayer);
			base.AddLayer(this._bannerEditorLayer.SceneLayer);
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00017EB7 File Offset: 0x000160B7
		protected override void OnDeactivate()
		{
			this._bannerEditorLayer.OnDeactivate();
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00017EC4 File Offset: 0x000160C4
		void IGameStateListener.OnActivate()
		{
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00017EC6 File Offset: 0x000160C6
		void IGameStateListener.OnDeactivate()
		{
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x00017EC8 File Offset: 0x000160C8
		void IGameStateListener.OnInitialize()
		{
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00017ECA File Offset: 0x000160CA
		void IGameStateListener.OnFinalize()
		{
		}

		// Token: 0x040001BC RID: 444
		private const int ViewOrderPriority = 15;

		// Token: 0x040001BD RID: 445
		private readonly BannerEditorView _bannerEditorLayer;

		// Token: 0x040001BE RID: 446
		private readonly Clan _clan;
	}
}
