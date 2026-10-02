using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000322 RID: 802
	[AttributeUsage(AttributeTargets.Field)]
	public class MultiplayerOptionsProperty : Attribute
	{
		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x06002D95 RID: 11669 RVA: 0x000B001A File Offset: 0x000AE21A
		public bool HasBounds
		{
			get
			{
				return this.BoundsMax > this.BoundsMin;
			}
		}

		// Token: 0x06002D96 RID: 11670 RVA: 0x000B002C File Offset: 0x000AE22C
		public MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType optionValueType, MultiplayerOptionsProperty.ReplicationOccurrence replicationOccurrence, string description = null, int boundsMin = 0, int boundsMax = 0, string[] validGameModes = null, bool hasMultipleSelections = false, Type enumType = null)
		{
			this.OptionValueType = optionValueType;
			this.Replication = replicationOccurrence;
			this.Description = description;
			this.BoundsMin = boundsMin;
			this.BoundsMax = boundsMax;
			this.ValidGameModes = validGameModes;
			this.HasMultipleSelections = hasMultipleSelections;
			this.EnumType = enumType;
		}

		// Token: 0x040011F3 RID: 4595
		public readonly MultiplayerOptions.OptionValueType OptionValueType;

		// Token: 0x040011F4 RID: 4596
		public readonly MultiplayerOptionsProperty.ReplicationOccurrence Replication;

		// Token: 0x040011F5 RID: 4597
		public readonly string Description;

		// Token: 0x040011F6 RID: 4598
		public readonly int BoundsMin;

		// Token: 0x040011F7 RID: 4599
		public readonly int BoundsMax;

		// Token: 0x040011F8 RID: 4600
		public readonly string[] ValidGameModes;

		// Token: 0x040011F9 RID: 4601
		public readonly bool HasMultipleSelections;

		// Token: 0x040011FA RID: 4602
		public readonly Type EnumType;

		// Token: 0x02000604 RID: 1540
		public enum ReplicationOccurrence
		{
			// Token: 0x0400202F RID: 8239
			Never,
			// Token: 0x04002030 RID: 8240
			AtMapLoad,
			// Token: 0x04002031 RID: 8241
			Immediately
		}
	}
}
