using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001D3 RID: 467
	public static class MusicParameters
	{
		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06001BC9 RID: 7113 RVA: 0x000606A6 File Offset: 0x0005E8A6
		public static int SmallBattleTreshold
		{
			get
			{
				return (int)MusicParameters._parameters[0];
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06001BCA RID: 7114 RVA: 0x000606B0 File Offset: 0x0005E8B0
		public static int MediumBattleTreshold
		{
			get
			{
				return (int)MusicParameters._parameters[1];
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06001BCB RID: 7115 RVA: 0x000606BA File Offset: 0x0005E8BA
		public static int LargeBattleTreshold
		{
			get
			{
				return (int)MusicParameters._parameters[2];
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06001BCC RID: 7116 RVA: 0x000606C4 File Offset: 0x0005E8C4
		public static float SmallBattleDistanceTreshold
		{
			get
			{
				return MusicParameters._parameters[3];
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06001BCD RID: 7117 RVA: 0x000606CD File Offset: 0x0005E8CD
		public static float MediumBattleDistanceTreshold
		{
			get
			{
				return MusicParameters._parameters[4];
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06001BCE RID: 7118 RVA: 0x000606D6 File Offset: 0x0005E8D6
		public static float LargeBattleDistanceTreshold
		{
			get
			{
				return MusicParameters._parameters[5];
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06001BCF RID: 7119 RVA: 0x000606DF File Offset: 0x0005E8DF
		public static float MaxBattleDistanceTreshold
		{
			get
			{
				return MusicParameters._parameters[6];
			}
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06001BD0 RID: 7120 RVA: 0x000606E8 File Offset: 0x0005E8E8
		public static float MinIntensity
		{
			get
			{
				return MusicParameters._parameters[7];
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06001BD1 RID: 7121 RVA: 0x000606F1 File Offset: 0x0005E8F1
		public static float DefaultStartIntensity
		{
			get
			{
				return MusicParameters._parameters[8];
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06001BD2 RID: 7122 RVA: 0x000606FA File Offset: 0x0005E8FA
		public static float PlayerChargeEffectMultiplierOnIntensity
		{
			get
			{
				return MusicParameters._parameters[9];
			}
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06001BD3 RID: 7123 RVA: 0x00060704 File Offset: 0x0005E904
		public static float BattleSizeEffectOnStartIntensity
		{
			get
			{
				return MusicParameters._parameters[10];
			}
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06001BD4 RID: 7124 RVA: 0x0006070E File Offset: 0x0005E90E
		public static float RandomEffectMultiplierOnStartIntensity
		{
			get
			{
				return MusicParameters._parameters[11];
			}
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x06001BD5 RID: 7125 RVA: 0x00060718 File Offset: 0x0005E918
		public static float FriendlyTroopDeadEffectOnIntensity
		{
			get
			{
				return MusicParameters._parameters[12];
			}
		}

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06001BD6 RID: 7126 RVA: 0x00060722 File Offset: 0x0005E922
		public static float EnemyTroopDeadEffectOnIntensity
		{
			get
			{
				return MusicParameters._parameters[13];
			}
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06001BD7 RID: 7127 RVA: 0x0006072C File Offset: 0x0005E92C
		public static float PlayerTroopDeadEffectMultiplierOnIntensity
		{
			get
			{
				return MusicParameters._parameters[14];
			}
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06001BD8 RID: 7128 RVA: 0x00060736 File Offset: 0x0005E936
		public static float BattleRatioTresholdOnIntensity
		{
			get
			{
				return MusicParameters._parameters[15];
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06001BD9 RID: 7129 RVA: 0x00060740 File Offset: 0x0005E940
		public static float BattleTurnsOneSideCooldown
		{
			get
			{
				return MusicParameters._parameters[16];
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06001BDA RID: 7130 RVA: 0x0006074A File Offset: 0x0005E94A
		public static float CampaignDarkModeThreshold
		{
			get
			{
				return MusicParameters._parameters[17];
			}
		}

		// Token: 0x06001BDB RID: 7131 RVA: 0x00060754 File Offset: 0x0005E954
		public static void LoadFromXml()
		{
			MusicParameters._parameters = new float[18];
			string text = ModuleHelper.GetModuleFullPath("Native") + "ModuleData/music_parameters.xml";
			XmlDocument xmlDocument = new XmlDocument();
			StreamReader streamReader = new StreamReader(text);
			string text2 = streamReader.ReadToEnd();
			xmlDocument.LoadXml(text2);
			streamReader.Close();
			foreach (object obj in xmlDocument.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.NodeType == XmlNodeType.Element && xmlNode.Name == "music_parameters")
				{
					using (IEnumerator enumerator2 = xmlNode.ChildNodes.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							XmlNode xmlNode2 = (XmlNode)obj2;
							if (xmlNode2.NodeType == XmlNodeType.Element)
							{
								MusicParameters.MusicParametersEnum musicParametersEnum = (MusicParameters.MusicParametersEnum)Enum.Parse(typeof(MusicParameters.MusicParametersEnum), xmlNode2.Attributes["id"].Value);
								float num = float.Parse(xmlNode2.Attributes["value"].Value, CultureInfo.InvariantCulture);
								MusicParameters._parameters[(int)musicParametersEnum] = num;
							}
						}
						break;
					}
				}
			}
			Debug.Print("MusicParameters have been resetted.", 0, Debug.DebugColor.Green, 281474976710656UL);
		}

		// Token: 0x04000956 RID: 2390
		private static float[] _parameters;

		// Token: 0x04000957 RID: 2391
		public const float ZeroIntensity = 0f;

		// Token: 0x0200050B RID: 1291
		private enum MusicParametersEnum
		{
			// Token: 0x04001CCC RID: 7372
			SmallBattleTreshold,
			// Token: 0x04001CCD RID: 7373
			MediumBattleTreshold,
			// Token: 0x04001CCE RID: 7374
			LargeBattleTreshold,
			// Token: 0x04001CCF RID: 7375
			SmallBattleDistanceTreshold,
			// Token: 0x04001CD0 RID: 7376
			MediumBattleDistanceTreshold,
			// Token: 0x04001CD1 RID: 7377
			LargeBattleDistanceTreshold,
			// Token: 0x04001CD2 RID: 7378
			MaxBattleDistanceTreshold,
			// Token: 0x04001CD3 RID: 7379
			MinIntensity,
			// Token: 0x04001CD4 RID: 7380
			DefaultStartIntensity,
			// Token: 0x04001CD5 RID: 7381
			PlayerChargeEffectMultiplierOnIntensity,
			// Token: 0x04001CD6 RID: 7382
			BattleSizeEffectOnStartIntensity,
			// Token: 0x04001CD7 RID: 7383
			RandomEffectMultiplierOnStartIntensity,
			// Token: 0x04001CD8 RID: 7384
			FriendlyTroopDeadEffectOnIntensity,
			// Token: 0x04001CD9 RID: 7385
			EnemyTroopDeadEffectOnIntensity,
			// Token: 0x04001CDA RID: 7386
			PlayerTroopDeadEffectMultiplierOnIntensity,
			// Token: 0x04001CDB RID: 7387
			BattleRatioTresholdOnIntensity,
			// Token: 0x04001CDC RID: 7388
			BattleTurnsOneSideCooldown,
			// Token: 0x04001CDD RID: 7389
			CampaignDarkModeThreshold,
			// Token: 0x04001CDE RID: 7390
			Count
		}
	}
}
