using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.Naval
{
	// Token: 0x0200022E RID: 558
	public class DefaultFigureheads
	{
		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x0600220C RID: 8716 RVA: 0x00096341 File Offset: 0x00094541
		public static DefaultFigureheads Instance
		{
			get
			{
				return Campaign.Current.DefaultFigureheads;
			}
		}

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x0600220D RID: 8717 RVA: 0x0009634D File Offset: 0x0009454D
		public static Figurehead Hawk
		{
			get
			{
				return DefaultFigureheads.Instance._hawk;
			}
		}

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x0600220E RID: 8718 RVA: 0x00096359 File Offset: 0x00094559
		public static Figurehead Lion
		{
			get
			{
				return DefaultFigureheads.Instance._lion;
			}
		}

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x0600220F RID: 8719 RVA: 0x00096365 File Offset: 0x00094565
		public static Figurehead Dragon
		{
			get
			{
				return DefaultFigureheads.Instance._dragon;
			}
		}

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06002210 RID: 8720 RVA: 0x00096371 File Offset: 0x00094571
		public static Figurehead WingsOfVictory
		{
			get
			{
				return DefaultFigureheads.Instance._wingsOfVictory;
			}
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06002211 RID: 8721 RVA: 0x0009637D File Offset: 0x0009457D
		public static Figurehead Ram
		{
			get
			{
				return DefaultFigureheads.Instance._ram;
			}
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06002212 RID: 8722 RVA: 0x00096389 File Offset: 0x00094589
		public static Figurehead SeaSerpent
		{
			get
			{
				return DefaultFigureheads.Instance._seaSerpent;
			}
		}

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x06002213 RID: 8723 RVA: 0x00096395 File Offset: 0x00094595
		public static Figurehead Viper
		{
			get
			{
				return DefaultFigureheads.Instance._viper;
			}
		}

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x06002214 RID: 8724 RVA: 0x000963A1 File Offset: 0x000945A1
		public static Figurehead SaberToothTiger
		{
			get
			{
				return DefaultFigureheads.Instance._saberToothTiger;
			}
		}

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x06002215 RID: 8725 RVA: 0x000963AD File Offset: 0x000945AD
		public static Figurehead Siren
		{
			get
			{
				return DefaultFigureheads.Instance._siren;
			}
		}

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x06002216 RID: 8726 RVA: 0x000963B9 File Offset: 0x000945B9
		public static Figurehead Horse
		{
			get
			{
				return DefaultFigureheads.Instance._horse;
			}
		}

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06002217 RID: 8727 RVA: 0x000963C5 File Offset: 0x000945C5
		public static Figurehead Turtle
		{
			get
			{
				return DefaultFigureheads.Instance._turtle;
			}
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06002218 RID: 8728 RVA: 0x000963D1 File Offset: 0x000945D1
		public static Figurehead Boar
		{
			get
			{
				return DefaultFigureheads.Instance._boar;
			}
		}

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x06002219 RID: 8729 RVA: 0x000963DD File Offset: 0x000945DD
		public static Figurehead Oxen
		{
			get
			{
				return DefaultFigureheads.Instance._oxen;
			}
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x0600221A RID: 8730 RVA: 0x000963E9 File Offset: 0x000945E9
		public static Figurehead Swan
		{
			get
			{
				return DefaultFigureheads.Instance._swan;
			}
		}

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x0600221B RID: 8731 RVA: 0x000963F5 File Offset: 0x000945F5
		public static Figurehead Deer
		{
			get
			{
				return DefaultFigureheads.Instance._deer;
			}
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x0600221C RID: 8732 RVA: 0x00096401 File Offset: 0x00094601
		public static Figurehead Raven
		{
			get
			{
				return DefaultFigureheads.Instance._raven;
			}
		}

		// Token: 0x0600221D RID: 8733 RVA: 0x0009640D File Offset: 0x0009460D
		public DefaultFigureheads()
		{
			this.RegisterAll();
		}

		// Token: 0x0600221E RID: 8734 RVA: 0x0009641C File Offset: 0x0009461C
		private void RegisterAll()
		{
			this._hawk = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("hawk"));
			this._lion = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("lion"));
			this._dragon = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("dragon"));
			this._wingsOfVictory = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("wings_of_victory"));
			this._ram = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("ram"));
			this._seaSerpent = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("sea_serpent"));
			this._viper = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("viper"));
			this._saberToothTiger = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("saber_tooth_tiger"));
			this._siren = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("siren"));
			this._horse = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("horse"));
			this._turtle = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("turtle"));
			this._boar = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("boar"));
			this._oxen = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("oxen"));
			this._swan = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("swan"));
			this._deer = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("deer"));
			this._raven = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("raven"));
			this.InitializeAll();
		}

		// Token: 0x0600221F RID: 8735 RVA: 0x000965D0 File Offset: 0x000947D0
		private void InitializeAll()
		{
			this._hawk.Initialize(new TextObject("{=VKFTub9a}Hawk", null), new TextObject("{=ku2DXiY9}Crew ranged accuracy {EFFECT_AMOUNT}%", null), 0.15f, MBObjectManager.Instance.GetObject<CultureObject>("aserai"), EffectIncrementType.AddFactor);
			this._lion.Initialize(new TextObject("{=D0SX1cFQ}Lion", null), new TextObject("{=EjjAmdXp}Crew battle morale {EFFECT_AMOUNT}", null), 10f, MBObjectManager.Instance.GetObject<CultureObject>("vlandia"), EffectIncrementType.Add);
			this._dragon.Initialize(new TextObject("{=GkvX7z6Y}Dragon", null), new TextObject("{=4Ok7GnHs}Boarded enemy crew morale {EFFECT_AMOUNT}", null), -5f, MBObjectManager.Instance.GetObject<CultureObject>("nord"), EffectIncrementType.Add);
			this._wingsOfVictory.Initialize(new TextObject("{=ci0npfYB}Wings Of Victory", null), new TextObject("{=mQuaMNVb}Party battle experience {EFFECT_AMOUNT}%", null), 0.15f, MBObjectManager.Instance.GetObject<CultureObject>("empire"), EffectIncrementType.AddFactor);
			this._ram.Initialize(new TextObject("{=shipFigureheadRam}Ram", null), new TextObject("{=eJ4MC1KO}Ramming ship and morale damage {EFFECT_AMOUNT}%.", null), 0.2f, MBObjectManager.Instance.GetObject<CultureObject>("empire"), EffectIncrementType.AddFactor);
			this._seaSerpent.Initialize(new TextObject("{=fsb5EEbg}Sea Serpent", null), new TextObject("{=OraB7RjB}Fire damage resistance {EFFECT_AMOUNT}%", null), 0.4f, MBObjectManager.Instance.GetObject<CultureObject>("nord"), EffectIncrementType.AddFactor);
			this._viper.Initialize(new TextObject("{=LTOaBiw3}Viper", null), new TextObject("{=NxIUg152}Ballista reload speed {EFFECT_AMOUNT}", null), 0.25f, MBObjectManager.Instance.GetObject<CultureObject>("aserai"), EffectIncrementType.AddFactor);
			this._saberToothTiger.Initialize(new TextObject("{=113F2KC5}Saber Tooth Tiger", null), new TextObject("{=qWDM0Oa1}Archer armor penetration {EFFECT_AMOUNT}", null), 0.1f, MBObjectManager.Instance.GetObject<CultureObject>("vlandia"), EffectIncrementType.AddFactor);
			this._siren.Initialize(new TextObject("{=wrwdRGkW}Siren", null), new TextObject("{=iBPMtWzZ}Boarded enemy crew melee damage {EFFECT_AMOUNT}%", null), -0.1f, MBObjectManager.Instance.GetObject<CultureObject>("sturgia"), EffectIncrementType.AddFactor);
			this._horse.Initialize(new TextObject("{=LwfILaRH}Horse", null), new TextObject("{=sMCpa5Sk}Ship travel speed {EFFECT_AMOUNT}%", null), 0.1f, MBObjectManager.Instance.GetObject<CultureObject>("khuzait"), EffectIncrementType.AddFactor);
			this._turtle.Initialize(new TextObject("{=Ni8CSaxD}Turtle", null), new TextObject("{=bAWHXCsb}Crew shield hitpoints {EFFECT_AMOUNT}%", null), 0.4f, MBObjectManager.Instance.GetObject<CultureObject>("sturgia"), EffectIncrementType.AddFactor);
			this._boar.Initialize(new TextObject("{=0OrIliBh}Boar", null), new TextObject("{=FPZ9QOGl}Crew armor {EFFECT_AMOUNT}%", null), 0.1f, MBObjectManager.Instance.GetObject<CultureObject>("battania"), EffectIncrementType.AddFactor);
			this._oxen.Initialize(new TextObject("{=mGy1EcUd}Oxen", null), new TextObject("{=D2ZA2XT6}Crew hitpoints {EFFECT_AMOUNT}", null), 10f, MBObjectManager.Instance.GetObject<CultureObject>("battania"), EffectIncrementType.Add);
			this._swan.Initialize(new TextObject("{=ZSA1mySL}Swan", null), new TextObject("{=JJTWn3zs}Sail force {EFFECT_AMOUNT}%", null), 0.1f, MBObjectManager.Instance.GetObject<CultureObject>("empire"), EffectIncrementType.AddFactor);
			this._deer.Initialize(new TextObject("{=XbNVQdZN}Deer", null), new TextObject("{=foC3qNav}Oar force {EFFECT_AMOUNT}%", null), 0.15f, MBObjectManager.Instance.GetObject<CultureObject>("sturgia"), EffectIncrementType.AddFactor);
			this._raven.Initialize(new TextObject("{=NVKwvl1G}Raven", null), new TextObject("{=QsR8WTpA}Crew throwing weapon damage {EFFECT_AMOUNT}%", null), 0.1f, MBObjectManager.Instance.GetObject<CultureObject>("nord"), EffectIncrementType.AddFactor);
		}

		// Token: 0x040009E8 RID: 2536
		private Figurehead _hawk;

		// Token: 0x040009E9 RID: 2537
		private Figurehead _lion;

		// Token: 0x040009EA RID: 2538
		private Figurehead _dragon;

		// Token: 0x040009EB RID: 2539
		private Figurehead _wingsOfVictory;

		// Token: 0x040009EC RID: 2540
		private Figurehead _ram;

		// Token: 0x040009ED RID: 2541
		private Figurehead _seaSerpent;

		// Token: 0x040009EE RID: 2542
		private Figurehead _viper;

		// Token: 0x040009EF RID: 2543
		private Figurehead _saberToothTiger;

		// Token: 0x040009F0 RID: 2544
		private Figurehead _siren;

		// Token: 0x040009F1 RID: 2545
		private Figurehead _horse;

		// Token: 0x040009F2 RID: 2546
		private Figurehead _turtle;

		// Token: 0x040009F3 RID: 2547
		private Figurehead _boar;

		// Token: 0x040009F4 RID: 2548
		private Figurehead _oxen;

		// Token: 0x040009F5 RID: 2549
		private Figurehead _swan;

		// Token: 0x040009F6 RID: 2550
		private Figurehead _deer;

		// Token: 0x040009F7 RID: 2551
		private Figurehead _raven;
	}
}
