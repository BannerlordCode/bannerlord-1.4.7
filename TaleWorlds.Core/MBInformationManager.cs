using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.Core
{
	// Token: 0x020000AF RID: 175
	public static class MBInformationManager
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000933 RID: 2355 RVA: 0x0001E1C8 File Offset: 0x0001C3C8
		// (remove) Token: 0x06000934 RID: 2356 RVA: 0x0001E1FC File Offset: 0x0001C3FC
		public static event Action<string, int, BasicCharacterObject, Equipment, string> FiringQuickInformation;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000935 RID: 2357 RVA: 0x0001E230 File Offset: 0x0001C430
		// (remove) Token: 0x06000936 RID: 2358 RVA: 0x0001E264 File Offset: 0x0001C464
		public static event Action ClearingQuickInformations;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000937 RID: 2359 RVA: 0x0001E298 File Offset: 0x0001C498
		// (remove) Token: 0x06000938 RID: 2360 RVA: 0x0001E2CC File Offset: 0x0001C4CC
		public static event Action<MultiSelectionInquiryData, bool, bool> OnShowMultiSelectionInquiry;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000939 RID: 2361 RVA: 0x0001E300 File Offset: 0x0001C500
		// (remove) Token: 0x0600093A RID: 2362 RVA: 0x0001E334 File Offset: 0x0001C534
		public static event Action<InformationData> OnAddMapNotice;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x0600093B RID: 2363 RVA: 0x0001E368 File Offset: 0x0001C568
		// (remove) Token: 0x0600093C RID: 2364 RVA: 0x0001E39C File Offset: 0x0001C59C
		public static event Action<InformationData> OnRemoveMapNotice;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600093D RID: 2365 RVA: 0x0001E3D0 File Offset: 0x0001C5D0
		// (remove) Token: 0x0600093E RID: 2366 RVA: 0x0001E404 File Offset: 0x0001C604
		public static event Action<SceneNotificationData> OnShowSceneNotification;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600093F RID: 2367 RVA: 0x0001E438 File Offset: 0x0001C638
		// (remove) Token: 0x06000940 RID: 2368 RVA: 0x0001E46C File Offset: 0x0001C66C
		public static event Action OnHideSceneNotification;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000941 RID: 2369 RVA: 0x0001E4A0 File Offset: 0x0001C6A0
		// (remove) Token: 0x06000942 RID: 2370 RVA: 0x0001E4D4 File Offset: 0x0001C6D4
		public static event Func<bool> IsAnySceneNotificationActive;

		// Token: 0x06000943 RID: 2371 RVA: 0x0001E507 File Offset: 0x0001C707
		public static void AddQuickInformation(TextObject message, int extraTimeInMs = 0, BasicCharacterObject announcerCharacter = null, Equipment equipment = null, string soundEventPath = "")
		{
			Action<string, int, BasicCharacterObject, Equipment, string> firingQuickInformation = MBInformationManager.FiringQuickInformation;
			if (firingQuickInformation != null)
			{
				firingQuickInformation(message.ToString(), extraTimeInMs, announcerCharacter, equipment, soundEventPath);
			}
			Debug.Print(message.ToString(), 0, Debug.DebugColor.White, 1125899906842624UL);
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x0001E53B File Offset: 0x0001C73B
		public static void ClearQuickInformations()
		{
			Action clearingQuickInformations = MBInformationManager.ClearingQuickInformations;
			if (clearingQuickInformations == null)
			{
				return;
			}
			clearingQuickInformations();
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x0001E54C File Offset: 0x0001C74C
		public static void ShowMultiSelectionInquiry(MultiSelectionInquiryData data, bool pauseGameActiveState = false, bool prioritize = false)
		{
			Action<MultiSelectionInquiryData, bool, bool> onShowMultiSelectionInquiry = MBInformationManager.OnShowMultiSelectionInquiry;
			if (onShowMultiSelectionInquiry == null)
			{
				return;
			}
			onShowMultiSelectionInquiry(data, pauseGameActiveState, prioritize);
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x0001E560 File Offset: 0x0001C760
		public static void AddNotice(InformationData data)
		{
			Action<InformationData> onAddMapNotice = MBInformationManager.OnAddMapNotice;
			if (onAddMapNotice == null)
			{
				return;
			}
			onAddMapNotice(data);
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x0001E572 File Offset: 0x0001C772
		public static void MapNoticeRemoved(InformationData data)
		{
			Action<InformationData> onRemoveMapNotice = MBInformationManager.OnRemoveMapNotice;
			if (onRemoveMapNotice == null)
			{
				return;
			}
			onRemoveMapNotice(data);
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x0001E584 File Offset: 0x0001C784
		public static void ShowHint(string hint)
		{
			InformationManager.ShowTooltip(typeof(string), new object[] { hint });
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0001E59F File Offset: 0x0001C79F
		public static void HideInformations()
		{
			InformationManager.HideTooltip();
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x0001E5A6 File Offset: 0x0001C7A6
		public static void ShowSceneNotification(SceneNotificationData data)
		{
			Action<SceneNotificationData> onShowSceneNotification = MBInformationManager.OnShowSceneNotification;
			if (onShowSceneNotification == null)
			{
				return;
			}
			onShowSceneNotification(data);
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0001E5B8 File Offset: 0x0001C7B8
		public static void HideSceneNotification()
		{
			Action onHideSceneNotification = MBInformationManager.OnHideSceneNotification;
			if (onHideSceneNotification == null)
			{
				return;
			}
			onHideSceneNotification();
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0001E5CC File Offset: 0x0001C7CC
		public static bool? GetIsAnySceneNotificationActive()
		{
			Func<bool> isAnySceneNotificationActive = MBInformationManager.IsAnySceneNotificationActive;
			if (isAnySceneNotificationActive == null)
			{
				return null;
			}
			return new bool?(isAnySceneNotificationActive());
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x0001E5F6 File Offset: 0x0001C7F6
		public static void Clear()
		{
			MBInformationManager.FiringQuickInformation = null;
			MBInformationManager.OnShowMultiSelectionInquiry = null;
			MBInformationManager.OnAddMapNotice = null;
			MBInformationManager.OnRemoveMapNotice = null;
			MBInformationManager.OnShowSceneNotification = null;
			MBInformationManager.OnHideSceneNotification = null;
		}

		// Token: 0x02000120 RID: 288
		public enum NotificationPriority
		{
			// Token: 0x040007B3 RID: 1971
			Lowest,
			// Token: 0x040007B4 RID: 1972
			Low,
			// Token: 0x040007B5 RID: 1973
			Medium,
			// Token: 0x040007B6 RID: 1974
			High,
			// Token: 0x040007B7 RID: 1975
			Highest
		}

		// Token: 0x02000121 RID: 289
		public enum NotificationStatus
		{
			// Token: 0x040007B9 RID: 1977
			Inactive,
			// Token: 0x040007BA RID: 1978
			CurrentlyActive,
			// Token: 0x040007BB RID: 1979
			InQueue
		}

		// Token: 0x02000122 RID: 290
		public class DialogNotificationHandle
		{
		}
	}
}
