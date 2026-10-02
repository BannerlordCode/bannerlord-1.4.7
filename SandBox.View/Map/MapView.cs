using System;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SandBox.View.Map
{
	// Token: 0x0200005B RID: 91
	public abstract class MapView : SandboxView
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600037F RID: 895 RVA: 0x0001C8E3 File Offset: 0x0001AAE3
		// (set) Token: 0x06000380 RID: 896 RVA: 0x0001C8EB File Offset: 0x0001AAEB
		public MapScreen MapScreen { get; internal set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000381 RID: 897 RVA: 0x0001C8F4 File Offset: 0x0001AAF4
		// (set) Token: 0x06000382 RID: 898 RVA: 0x0001C8FC File Offset: 0x0001AAFC
		public MapState MapState { get; internal set; }

		// Token: 0x06000383 RID: 899 RVA: 0x0001C905 File Offset: 0x0001AB05
		protected internal virtual void CreateLayout()
		{
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0001C907 File Offset: 0x0001AB07
		protected internal virtual void OnResume()
		{
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0001C909 File Offset: 0x0001AB09
		protected internal virtual void OnHourlyTick()
		{
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0001C90B File Offset: 0x0001AB0B
		protected internal virtual void OnStartWait(string waitMenuId)
		{
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0001C90D File Offset: 0x0001AB0D
		protected internal virtual void OnMainPartyEncounter()
		{
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0001C90F File Offset: 0x0001AB0F
		protected internal virtual void OnDispersePlayerLeadedArmy()
		{
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0001C911 File Offset: 0x0001AB11
		protected internal virtual void OnArmyLeft()
		{
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0001C913 File Offset: 0x0001AB13
		protected internal virtual bool IsEscaped()
		{
			return false;
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0001C916 File Offset: 0x0001AB16
		protected internal virtual bool IsOpeningEscapeMenuOnFocusChangeAllowed()
		{
			return true;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0001C919 File Offset: 0x0001AB19
		protected internal virtual void OnOverlayCreated()
		{
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0001C91B File Offset: 0x0001AB1B
		protected internal virtual void OnOverlayClosed()
		{
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0001C91D File Offset: 0x0001AB1D
		protected internal virtual void OnMenuModeTick(float dt)
		{
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0001C91F File Offset: 0x0001AB1F
		protected internal virtual void OnMapScreenUpdate(float dt)
		{
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0001C921 File Offset: 0x0001AB21
		protected internal virtual void OnIdleTick(float dt)
		{
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0001C923 File Offset: 0x0001AB23
		protected internal virtual void OnMapTerrainClick()
		{
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0001C925 File Offset: 0x0001AB25
		protected internal virtual void OnSiegeEngineClick(MatrixFrame siegeEngineFrame)
		{
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0001C927 File Offset: 0x0001AB27
		protected internal virtual void OnMapConversationStart()
		{
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0001C929 File Offset: 0x0001AB29
		protected internal virtual void OnMapConversationOver()
		{
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0001C92B File Offset: 0x0001AB2B
		protected internal virtual TutorialContexts GetTutorialContext()
		{
			return TutorialContexts.MapWindow;
		}

		// Token: 0x040001D5 RID: 469
		protected const float ContextAlphaModifier = 8.5f;
	}
}
