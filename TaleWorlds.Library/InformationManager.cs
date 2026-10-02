using System;
using System.Collections.Generic;

namespace TaleWorlds.Library
{
	// Token: 0x0200003B RID: 59
	public static class InformationManager
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060001CC RID: 460 RVA: 0x000070E8 File Offset: 0x000052E8
		// (remove) Token: 0x060001CD RID: 461 RVA: 0x0000711C File Offset: 0x0000531C
		public static event Action<InformationMessage> DisplayMessageInternal;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060001CE RID: 462 RVA: 0x00007150 File Offset: 0x00005350
		// (remove) Token: 0x060001CF RID: 463 RVA: 0x00007184 File Offset: 0x00005384
		public static event Action ClearAllMessagesInternal;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060001D0 RID: 464 RVA: 0x000071B8 File Offset: 0x000053B8
		// (remove) Token: 0x060001D1 RID: 465 RVA: 0x000071EC File Offset: 0x000053EC
		public static event Action HideAllMessagesInternal;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060001D2 RID: 466 RVA: 0x00007220 File Offset: 0x00005420
		// (remove) Token: 0x060001D3 RID: 467 RVA: 0x00007254 File Offset: 0x00005454
		public static event Action<string> OnAddSystemNotification;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060001D4 RID: 468 RVA: 0x00007288 File Offset: 0x00005488
		// (remove) Token: 0x060001D5 RID: 469 RVA: 0x000072BC File Offset: 0x000054BC
		public static event Action<Type, object[]> OnShowTooltip;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060001D6 RID: 470 RVA: 0x000072F0 File Offset: 0x000054F0
		// (remove) Token: 0x060001D7 RID: 471 RVA: 0x00007324 File Offset: 0x00005524
		public static event Action OnHideTooltip;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060001D8 RID: 472 RVA: 0x00007358 File Offset: 0x00005558
		// (remove) Token: 0x060001D9 RID: 473 RVA: 0x0000738C File Offset: 0x0000558C
		public static event Action<InquiryData, bool, bool> OnShowInquiry;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060001DA RID: 474 RVA: 0x000073C0 File Offset: 0x000055C0
		// (remove) Token: 0x060001DB RID: 475 RVA: 0x000073F4 File Offset: 0x000055F4
		public static event Action<TextInquiryData, bool, bool> OnShowTextInquiry;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x060001DC RID: 476 RVA: 0x00007428 File Offset: 0x00005628
		// (remove) Token: 0x060001DD RID: 477 RVA: 0x0000745C File Offset: 0x0000565C
		public static event Action OnHideInquiry;

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060001DE RID: 478 RVA: 0x0000748F File Offset: 0x0000568F
		public static IReadOnlyDictionary<Type, InformationManager.TooltipRegistry> RegisteredTypes
		{
			get
			{
				return InformationManager._registeredTypes;
			}
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00007496 File Offset: 0x00005696
		public static bool IsAnyInquiryActive()
		{
			return InformationManager.IsAnyInquiryActiveInternal != null && InformationManager.IsAnyInquiryActiveInternal();
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x000074AB File Offset: 0x000056AB
		public static void DisplayMessage(InformationMessage message)
		{
			Action<InformationMessage> displayMessageInternal = InformationManager.DisplayMessageInternal;
			if (displayMessageInternal == null)
			{
				return;
			}
			displayMessageInternal(message);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x000074BD File Offset: 0x000056BD
		public static void HideAllMessages()
		{
			Action hideAllMessagesInternal = InformationManager.HideAllMessagesInternal;
			if (hideAllMessagesInternal == null)
			{
				return;
			}
			hideAllMessagesInternal();
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x000074CE File Offset: 0x000056CE
		public static void ClearAllMessages()
		{
			Action clearAllMessagesInternal = InformationManager.ClearAllMessagesInternal;
			if (clearAllMessagesInternal == null)
			{
				return;
			}
			clearAllMessagesInternal();
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x000074DF File Offset: 0x000056DF
		public static void AddSystemNotification(string message)
		{
			Action<string> onAddSystemNotification = InformationManager.OnAddSystemNotification;
			if (onAddSystemNotification == null)
			{
				return;
			}
			onAddSystemNotification(message);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x000074F1 File Offset: 0x000056F1
		public static void ShowTooltip(Type type, params object[] args)
		{
			Action<Type, object[]> onShowTooltip = InformationManager.OnShowTooltip;
			if (onShowTooltip == null)
			{
				return;
			}
			onShowTooltip(type, args);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00007504 File Offset: 0x00005704
		public static void HideTooltip()
		{
			Action onHideTooltip = InformationManager.OnHideTooltip;
			if (onHideTooltip == null)
			{
				return;
			}
			onHideTooltip();
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00007515 File Offset: 0x00005715
		public static void ShowInquiry(InquiryData data, bool pauseGameActiveState = false, bool prioritize = false)
		{
			Action<InquiryData, bool, bool> onShowInquiry = InformationManager.OnShowInquiry;
			if (onShowInquiry == null)
			{
				return;
			}
			onShowInquiry(data, pauseGameActiveState, prioritize);
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00007529 File Offset: 0x00005729
		public static void ShowTextInquiry(TextInquiryData textData, bool pauseGameActiveState = false, bool prioritize = false)
		{
			Action<TextInquiryData, bool, bool> onShowTextInquiry = InformationManager.OnShowTextInquiry;
			if (onShowTextInquiry == null)
			{
				return;
			}
			onShowTextInquiry(textData, pauseGameActiveState, prioritize);
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000753D File Offset: 0x0000573D
		public static void HideInquiry()
		{
			Action onHideInquiry = InformationManager.OnHideInquiry;
			if (onHideInquiry == null)
			{
				return;
			}
			onHideInquiry();
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00007550 File Offset: 0x00005750
		public static bool GetIsAnyTooltipActive()
		{
			if (InformationManager.IsAnyTooltipActiveInternal != null)
			{
				bool flag;
				bool flag2;
				InformationManager.IsAnyTooltipActiveInternal(out flag, out flag2);
				return flag;
			}
			return false;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00007578 File Offset: 0x00005778
		public static bool GetIsAnyTooltipActiveAndExtended()
		{
			if (InformationManager.IsAnyTooltipActiveInternal != null)
			{
				bool flag;
				bool flag2;
				InformationManager.IsAnyTooltipActiveInternal(out flag, out flag2);
				return flag && flag2;
			}
			return false;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x000075A0 File Offset: 0x000057A0
		public static void RegisterTooltip<TRegistered, TTooltip>(Action<TTooltip, object[]> onRefreshData, string movieName) where TTooltip : TooltipBaseVM
		{
			Type typeFromHandle = typeof(TRegistered);
			Type typeFromHandle2 = typeof(TTooltip);
			InformationManager._registeredTypes[typeFromHandle] = new InformationManager.TooltipRegistry(typeFromHandle2, onRefreshData, movieName);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x000075D8 File Offset: 0x000057D8
		public static void UnregisterTooltip<TRegistered>()
		{
			Type typeFromHandle = typeof(TRegistered);
			if (InformationManager._registeredTypes.ContainsKey(typeFromHandle))
			{
				InformationManager._registeredTypes.Remove(typeFromHandle);
				Debug.Print("Unregister tooltip for type: " + typeof(TRegistered).Name, 0, Debug.DebugColor.White, 17592186044416UL);
				return;
			}
			Debug.Print("Unable to unregister tooltip because it was not found: " + typeof(TRegistered).Name, 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000765E File Offset: 0x0000585E
		public static void Clear()
		{
			InformationManager.DisplayMessageInternal = null;
			InformationManager.HideAllMessagesInternal = null;
			InformationManager.ClearAllMessagesInternal = null;
			InformationManager.OnShowInquiry = null;
			InformationManager.OnShowTextInquiry = null;
			InformationManager.OnHideInquiry = null;
			InformationManager.IsAnyInquiryActiveInternal = null;
			InformationManager.OnShowTooltip = null;
			InformationManager.OnHideTooltip = null;
		}

		// Token: 0x040000AC RID: 172
		public static Func<bool> IsAnyInquiryActiveInternal;

		// Token: 0x040000B1 RID: 177
		public static InformationManager.IsAnyTooltipActiveDelegate IsAnyTooltipActiveInternal;

		// Token: 0x040000B7 RID: 183
		private static Dictionary<Type, InformationManager.TooltipRegistry> _registeredTypes = new Dictionary<Type, InformationManager.TooltipRegistry>();

		// Token: 0x020000D4 RID: 212
		public struct TooltipRegistry
		{
			// Token: 0x0600075F RID: 1887 RVA: 0x0001877E File Offset: 0x0001697E
			public TooltipRegistry(Type tooltipType, object onRefreshData, string movieName)
			{
				this.TooltipType = tooltipType;
				this.OnRefreshData = onRefreshData;
				this.MovieName = movieName;
			}

			// Token: 0x040002AD RID: 685
			public Type TooltipType;

			// Token: 0x040002AE RID: 686
			public object OnRefreshData;

			// Token: 0x040002AF RID: 687
			public string MovieName;
		}

		// Token: 0x020000D5 RID: 213
		// (Invoke) Token: 0x06000761 RID: 1889
		public delegate void IsAnyTooltipActiveDelegate(out bool isAnyTooltipActive, out bool isAnyTooltipExtended);
	}
}
