using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200036E RID: 878
	public class DuelZoneLandmark : ScriptComponentBehavior, IFocusable
	{
		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x06003245 RID: 12869 RVA: 0x000CD374 File Offset: 0x000CB574
		public FocusableObjectType FocusableObjectType
		{
			get
			{
				return FocusableObjectType.None;
			}
		}

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x06003246 RID: 12870 RVA: 0x000CD377 File Offset: 0x000CB577
		public virtual bool IsFocusable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003247 RID: 12871 RVA: 0x000CD37A File Offset: 0x000CB57A
		public void OnFocusGain(Agent userAgent)
		{
		}

		// Token: 0x06003248 RID: 12872 RVA: 0x000CD37C File Offset: 0x000CB57C
		public void OnFocusLose(Agent userAgent)
		{
		}

		// Token: 0x06003249 RID: 12873 RVA: 0x000CD37E File Offset: 0x000CB57E
		public TextObject GetInfoTextForBeingNotInteractable(Agent userAgent)
		{
			return null;
		}

		// Token: 0x0600324A RID: 12874 RVA: 0x000CD381 File Offset: 0x000CB581
		public TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			return null;
		}

		// Token: 0x04001544 RID: 5444
		public TroopType ZoneTroopType;
	}
}
