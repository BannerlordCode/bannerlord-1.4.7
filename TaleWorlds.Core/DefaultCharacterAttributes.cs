using System;
using TaleWorlds.Localization;

namespace TaleWorlds.Core
{
	// Token: 0x0200004F RID: 79
	public class DefaultCharacterAttributes
	{
		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x00016002 File Offset: 0x00014202
		private static DefaultCharacterAttributes Instance
		{
			get
			{
				return Game.Current.DefaultCharacterAttributes;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x0600065A RID: 1626 RVA: 0x0001600E File Offset: 0x0001420E
		public static CharacterAttribute Vigor
		{
			get
			{
				return DefaultCharacterAttributes.Instance._vigor;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x0600065B RID: 1627 RVA: 0x0001601A File Offset: 0x0001421A
		public static CharacterAttribute Control
		{
			get
			{
				return DefaultCharacterAttributes.Instance._control;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x00016026 File Offset: 0x00014226
		public static CharacterAttribute Endurance
		{
			get
			{
				return DefaultCharacterAttributes.Instance._endurance;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x00016032 File Offset: 0x00014232
		public static CharacterAttribute Cunning
		{
			get
			{
				return DefaultCharacterAttributes.Instance._cunning;
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x0600065E RID: 1630 RVA: 0x0001603E File Offset: 0x0001423E
		public static CharacterAttribute Social
		{
			get
			{
				return DefaultCharacterAttributes.Instance._social;
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x0001604A File Offset: 0x0001424A
		public static CharacterAttribute Intelligence
		{
			get
			{
				return DefaultCharacterAttributes.Instance._intelligence;
			}
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x00016056 File Offset: 0x00014256
		private CharacterAttribute Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<CharacterAttribute>(new CharacterAttribute(stringId));
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x0001606D File Offset: 0x0001426D
		internal DefaultCharacterAttributes()
		{
			this.RegisterAll();
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x0001607C File Offset: 0x0001427C
		private void RegisterAll()
		{
			this._vigor = this.Create("vigor");
			this._control = this.Create("control");
			this._endurance = this.Create("endurance");
			this._cunning = this.Create("cunning");
			this._social = this.Create("social");
			this._intelligence = this.Create("intelligence");
			this.InitializeAll();
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x000160F8 File Offset: 0x000142F8
		private void InitializeAll()
		{
			this._vigor.Initialize(new TextObject("{=YWkdD7Ki}Vigor", null), new TextObject("{=jJ9sLOLb}Vigor represents the ability to move with speed and force. It's important for melee combat.", null), new TextObject("{=Ve8xoa3i}VIG", null));
			this._control.Initialize(new TextObject("{=controlskill}Control", null), new TextObject("{=vx0OCvaj}Control represents the ability to use strength without sacrificing precision. It's necessary for using ranged weapons.", null), new TextObject("{=HuXafdmR}CTR", null));
			this._endurance.Initialize(new TextObject("{=kvOavzcs}Endurance", null), new TextObject("{=K8rCOQUZ}Endurance is the ability to perform taxing physical activity for a long time.", null), new TextObject("{=d2ApwXJr}END", null));
			this._cunning.Initialize(new TextObject("{=JZM1mQvb}Cunning", null), new TextObject("{=YO5LUfiO}Cunning is the ability to predict what other people will do, and to outwit their plans.", null), new TextObject("{=tH6Ooj0P}CNG", null));
			this._social.Initialize(new TextObject("{=socialskill}Social", null), new TextObject("{=XMDTt96y}Social is the ability to understand people's motivations and to sway them.", null), new TextObject("{=PHoxdReD}SOC", null));
			this._intelligence.Initialize(new TextObject("{=sOrJoxiC}Intelligence", null), new TextObject("{=TeUtEGV0}Intelligence represents aptitude for reading and theoretical learning.", null), new TextObject("{=Bn7IsMpu}INT", null));
		}

		// Token: 0x0400030B RID: 779
		private CharacterAttribute _control;

		// Token: 0x0400030C RID: 780
		private CharacterAttribute _vigor;

		// Token: 0x0400030D RID: 781
		private CharacterAttribute _endurance;

		// Token: 0x0400030E RID: 782
		private CharacterAttribute _cunning;

		// Token: 0x0400030F RID: 783
		private CharacterAttribute _social;

		// Token: 0x04000310 RID: 784
		private CharacterAttribute _intelligence;
	}
}
