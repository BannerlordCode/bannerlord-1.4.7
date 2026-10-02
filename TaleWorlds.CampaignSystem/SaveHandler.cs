using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x020000A8 RID: 168
	public class SaveHandler
	{
		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06001351 RID: 4945 RVA: 0x0005A3BD File Offset: 0x000585BD
		// (set) Token: 0x06001352 RID: 4946 RVA: 0x0005A3C5 File Offset: 0x000585C5
		public IMainHeroVisualSupplier MainHeroVisualSupplier { get; set; }

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06001353 RID: 4947 RVA: 0x0005A3CE File Offset: 0x000585CE
		public bool IsSaving
		{
			get
			{
				return !this.SaveArgsQueue.IsEmpty<SaveHandler.SaveArgs>();
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06001354 RID: 4948 RVA: 0x0005A3DE File Offset: 0x000585DE
		public string IronmanModSaveName
		{
			get
			{
				return "Ironman" + Campaign.Current.UniqueGameId;
			}
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06001355 RID: 4949 RVA: 0x0005A3F4 File Offset: 0x000585F4
		private bool _isAutoSaveEnabled
		{
			get
			{
				return this.AutoSaveInterval > -1;
			}
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06001356 RID: 4950 RVA: 0x0005A3FF File Offset: 0x000585FF
		private double _autoSavePriorityTimeLimit
		{
			get
			{
				return (double)this.AutoSaveInterval * 0.75;
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06001357 RID: 4951 RVA: 0x0005A412 File Offset: 0x00058612
		public int AutoSaveInterval
		{
			get
			{
				ISaveManager sandBoxSaveManager = Campaign.Current.SandBoxManager.SandBoxSaveManager;
				if (sandBoxSaveManager == null)
				{
					return 15;
				}
				return sandBoxSaveManager.GetAutoSaveInterval();
			}
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x0005A42F File Offset: 0x0005862F
		public void QuickSaveCurrentGame()
		{
			this.SetSaveArgs(SaveHandler.SaveArgs.SaveMode.QuickSave, null);
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x0005A439 File Offset: 0x00058639
		public void SaveAs(string saveName)
		{
			this.SetSaveArgs(SaveHandler.SaveArgs.SaveMode.SaveAs, saveName);
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x0005A444 File Offset: 0x00058644
		private void TryAutoSave(bool isPriority)
		{
			MapState mapState;
			if (this._isAutoSaveEnabled && (mapState = GameStateManager.Current.ActiveState as MapState) != null && !mapState.MapConversationActive)
			{
				double totalMinutes = (DateTime.Now - this._lastAutoSaveTime).TotalMinutes;
				double num = (isPriority ? this._autoSavePriorityTimeLimit : ((double)this.AutoSaveInterval));
				if (totalMinutes > num)
				{
					this.SetSaveArgs(SaveHandler.SaveArgs.SaveMode.AutoSave, null);
				}
			}
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x0005A4AA File Offset: 0x000586AA
		public void CampaignTick()
		{
			if (Campaign.Current.TimeControlMode != CampaignTimeControlMode.Stop)
			{
				this.TryAutoSave(false);
			}
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x0005A4C0 File Offset: 0x000586C0
		internal void SaveTick()
		{
			if (!this.SaveArgsQueue.IsEmpty<SaveHandler.SaveArgs>())
			{
				switch (this._saveStep)
				{
				case SaveHandler.SaveSteps.PreSave:
					this._saveStep++;
					this.OnSaveStarted();
					return;
				case SaveHandler.SaveSteps.Saving:
				{
					this._saveStep++;
					CampaignEventDispatcher.Instance.OnBeforeSave();
					if (CampaignOptions.IsIronmanMode)
					{
						MBSaveLoad.SaveAsCurrentGame(this.GetSaveMetaData(), this.IronmanModSaveName, new Action<ValueTuple<SaveResult, string>>(this.OnSaveCompleted));
						return;
					}
					SaveHandler.SaveArgs saveArgs = this.SaveArgsQueue.Peek();
					switch (saveArgs.Mode)
					{
					case SaveHandler.SaveArgs.SaveMode.SaveAs:
						MBSaveLoad.SaveAsCurrentGame(this.GetSaveMetaData(), saveArgs.Name, new Action<ValueTuple<SaveResult, string>>(this.OnSaveCompleted));
						return;
					case SaveHandler.SaveArgs.SaveMode.QuickSave:
						MBSaveLoad.QuickSaveCurrentGame(this.GetSaveMetaData(), new Action<ValueTuple<SaveResult, string>>(this.OnSaveCompleted));
						return;
					case SaveHandler.SaveArgs.SaveMode.AutoSave:
						MBSaveLoad.AutoSaveCurrentGame(this.GetSaveMetaData(), new Action<ValueTuple<SaveResult, string>>(this.OnSaveCompleted));
						return;
					default:
						return;
					}
					break;
				}
				case SaveHandler.SaveSteps.AwaitingCompletion:
					return;
				}
				this._saveStep++;
			}
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x0005A5D3 File Offset: 0x000587D3
		private void OnSaveCompleted(ValueTuple<SaveResult, string> result)
		{
			this._saveStep = SaveHandler.SaveSteps.PreSave;
			if (this.SaveArgsQueue.Dequeue().Mode == SaveHandler.SaveArgs.SaveMode.AutoSave)
			{
				this._lastAutoSaveTime = DateTime.Now;
			}
			this.OnSaveEnded(result.Item1 == SaveResult.Success, result.Item2);
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x0005A60F File Offset: 0x0005880F
		public void SignalAutoSave()
		{
			this.TryAutoSave(true);
		}

		// Token: 0x0600135F RID: 4959 RVA: 0x0005A618 File Offset: 0x00058818
		private void OnSaveStarted()
		{
			Campaign.Current.WaitAsyncTasks();
			CampaignEventDispatcher.Instance.OnSaveStarted();
			MBInformationManager.HideInformations();
		}

		// Token: 0x06001360 RID: 4960 RVA: 0x0005A634 File Offset: 0x00058834
		private void OnSaveEnded(bool isSaveSuccessful, string newSaveGameName)
		{
			ISaveManager sandBoxSaveManager = Campaign.Current.SandBoxManager.SandBoxSaveManager;
			if (sandBoxSaveManager != null)
			{
				sandBoxSaveManager.OnSaveOver(isSaveSuccessful, newSaveGameName);
			}
			CampaignEventDispatcher.Instance.OnSaveOver(isSaveSuccessful, newSaveGameName);
			if (!isSaveSuccessful)
			{
				MBInformationManager.AddQuickInformation(new TextObject("{=u9PPxTNL}Save Error!", null), 0, null, null, "");
			}
		}

		// Token: 0x06001361 RID: 4961 RVA: 0x0005A684 File Offset: 0x00058884
		private void SetSaveArgs(SaveHandler.SaveArgs.SaveMode saveType, string saveName = null)
		{
			this.SaveArgsQueue.Enqueue(new SaveHandler.SaveArgs(saveType, saveName));
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x0005A698 File Offset: 0x00058898
		public void ForceAutoSave()
		{
			if (!Campaign.Current.SandBoxManager.SandBoxSaveManager.IsAutoSaveDisabled())
			{
				this.SetSaveArgs(SaveHandler.SaveArgs.SaveMode.AutoSave, null);
			}
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x0005A6B8 File Offset: 0x000588B8
		public CampaignSaveMetaDataArgs GetSaveMetaData()
		{
			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
			list.Add(new KeyValuePair<string, string>("UniqueGameId", Campaign.Current.UniqueGameId ?? ""));
			list.Add(new KeyValuePair<string, string>("MainHeroLevel", Hero.MainHero.Level.ToString(SaveHandler._invariantCulture)));
			list.Add(new KeyValuePair<string, string>("MainPartyFood", Campaign.Current.MainParty.Food.ToString(SaveHandler._invariantCulture)));
			list.Add(new KeyValuePair<string, string>("MainHeroGold", Hero.MainHero.Gold.ToString(SaveHandler._invariantCulture)));
			list.Add(new KeyValuePair<string, string>("ClanInfluence", Clan.PlayerClan.Influence.ToString(SaveHandler._invariantCulture)));
			list.Add(new KeyValuePair<string, string>("ClanFiefs", Clan.PlayerClan.Settlements.Count.ToString(SaveHandler._invariantCulture)));
			list.Add(new KeyValuePair<string, string>("MainPartyShipCount", Campaign.Current.MainParty.Ships.Count.ToString(SaveHandler._invariantCulture)));
			list.Add(new KeyValuePair<string, string>("MainPartyHealthyMemberCount", Campaign.Current.MainParty.MemberRoster.TotalHealthyCount.ToString(SaveHandler._invariantCulture)));
			list.Add(new KeyValuePair<string, string>("MainPartyPrisonerMemberCount", Campaign.Current.MainParty.PrisonRoster.TotalManCount.ToString(SaveHandler._invariantCulture)));
			list.Add(new KeyValuePair<string, string>("MainPartyWoundedMemberCount", Campaign.Current.MainParty.MemberRoster.TotalWounded.ToString(SaveHandler._invariantCulture)));
			string text = "CharacterName";
			TextObject name = Hero.MainHero.Name;
			list.Add(new KeyValuePair<string, string>(text, (name != null) ? name.ToString() : null));
			list.Add(new KeyValuePair<string, string>("DayLong", Campaign.Current.Models.CampaignTimeModel.CampaignStartTime.ElapsedDaysUntilNow.ToString(SaveHandler._invariantCulture)));
			list.Add(new KeyValuePair<string, string>("ClanBannerCode", Clan.PlayerClan.Banner.Serialize()));
			string text2 = "MainHeroVisual";
			IMainHeroVisualSupplier mainHeroVisualSupplier = this.MainHeroVisualSupplier;
			list.Add(new KeyValuePair<string, string>(text2, ((mainHeroVisualSupplier != null) ? mainHeroVisualSupplier.GetMainHeroVisualCode() : null) ?? string.Empty));
			list.Add(new KeyValuePair<string, string>("IronmanMode", (CampaignOptions.IsIronmanMode ? 1 : 0).ToString()));
			list.Add(new KeyValuePair<string, string>("HealthPercentage", MBMath.ClampInt(Hero.MainHero.HitPoints * 100 / Hero.MainHero.MaxHitPoints, 1, 100).ToString()));
			list.Add(new KeyValuePair<string, string>("NewGameVersion", string.IsNullOrEmpty(Campaign.Current.NewGameVersion) ? string.Empty : Campaign.Current.NewGameVersion));
			List<KeyValuePair<string, string>> list2 = list;
			CampaignEventDispatcher.Instance.CollectMetadataEntries(list2);
			return new CampaignSaveMetaDataArgs((from x in ModuleHelper.GetActiveModules()
				select x.Id).ToArray<string>(), list2.ToArray());
		}

		// Token: 0x0400063B RID: 1595
		private SaveHandler.SaveSteps _saveStep;

		// Token: 0x0400063C RID: 1596
		private static readonly CultureInfo _invariantCulture = CultureInfo.InvariantCulture;

		// Token: 0x0400063E RID: 1598
		private Queue<SaveHandler.SaveArgs> SaveArgsQueue = new Queue<SaveHandler.SaveArgs>();

		// Token: 0x0400063F RID: 1599
		private DateTime _lastAutoSaveTime = DateTime.Now;

		// Token: 0x02000545 RID: 1349
		private readonly struct SaveArgs
		{
			// Token: 0x06004D47 RID: 19783 RVA: 0x0017FEDB File Offset: 0x0017E0DB
			public SaveArgs(SaveHandler.SaveArgs.SaveMode mode, string name)
			{
				this.Mode = mode;
				this.Name = name;
			}

			// Token: 0x0400169D RID: 5789
			public readonly SaveHandler.SaveArgs.SaveMode Mode;

			// Token: 0x0400169E RID: 5790
			public readonly string Name;

			// Token: 0x020008B2 RID: 2226
			public enum SaveMode
			{
				// Token: 0x0400250C RID: 9484
				SaveAs,
				// Token: 0x0400250D RID: 9485
				QuickSave,
				// Token: 0x0400250E RID: 9486
				AutoSave
			}
		}

		// Token: 0x02000546 RID: 1350
		private enum SaveSteps
		{
			// Token: 0x040016A0 RID: 5792
			PreSave,
			// Token: 0x040016A1 RID: 5793
			Saving = 2,
			// Token: 0x040016A2 RID: 5794
			AwaitingCompletion
		}
	}
}
