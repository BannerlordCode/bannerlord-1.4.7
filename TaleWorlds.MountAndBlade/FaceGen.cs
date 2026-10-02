using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200021B RID: 539
	public class FaceGen : IFaceGen
	{
		// Token: 0x06001F4E RID: 8014 RVA: 0x0006C690 File Offset: 0x0006A890
		private FaceGen()
		{
			this._raceNamesDictionary = new Dictionary<string, int>();
			this._raceNamesArray = MBAPI.IMBFaceGen.GetRaceIds().Split(new char[] { ';' });
			for (int i = 0; i < this._raceNamesArray.Length; i++)
			{
				this._raceNamesDictionary[this._raceNamesArray[i]] = i;
			}
			this._monstersDictionary = new Dictionary<string, Monster>();
			this._monstersArray = new Monster[this._raceNamesArray.Length];
		}

		// Token: 0x06001F4F RID: 8015 RVA: 0x0006C713 File Offset: 0x0006A913
		public static void CreateInstance()
		{
			FaceGen.SetInstance(new FaceGen());
		}

		// Token: 0x06001F50 RID: 8016 RVA: 0x0006C720 File Offset: 0x0006A920
		public Monster GetMonster(string monsterID)
		{
			Monster @object;
			if (!this._monstersDictionary.TryGetValue(monsterID, out @object))
			{
				@object = Game.Current.ObjectManager.GetObject<Monster>(monsterID);
				this._monstersDictionary[monsterID] = @object;
			}
			return @object;
		}

		// Token: 0x06001F51 RID: 8017 RVA: 0x0006C75C File Offset: 0x0006A95C
		public Monster GetMonsterWithSuffix(int race, string suffix)
		{
			return this.GetMonster(this._raceNamesArray[race] + suffix);
		}

		// Token: 0x06001F52 RID: 8018 RVA: 0x0006C774 File Offset: 0x0006A974
		public Monster GetBaseMonsterFromRace(int race)
		{
			if (race >= 0 && race < this._monstersArray.Length)
			{
				Monster monster = this._monstersArray[race];
				if (monster == null)
				{
					monster = Game.Current.ObjectManager.GetObject<Monster>(this._raceNamesArray[race]);
					this._monstersArray[race] = monster;
				}
				return monster;
			}
			Debug.FailedAssert("Monster race index is out of bounds: " + race, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\FaceGen.cs", "GetBaseMonsterFromRace", 65);
			return null;
		}

		// Token: 0x06001F53 RID: 8019 RVA: 0x0006C7E4 File Offset: 0x0006A9E4
		public BodyProperties GetRandomBodyProperties(int race, bool isFemale, BodyProperties bodyPropertiesMin, BodyProperties bodyPropertiesMax, int hairCoverType, int seed, string hairTags, string beardTags, string tattooTags, float variationAmount)
		{
			return MBBodyProperties.GetRandomBodyProperties(race, isFemale, bodyPropertiesMin, bodyPropertiesMax, hairCoverType, seed, hairTags, beardTags, tattooTags, variationAmount);
		}

		// Token: 0x06001F54 RID: 8020 RVA: 0x0006C807 File Offset: 0x0006AA07
		void IFaceGen.GenerateParentBody(BodyProperties childBodyProperties, int race, ref BodyProperties motherBodyProperties, ref BodyProperties fatherBodyProperties)
		{
			MBBodyProperties.GenerateParentKey(childBodyProperties, race, ref motherBodyProperties, ref fatherBodyProperties);
		}

		// Token: 0x06001F55 RID: 8021 RVA: 0x0006C813 File Offset: 0x0006AA13
		void IFaceGen.SetHair(ref BodyProperties bodyProperties, int hair, int beard, int tattoo)
		{
			MBBodyProperties.SetHair(ref bodyProperties, hair, beard, tattoo);
		}

		// Token: 0x06001F56 RID: 8022 RVA: 0x0006C81F File Offset: 0x0006AA1F
		void IFaceGen.SetBody(ref BodyProperties bodyProperties, int build, int weight)
		{
			MBBodyProperties.SetBody(ref bodyProperties, build, weight);
		}

		// Token: 0x06001F57 RID: 8023 RVA: 0x0006C829 File Offset: 0x0006AA29
		void IFaceGen.SetPigmentation(ref BodyProperties bodyProperties, int skinColor, int hairColor, int eyeColor)
		{
			MBBodyProperties.SetPigmentation(ref bodyProperties, skinColor, hairColor, eyeColor);
		}

		// Token: 0x06001F58 RID: 8024 RVA: 0x0006C835 File Offset: 0x0006AA35
		public BodyProperties GetBodyPropertiesWithAge(ref BodyProperties bodyProperties, float age)
		{
			return MBBodyProperties.GetBodyPropertiesWithAge(ref bodyProperties, age);
		}

		// Token: 0x06001F59 RID: 8025 RVA: 0x0006C83E File Offset: 0x0006AA3E
		public void GetParamsFromBody(ref FaceGenerationParams faceGenerationParams, BodyProperties bodyProperties, bool earsAreHidden, bool mouthIsHidden)
		{
			MBBodyProperties.GetParamsFromKey(ref faceGenerationParams, bodyProperties, earsAreHidden, mouthIsHidden);
		}

		// Token: 0x06001F5A RID: 8026 RVA: 0x0006C84A File Offset: 0x0006AA4A
		public BodyMeshMaturityType GetMaturityTypeWithAge(float age)
		{
			return MBBodyProperties.GetMaturityType(age);
		}

		// Token: 0x06001F5B RID: 8027 RVA: 0x0006C852 File Offset: 0x0006AA52
		public static void FlushFaceCache()
		{
			MBBodyProperties.FlushFaceCache();
		}

		// Token: 0x06001F5C RID: 8028 RVA: 0x0006C859 File Offset: 0x0006AA59
		public int GetRaceCount()
		{
			return this._raceNamesArray.Length;
		}

		// Token: 0x06001F5D RID: 8029 RVA: 0x0006C863 File Offset: 0x0006AA63
		public int GetRaceOrDefault(string raceId)
		{
			return this._raceNamesDictionary[raceId];
		}

		// Token: 0x06001F5E RID: 8030 RVA: 0x0006C871 File Offset: 0x0006AA71
		public string GetBaseMonsterNameFromRace(int race)
		{
			return this._raceNamesArray[race];
		}

		// Token: 0x06001F5F RID: 8031 RVA: 0x0006C87B File Offset: 0x0006AA7B
		public string[] GetRaceNames()
		{
			return (string[])this._raceNamesArray.Clone();
		}

		// Token: 0x06001F60 RID: 8032 RVA: 0x0006C88D File Offset: 0x0006AA8D
		public int[] GetHairIndicesByTag(int race, int curGender, float age, string tag)
		{
			return MBBodyProperties.GetHairIndicesByTag(race, curGender, age, tag);
		}

		// Token: 0x06001F61 RID: 8033 RVA: 0x0006C899 File Offset: 0x0006AA99
		public int[] GetFacialIndicesByTag(int race, int curGender, float age, string tag)
		{
			return MBBodyProperties.GetFacialIndicesByTag(race, curGender, age, tag);
		}

		// Token: 0x06001F62 RID: 8034 RVA: 0x0006C8A5 File Offset: 0x0006AAA5
		public int[] GetTattooIndicesByTag(int race, int curGender, float age, string tag)
		{
			return MBBodyProperties.GetTattooIndicesByTag(race, curGender, age, tag);
		}

		// Token: 0x06001F63 RID: 8035 RVA: 0x0006C8B4 File Offset: 0x0006AAB4
		public float GetTattooZeroProbability(int race, int curGender, float age)
		{
			float num = 0f;
			MBBodyProperties.GetZeroProbabilities(race, curGender, age, ref num);
			return num;
		}

		// Token: 0x04000AA8 RID: 2728
		private readonly Dictionary<string, int> _raceNamesDictionary;

		// Token: 0x04000AA9 RID: 2729
		private readonly string[] _raceNamesArray;

		// Token: 0x04000AAA RID: 2730
		private readonly Dictionary<string, Monster> _monstersDictionary;

		// Token: 0x04000AAB RID: 2731
		private readonly Monster[] _monstersArray;
	}
}
