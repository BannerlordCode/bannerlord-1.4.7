using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Missions.Interaction.InteractionItems
{
	// Token: 0x02000043 RID: 67
	public abstract class MissionInteractionItemBaseVM : ViewModel
	{
		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060005D2 RID: 1490 RVA: 0x000162A0 File Offset: 0x000144A0
		// (set) Token: 0x060005D3 RID: 1491 RVA: 0x000162A8 File Offset: 0x000144A8
		public bool IsDisplayed { get; internal set; }

		// Token: 0x060005D4 RID: 1492 RVA: 0x000162B1 File Offset: 0x000144B1
		public MissionInteractionItemBaseVM()
		{
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060005D5 RID: 1493 RVA: 0x000162B9 File Offset: 0x000144B9
		// (set) Token: 0x060005D6 RID: 1494 RVA: 0x000162C1 File Offset: 0x000144C1
		[DataSourceProperty]
		public bool IsDisabled
		{
			get
			{
				return this._isDisabled;
			}
			set
			{
				if (value != this._isDisabled)
				{
					this._isDisabled = value;
					base.OnPropertyChangedWithValue(value, "IsDisabled");
				}
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060005D7 RID: 1495 RVA: 0x000162DF File Offset: 0x000144DF
		// (set) Token: 0x060005D8 RID: 1496 RVA: 0x000162E7 File Offset: 0x000144E7
		[DataSourceProperty]
		public string Message
		{
			get
			{
				return this._message;
			}
			set
			{
				if (value != this._message)
				{
					this._message = value;
					base.OnPropertyChangedWithValue<string>(value, "Message");
				}
			}
		}

		// Token: 0x0400029D RID: 669
		private bool _isDisabled;

		// Token: 0x0400029E RID: 670
		private string _message;
	}
}
