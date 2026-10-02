using System;
using System.Linq;
using SandBox.View.Map.Navigation.NavigationElements;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace SandBox.View.Map.Navigation
{
	// Token: 0x02000067 RID: 103
	public class MapNavigationHandler : INavigationHandler
	{
		// Token: 0x0600046C RID: 1132 RVA: 0x00024193 File Offset: 0x00022393
		public INavigationElement[] GetElements()
		{
			return this._elements;
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600046D RID: 1133 RVA: 0x0002419B File Offset: 0x0002239B
		// (set) Token: 0x0600046E RID: 1134 RVA: 0x000241A3 File Offset: 0x000223A3
		public bool IsNavigationLocked { get; set; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600046F RID: 1135 RVA: 0x000241AC File Offset: 0x000223AC
		public bool IsEscapeMenuActive
		{
			get
			{
				return this._elements.Any<INavigationElement>(delegate(INavigationElement e)
				{
					EscapeMenuNavigationElement escapeMenuNavigationElement;
					return (escapeMenuNavigationElement = e as EscapeMenuNavigationElement) != null && escapeMenuNavigationElement.IsActive;
				});
			}
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x000241D8 File Offset: 0x000223D8
		public MapNavigationHandler()
		{
			this._game = Game.Current;
			this._elements = this.OnCreateElements();
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x000241F8 File Offset: 0x000223F8
		public bool IsAnyElementActive()
		{
			for (int i = 0; i < this._elements.Length; i++)
			{
				if (this._elements[i].IsActive)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0002422C File Offset: 0x0002242C
		protected virtual INavigationElement[] OnCreateElements()
		{
			return new INavigationElement[]
			{
				new EscapeMenuNavigationElement(this),
				new CharacterDeveloperNavigationElement(this),
				new InventoryNavigationElement(this),
				new PartyNavigationElement(this),
				new QuestsNavigationElement(this),
				new ClanNavigationElement(this),
				new KingdomNavigationElement(this)
			};
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00024280 File Offset: 0x00022480
		public INavigationElement GetElement(string id)
		{
			for (int i = 0; i < this._elements.Length; i++)
			{
				if (this._elements[i].StringId == id)
				{
					return this._elements[i];
				}
			}
			return null;
		}

		// Token: 0x0400021F RID: 543
		protected readonly Game _game;

		// Token: 0x04000220 RID: 544
		private INavigationElement[] _elements;
	}
}
