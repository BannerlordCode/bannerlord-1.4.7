using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000059 RID: 89
	public class CampaignInformationManager
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060008D0 RID: 2256 RVA: 0x00027134 File Offset: 0x00025334
		// (remove) Token: 0x060008D1 RID: 2257 RVA: 0x00027168 File Offset: 0x00025368
		public static event Func<TextObject, int, BasicCharacterObject, Equipment, MBInformationManager.NotificationPriority, string, MBInformationManager.DialogNotificationHandle> OnDisplayDialog;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060008D2 RID: 2258 RVA: 0x0002719C File Offset: 0x0002539C
		// (remove) Token: 0x060008D3 RID: 2259 RVA: 0x000271D0 File Offset: 0x000253D0
		public static event Func<MBInformationManager.DialogNotificationHandle, MBInformationManager.NotificationStatus> OnGetStatusOfDialogNotification;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060008D4 RID: 2260 RVA: 0x00027204 File Offset: 0x00025404
		// (remove) Token: 0x060008D5 RID: 2261 RVA: 0x00027238 File Offset: 0x00025438
		public static event Action<MBInformationManager.DialogNotificationHandle, bool> OnClearDialogNotification;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060008D6 RID: 2262 RVA: 0x0002726C File Offset: 0x0002546C
		// (remove) Token: 0x060008D7 RID: 2263 RVA: 0x000272A0 File Offset: 0x000254A0
		public static event Func<bool> IsAnyDialogNotificationActiveOrQueued;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060008D8 RID: 2264 RVA: 0x000272D4 File Offset: 0x000254D4
		// (remove) Token: 0x060008D9 RID: 2265 RVA: 0x00027308 File Offset: 0x00025508
		public static event Action<bool> OnClearAllDialogNotifications;

		// Token: 0x060008DA RID: 2266 RVA: 0x0002733B File Offset: 0x0002553B
		public CampaignInformationManager()
		{
			this._mapNotices = new List<InformationData>();
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x00027350 File Offset: 0x00025550
		private void MapNoticeRemoved(InformationData obj)
		{
			int num = -1;
			for (int i = 0; i < this._mapNotices.Count; i++)
			{
				if (obj == this._mapNotices[i])
				{
					num = i;
				}
			}
			if (num >= 0)
			{
				this._mapNotices.RemoveAt(num);
			}
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x00027398 File Offset: 0x00025598
		internal void NewLogEntryAdded(LogEntry log)
		{
			IChatNotification chatNotification;
			if (this._isSessionLaunched && (chatNotification = log as IChatNotification) != null && chatNotification.IsVisibleNotification)
			{
				InformationManager.DisplayMessage(new InformationMessage
				{
					Information = chatNotification.GetNotificationText().ToString(),
					Color = Color.FromUint(Campaign.Current.Models.DiplomacyModel.GetNotificationColor(chatNotification.NotificationType))
				});
			}
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x000273FF File Offset: 0x000255FF
		private void AddInformationData(InformationData informationData)
		{
			List<InformationData> mapNotices = this._mapNotices;
			if (mapNotices != null)
			{
				mapNotices.Add(informationData);
			}
			MBInformationManager.AddNotice(informationData);
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x00027419 File Offset: 0x00025619
		internal void RegisterEvents()
		{
			this._isSessionLaunched = true;
			MBInformationManager.OnRemoveMapNotice += this.MapNoticeRemoved;
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x00027433 File Offset: 0x00025633
		internal void DeRegisterEvents()
		{
			this._isSessionLaunched = false;
			MBInformationManager.OnRemoveMapNotice -= this.MapNoticeRemoved;
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x00027450 File Offset: 0x00025650
		public void OnGameLoaded()
		{
			this._mapNotices.RemoveAll((InformationData t) => t == null || !t.IsValid());
			foreach (InformationData informationData in this._mapNotices)
			{
				MBInformationManager.AddNotice(informationData);
			}
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x000274CC File Offset: 0x000256CC
		public void NewMapNoticeAdded(InformationData informationData)
		{
			this.AddInformationData(informationData);
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x000274D8 File Offset: 0x000256D8
		public bool InformationDataExists<T>(Func<T, bool> predicate) where T : InformationData
		{
			using (List<InformationData>.Enumerator enumerator = this._mapNotices.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					T t;
					if ((t = enumerator.Current as T) != null && (predicate == null || predicate(t)))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x0002754C File Offset: 0x0002574C
		public static MBInformationManager.DialogNotificationHandle AddDialogLine(TextObject text, CharacterObject speakerCharacter, Equipment equipment = null, int extraTimeInMs = 0, MBInformationManager.NotificationPriority priority = MBInformationManager.NotificationPriority.Medium)
		{
			Debug.Print(text.ToString(), 0, Debug.DebugColor.White, 4503599627370496UL);
			Func<TextObject, int, BasicCharacterObject, Equipment, MBInformationManager.NotificationPriority, string, MBInformationManager.DialogNotificationHandle> onDisplayDialog = CampaignInformationManager.OnDisplayDialog;
			return ((onDisplayDialog != null) ? onDisplayDialog(text, extraTimeInMs, speakerCharacter, equipment, priority, CampaignInformationManager.GetSoundPath(text, speakerCharacter)) : null) ?? null;
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x00027588 File Offset: 0x00025788
		public static MBInformationManager.NotificationStatus GetStatusOfDialogNotification(MBInformationManager.DialogNotificationHandle handle)
		{
			Func<MBInformationManager.DialogNotificationHandle, MBInformationManager.NotificationStatus> onGetStatusOfDialogNotification = CampaignInformationManager.OnGetStatusOfDialogNotification;
			if (onGetStatusOfDialogNotification == null)
			{
				return MBInformationManager.NotificationStatus.Inactive;
			}
			return onGetStatusOfDialogNotification(handle);
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x0002759B File Offset: 0x0002579B
		public static void ClearDialogNotification(MBInformationManager.DialogNotificationHandle handle, bool fadeOut = true)
		{
			Action<MBInformationManager.DialogNotificationHandle, bool> onClearDialogNotification = CampaignInformationManager.OnClearDialogNotification;
			if (onClearDialogNotification == null)
			{
				return;
			}
			onClearDialogNotification(handle, fadeOut);
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x000275AE File Offset: 0x000257AE
		public static bool GetIsAnyDialogNotificationActiveOrQueued()
		{
			Func<bool> isAnyDialogNotificationActiveOrQueued = CampaignInformationManager.IsAnyDialogNotificationActiveOrQueued;
			return isAnyDialogNotificationActiveOrQueued != null && isAnyDialogNotificationActiveOrQueued();
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x000275C0 File Offset: 0x000257C0
		public static void ClearAllDialogNotifications(bool fadeOut)
		{
			Action<bool> onClearAllDialogNotifications = CampaignInformationManager.OnClearAllDialogNotifications;
			if (onClearAllDialogNotifications == null)
			{
				return;
			}
			onClearAllDialogNotifications(fadeOut);
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x000275D4 File Offset: 0x000257D4
		private static string GetSoundPath(TextObject line, CharacterObject characterObject)
		{
			VoiceObject voiceObject;
			string text;
			if (characterObject != null && MBTextManager.TryGetVoiceObject(line, out voiceObject, out text))
			{
				return Campaign.Current.Models.VoiceOverModel.GetSoundPathForCharacter(characterObject, voiceObject);
			}
			Debug.FailedAssert("Sound path for voice line not found! Character: " + ((characterObject != null) ? characterObject.ToString() : null) + ", Line: " + line.ToString(), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignInformationManager.cs", "GetSoundPath", 180);
			return null;
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0002763D File Offset: 0x0002583D
		internal static void AutoGeneratedStaticCollectObjectsCampaignInformationManager(object o, List<object> collectedObjects)
		{
			((CampaignInformationManager)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x0002764B File Offset: 0x0002584B
		protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			collectedObjects.Add(this._mapNotices);
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x00027659 File Offset: 0x00025859
		internal static object AutoGeneratedGetMemberValue_mapNotices(object o)
		{
			return ((CampaignInformationManager)o)._mapNotices;
		}

		// Token: 0x040002C6 RID: 710
		[SaveableField(10)]
		private List<InformationData> _mapNotices;

		// Token: 0x040002C7 RID: 711
		[CachedData]
		private bool _isSessionLaunched;

		// Token: 0x02000514 RID: 1300
		public enum NoticeType
		{
			// Token: 0x040015DA RID: 5594
			None,
			// Token: 0x040015DB RID: 5595
			WarAnnouncement,
			// Token: 0x040015DC RID: 5596
			PeaceAnnouncement,
			// Token: 0x040015DD RID: 5597
			ChangeSettlementOwner,
			// Token: 0x040015DE RID: 5598
			FortificationIsCaptured,
			// Token: 0x040015DF RID: 5599
			HeroChangedFaction,
			// Token: 0x040015E0 RID: 5600
			BarterAnnouncement
		}
	}
}
