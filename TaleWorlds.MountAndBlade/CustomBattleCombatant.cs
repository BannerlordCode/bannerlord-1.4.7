using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200020A RID: 522
	public class CustomBattleCombatant : IBattleCombatant
	{
		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06001E2D RID: 7725 RVA: 0x00068204 File Offset: 0x00066404
		// (set) Token: 0x06001E2E RID: 7726 RVA: 0x0006820C File Offset: 0x0006640C
		public TextObject Name { get; private set; }

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06001E2F RID: 7727 RVA: 0x00068215 File Offset: 0x00066415
		// (set) Token: 0x06001E30 RID: 7728 RVA: 0x0006821D File Offset: 0x0006641D
		public BattleSideEnum Side { get; set; }

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06001E31 RID: 7729 RVA: 0x00068226 File Offset: 0x00066426
		public BasicCharacterObject General
		{
			get
			{
				return this._general;
			}
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06001E32 RID: 7730 RVA: 0x0006822E File Offset: 0x0006642E
		// (set) Token: 0x06001E33 RID: 7731 RVA: 0x00068236 File Offset: 0x00066436
		public BasicCultureObject BasicCulture { get; private set; }

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06001E34 RID: 7732 RVA: 0x0006823F File Offset: 0x0006643F
		public Tuple<uint, uint> PrimaryColorPair
		{
			get
			{
				return new Tuple<uint, uint>(this.Banner.GetPrimaryColor(), this.Banner.GetFirstIconColor());
			}
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06001E35 RID: 7733 RVA: 0x0006825C File Offset: 0x0006645C
		public Tuple<uint, uint> AlternativeColorPair
		{
			get
			{
				return new Tuple<uint, uint>(this.Banner.GetFirstIconColor(), this.Banner.GetPrimaryColor());
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x06001E36 RID: 7734 RVA: 0x00068279 File Offset: 0x00066479
		// (set) Token: 0x06001E37 RID: 7735 RVA: 0x00068281 File Offset: 0x00066481
		public Banner Banner { get; private set; }

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x06001E38 RID: 7736 RVA: 0x0006828A File Offset: 0x0006648A
		public IEnumerable<BasicCharacterObject> Characters
		{
			get
			{
				return this._characters.AsReadOnly();
			}
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x06001E39 RID: 7737 RVA: 0x00068297 File Offset: 0x00066497
		public int CountOfCharacters
		{
			get
			{
				return this._characters.Count<BasicCharacterObject>();
			}
		}

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x06001E3A RID: 7738 RVA: 0x000682A4 File Offset: 0x000664A4
		// (set) Token: 0x06001E3B RID: 7739 RVA: 0x000682AC File Offset: 0x000664AC
		public int NumberOfAllMembers { get; private set; }

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x06001E3C RID: 7740 RVA: 0x000682B5 File Offset: 0x000664B5
		public int NumberOfHealthyMembers
		{
			get
			{
				return this._characters.Count;
			}
		}

		// Token: 0x06001E3D RID: 7741 RVA: 0x000682C2 File Offset: 0x000664C2
		public CustomBattleCombatant(TextObject name, BasicCultureObject culture, Banner banner)
		{
			this.Name = name;
			this.BasicCulture = culture;
			this.Banner = banner;
			this._characters = new List<BasicCharacterObject>();
			this._general = null;
		}

		// Token: 0x06001E3E RID: 7742 RVA: 0x000682F4 File Offset: 0x000664F4
		public void AddCharacter(BasicCharacterObject characterObject, int number)
		{
			for (int i = 0; i < number; i++)
			{
				this._characters.Add(characterObject);
			}
			this.NumberOfAllMembers += number;
		}

		// Token: 0x06001E3F RID: 7743 RVA: 0x00068327 File Offset: 0x00066527
		public void SetGeneral(BasicCharacterObject generalCharacter)
		{
			this._general = generalCharacter;
		}

		// Token: 0x06001E40 RID: 7744 RVA: 0x00068330 File Offset: 0x00066530
		public int GetTacticsSkillAmount()
		{
			if (this._characters.Count > 0)
			{
				return this._characters.Max<BasicCharacterObject>((BasicCharacterObject h) => h.GetSkillValue(DefaultSkills.Tactics));
			}
			return 0;
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x0006836C File Offset: 0x0006656C
		public int GetNumberOfMissionReadyTroops()
		{
			return this.NumberOfHealthyMembers;
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x00068374 File Offset: 0x00066574
		public bool IsUnderPlayersCommand(BattleSideEnum playerSide)
		{
			return this.Side == playerSide && this.General.IsPlayerCharacter;
		}

		// Token: 0x04000A52 RID: 2642
		private List<BasicCharacterObject> _characters;

		// Token: 0x04000A53 RID: 2643
		private BasicCharacterObject _general;
	}
}
