using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CharacterDevelopment
{
	// Token: 0x020003AA RID: 938
	public class DefaultTraits
	{
		// Token: 0x17000CDE RID: 3294
		// (get) Token: 0x06003660 RID: 13920 RVA: 0x000E3F99 File Offset: 0x000E2199
		private static DefaultTraits Instance
		{
			get
			{
				return Campaign.Current.DefaultTraits;
			}
		}

		// Token: 0x17000CDF RID: 3295
		// (get) Token: 0x06003661 RID: 13921 RVA: 0x000E3FA5 File Offset: 0x000E21A5
		public static TraitObject Frequency
		{
			get
			{
				return DefaultTraits.Instance._traitFrequency;
			}
		}

		// Token: 0x17000CE0 RID: 3296
		// (get) Token: 0x06003662 RID: 13922 RVA: 0x000E3FB1 File Offset: 0x000E21B1
		public static TraitObject Mercy
		{
			get
			{
				return DefaultTraits.Instance._traitMercy;
			}
		}

		// Token: 0x17000CE1 RID: 3297
		// (get) Token: 0x06003663 RID: 13923 RVA: 0x000E3FBD File Offset: 0x000E21BD
		public static TraitObject Valor
		{
			get
			{
				return DefaultTraits.Instance._traitValor;
			}
		}

		// Token: 0x17000CE2 RID: 3298
		// (get) Token: 0x06003664 RID: 13924 RVA: 0x000E3FC9 File Offset: 0x000E21C9
		public static TraitObject Honor
		{
			get
			{
				return DefaultTraits.Instance._traitHonor;
			}
		}

		// Token: 0x17000CE3 RID: 3299
		// (get) Token: 0x06003665 RID: 13925 RVA: 0x000E3FD5 File Offset: 0x000E21D5
		public static TraitObject Generosity
		{
			get
			{
				return DefaultTraits.Instance._traitGenerosity;
			}
		}

		// Token: 0x17000CE4 RID: 3300
		// (get) Token: 0x06003666 RID: 13926 RVA: 0x000E3FE1 File Offset: 0x000E21E1
		public static TraitObject Calculating
		{
			get
			{
				return DefaultTraits.Instance._traitCalculating;
			}
		}

		// Token: 0x17000CE5 RID: 3301
		// (get) Token: 0x06003667 RID: 13927 RVA: 0x000E3FED File Offset: 0x000E21ED
		public static TraitObject PersonaCurt
		{
			get
			{
				return DefaultTraits.Instance._traitPersonaCurt;
			}
		}

		// Token: 0x17000CE6 RID: 3302
		// (get) Token: 0x06003668 RID: 13928 RVA: 0x000E3FF9 File Offset: 0x000E21F9
		public static TraitObject PersonaEarnest
		{
			get
			{
				return DefaultTraits.Instance._traitPersonaEarnest;
			}
		}

		// Token: 0x17000CE7 RID: 3303
		// (get) Token: 0x06003669 RID: 13929 RVA: 0x000E4005 File Offset: 0x000E2205
		public static TraitObject PersonaIronic
		{
			get
			{
				return DefaultTraits.Instance._traitPersonaIronic;
			}
		}

		// Token: 0x17000CE8 RID: 3304
		// (get) Token: 0x0600366A RID: 13930 RVA: 0x000E4011 File Offset: 0x000E2211
		public static TraitObject PersonaSoftspoken
		{
			get
			{
				return DefaultTraits.Instance._traitPersonaSoftspoken;
			}
		}

		// Token: 0x17000CE9 RID: 3305
		// (get) Token: 0x0600366B RID: 13931 RVA: 0x000E401D File Offset: 0x000E221D
		public static TraitObject Surgery
		{
			get
			{
				return DefaultTraits.Instance._traitSurgery;
			}
		}

		// Token: 0x17000CEA RID: 3306
		// (get) Token: 0x0600366C RID: 13932 RVA: 0x000E4029 File Offset: 0x000E2229
		public static TraitObject SergeantCommandSkills
		{
			get
			{
				return DefaultTraits.Instance._traitSergeantCommandSkills;
			}
		}

		// Token: 0x17000CEB RID: 3307
		// (get) Token: 0x0600366D RID: 13933 RVA: 0x000E4035 File Offset: 0x000E2235
		public static TraitObject RogueSkills
		{
			get
			{
				return DefaultTraits.Instance._traitRogueSkills;
			}
		}

		// Token: 0x17000CEC RID: 3308
		// (get) Token: 0x0600366E RID: 13934 RVA: 0x000E4041 File Offset: 0x000E2241
		public static TraitObject Siegecraft
		{
			get
			{
				return DefaultTraits.Instance._traitEngineerSkills;
			}
		}

		// Token: 0x17000CED RID: 3309
		// (get) Token: 0x0600366F RID: 13935 RVA: 0x000E404D File Offset: 0x000E224D
		public static TraitObject ScoutSkills
		{
			get
			{
				return DefaultTraits.Instance._traitScoutSkills;
			}
		}

		// Token: 0x17000CEE RID: 3310
		// (get) Token: 0x06003670 RID: 13936 RVA: 0x000E4059 File Offset: 0x000E2259
		public static TraitObject Blacksmith
		{
			get
			{
				return DefaultTraits.Instance._traitBlacksmith;
			}
		}

		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x06003671 RID: 13937 RVA: 0x000E4065 File Offset: 0x000E2265
		public static TraitObject Commander
		{
			get
			{
				return DefaultTraits.Instance._traitCommander;
			}
		}

		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x06003672 RID: 13938 RVA: 0x000E4071 File Offset: 0x000E2271
		public static TraitObject Trader
		{
			get
			{
				return DefaultTraits.Instance._traitTraderSkills;
			}
		}

		// Token: 0x17000CF1 RID: 3313
		// (get) Token: 0x06003673 RID: 13939 RVA: 0x000E407D File Offset: 0x000E227D
		public static TraitObject Thug
		{
			get
			{
				return DefaultTraits.Instance._traitThug;
			}
		}

		// Token: 0x17000CF2 RID: 3314
		// (get) Token: 0x06003674 RID: 13940 RVA: 0x000E4089 File Offset: 0x000E2289
		public static TraitObject Smuggler
		{
			get
			{
				return DefaultTraits.Instance._traitSmuggler;
			}
		}

		// Token: 0x17000CF3 RID: 3315
		// (get) Token: 0x06003675 RID: 13941 RVA: 0x000E4095 File Offset: 0x000E2295
		public static TraitObject Egalitarian
		{
			get
			{
				return DefaultTraits.Instance._traitEgalitarian;
			}
		}

		// Token: 0x17000CF4 RID: 3316
		// (get) Token: 0x06003676 RID: 13942 RVA: 0x000E40A1 File Offset: 0x000E22A1
		public static TraitObject Oligarchic
		{
			get
			{
				return DefaultTraits.Instance._traitOligarchic;
			}
		}

		// Token: 0x17000CF5 RID: 3317
		// (get) Token: 0x06003677 RID: 13943 RVA: 0x000E40AD File Offset: 0x000E22AD
		public static TraitObject Authoritarian
		{
			get
			{
				return DefaultTraits.Instance._traitAuthoritarian;
			}
		}

		// Token: 0x17000CF6 RID: 3318
		// (get) Token: 0x06003678 RID: 13944 RVA: 0x000E40B9 File Offset: 0x000E22B9
		public static TraitObject NavalSoldier
		{
			get
			{
				return DefaultTraits.Instance._traitNavalSoldier;
			}
		}

		// Token: 0x17000CF7 RID: 3319
		// (get) Token: 0x06003679 RID: 13945 RVA: 0x000E40C5 File Offset: 0x000E22C5
		public static IEnumerable<TraitObject> Personality
		{
			get
			{
				return DefaultTraits.Instance._personality;
			}
		}

		// Token: 0x0600367A RID: 13946 RVA: 0x000E40D4 File Offset: 0x000E22D4
		public DefaultTraits()
		{
			this.RegisterAll();
			this._personality = new TraitObject[] { this._traitMercy, this._traitValor, this._traitHonor, this._traitGenerosity, this._traitCalculating };
		}

		// Token: 0x0600367B RID: 13947 RVA: 0x000E4128 File Offset: 0x000E2328
		public void RegisterAll()
		{
			this._traitFrequency = this.Create("Frequency");
			this._traitMercy = this.Create("Mercy");
			this._traitValor = this.Create("Valor");
			this._traitHonor = this.Create("Honor");
			this._traitGenerosity = this.Create("Generosity");
			this._traitCalculating = this.Create("Calculating");
			this._traitPersonaCurt = this.Create("curt");
			this._traitPersonaIronic = this.Create("ironic");
			this._traitPersonaEarnest = this.Create("earnest");
			this._traitPersonaSoftspoken = this.Create("softspoken");
			this._traitCommander = this.Create("Commander");
			this._traitTraderSkills = this.Create("Trader");
			this._traitSurgery = this.Create("Surgeon");
			this._traitTracking = this.Create("Tracking");
			this._traitBlacksmith = this.Create("Blacksmith");
			this._traitSergeantCommandSkills = this.Create("SergeantCommandSkills");
			this._traitEngineerSkills = this.Create("EngineerSkills");
			this._traitRogueSkills = this.Create("RogueSkills");
			this._traitScoutSkills = this.Create("ScoutSkills");
			this._traitThug = this.Create("Thug");
			this._traitSmuggler = this.Create("Smuggler");
			this._traitEgalitarian = this.Create("Egalitarian");
			this._traitOligarchic = this.Create("Oligarchic");
			this._traitAuthoritarian = this.Create("Authoritarian");
			this._traitNavalSoldier = this.Create("NavalSoldier");
			this.InitializeAll();
		}

		// Token: 0x0600367C RID: 13948 RVA: 0x000E42E4 File Offset: 0x000E24E4
		private TraitObject Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<TraitObject>(new TraitObject(stringId));
		}

		// Token: 0x0600367D RID: 13949 RVA: 0x000E42FC File Offset: 0x000E24FC
		private void InitializeAll()
		{
			this._traitFrequency.Initialize(new TextObject("{=vsoyhPnl}Frequency", null), new TextObject("{=!}Frequency Description", null), true, 0, 20);
			this._traitMercy.Initialize(new TextObject("{=2I2uKJlw}Mercy", null), new TextObject("{=Au7VCWTa}Mercy represents your general aversion to suffering and your willingness to help strangers or even enemies.", null), false, -2, 2);
			this._traitValor.Initialize(new TextObject("{=toQLHG6x}Valor", null), new TextObject("{=Ugm9nO49}Valor represents your reputation for risking your life to win glory or wealth or advance your cause.", null), false, -2, 2);
			this._traitHonor.Initialize(new TextObject("{=0oGz5rVx}Honor", null), new TextObject("{=1vYgkaaK}Honor represents your reputation for respecting your formal commitments, like keeping your word and obeying the law.", null), false, -2, 2);
			this._traitGenerosity.Initialize(new TextObject("{=IuWu5Bu7}Generosity", null), new TextObject("{=IKzqzPDS}Generosity represents your loyalty to your kin and those who serve you, and your gratitude to those who have done you a favor.", null), false, -2, 2);
			this._traitCalculating.Initialize(new TextObject("{=5sMBbn7y}Calculating", null), new TextObject("{=QKjF5gTR}Calculating represents your ability to control your emotions for the sake of your long-term interests.", null), false, -2, 2);
			this._traitPersonaCurt.Initialize(new TextObject("{=!}PersonaCurt", null), new TextObject("{=!}PersonaCurt Description", null), false, -2, 2);
			this._traitPersonaIronic.Initialize(new TextObject("{=!}PersonaIronic", null), new TextObject("{=!}PersonaIronic Description", null), false, -2, 2);
			this._traitPersonaEarnest.Initialize(new TextObject("{=!}PersonaEarnest", null), new TextObject("{=!}PersonaEarnest Description", null), false, -2, 2);
			this._traitPersonaSoftspoken.Initialize(new TextObject("{=!}PersonaSoftspoken", null), new TextObject("{=!}PersonaSoftspoken Description", null), false, -2, 2);
			this._traitCommander.Initialize(new TextObject("{=RvKwdXWs}Commander", null), new TextObject("{=!}Commander Description", null), true, 0, 20);
			this._traitSurgery.Initialize(new TextObject("{=QBPrRdQJ}Surgeon", null), new TextObject("{=!}Surgeon Description", null), true, 0, 20);
			this._traitTracking.Initialize(new TextObject("{=dx0hmeH6}Tracking", null), new TextObject("{=!}Tracking Description", null), true, 0, 20);
			this._traitBlacksmith.Initialize(new TextObject("{=bNnQt4jN}Blacksmith", null), new TextObject("{=!}Blacksmith Description", null), true, 0, 20);
			this._traitSergeantCommandSkills.Initialize(new TextObject("{=!}SergeantCommandSkills", null), new TextObject("{=!}SergeantCommandSkills Description", null), true, 0, 20);
			this._traitEngineerSkills.Initialize(new TextObject("{=!}EngineerSkills", null), new TextObject("{=!}EngineerSkills Description", null), true, 0, 20);
			this._traitRogueSkills.Initialize(new TextObject("{=!}RogueSkills", null), new TextObject("{=!}RogueSkills Description", null), true, 0, 20);
			this._traitScoutSkills.Initialize(new TextObject("{=!}ScoutSkills", null), new TextObject("{=!}ScoutSkills Description", null), true, 0, 20);
			this._traitTraderSkills.Initialize(new TextObject("{=!}TraderSkills", null), new TextObject("{=!}Trader Description", null), true, 0, 20);
			this._traitThug.Initialize(new TextObject("{=thugtrait}Thug", null), new TextObject("{=Fjnw9ooa}Indicates a gang member specialized in extortion", null), true, 0, 20);
			this._traitSmuggler.Initialize(new TextObject("{=eeWx1yYd}Smuggler", null), new TextObject("{=87c7IhkZ}Indicates a gang member specialized in smuggling", null), true, 0, 20);
			this._traitEgalitarian.Initialize(new TextObject("{=HMFb1gaq}Egalitarian", null), new TextObject("{=!}Egalitarian Description", null), false, 0, 20);
			this._traitOligarchic.Initialize(new TextObject("{=hR6Zo6pD}Oligarchic", null), new TextObject("{=!}Oligarchic Description", null), false, 0, 20);
			this._traitAuthoritarian.Initialize(new TextObject("{=NaMPa4ML}Authoritarian", null), new TextObject("{=!}Authoritarian Description", null), false, 0, 20);
			this._traitNavalSoldier.Initialize(new TextObject("{=rGUOr2wg}Naval Soldier", null), new TextObject("{=!}Naval Soldier Description", null), true, 0, 20);
		}

		// Token: 0x040010F2 RID: 4338
		private const int MaxPersonalityTraitValue = 2;

		// Token: 0x040010F3 RID: 4339
		private const int MinPersonalityTraitValue = -2;

		// Token: 0x040010F4 RID: 4340
		private const int MaxHiddenTraitValue = 20;

		// Token: 0x040010F5 RID: 4341
		private const int MinHiddenTraitValue = 0;

		// Token: 0x040010F6 RID: 4342
		private TraitObject _traitMercy;

		// Token: 0x040010F7 RID: 4343
		private TraitObject _traitValor;

		// Token: 0x040010F8 RID: 4344
		private TraitObject _traitHonor;

		// Token: 0x040010F9 RID: 4345
		private TraitObject _traitGenerosity;

		// Token: 0x040010FA RID: 4346
		private TraitObject _traitCalculating;

		// Token: 0x040010FB RID: 4347
		private TraitObject _traitPersonaCurt;

		// Token: 0x040010FC RID: 4348
		private TraitObject _traitPersonaEarnest;

		// Token: 0x040010FD RID: 4349
		private TraitObject _traitPersonaIronic;

		// Token: 0x040010FE RID: 4350
		private TraitObject _traitPersonaSoftspoken;

		// Token: 0x040010FF RID: 4351
		private TraitObject _traitEgalitarian;

		// Token: 0x04001100 RID: 4352
		private TraitObject _traitOligarchic;

		// Token: 0x04001101 RID: 4353
		private TraitObject _traitAuthoritarian;

		// Token: 0x04001102 RID: 4354
		private TraitObject _traitSurgery;

		// Token: 0x04001103 RID: 4355
		private TraitObject _traitTracking;

		// Token: 0x04001104 RID: 4356
		private TraitObject _traitSergeantCommandSkills;

		// Token: 0x04001105 RID: 4357
		private TraitObject _traitRogueSkills;

		// Token: 0x04001106 RID: 4358
		private TraitObject _traitEngineerSkills;

		// Token: 0x04001107 RID: 4359
		private TraitObject _traitBlacksmith;

		// Token: 0x04001108 RID: 4360
		private TraitObject _traitScoutSkills;

		// Token: 0x04001109 RID: 4361
		private TraitObject _traitTraderSkills;

		// Token: 0x0400110A RID: 4362
		private TraitObject _traitFrequency;

		// Token: 0x0400110B RID: 4363
		private TraitObject _traitCommander;

		// Token: 0x0400110C RID: 4364
		private TraitObject _traitThug;

		// Token: 0x0400110D RID: 4365
		private TraitObject _traitSmuggler;

		// Token: 0x0400110E RID: 4366
		private TraitObject _traitNavalSoldier;

		// Token: 0x0400110F RID: 4367
		private readonly TraitObject[] _personality;
	}
}
