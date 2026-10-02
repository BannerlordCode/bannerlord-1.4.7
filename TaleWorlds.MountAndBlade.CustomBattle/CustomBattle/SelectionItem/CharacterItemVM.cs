using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Selector;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle.SelectionItem
{
	// Token: 0x0200001F RID: 31
	public class CharacterItemVM : SelectorItemVM
	{
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x0000A659 File Offset: 0x00008859
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x0000A661 File Offset: 0x00008861
		public BasicCharacterObject Character { get; private set; }

		// Token: 0x060001B7 RID: 439 RVA: 0x0000A66A File Offset: 0x0000886A
		public CharacterItemVM(BasicCharacterObject character)
			: base(character.Name.ToString())
		{
			this.Character = character;
		}
	}
}
