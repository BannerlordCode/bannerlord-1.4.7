using System;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000042 RID: 66
	public class EducationNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x060005EC RID: 1516 RVA: 0x0001F280 File Offset: 0x0001D480
		public EducationNotificationItemVM(EducationMapNotification data)
			: base(data)
		{
			base.NotificationIdentifier = "education";
			base.ForceInspection = true;
			this._child = data.Child;
			this._age = data.Age;
			this._onInspect = new Action(this.OnInspect);
			CampaignEvents.ChildEducationCompletedEvent.AddNonSerializedListener(this, new Action<Hero, int>(this.OnEducationCompletedForChild));
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x0001F2E8 File Offset: 0x0001D4E8
		private void OnInspect()
		{
			EducationMapNotification educationMapNotification = (EducationMapNotification)base.Data;
			if (educationMapNotification != null && !educationMapNotification.IsValid())
			{
				InformationManager.ShowInquiry(new InquiryData("", new TextObject("{=wGWYNYYX}This education stage is no longer relevant.", null).ToString(), true, false, GameTexts.FindText("str_ok", null).ToString(), "", null, null, "", 0f, null, null, null), false, false);
				base.ExecuteRemove();
				return;
			}
			Game.Current.GameStateManager.PushState(Game.Current.GameStateManager.CreateState<EducationState>(new object[] { this._child }), 0);
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0001F388 File Offset: 0x0001D588
		private void OnEducationCompletedForChild(Hero child, int age)
		{
			if (child == this._child && age >= this._age)
			{
				base.ExecuteRemove();
			}
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x0001F3A2 File Offset: 0x0001D5A2
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEvents.ChildEducationCompletedEvent.ClearListeners(this);
		}

		// Token: 0x04000286 RID: 646
		private readonly Hero _child;

		// Token: 0x04000287 RID: 647
		private readonly int _age;
	}
}
