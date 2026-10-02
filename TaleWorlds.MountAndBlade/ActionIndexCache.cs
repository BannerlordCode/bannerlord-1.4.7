using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200019F RID: 415
	public readonly struct ActionIndexCache : IEquatable<ActionIndexCache>
	{
		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06001638 RID: 5688 RVA: 0x00051CFF File Offset: 0x0004FEFF
		public int Index { get; }

		// Token: 0x06001639 RID: 5689 RVA: 0x00051D07 File Offset: 0x0004FF07
		public static ActionIndexCache Create(string actName)
		{
			if (!string.IsNullOrWhiteSpace(actName))
			{
				return new ActionIndexCache(actName);
			}
			return ActionIndexCache.act_none;
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x00051D1D File Offset: 0x0004FF1D
		private ActionIndexCache(string name)
		{
			this.Index = MBAnimation.GetActionCodeWithName(name);
		}

		// Token: 0x0600163B RID: 5691 RVA: 0x00051D2B File Offset: 0x0004FF2B
		internal ActionIndexCache(int actionIndex)
		{
			this.Index = actionIndex;
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x00051D34 File Offset: 0x0004FF34
		public string GetName()
		{
			if (this.Index != -1)
			{
				return MBAPI.IMBAnimation.GetActionNameWithCode(this.Index);
			}
			return "act_none";
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x00051D55 File Offset: 0x0004FF55
		public override bool Equals(object obj)
		{
			return this.Equals((ActionIndexCache)obj);
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x00051D63 File Offset: 0x0004FF63
		public bool Equals(ActionIndexCache other)
		{
			return this.Index == other.Index;
		}

		// Token: 0x0600163F RID: 5695 RVA: 0x00051D74 File Offset: 0x0004FF74
		public static bool operator ==(ActionIndexCache action0, ActionIndexCache action1)
		{
			return action0.Index == action1.Index;
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x00051D86 File Offset: 0x0004FF86
		public static bool operator !=(ActionIndexCache action0, ActionIndexCache action1)
		{
			return action0.Index != action1.Index;
		}

		// Token: 0x06001641 RID: 5697 RVA: 0x00051D9C File Offset: 0x0004FF9C
		public override int GetHashCode()
		{
			return this.Index.GetHashCode();
		}

		// Token: 0x0400077C RID: 1916
		public static readonly ActionIndexCache act_none = new ActionIndexCache(-1);

		// Token: 0x0400077D RID: 1917
		public static readonly ActionIndexCache act_pickup_down_begin = ActionIndexCache.Create("act_pickup_down_begin");

		// Token: 0x0400077E RID: 1918
		public static readonly ActionIndexCache act_pickup_down_end = ActionIndexCache.Create("act_pickup_down_end");

		// Token: 0x0400077F RID: 1919
		public static readonly ActionIndexCache act_pickup_down_begin_left_stance = ActionIndexCache.Create("act_pickup_down_begin_left_stance");

		// Token: 0x04000780 RID: 1920
		public static readonly ActionIndexCache act_pickup_down_end_left_stance = ActionIndexCache.Create("act_pickup_down_end_left_stance");

		// Token: 0x04000781 RID: 1921
		public static readonly ActionIndexCache act_pickup_down_left_begin = ActionIndexCache.Create("act_pickup_down_left_begin");

		// Token: 0x04000782 RID: 1922
		public static readonly ActionIndexCache act_pickup_down_left_end = ActionIndexCache.Create("act_pickup_down_left_end");

		// Token: 0x04000783 RID: 1923
		public static readonly ActionIndexCache act_pickup_down_left_begin_left_stance = ActionIndexCache.Create("act_pickup_down_left_begin_left_stance");

		// Token: 0x04000784 RID: 1924
		public static readonly ActionIndexCache act_pickup_down_left_end_left_stance = ActionIndexCache.Create("act_pickup_down_left_end_left_stance");

		// Token: 0x04000785 RID: 1925
		public static readonly ActionIndexCache act_pickup_middle_begin = ActionIndexCache.Create("act_pickup_middle_begin");

		// Token: 0x04000786 RID: 1926
		public static readonly ActionIndexCache act_pickup_middle_end = ActionIndexCache.Create("act_pickup_middle_end");

		// Token: 0x04000787 RID: 1927
		public static readonly ActionIndexCache act_pickup_middle_begin_left_stance = ActionIndexCache.Create("act_pickup_middle_begin_left_stance");

		// Token: 0x04000788 RID: 1928
		public static readonly ActionIndexCache act_pickup_middle_end_left_stance = ActionIndexCache.Create("act_pickup_middle_end_left_stance");

		// Token: 0x04000789 RID: 1929
		public static readonly ActionIndexCache act_pickup_middle_left_begin = ActionIndexCache.Create("act_pickup_middle_left_begin");

		// Token: 0x0400078A RID: 1930
		public static readonly ActionIndexCache act_pickup_middle_left_end = ActionIndexCache.Create("act_pickup_middle_left_end");

		// Token: 0x0400078B RID: 1931
		public static readonly ActionIndexCache act_pickup_middle_left_begin_left_stance = ActionIndexCache.Create("act_pickup_middle_left_begin_left_stance");

		// Token: 0x0400078C RID: 1932
		public static readonly ActionIndexCache act_pickup_middle_left_end_left_stance = ActionIndexCache.Create("act_pickup_middle_left_end_left_stance");

		// Token: 0x0400078D RID: 1933
		public static readonly ActionIndexCache act_pickup_up_begin = ActionIndexCache.Create("act_pickup_up_begin");

		// Token: 0x0400078E RID: 1934
		public static readonly ActionIndexCache act_pickup_up_end = ActionIndexCache.Create("act_pickup_up_end");

		// Token: 0x0400078F RID: 1935
		public static readonly ActionIndexCache act_pickup_up_begin_left_stance = ActionIndexCache.Create("act_pickup_up_begin_left_stance");

		// Token: 0x04000790 RID: 1936
		public static readonly ActionIndexCache act_pickup_up_end_left_stance = ActionIndexCache.Create("act_pickup_up_end_left_stance");

		// Token: 0x04000791 RID: 1937
		public static readonly ActionIndexCache act_pickup_up_left_begin = ActionIndexCache.Create("act_pickup_up_left_begin");

		// Token: 0x04000792 RID: 1938
		public static readonly ActionIndexCache act_pickup_up_left_end = ActionIndexCache.Create("act_pickup_up_left_end");

		// Token: 0x04000793 RID: 1939
		public static readonly ActionIndexCache act_pickup_up_left_begin_left_stance = ActionIndexCache.Create("act_pickup_up_left_begin_left_stance");

		// Token: 0x04000794 RID: 1940
		public static readonly ActionIndexCache act_pickup_up_left_end_left_stance = ActionIndexCache.Create("act_pickup_up_left_end_left_stance");

		// Token: 0x04000795 RID: 1941
		public static readonly ActionIndexCache act_pickup_from_right_down_horseback_begin = ActionIndexCache.Create("act_pickup_from_right_down_horseback_begin");

		// Token: 0x04000796 RID: 1942
		public static readonly ActionIndexCache act_pickup_from_right_down_horseback_end = ActionIndexCache.Create("act_pickup_from_right_down_horseback_end");

		// Token: 0x04000797 RID: 1943
		public static readonly ActionIndexCache act_pickup_from_right_down_horseback_left_begin = ActionIndexCache.Create("act_pickup_from_right_down_horseback_left_begin");

		// Token: 0x04000798 RID: 1944
		public static readonly ActionIndexCache act_pickup_from_right_down_horseback_left_end = ActionIndexCache.Create("act_pickup_from_right_down_horseback_left_end");

		// Token: 0x04000799 RID: 1945
		public static readonly ActionIndexCache act_pickup_from_right_middle_horseback_begin = ActionIndexCache.Create("act_pickup_from_right_middle_horseback_begin");

		// Token: 0x0400079A RID: 1946
		public static readonly ActionIndexCache act_pickup_from_right_middle_horseback_end = ActionIndexCache.Create("act_pickup_from_right_middle_horseback_end");

		// Token: 0x0400079B RID: 1947
		public static readonly ActionIndexCache act_pickup_from_right_middle_horseback_left_begin = ActionIndexCache.Create("act_pickup_from_right_middle_horseback_left_begin");

		// Token: 0x0400079C RID: 1948
		public static readonly ActionIndexCache act_pickup_from_right_middle_horseback_left_end = ActionIndexCache.Create("act_pickup_from_right_middle_horseback_left_end");

		// Token: 0x0400079D RID: 1949
		public static readonly ActionIndexCache act_pickup_from_right_up_horseback_begin = ActionIndexCache.Create("act_pickup_from_right_up_horseback_begin");

		// Token: 0x0400079E RID: 1950
		public static readonly ActionIndexCache act_pickup_from_right_up_horseback_end = ActionIndexCache.Create("act_pickup_from_right_up_horseback_end");

		// Token: 0x0400079F RID: 1951
		public static readonly ActionIndexCache act_pickup_from_right_up_horseback_left_begin = ActionIndexCache.Create("act_pickup_from_right_up_horseback_left_begin");

		// Token: 0x040007A0 RID: 1952
		public static readonly ActionIndexCache act_pickup_from_right_up_horseback_left_end = ActionIndexCache.Create("act_pickup_from_right_up_horseback_left_end");

		// Token: 0x040007A1 RID: 1953
		public static readonly ActionIndexCache act_pickup_from_left_down_horseback_begin = ActionIndexCache.Create("act_pickup_from_left_down_horseback_begin");

		// Token: 0x040007A2 RID: 1954
		public static readonly ActionIndexCache act_pickup_from_left_down_horseback_end = ActionIndexCache.Create("act_pickup_from_left_down_horseback_end");

		// Token: 0x040007A3 RID: 1955
		public static readonly ActionIndexCache act_pickup_from_left_down_horseback_left_begin = ActionIndexCache.Create("act_pickup_from_left_down_horseback_left_begin");

		// Token: 0x040007A4 RID: 1956
		public static readonly ActionIndexCache act_pickup_from_left_down_horseback_left_end = ActionIndexCache.Create("act_pickup_from_left_down_horseback_left_end");

		// Token: 0x040007A5 RID: 1957
		public static readonly ActionIndexCache act_pickup_from_left_middle_horseback_begin = ActionIndexCache.Create("act_pickup_from_left_middle_horseback_begin");

		// Token: 0x040007A6 RID: 1958
		public static readonly ActionIndexCache act_pickup_from_left_middle_horseback_end = ActionIndexCache.Create("act_pickup_from_left_middle_horseback_end");

		// Token: 0x040007A7 RID: 1959
		public static readonly ActionIndexCache act_pickup_from_left_middle_horseback_left_begin = ActionIndexCache.Create("act_pickup_from_left_middle_horseback_left_begin");

		// Token: 0x040007A8 RID: 1960
		public static readonly ActionIndexCache act_pickup_from_left_middle_horseback_left_end = ActionIndexCache.Create("act_pickup_from_left_middle_horseback_left_end");

		// Token: 0x040007A9 RID: 1961
		public static readonly ActionIndexCache act_pickup_from_left_up_horseback_begin = ActionIndexCache.Create("act_pickup_from_left_up_horseback_begin");

		// Token: 0x040007AA RID: 1962
		public static readonly ActionIndexCache act_pickup_from_left_up_horseback_end = ActionIndexCache.Create("act_pickup_from_left_up_horseback_end");

		// Token: 0x040007AB RID: 1963
		public static readonly ActionIndexCache act_pickup_from_left_up_horseback_left_begin = ActionIndexCache.Create("act_pickup_from_left_up_horseback_left_begin");

		// Token: 0x040007AC RID: 1964
		public static readonly ActionIndexCache act_pickup_from_left_up_horseback_left_end = ActionIndexCache.Create("act_pickup_from_left_up_horseback_left_end");

		// Token: 0x040007AD RID: 1965
		public static readonly ActionIndexCache act_pickup_boulder_begin = ActionIndexCache.Create("act_pickup_boulder_begin");

		// Token: 0x040007AE RID: 1966
		public static readonly ActionIndexCache act_pickup_boulder_end = ActionIndexCache.Create("act_pickup_boulder_end");

		// Token: 0x040007AF RID: 1967
		public static readonly ActionIndexCache act_usage_trebuchet_idle = ActionIndexCache.Create("act_usage_trebuchet_idle");

		// Token: 0x040007B0 RID: 1968
		public static readonly ActionIndexCache act_usage_trebuchet_reload = ActionIndexCache.Create("act_usage_trebuchet_reload");

		// Token: 0x040007B1 RID: 1969
		public static readonly ActionIndexCache act_usage_trebuchet_reload_2 = ActionIndexCache.Create("act_usage_trebuchet_reload_2");

		// Token: 0x040007B2 RID: 1970
		public static readonly ActionIndexCache act_usage_trebuchet_reload_idle = ActionIndexCache.Create("act_usage_trebuchet_reload_idle");

		// Token: 0x040007B3 RID: 1971
		public static readonly ActionIndexCache act_usage_trebuchet_reload_2_idle = ActionIndexCache.Create("act_usage_trebuchet_reload_2_idle");

		// Token: 0x040007B4 RID: 1972
		public static readonly ActionIndexCache act_usage_trebuchet_load_ammo = ActionIndexCache.Create("act_usage_trebuchet_load_ammo");

		// Token: 0x040007B5 RID: 1973
		public static readonly ActionIndexCache act_usage_trebuchet_shoot = ActionIndexCache.Create("act_usage_trebuchet_shoot");

		// Token: 0x040007B6 RID: 1974
		public static readonly ActionIndexCache act_usage_siege_machine_push = ActionIndexCache.Create("act_usage_siege_machine_push");

		// Token: 0x040007B7 RID: 1975
		public static readonly ActionIndexCache act_usage_ladder_lift_from_left_1_start = ActionIndexCache.Create("act_usage_ladder_lift_from_left_1_start");

		// Token: 0x040007B8 RID: 1976
		public static readonly ActionIndexCache act_usage_ladder_lift_from_left_2_start = ActionIndexCache.Create("act_usage_ladder_lift_from_left_2_start");

		// Token: 0x040007B9 RID: 1977
		public static readonly ActionIndexCache act_usage_ladder_lift_from_right_1_start = ActionIndexCache.Create("act_usage_ladder_lift_from_right_1_start");

		// Token: 0x040007BA RID: 1978
		public static readonly ActionIndexCache act_usage_ladder_lift_from_right_2_start = ActionIndexCache.Create("act_usage_ladder_lift_from_right_2_start");

		// Token: 0x040007BB RID: 1979
		public static readonly ActionIndexCache act_usage_ladder_pick_up_fork_begin = ActionIndexCache.Create("act_usage_ladder_pick_up_fork_begin");

		// Token: 0x040007BC RID: 1980
		public static readonly ActionIndexCache act_usage_ladder_pick_up_fork_end = ActionIndexCache.Create("act_usage_ladder_pick_up_fork_end");

		// Token: 0x040007BD RID: 1981
		public static readonly ActionIndexCache act_usage_ladder_push_back = ActionIndexCache.Create("act_usage_ladder_push_back");

		// Token: 0x040007BE RID: 1982
		public static readonly ActionIndexCache act_usage_ladder_push_back_stopped = ActionIndexCache.Create("act_usage_ladder_push_back_stopped");

		// Token: 0x040007BF RID: 1983
		public static readonly ActionIndexCache act_usage_batteringram_left = ActionIndexCache.Create("act_usage_batteringram_left");

		// Token: 0x040007C0 RID: 1984
		public static readonly ActionIndexCache act_usage_batteringram_left_slower = ActionIndexCache.Create("act_usage_batteringram_left_slower");

		// Token: 0x040007C1 RID: 1985
		public static readonly ActionIndexCache act_usage_batteringram_left_slowest = ActionIndexCache.Create("act_usage_batteringram_left_slowest");

		// Token: 0x040007C2 RID: 1986
		public static readonly ActionIndexCache act_usage_batteringram_right = ActionIndexCache.Create("act_usage_batteringram_right");

		// Token: 0x040007C3 RID: 1987
		public static readonly ActionIndexCache act_usage_batteringram_right_slower = ActionIndexCache.Create("act_usage_batteringram_right_slower");

		// Token: 0x040007C4 RID: 1988
		public static readonly ActionIndexCache act_usage_batteringram_right_slowest = ActionIndexCache.Create("act_usage_batteringram_right_slowest");

		// Token: 0x040007C5 RID: 1989
		public static readonly ActionIndexCache act_strike_bent_over = ActionIndexCache.Create("act_strike_bent_over");

		// Token: 0x040007C6 RID: 1990
		public static readonly ActionIndexCache act_strike_fall_back_back_rise = ActionIndexCache.Create("act_strike_fall_back_back_rise");

		// Token: 0x040007C7 RID: 1991
		public static readonly ActionIndexCache act_row_strike = ActionIndexCache.Create("act_row_strike");

		// Token: 0x040007C8 RID: 1992
		public static readonly ActionIndexCache act_stagger_forward = ActionIndexCache.Create("act_stagger_forward");

		// Token: 0x040007C9 RID: 1993
		public static readonly ActionIndexCache act_stagger_backward = ActionIndexCache.Create("act_stagger_backward");

		// Token: 0x040007CA RID: 1994
		public static readonly ActionIndexCache act_stagger_right = ActionIndexCache.Create("act_stagger_right");

		// Token: 0x040007CB RID: 1995
		public static readonly ActionIndexCache act_stagger_left = ActionIndexCache.Create("act_stagger_left");

		// Token: 0x040007CC RID: 1996
		public static readonly ActionIndexCache act_stagger_forward_2 = ActionIndexCache.Create("act_stagger_forward_2");

		// Token: 0x040007CD RID: 1997
		public static readonly ActionIndexCache act_stagger_backward_2 = ActionIndexCache.Create("act_stagger_backward_2");

		// Token: 0x040007CE RID: 1998
		public static readonly ActionIndexCache act_stagger_right_2 = ActionIndexCache.Create("act_stagger_right_2");

		// Token: 0x040007CF RID: 1999
		public static readonly ActionIndexCache act_stagger_left_2 = ActionIndexCache.Create("act_stagger_left_2");

		// Token: 0x040007D0 RID: 2000
		public static readonly ActionIndexCache act_stagger_forward_3 = ActionIndexCache.Create("act_stagger_forward_3");

		// Token: 0x040007D1 RID: 2001
		public static readonly ActionIndexCache act_stagger_backward_3 = ActionIndexCache.Create("act_stagger_backward_3");

		// Token: 0x040007D2 RID: 2002
		public static readonly ActionIndexCache act_stagger_right_3 = ActionIndexCache.Create("act_stagger_right_3");

		// Token: 0x040007D3 RID: 2003
		public static readonly ActionIndexCache act_stagger_left_3 = ActionIndexCache.Create("act_stagger_left_3");

		// Token: 0x040007D4 RID: 2004
		public static readonly ActionIndexCache act_command = ActionIndexCache.Create("act_command");

		// Token: 0x040007D5 RID: 2005
		public static readonly ActionIndexCache act_command_leftstance = ActionIndexCache.Create("act_command_leftstance");

		// Token: 0x040007D6 RID: 2006
		public static readonly ActionIndexCache act_command_unarmed = ActionIndexCache.Create("act_command_unarmed");

		// Token: 0x040007D7 RID: 2007
		public static readonly ActionIndexCache act_command_unarmed_leftstance = ActionIndexCache.Create("act_command_unarmed_leftstance");

		// Token: 0x040007D8 RID: 2008
		public static readonly ActionIndexCache act_command_2h = ActionIndexCache.Create("act_command_2h");

		// Token: 0x040007D9 RID: 2009
		public static readonly ActionIndexCache act_command_2h_leftstance = ActionIndexCache.Create("act_command_2h_leftstance");

		// Token: 0x040007DA RID: 2010
		public static readonly ActionIndexCache act_command_bow = ActionIndexCache.Create("act_command_bow");

		// Token: 0x040007DB RID: 2011
		public static readonly ActionIndexCache act_command_follow = ActionIndexCache.Create("act_command_follow");

		// Token: 0x040007DC RID: 2012
		public static readonly ActionIndexCache act_command_follow_leftstance = ActionIndexCache.Create("act_command_follow_leftstance");

		// Token: 0x040007DD RID: 2013
		public static readonly ActionIndexCache act_command_follow_unarmed = ActionIndexCache.Create("act_command_follow_unarmed");

		// Token: 0x040007DE RID: 2014
		public static readonly ActionIndexCache act_command_follow_unarmed_leftstance = ActionIndexCache.Create("act_command_follow_unarmed_leftstance");

		// Token: 0x040007DF RID: 2015
		public static readonly ActionIndexCache act_command_follow_2h = ActionIndexCache.Create("act_command_follow_2h");

		// Token: 0x040007E0 RID: 2016
		public static readonly ActionIndexCache act_command_follow_2h_leftstance = ActionIndexCache.Create("act_command_follow_2h_leftstance");

		// Token: 0x040007E1 RID: 2017
		public static readonly ActionIndexCache act_command_follow_bow = ActionIndexCache.Create("act_command_follow_bow");

		// Token: 0x040007E2 RID: 2018
		public static readonly ActionIndexCache act_horse_command = ActionIndexCache.Create("act_horse_command");

		// Token: 0x040007E3 RID: 2019
		public static readonly ActionIndexCache act_horse_command_unarmed = ActionIndexCache.Create("act_horse_command_unarmed");

		// Token: 0x040007E4 RID: 2020
		public static readonly ActionIndexCache act_horse_command_2h = ActionIndexCache.Create("act_horse_command_2h");

		// Token: 0x040007E5 RID: 2021
		public static readonly ActionIndexCache act_horse_command_bow = ActionIndexCache.Create("act_horse_command_bow");

		// Token: 0x040007E6 RID: 2022
		public static readonly ActionIndexCache act_horse_command_follow = ActionIndexCache.Create("act_horse_command_follow");

		// Token: 0x040007E7 RID: 2023
		public static readonly ActionIndexCache act_horse_command_follow_unarmed = ActionIndexCache.Create("act_horse_command_follow_unarmed");

		// Token: 0x040007E8 RID: 2024
		public static readonly ActionIndexCache act_horse_command_follow_2h = ActionIndexCache.Create("act_horse_command_follow_2h");

		// Token: 0x040007E9 RID: 2025
		public static readonly ActionIndexCache act_horse_command_follow_bow = ActionIndexCache.Create("act_horse_command_follow_bow");

		// Token: 0x040007EA RID: 2026
		public static readonly ActionIndexCache act_ship_connection_break = ActionIndexCache.Create("act_ship_connection_break");

		// Token: 0x040007EB RID: 2027
		public static readonly ActionIndexCache act_usage_hook_ready = ActionIndexCache.Create("act_usage_hook_ready");

		// Token: 0x040007EC RID: 2028
		public static readonly ActionIndexCache act_usage_hook_release = ActionIndexCache.Create("act_usage_hook_release");

		// Token: 0x040007ED RID: 2029
		public static readonly ActionIndexCache act_usage_row_idle_no_hold = ActionIndexCache.Create("act_usage_row_idle_no_hold");

		// Token: 0x040007EE RID: 2030
		public static readonly ActionIndexCache act_t_pose = ActionIndexCache.Create("act_t_pose");

		// Token: 0x040007EF RID: 2031
		public static readonly ActionIndexCache act_jump_loop = ActionIndexCache.Create("act_jump_loop");

		// Token: 0x040007F0 RID: 2032
		public static readonly ActionIndexCache act_stand_1 = ActionIndexCache.Create("act_stand_1");

		// Token: 0x040007F1 RID: 2033
		public static readonly ActionIndexCache act_idle_unarmed_1 = ActionIndexCache.Create("act_idle_unarmed_1");

		// Token: 0x040007F2 RID: 2034
		public static readonly ActionIndexCache act_walk_idle_1h_with_shield_left_stance = ActionIndexCache.Create("act_walk_idle_1h_with_shield_left_stance");

		// Token: 0x040007F3 RID: 2035
		public static readonly ActionIndexCache act_crouch_walk_idle_unarmed = ActionIndexCache.Create("act_crouch_walk_idle_unarmed");

		// Token: 0x040007F4 RID: 2036
		public static readonly ActionIndexCache act_beggar_idle = ActionIndexCache.Create("act_beggar_idle");

		// Token: 0x040007F5 RID: 2037
		public static readonly ActionIndexCache act_walk_idle_unarmed = ActionIndexCache.Create("act_walk_idle_unarmed");

		// Token: 0x040007F6 RID: 2038
		public static readonly ActionIndexCache act_horse_stand_1 = ActionIndexCache.Create("act_horse_stand_1");

		// Token: 0x040007F7 RID: 2039
		public static readonly ActionIndexCache act_hero_mount_idle_camel = ActionIndexCache.Create("act_hero_mount_idle_camel");

		// Token: 0x040007F8 RID: 2040
		public static readonly ActionIndexCache act_camel_idle_1 = ActionIndexCache.Create("act_camel_idle_1");

		// Token: 0x040007F9 RID: 2041
		public static readonly ActionIndexCache act_tableau_hand_armor_pose = ActionIndexCache.Create("act_tableau_hand_armor_pose");

		// Token: 0x040007FA RID: 2042
		public static readonly ActionIndexCache act_inventory_idle_start = ActionIndexCache.Create("act_inventory_idle_start");

		// Token: 0x040007FB RID: 2043
		public static readonly ActionIndexCache act_inventory_idle = ActionIndexCache.Create("act_inventory_idle");

		// Token: 0x040007FC RID: 2044
		public static readonly ActionIndexCache act_inventory_glove_equip = ActionIndexCache.Create("act_inventory_glove_equip");

		// Token: 0x040007FD RID: 2045
		public static readonly ActionIndexCache act_inventory_cloth_equip = ActionIndexCache.Create("act_inventory_cloth_equip");

		// Token: 0x040007FE RID: 2046
		public static readonly ActionIndexCache act_conversation_normal_loop = ActionIndexCache.Create("act_conversation_normal_loop");

		// Token: 0x040007FF RID: 2047
		public static readonly ActionIndexCache act_conversation_warrior_loop = ActionIndexCache.Create("act_conversation_warrior_loop");

		// Token: 0x04000800 RID: 2048
		public static readonly ActionIndexCache act_conversation_hip_loop = ActionIndexCache.Create("act_conversation_hip_loop");

		// Token: 0x04000801 RID: 2049
		public static readonly ActionIndexCache act_conversation_closed_loop = ActionIndexCache.Create("act_conversation_closed_loop");

		// Token: 0x04000802 RID: 2050
		public static readonly ActionIndexCache act_conversation_demure_loop = ActionIndexCache.Create("act_conversation_demure_loop");

		// Token: 0x04000803 RID: 2051
		public static readonly ActionIndexCache act_scared_reaction_1 = ActionIndexCache.Create("act_scared_reaction_1");

		// Token: 0x04000804 RID: 2052
		public static readonly ActionIndexCache act_scared_idle_1 = ActionIndexCache.Create("act_scared_idle_1");

		// Token: 0x04000805 RID: 2053
		public static readonly ActionIndexCache act_greeting_front_1 = ActionIndexCache.Create("act_greeting_front_1");

		// Token: 0x04000806 RID: 2054
		public static readonly ActionIndexCache act_greeting_front_2 = ActionIndexCache.Create("act_greeting_front_2");

		// Token: 0x04000807 RID: 2055
		public static readonly ActionIndexCache act_greeting_front_3 = ActionIndexCache.Create("act_greeting_front_3");

		// Token: 0x04000808 RID: 2056
		public static readonly ActionIndexCache act_greeting_front_4 = ActionIndexCache.Create("act_greeting_front_4");

		// Token: 0x04000809 RID: 2057
		public static readonly ActionIndexCache act_greeting_right_1 = ActionIndexCache.Create("act_greeting_right_1");

		// Token: 0x0400080A RID: 2058
		public static readonly ActionIndexCache act_greeting_right_2 = ActionIndexCache.Create("act_greeting_right_2");

		// Token: 0x0400080B RID: 2059
		public static readonly ActionIndexCache act_greeting_right_3 = ActionIndexCache.Create("act_greeting_right_3");

		// Token: 0x0400080C RID: 2060
		public static readonly ActionIndexCache act_greeting_right_4 = ActionIndexCache.Create("act_greeting_right_4");

		// Token: 0x0400080D RID: 2061
		public static readonly ActionIndexCache act_greeting_left_1 = ActionIndexCache.Create("act_greeting_left_1");

		// Token: 0x0400080E RID: 2062
		public static readonly ActionIndexCache act_greeting_left_2 = ActionIndexCache.Create("act_greeting_left_2");

		// Token: 0x0400080F RID: 2063
		public static readonly ActionIndexCache act_greeting_left_3 = ActionIndexCache.Create("act_greeting_left_3");

		// Token: 0x04000810 RID: 2064
		public static readonly ActionIndexCache act_greeting_left_4 = ActionIndexCache.Create("act_greeting_left_4");

		// Token: 0x04000811 RID: 2065
		public static readonly ActionIndexCache act_guard_cautious_look_around_1 = ActionIndexCache.Create("act_guard_cautious_look_around_1");

		// Token: 0x04000812 RID: 2066
		public static readonly ActionIndexCache act_guard_patrolling_cautious_look_around_1 = ActionIndexCache.Create("act_guard_patrolling_cautious_look_around_1");

		// Token: 0x04000813 RID: 2067
		public static readonly ActionIndexCache act_use_smithing_machine_ready = ActionIndexCache.Create("act_use_smithing_machine_ready");

		// Token: 0x04000814 RID: 2068
		public static readonly ActionIndexCache act_use_smithing_machine_loop = ActionIndexCache.Create("act_use_smithing_machine_loop");

		// Token: 0x04000815 RID: 2069
		public static readonly ActionIndexCache act_smithing_machine_anvil_start = ActionIndexCache.Create("act_smithing_machine_anvil_start");

		// Token: 0x04000816 RID: 2070
		public static readonly ActionIndexCache act_smithing_machine_anvil_part_2 = ActionIndexCache.Create("act_smithing_machine_anvil_part_2");

		// Token: 0x04000817 RID: 2071
		public static readonly ActionIndexCache act_smithing_machine_anvil_part_4 = ActionIndexCache.Create("act_smithing_machine_anvil_part_4");

		// Token: 0x04000818 RID: 2072
		public static readonly ActionIndexCache act_smithing_machine_anvil_part_5 = ActionIndexCache.Create("act_smithing_machine_anvil_part_5");

		// Token: 0x04000819 RID: 2073
		public static readonly ActionIndexCache act_childhood_schooled = ActionIndexCache.Create("act_childhood_schooled");

		// Token: 0x0400081A RID: 2074
		public static readonly ActionIndexCache act_arena_spectator = ActionIndexCache.Create("act_arena_spectator");

		// Token: 0x0400081B RID: 2075
		public static readonly ActionIndexCache act_argue_trio_middle = ActionIndexCache.Create("act_argue_trio_middle");

		// Token: 0x0400081C RID: 2076
		public static readonly ActionIndexCache act_argue_trio_middle_2 = ActionIndexCache.Create("act_argue_trio_middle_2");

		// Token: 0x0400081D RID: 2077
		public static readonly ActionIndexCache act_argue_trio_left = ActionIndexCache.Create("act_argue_trio_left");

		// Token: 0x0400081E RID: 2078
		public static readonly ActionIndexCache act_argue_trio_right = ActionIndexCache.Create("act_argue_trio_right");

		// Token: 0x0400081F RID: 2079
		public static readonly ActionIndexCache act_taunt_cheer_1 = ActionIndexCache.Create("act_taunt_cheer_1");

		// Token: 0x04000820 RID: 2080
		public static readonly ActionIndexCache act_taunt_cheer_2 = ActionIndexCache.Create("act_taunt_cheer_2");

		// Token: 0x04000821 RID: 2081
		public static readonly ActionIndexCache act_taunt_cheer_3 = ActionIndexCache.Create("act_taunt_cheer_3");

		// Token: 0x04000822 RID: 2082
		public static readonly ActionIndexCache act_taunt_cheer_4 = ActionIndexCache.Create("act_taunt_cheer_4");

		// Token: 0x04000823 RID: 2083
		public static readonly ActionIndexCache act_cheering_low_01 = ActionIndexCache.Create("act_cheering_low_01");

		// Token: 0x04000824 RID: 2084
		public static readonly ActionIndexCache act_cheering_low_02 = ActionIndexCache.Create("act_cheering_low_02");

		// Token: 0x04000825 RID: 2085
		public static readonly ActionIndexCache act_cheering_low_03 = ActionIndexCache.Create("act_cheering_low_03");

		// Token: 0x04000826 RID: 2086
		public static readonly ActionIndexCache act_cheering_low_04 = ActionIndexCache.Create("act_cheering_low_04");

		// Token: 0x04000827 RID: 2087
		public static readonly ActionIndexCache act_cheering_low_05 = ActionIndexCache.Create("act_cheering_low_05");

		// Token: 0x04000828 RID: 2088
		public static readonly ActionIndexCache act_cheering_low_06 = ActionIndexCache.Create("act_cheering_low_06");

		// Token: 0x04000829 RID: 2089
		public static readonly ActionIndexCache act_cheering_low_07 = ActionIndexCache.Create("act_cheering_low_07");

		// Token: 0x0400082A RID: 2090
		public static readonly ActionIndexCache act_cheering_low_08 = ActionIndexCache.Create("act_cheering_low_08");

		// Token: 0x0400082B RID: 2091
		public static readonly ActionIndexCache act_cheering_low_09 = ActionIndexCache.Create("act_cheering_low_09");

		// Token: 0x0400082C RID: 2092
		public static readonly ActionIndexCache act_cheering_low_10 = ActionIndexCache.Create("act_cheering_low_10");

		// Token: 0x0400082D RID: 2093
		public static readonly ActionIndexCache act_cheer_1 = ActionIndexCache.Create("act_cheer_1");

		// Token: 0x0400082E RID: 2094
		public static readonly ActionIndexCache act_cheer_2 = ActionIndexCache.Create("act_cheer_2");

		// Token: 0x0400082F RID: 2095
		public static readonly ActionIndexCache act_cheer_3 = ActionIndexCache.Create("act_cheer_3");

		// Token: 0x04000830 RID: 2096
		public static readonly ActionIndexCache act_cheer_4 = ActionIndexCache.Create("act_cheer_4");

		// Token: 0x04000831 RID: 2097
		public static readonly ActionIndexCache act_cheering_high_01 = ActionIndexCache.Create("act_cheering_high_01");

		// Token: 0x04000832 RID: 2098
		public static readonly ActionIndexCache act_cheering_high_02 = ActionIndexCache.Create("act_cheering_high_02");

		// Token: 0x04000833 RID: 2099
		public static readonly ActionIndexCache act_cheering_high_03 = ActionIndexCache.Create("act_cheering_high_03");

		// Token: 0x04000834 RID: 2100
		public static readonly ActionIndexCache act_cheering_high_04 = ActionIndexCache.Create("act_cheering_high_04");

		// Token: 0x04000835 RID: 2101
		public static readonly ActionIndexCache act_cheering_high_05 = ActionIndexCache.Create("act_cheering_high_05");

		// Token: 0x04000836 RID: 2102
		public static readonly ActionIndexCache act_cheering_high_06 = ActionIndexCache.Create("act_cheering_high_06");

		// Token: 0x04000837 RID: 2103
		public static readonly ActionIndexCache act_cheering_high_07 = ActionIndexCache.Create("act_cheering_high_07");

		// Token: 0x04000838 RID: 2104
		public static readonly ActionIndexCache act_cheering_high_08 = ActionIndexCache.Create("act_cheering_high_08");

		// Token: 0x04000839 RID: 2105
		public static readonly ActionIndexCache act_map_raid = ActionIndexCache.Create("act_map_raid");

		// Token: 0x0400083A RID: 2106
		public static readonly ActionIndexCache act_map_rider_camel_attack_1h = ActionIndexCache.Create("act_map_rider_camel_attack_1h");

		// Token: 0x0400083B RID: 2107
		public static readonly ActionIndexCache act_map_rider_camel_attack_1h_spear = ActionIndexCache.Create("act_map_rider_camel_attack_1h_spear");

		// Token: 0x0400083C RID: 2108
		public static readonly ActionIndexCache act_map_rider_camel_attack_1h_swing = ActionIndexCache.Create("act_map_rider_camel_attack_1h_swing");

		// Token: 0x0400083D RID: 2109
		public static readonly ActionIndexCache act_map_rider_camel_attack_2h_swing = ActionIndexCache.Create("act_map_rider_camel_attack_2h_swing");

		// Token: 0x0400083E RID: 2110
		public static readonly ActionIndexCache act_map_rider_camel_attack_unarmed = ActionIndexCache.Create("act_map_rider_camel_attack_unarmed");

		// Token: 0x0400083F RID: 2111
		public static readonly ActionIndexCache act_map_rider_horse_attack_1h = ActionIndexCache.Create("act_map_rider_horse_attack_1h");

		// Token: 0x04000840 RID: 2112
		public static readonly ActionIndexCache act_map_rider_horse_attack_1h_spear = ActionIndexCache.Create("act_map_rider_horse_attack_1h_spear");

		// Token: 0x04000841 RID: 2113
		public static readonly ActionIndexCache act_map_rider_horse_attack_1h_swing = ActionIndexCache.Create("act_map_rider_horse_attack_1h_swing");

		// Token: 0x04000842 RID: 2114
		public static readonly ActionIndexCache act_map_rider_horse_attack_2h_swing = ActionIndexCache.Create("act_map_rider_horse_attack_2h_swing");

		// Token: 0x04000843 RID: 2115
		public static readonly ActionIndexCache act_map_rider_horse_attack_unarmed = ActionIndexCache.Create("act_map_rider_horse_attack_unarmed");

		// Token: 0x04000844 RID: 2116
		public static readonly ActionIndexCache act_map_mount_attack_1h = ActionIndexCache.Create("act_map_mount_attack_1h");

		// Token: 0x04000845 RID: 2117
		public static readonly ActionIndexCache act_map_mount_attack_spear = ActionIndexCache.Create("act_map_mount_attack_spear");

		// Token: 0x04000846 RID: 2118
		public static readonly ActionIndexCache act_map_mount_attack_swing = ActionIndexCache.Create("act_map_mount_attack_swing");

		// Token: 0x04000847 RID: 2119
		public static readonly ActionIndexCache act_map_mount_attack_unarmed = ActionIndexCache.Create("act_map_mount_attack_unarmed");

		// Token: 0x04000848 RID: 2120
		public static readonly ActionIndexCache act_map_attack_1h = ActionIndexCache.Create("act_map_attack_1h");

		// Token: 0x04000849 RID: 2121
		public static readonly ActionIndexCache act_map_attack_2h = ActionIndexCache.Create("act_map_attack_2h");

		// Token: 0x0400084A RID: 2122
		public static readonly ActionIndexCache act_map_attack_spear_1h_or_2h = ActionIndexCache.Create("act_map_attack_spear_1h_or_2h");

		// Token: 0x0400084B RID: 2123
		public static readonly ActionIndexCache act_map_attack_unarmed = ActionIndexCache.Create("act_map_attack_unarmed");

		// Token: 0x0400084C RID: 2124
		public static readonly ActionIndexCache act_conversation_naval_start = ActionIndexCache.Create("act_conversation_naval_start");

		// Token: 0x0400084D RID: 2125
		public static readonly ActionIndexCache act_conversation_naval_idle_loop = ActionIndexCache.Create("act_conversation_naval_idle_loop");

		// Token: 0x0400084E RID: 2126
		public static readonly ActionIndexCache act_death_by_arrow_pelvis = ActionIndexCache.Create("act_death_by_arrow_pelvis");

		// Token: 0x0400084F RID: 2127
		public static readonly ActionIndexCache act_horse_fall_right = ActionIndexCache.Create("act_horse_fall_right");

		// Token: 0x04000850 RID: 2128
		public static readonly ActionIndexCache act_cutscene_npc_argue_player_1 = ActionIndexCache.Create("act_cutscene_npc_argue_player_1");

		// Token: 0x04000851 RID: 2129
		public static readonly ActionIndexCache act_escape_jump = ActionIndexCache.Create("act_escape_jump");

		// Token: 0x04000852 RID: 2130
		public static readonly ActionIndexCache act_raid_jump = ActionIndexCache.Create("act_raid_jump_1");
	}
}
