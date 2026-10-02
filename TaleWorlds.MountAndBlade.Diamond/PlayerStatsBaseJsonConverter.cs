using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TaleWorlds.Diamond;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000148 RID: 328
	public class PlayerStatsBaseJsonConverter : JsonConverter
	{
		// Token: 0x0600090E RID: 2318 RVA: 0x0000D456 File Offset: 0x0000B656
		public override bool CanConvert(Type objectType)
		{
			return typeof(AccessObject).IsAssignableFrom(objectType);
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x0000D468 File Offset: 0x0000B668
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JObject jobject = JObject.Load(reader);
			string text = (string)jobject["gameType"];
			if (text == null)
			{
				text = (string)jobject["GameType"];
			}
			PlayerStatsBase playerStatsBase;
			if (text == "Skirmish")
			{
				playerStatsBase = new PlayerStatsSkirmish();
			}
			else if (text == "Captain")
			{
				playerStatsBase = new PlayerStatsCaptain();
			}
			else if (text == "TeamDeathmatch")
			{
				playerStatsBase = new PlayerStatsTeamDeathmatch();
			}
			else if (text == "Siege")
			{
				playerStatsBase = new PlayerStatsSiege();
			}
			else if (text == "Duel")
			{
				playerStatsBase = new PlayerStatsDuel();
			}
			else
			{
				if (!(text == "Battle"))
				{
					return null;
				}
				playerStatsBase = new PlayerStatsBattle();
			}
			serializer.Populate(jobject.CreateReader(), playerStatsBase);
			return playerStatsBase;
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000910 RID: 2320 RVA: 0x0000D530 File Offset: 0x0000B730
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0000D533 File Offset: 0x0000B733
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
		}
	}
}
