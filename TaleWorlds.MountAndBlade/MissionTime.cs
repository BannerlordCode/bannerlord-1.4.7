using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002A0 RID: 672
	public struct MissionTime : IComparable<MissionTime>
	{
		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x06002519 RID: 9497 RVA: 0x00086F57 File Offset: 0x00085157
		public long NumberOfTicks
		{
			get
			{
				return this._numberOfTicks;
			}
		}

		// Token: 0x0600251A RID: 9498 RVA: 0x00086F5F File Offset: 0x0008515F
		public MissionTime(long numberOfTicks)
		{
			this._numberOfTicks = numberOfTicks;
		}

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x0600251B RID: 9499 RVA: 0x00086F68 File Offset: 0x00085168
		private static long CurrentNumberOfTicks
		{
			get
			{
				return Mission.Current.MissionTimeTracker.NumberOfTicks;
			}
		}

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x0600251C RID: 9500 RVA: 0x00086F79 File Offset: 0x00085179
		public static MissionTime DeltaTime
		{
			get
			{
				return new MissionTime(Mission.Current.MissionTimeTracker.DeltaTimeInTicks);
			}
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x0600251D RID: 9501 RVA: 0x00086F8F File Offset: 0x0008518F
		private static long DeltaTimeInTicks
		{
			get
			{
				return Mission.Current.MissionTimeTracker.DeltaTimeInTicks;
			}
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x0600251E RID: 9502 RVA: 0x00086FA0 File Offset: 0x000851A0
		public static MissionTime Now
		{
			get
			{
				return new MissionTime(Mission.Current.MissionTimeTracker.NumberOfTicks);
			}
		}

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x0600251F RID: 9503 RVA: 0x00086FB6 File Offset: 0x000851B6
		public bool IsFuture
		{
			get
			{
				return MissionTime.CurrentNumberOfTicks < this._numberOfTicks;
			}
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06002520 RID: 9504 RVA: 0x00086FC5 File Offset: 0x000851C5
		public bool IsPast
		{
			get
			{
				return MissionTime.CurrentNumberOfTicks > this._numberOfTicks;
			}
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x06002521 RID: 9505 RVA: 0x00086FD4 File Offset: 0x000851D4
		public bool IsNow
		{
			get
			{
				return MissionTime.CurrentNumberOfTicks == this._numberOfTicks;
			}
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06002522 RID: 9506 RVA: 0x00086FE3 File Offset: 0x000851E3
		public float ElapsedHours
		{
			get
			{
				return (float)(MissionTime.CurrentNumberOfTicks - this._numberOfTicks) / 3.6E+10f;
			}
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06002523 RID: 9507 RVA: 0x00086FF8 File Offset: 0x000851F8
		public float ElapsedSeconds
		{
			get
			{
				return (float)(MissionTime.CurrentNumberOfTicks - this._numberOfTicks) * 1E-07f;
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06002524 RID: 9508 RVA: 0x0008700D File Offset: 0x0008520D
		public float ElapsedMilliseconds
		{
			get
			{
				return (float)(MissionTime.CurrentNumberOfTicks - this._numberOfTicks) / 10000f;
			}
		}

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06002525 RID: 9509 RVA: 0x00087022 File Offset: 0x00085222
		public double ToHours
		{
			get
			{
				return (double)this._numberOfTicks / 36000000000.0;
			}
		}

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06002526 RID: 9510 RVA: 0x00087035 File Offset: 0x00085235
		public double ToMinutes
		{
			get
			{
				return (double)this._numberOfTicks / 600000000.0;
			}
		}

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06002527 RID: 9511 RVA: 0x00087048 File Offset: 0x00085248
		public double ToSeconds
		{
			get
			{
				return (double)this._numberOfTicks * 1.0000000116860974E-07;
			}
		}

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x06002528 RID: 9512 RVA: 0x0008705B File Offset: 0x0008525B
		public double ToMilliseconds
		{
			get
			{
				return (double)this._numberOfTicks / 10000.0;
			}
		}

		// Token: 0x06002529 RID: 9513 RVA: 0x0008706E File Offset: 0x0008526E
		public static MissionTime MillisecondsFromNow(float valueInMilliseconds)
		{
			return new MissionTime((long)(valueInMilliseconds * 10000f + (float)MissionTime.CurrentNumberOfTicks));
		}

		// Token: 0x0600252A RID: 9514 RVA: 0x00087084 File Offset: 0x00085284
		public static MissionTime SecondsFromNow(float valueInSeconds)
		{
			return new MissionTime((long)(valueInSeconds * 10000000f + (float)MissionTime.CurrentNumberOfTicks));
		}

		// Token: 0x0600252B RID: 9515 RVA: 0x0008709A File Offset: 0x0008529A
		public bool Equals(MissionTime other)
		{
			return this._numberOfTicks == other._numberOfTicks;
		}

		// Token: 0x0600252C RID: 9516 RVA: 0x000870AA File Offset: 0x000852AA
		public override bool Equals(object obj)
		{
			return obj != null && obj is MissionTime && this.Equals((MissionTime)obj);
		}

		// Token: 0x0600252D RID: 9517 RVA: 0x000870C8 File Offset: 0x000852C8
		public override int GetHashCode()
		{
			return this._numberOfTicks.GetHashCode();
		}

		// Token: 0x0600252E RID: 9518 RVA: 0x000870E3 File Offset: 0x000852E3
		public int CompareTo(MissionTime other)
		{
			if (this._numberOfTicks == other._numberOfTicks)
			{
				return 0;
			}
			if (this._numberOfTicks > other._numberOfTicks)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x0600252F RID: 9519 RVA: 0x00087106 File Offset: 0x00085306
		public static bool operator <(MissionTime x, MissionTime y)
		{
			return x._numberOfTicks < y._numberOfTicks;
		}

		// Token: 0x06002530 RID: 9520 RVA: 0x00087116 File Offset: 0x00085316
		public static bool operator >(MissionTime x, MissionTime y)
		{
			return x._numberOfTicks > y._numberOfTicks;
		}

		// Token: 0x06002531 RID: 9521 RVA: 0x00087126 File Offset: 0x00085326
		public static bool operator ==(MissionTime x, MissionTime y)
		{
			return x._numberOfTicks == y._numberOfTicks;
		}

		// Token: 0x06002532 RID: 9522 RVA: 0x00087136 File Offset: 0x00085336
		public static bool operator !=(MissionTime x, MissionTime y)
		{
			return !(x == y);
		}

		// Token: 0x06002533 RID: 9523 RVA: 0x00087142 File Offset: 0x00085342
		public static bool operator <=(MissionTime x, MissionTime y)
		{
			return x._numberOfTicks <= y._numberOfTicks;
		}

		// Token: 0x06002534 RID: 9524 RVA: 0x00087155 File Offset: 0x00085355
		public static bool operator >=(MissionTime x, MissionTime y)
		{
			return x._numberOfTicks >= y._numberOfTicks;
		}

		// Token: 0x06002535 RID: 9525 RVA: 0x00087168 File Offset: 0x00085368
		public static MissionTime Milliseconds(float valueInMilliseconds)
		{
			return new MissionTime((long)(valueInMilliseconds * 10000f));
		}

		// Token: 0x06002536 RID: 9526 RVA: 0x00087177 File Offset: 0x00085377
		public static MissionTime Seconds(float valueInSeconds)
		{
			return new MissionTime((long)(valueInSeconds * 10000000f));
		}

		// Token: 0x06002537 RID: 9527 RVA: 0x00087186 File Offset: 0x00085386
		public static MissionTime Minutes(float valueInMinutes)
		{
			return new MissionTime((long)(valueInMinutes * 600000000f));
		}

		// Token: 0x06002538 RID: 9528 RVA: 0x00087195 File Offset: 0x00085395
		public static MissionTime Hours(float valueInHours)
		{
			return new MissionTime((long)(valueInHours * 3.6E+10f));
		}

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x06002539 RID: 9529 RVA: 0x000871A4 File Offset: 0x000853A4
		public static MissionTime Zero
		{
			get
			{
				return new MissionTime(0L);
			}
		}

		// Token: 0x0600253A RID: 9530 RVA: 0x000871AD File Offset: 0x000853AD
		public static MissionTime operator +(MissionTime g1, MissionTime g2)
		{
			return new MissionTime(g1._numberOfTicks + g2._numberOfTicks);
		}

		// Token: 0x0600253B RID: 9531 RVA: 0x000871C1 File Offset: 0x000853C1
		public static MissionTime operator -(MissionTime g1, MissionTime g2)
		{
			return new MissionTime(g1._numberOfTicks - g2._numberOfTicks);
		}

		// Token: 0x04000E59 RID: 3673
		public const long TimeTicksPerMilliSecond = 10000L;

		// Token: 0x04000E5A RID: 3674
		public const long TimeTicksPerSecond = 10000000L;

		// Token: 0x04000E5B RID: 3675
		public const long TimeTicksPerMinute = 600000000L;

		// Token: 0x04000E5C RID: 3676
		public const long TimeTicksPerHour = 36000000000L;

		// Token: 0x04000E5D RID: 3677
		public const float InvTimeTicksPerSecond = 1E-07f;

		// Token: 0x04000E5E RID: 3678
		private readonly long _numberOfTicks;
	}
}
