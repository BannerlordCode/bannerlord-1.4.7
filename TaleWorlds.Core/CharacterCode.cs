using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core
{
	// Token: 0x02000024 RID: 36
	public class CharacterCode
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x000071CF File Offset: 0x000053CF
		public bool IsEmpty
		{
			get
			{
				return string.IsNullOrEmpty(this.Code);
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x000071DC File Offset: 0x000053DC
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x000071E4 File Offset: 0x000053E4
		public string EquipmentCode { get; private set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x000071ED File Offset: 0x000053ED
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x000071F5 File Offset: 0x000053F5
		public string Code { get; private set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x000071FE File Offset: 0x000053FE
		// (set) Token: 0x060001BA RID: 442 RVA: 0x00007206 File Offset: 0x00005406
		public bool IsFemale { get; private set; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001BB RID: 443 RVA: 0x0000720F File Offset: 0x0000540F
		// (set) Token: 0x060001BC RID: 444 RVA: 0x00007217 File Offset: 0x00005417
		public bool IsHero { get; private set; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001BD RID: 445 RVA: 0x00007220 File Offset: 0x00005420
		public float FaceDirtAmount
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001BE RID: 446 RVA: 0x00007227 File Offset: 0x00005427
		public Banner Banner
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001BF RID: 447 RVA: 0x0000722A File Offset: 0x0000542A
		// (set) Token: 0x060001C0 RID: 448 RVA: 0x00007232 File Offset: 0x00005432
		public FormationClass FormationClass { get; set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x0000723B File Offset: 0x0000543B
		// (set) Token: 0x060001C2 RID: 450 RVA: 0x00007243 File Offset: 0x00005443
		public uint Color1 { get; set; } = Color.White.ToUnsignedInteger();

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x0000724C File Offset: 0x0000544C
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x00007254 File Offset: 0x00005454
		public uint Color2 { get; set; } = Color.White.ToUnsignedInteger();

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x0000725D File Offset: 0x0000545D
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x00007265 File Offset: 0x00005465
		public int Race { get; private set; }

		// Token: 0x060001C7 RID: 455 RVA: 0x0000726E File Offset: 0x0000546E
		public Equipment CalculateEquipment()
		{
			if (this.EquipmentCode == null)
			{
				return null;
			}
			return Equipment.CreateFromEquipmentCode(this.EquipmentCode);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00007288 File Offset: 0x00005488
		private CharacterCode()
		{
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x000072C1 File Offset: 0x000054C1
		public static CharacterCode CreateFrom(BasicCharacterObject character)
		{
			return CharacterCode.CreateFrom(character, character.Equipment);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x000072D0 File Offset: 0x000054D0
		public static CharacterCode CreateFrom(BasicCharacterObject character, Equipment equipment)
		{
			CharacterCode characterCode = new CharacterCode();
			Equipment equipment2 = equipment ?? character.Equipment;
			string text = ((equipment2 != null) ? equipment2.CalculateEquipmentCode() : null);
			characterCode.EquipmentCode = text;
			characterCode.BodyProperties = character.GetBodyProperties(equipment2, -1);
			characterCode.IsFemale = character.IsFemale;
			characterCode.IsHero = character.IsHero;
			characterCode.FormationClass = character.DefaultFormationClass;
			characterCode.Race = character.Race;
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(16, "CreateFrom");
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(text);
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(characterCode.BodyProperties.ToString());
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(characterCode.IsFemale ? "1" : "0");
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(characterCode.IsHero ? "1" : "0");
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(((int)characterCode.FormationClass).ToString());
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(characterCode.Color1.ToString());
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(characterCode.Color2.ToString());
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(characterCode.Race.ToString());
			mbstringBuilder.Append<string>("@---@");
			characterCode.Code = mbstringBuilder.ToStringAndRelease();
			return characterCode;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00007490 File Offset: 0x00005690
		public static CharacterCode CreateFrom(string equipmentCode, BodyProperties bodyProperties, bool isFemale, bool isHero, uint color1, uint color2, FormationClass formationClass, int race)
		{
			CharacterCode characterCode = new CharacterCode();
			characterCode.EquipmentCode = equipmentCode;
			characterCode.BodyProperties = bodyProperties;
			characterCode.IsFemale = isFemale;
			characterCode.IsHero = isHero;
			characterCode.Color1 = color1;
			characterCode.Color2 = color2;
			characterCode.FormationClass = formationClass;
			characterCode.Race = race;
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(16, "CreateFrom");
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(equipmentCode);
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(characterCode.BodyProperties.ToString());
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(characterCode.IsFemale ? "1" : "0");
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(characterCode.IsHero ? "1" : "0");
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(((int)characterCode.FormationClass).ToString());
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(characterCode.Color1.ToString());
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(characterCode.Color2.ToString());
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(characterCode.Race.ToString());
			mbstringBuilder.Append<string>("@---@");
			characterCode.Code = mbstringBuilder.ToStringAndRelease();
			return characterCode;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000762C File Offset: 0x0000582C
		public string CreateNewCodeString()
		{
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(16, "CreateNewCodeString");
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(this.EquipmentCode);
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(this.BodyProperties.ToString());
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(this.IsFemale ? "1" : "0");
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(this.IsHero ? "1" : "0");
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(((int)this.FormationClass).ToString());
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(this.Color1.ToString());
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(this.Color2.ToString());
			mbstringBuilder.Append<string>("@---@");
			mbstringBuilder.Append<string>(this.Race.ToString());
			mbstringBuilder.Append<string>("@---@");
			return mbstringBuilder.ToStringAndRelease();
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00007782 File Offset: 0x00005982
		public static CharacterCode CreateEmpty()
		{
			return new CharacterCode();
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000778C File Offset: 0x0000598C
		public static CharacterCode CreateFrom(string code)
		{
			CharacterCode characterCode = new CharacterCode();
			int num = 0;
			int num2;
			for (num2 = code.IndexOf("@---@", StringComparison.InvariantCulture); num2 == num; num2 = code.IndexOf("@---@", num, StringComparison.InvariantCulture))
			{
				num = num2 + 5;
			}
			string text = code.Substring(num, num2 - num);
			do
			{
				num = num2 + 5;
				num2 = code.IndexOf("@---@", num, StringComparison.InvariantCulture);
			}
			while (num2 == num);
			string text2 = code.Substring(num, num2 - num);
			do
			{
				num = num2 + 5;
				num2 = code.IndexOf("@---@", num, StringComparison.InvariantCulture);
			}
			while (num2 == num);
			string text3 = code.Substring(num, num2 - num);
			do
			{
				num = num2 + 5;
				num2 = code.IndexOf("@---@", num, StringComparison.InvariantCulture);
			}
			while (num2 == num);
			string text4 = code.Substring(num, num2 - num);
			do
			{
				num = num2 + 5;
				num2 = code.IndexOf("@---@", num, StringComparison.InvariantCulture);
			}
			while (num2 == num);
			string text5 = code.Substring(num, num2 - num);
			do
			{
				num = num2 + 5;
				num2 = code.IndexOf("@---@", num, StringComparison.InvariantCulture);
			}
			while (num2 == num);
			string text6 = code.Substring(num, num2 - num);
			do
			{
				num = num2 + 5;
				num2 = code.IndexOf("@---@", num, StringComparison.InvariantCulture);
			}
			while (num2 == num);
			string text7 = code.Substring(num, num2 - num);
			num = num2 + 5;
			num2 = code.IndexOf("@---@", num, StringComparison.InvariantCulture);
			string text8 = ((num2 >= 0) ? code.Substring(num, num2 - num) : code.Substring(num));
			characterCode.EquipmentCode = text;
			BodyProperties bodyProperties;
			if (BodyProperties.FromString(text2, out bodyProperties))
			{
				characterCode.BodyProperties = bodyProperties;
			}
			else
			{
				Debug.FailedAssert("Cannot read the character code body property", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.Core\\CharacterCode.cs", "CreateFrom", 241);
			}
			characterCode.IsFemale = Convert.ToInt32(text3) == 1;
			characterCode.IsHero = Convert.ToInt32(text4) == 1;
			characterCode.FormationClass = (FormationClass)Convert.ToInt32(text5);
			characterCode.Color1 = Convert.ToUInt32(text6);
			characterCode.Color2 = Convert.ToUInt32(text7);
			characterCode.Race = Convert.ToInt32(text8);
			characterCode.Code = code;
			return characterCode;
		}

		// Token: 0x04000179 RID: 377
		public const string SpecialCodeSeparator = "@---@";

		// Token: 0x0400017A RID: 378
		public const int SpecialCodeSeparatorLength = 5;

		// Token: 0x0400017D RID: 381
		public BodyProperties BodyProperties;
	}
}
