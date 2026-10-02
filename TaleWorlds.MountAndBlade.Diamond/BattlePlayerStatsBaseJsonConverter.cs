using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000F3 RID: 243
	public class BattlePlayerStatsBaseJsonConverter : JsonConverter
	{
		// Token: 0x060004CA RID: 1226 RVA: 0x00005633 File Offset: 0x00003833
		public override bool CanConvert(Type objectType)
		{
			return typeof(BattlePlayerStatsBase).IsAssignableFrom(objectType);
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x00005648 File Offset: 0x00003848
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			JObject jobject = JObject.Load(reader);
			string text = (string)jobject["GameType"];
			BattlePlayerStatsBase battlePlayerStatsBase;
			if (text == "Skirmish")
			{
				battlePlayerStatsBase = new BattlePlayerStatsSkirmish();
			}
			else if (text == "Captain")
			{
				battlePlayerStatsBase = new BattlePlayerStatsCaptain();
			}
			else if (text == "Siege")
			{
				battlePlayerStatsBase = new BattlePlayerStatsSiege();
			}
			else if (text == "TeamDeathmatch")
			{
				battlePlayerStatsBase = new BattlePlayerStatsTeamDeathmatch();
			}
			else if (text == "Duel")
			{
				battlePlayerStatsBase = new BattlePlayerStatsDuel();
			}
			else
			{
				if (!(text == "Battle"))
				{
					return null;
				}
				battlePlayerStatsBase = new BattlePlayerStatsBattle();
			}
			serializer.Populate(jobject.CreateReader(), battlePlayerStatsBase);
			return battlePlayerStatsBase;
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060004CC RID: 1228 RVA: 0x00005708 File Offset: 0x00003908
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0000570B File Offset: 0x0000390B
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
		}
	}
}
