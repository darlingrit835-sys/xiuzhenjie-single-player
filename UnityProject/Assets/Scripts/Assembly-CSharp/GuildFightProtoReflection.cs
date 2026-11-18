using System;
using Google.Protobuf.Reflection;

public static class GuildFightProtoReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static GuildFightProtoReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChVHdWlsZEZpZ2h0UHJvdG8ucHJvdG8igwEKK0d1aWxkTW9uc3RlckZpZ2h0" + "QmVnaW5Ccm9hZGNhc3RSZXNwXzEwMTgwMDESDwoHZ3VpbGRJZBgBIAEoBRIQ" + "CghkZWZlbnNlcxgCIAMoBRIYChBndWlsZEF0dGFja1Bvd2VyGAMgASgFEhcK" + "D3JvbGVBdHRhY2tQb3dlchgEIAEoBSI7CihHdWlsZE1vbnN0ZXJGaWdodGlu" + "Z0Jyb2FkY2FzdFJlcV8xMDE4MDAyEg8KB2d1aWxkSWQYASABKAUiuQEKKUd1" + "aWxkTW9uc3RlckZpZ2h0aW5nQnJvYWRjYXN0UmVzcF8xMDE4MDAyEg8KB2d1" + "aWxkSWQYASABKAUSEAoIZGVmZW5zZXMYAiADKAUSGAoQZ3VpbGRBdHRhY2tQ" + "b3dlchgDIAEoBRIVCg1ndWlsZERlZmVuc2VzGAQgASgFEg0KBXRpbWVzGAUg" + "ASgFEhUKDW1vbnN0ZXJQb3dlcnMYBiADKAUSEgoKaXNGaW5pc2hlZBgHIAEo" + "CCJMCidHdWlsZE1vbnN0ZXJGaWdodGluZ1JvbGVIZWxwUmVxXzEwMTgwMDMS" + "DgoGcm9sZUlkGAEgASgJEhEKCWhlbHBJbmRleBgCIAEoBSJLCihHdWlsZE1v" + "bnN0ZXJGaWdodGluZ1JvbGVIZWxwUmVzcF8xMDE4MDAzEgwKBGNvZGUYASAB" + "KAUSEQoJaXNTdWNjZXNzGAIgASgIIvABCiBHdWlsZE1vbnN0ZXJGaW5pc2hl" + "ZFJlc3BfMTAxODAwNBIPCgdndWlsZElkGAEgASgFEhEKCWlzVmljdG9yeRgC" + "IAEoCBIUCgxsaW5nQ2FpQ291bnQYAyABKAUSFgoObGluZ0t1YW5nQ291bnQY" + "BCABKAUSFAoMbGluZ1poaUNvdW50GAUgASgFEhIKCnN0b25lQ291bnQYBiAB" + "KAMSEQoJamFkZUNvdW50GAcgASgFEhYKDmNob3VKaWFuZ0Npc2h1GAggASgF" + "EhIKCmRlYXRoQ291bnQYCSABKAUSEQoJa2lsbENvdW50GAogASgFIlEKLUd1" + "aWxkTW9uc3RlckZpZ2h0TWVzc2FnZUJyb2FkY2FzdFJlc3BfMTAxODAwNRIP" + "CgdndWlsZElkGAEgASgFEg8KB21lc3NhZ2UYAiABKAlCMgofY29tLmxvbmdt" + "by5nYW1lc2VydmVyLm5ldC5wcm90b0IPR3VpbGRGaWdodFByb3RvYgZwcm90" + "bzM="), new FileDescriptor[0], new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[7]
		{
			new GeneratedClrTypeInfo(typeof(GuildMonsterFightBeginBroadcastResp_1018001), GuildMonsterFightBeginBroadcastResp_1018001.Parser, new string[4] { "GuildId", "Defenses", "GuildAttackPower", "RoleAttackPower" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(GuildMonsterFightingBroadcastReq_1018002), GuildMonsterFightingBroadcastReq_1018002.Parser, new string[1] { "GuildId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(GuildMonsterFightingBroadcastResp_1018002), GuildMonsterFightingBroadcastResp_1018002.Parser, new string[7] { "GuildId", "Defenses", "GuildAttackPower", "GuildDefenses", "Times", "MonsterPowers", "IsFinished" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(GuildMonsterFightingRoleHelpReq_1018003), GuildMonsterFightingRoleHelpReq_1018003.Parser, new string[2] { "RoleId", "HelpIndex" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(GuildMonsterFightingRoleHelpResp_1018003), GuildMonsterFightingRoleHelpResp_1018003.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(GuildMonsterFinishedResp_1018004), GuildMonsterFinishedResp_1018004.Parser, new string[10] { "GuildId", "IsVictory", "LingCaiCount", "LingKuangCount", "LingZhiCount", "StoneCount", "JadeCount", "ChouJiangCishu", "DeathCount", "KillCount" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(GuildMonsterFightMessageBroadcastResp_1018005), GuildMonsterFightMessageBroadcastResp_1018005.Parser, new string[2] { "GuildId", "Message" }, null, null, null)
		}));
	}
}
