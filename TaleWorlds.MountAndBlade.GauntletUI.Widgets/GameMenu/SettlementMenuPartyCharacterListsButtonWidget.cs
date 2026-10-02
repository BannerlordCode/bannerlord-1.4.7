using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.GameMenu
{
	// Token: 0x02000154 RID: 340
	public class SettlementMenuPartyCharacterListsButtonWidget : ButtonWidget
	{
		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06001225 RID: 4645 RVA: 0x000322D5 File Offset: 0x000304D5
		// (set) Token: 0x06001226 RID: 4646 RVA: 0x000322DD File Offset: 0x000304DD
		public Brush PartyListButtonBrush { get; set; }

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x06001227 RID: 4647 RVA: 0x000322E6 File Offset: 0x000304E6
		// (set) Token: 0x06001228 RID: 4648 RVA: 0x000322EE File Offset: 0x000304EE
		public Brush CharacterListButtonBrush { get; set; }

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06001229 RID: 4649 RVA: 0x000322F7 File Offset: 0x000304F7
		// (set) Token: 0x0600122A RID: 4650 RVA: 0x000322FF File Offset: 0x000304FF
		public ContainerPageControlWidget CharactersList { get; set; }

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x0600122B RID: 4651 RVA: 0x00032308 File Offset: 0x00030508
		// (set) Token: 0x0600122C RID: 4652 RVA: 0x00032310 File Offset: 0x00030510
		public ContainerPageControlWidget PartiesList { get; set; }

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x0600122D RID: 4653 RVA: 0x00032319 File Offset: 0x00030519
		// (set) Token: 0x0600122E RID: 4654 RVA: 0x00032321 File Offset: 0x00030521
		public int MaxNumOfVisuals { get; set; } = 5;

		// Token: 0x0600122F RID: 4655 RVA: 0x0003232A File Offset: 0x0003052A
		public SettlementMenuPartyCharacterListsButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x0003233C File Offset: 0x0003053C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			base.Brush = (this.ChildPartiesList.IsVisible ? this.PartyListButtonBrush : (this.ChildCharactersList.IsVisible ? this.CharacterListButtonBrush : null));
			if (!this._initialized)
			{
				if (this.CharactersList.IsVisible)
				{
					this.SetCharacterListVisible();
				}
				else if (this.PartiesList.IsVisible)
				{
					this.SetPartyListVisible();
				}
				this._initialized = true;
			}
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x000323B8 File Offset: 0x000305B8
		protected override void HandleClick()
		{
			base.HandleClick();
			if (!this.PartiesList.IsVisible && this.CharactersList.IsVisible)
			{
				this.SetPartyListVisible();
				return;
			}
			if (this.PartiesList.IsVisible && !this.CharactersList.IsVisible)
			{
				this.SetCharacterListVisible();
			}
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x0003240C File Offset: 0x0003060C
		private void SetCharacterListVisible()
		{
			this.CharactersList.IsVisible = true;
			this.PartiesList.IsVisible = false;
			this.ChildPartiesList.IsVisible = true;
			this.ChildCharactersList.IsVisible = false;
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x0003243E File Offset: 0x0003063E
		private void SetPartyListVisible()
		{
			this.CharactersList.IsVisible = false;
			this.PartiesList.IsVisible = true;
			this.ChildPartiesList.IsVisible = false;
			this.ChildCharactersList.IsVisible = true;
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x06001234 RID: 4660 RVA: 0x00032470 File Offset: 0x00030670
		// (set) Token: 0x06001235 RID: 4661 RVA: 0x00032478 File Offset: 0x00030678
		public ListPanel ChildCharactersList
		{
			get
			{
				return this._childCharactersList;
			}
			set
			{
				if (value != this._childCharactersList)
				{
					this._childCharactersList = value;
					this._childCharactersList.ItemAddEventHandlers.Add(new Action<Widget, Widget>(this.OnListItemAdded));
				}
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x06001236 RID: 4662 RVA: 0x000324A6 File Offset: 0x000306A6
		// (set) Token: 0x06001237 RID: 4663 RVA: 0x000324AE File Offset: 0x000306AE
		public ListPanel ChildPartiesList
		{
			get
			{
				return this._childPartiesList;
			}
			set
			{
				if (value != this._childPartiesList)
				{
					this._childPartiesList = value;
					this._childPartiesList.ItemAddEventHandlers.Add(new Action<Widget, Widget>(this.OnListItemAdded));
				}
			}
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x000324DC File Offset: 0x000306DC
		private void OnListItemAdded(Widget parent, Widget child)
		{
			if (parent.ChildCount > this.MaxNumOfVisuals)
			{
				child.IsVisible = false;
			}
		}

		// Token: 0x0400084A RID: 2122
		private bool _initialized;

		// Token: 0x0400084B RID: 2123
		private ListPanel _childCharactersList;

		// Token: 0x0400084C RID: 2124
		private ListPanel _childPartiesList;
	}
}
