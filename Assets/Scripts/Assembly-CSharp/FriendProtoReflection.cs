using System;
using Google.Protobuf.Reflection;

public static class FriendProtoReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static FriendProtoReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChFGcmllbmRQcm90by5wcm90byKpAgoXRW50ZXJGcmllbmRSZXNwXzEwMTQw" + "MDESDAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSOwoGZnJpZW5k" + "GAMgAygLMisuRW50ZXJGcmllbmRSZXNwXzEwMTQwMDEuRnJpZW5kUmVzcF8x" + "MDE0MDAyEhUKDXN0cmFuZ2VyQ291bnQYBCABKAUamAEKEkZyaWVuZFJlc3Bf" + "MTAxNDAwMhIKCgJpZBgBIAEoCRIMCgRuYW1lGAIgASgJEhYKDmhlYWRQb3J0" + "cmFpdElkGAMgASgJEg0KBWxldmVsGAQgASgFEhAKCGludGltYWN5GAUgASgF" + "EhAKCGxpdmVUeXBlGAYgAygFEgsKA3NleBgHIAEoCBIQCghpc09uTGluZRgI" + "IAEoCCKMAgoaRW50ZXJBZGRGcmllbmRSZXNwXzEwMTQwMDMSDAoEY29kZRgB" + "IAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSQgoIc3RyYW5nZXIYAyADKAsyMC5F" + "bnRlckFkZEZyaWVuZFJlc3BfMTAxNDAwMy5TdHJhbmdlclJlc3BfMTAxNDAw" + "NBqIAQoUU3RyYW5nZXJSZXNwXzEwMTQwMDQSCgoCaWQYASABKAkSDAoEbmFt" + "ZRgCIAEoCRIWCg5oZWFkUG9ydHJhaXRJZBgDIAEoCRINCgVsZXZlbBgEIAEo" + "BRIQCghsaXZlVHlwZRgFIAMoBRILCgNzZXgYBiABKAgSEAoIaXNPbkxpbmUY" + "ByABKAgipQEKGlJlbW90ZVNlbmRHb29kc1JlcV8xMDE0MDA1EhYKDnRhcmdl" + "dEZyaWVuZElkGAEgASgJEjsKBWdvb2RzGAIgAygLMiwuUmVtb3RlU2VuZEdv" + "b2RzUmVxXzEwMTQwMDUuR29vZHNSZXFfMTAxNDAxMRoyChBHb29kc1JlcV8x" + "MDE0MDExEg8KB2dvb2RzSWQYASABKAkSDQoFY291bnQYAiABKAUiWQobUmVt" + "b3RlU2VuZEdvb2RzUmVzcF8xMDE0MDA1EgwKBGNvZGUYASABKAUSEQoJaXNT" + "dWNjZXNzGAIgASgIEhkKEWN1cnJlbnRTaGlueVN0b25lGAMgASgDIi4KFEFk" + "ZEZyaWVuZFJlcV8xMDE0MDA2EhYKDnRhcmdldEZyaWVuZElkGAEgASgJIjgK" + "FUFkZEZyaWVuZFJlc3BfMTAxNDAwNhIMCgRjb2RlGAEgASgFEhEKCWlzU3Vj" + "Y2VzcxgCIAEoCCIqChpBZGRGcmllbmRCeU5hbWVSZXFfMTAxNDAwNxIMCgRu" + "YW1lGAEgASgJIj4KG0FkZEZyaWVuZEJ5TmFtZVJlc3BfMTAxNDAwNxIMCgRj" + "b2RlGAEgASgFEhEKCWlzU3VjY2VzcxgCIAEoCCI2ChxBZGRGcmllbmRCeVJv" + "bGVJZFJlcV8xMDE0MDA4EhYKDnRhcmdldEZyaWVuZElkGAEgASgJIkAKHUFk" + "ZEZyaWVuZEJ5Um9sZUlkUmVzcF8xMDE0MDA4EgwKBGNvZGUYASABKAUSEQoJ" + "aXNTdWNjZXNzGAIgASgIIjEKF1JlZnVzZUZyaWVuZFJlcV8xMDE0MDA5EhYK" + "DnRhcmdldEZyaWVuZElkGAEgASgJIjsKGFJlZnVzZUZyaWVuZFJlc3BfMTAx" + "NDAwORIMCgRjb2RlGAEgASgFEhEKCWlzU3VjY2VzcxgCIAEoCCIxChdEZWxl" + "dGVGcmllbmRSZXFfMTAxNDAxMBIWCg50YXJnZXRGcmllbmRJZBgBIAEoCSI7" + "ChhEZWxldGVGcmllbmRSZXNwXzEwMTQwMTASDAoEY29kZRgBIAEoBRIRCglp" + "c1N1Y2Nlc3MYAiABKAgi5gQKFkVudGVyU2hpVHVSZXNwXzEwMTQwMTISDAoE" + "Y29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSOAoFc2hpRnUYAyABKAsy" + "KS5FbnRlclNoaVR1UmVzcF8xMDE0MDEyLlNoaUZ1UmVzcF8xMDE0MDEzEjcK" + "BXR1RGlzGAQgAygLMiguRW50ZXJTaGlUdVJlc3BfMTAxNDAxMi5UdURpUmVz" + "cF8xMDE0MDE0EhEKCWNodVNoaU51bRgFIAEoBRITCgtxaW5DaHVhbk51bRgG" + "IAEoBRIRCglqaU1pbmdOdW0YByABKAUSFAoMYmFpU2hpVGllTnVtGAggASgF" + "EhEKCWlzR2V0VG1jZxgJIAEoCBIQCgh0bWNnVHlwZRgKIAEoBRIPCgd0bWNn" + "TnVtGAsgASgFGosBChFTaGlGdVJlc3BfMTAxNDAxMxIMCgRuYW1lGAIgASgJ" + "EhYKDmhlYWRQb3J0cmFpdElkGAMgASgJEg0KBWxldmVsGAQgASgFEhAKCGlu" + "dGltYWN5GAUgASgFEgsKA3NleBgHIAEoCBIQCghpc09uTGluZRgIIAEoCBIQ" + "Cghpc1FpbmdBbhgJIAEoCBqiAQoQVHVEaVJlc3BfMTAxNDAxNBIKCgJpZBgB" + "IAEoCRIMCgRuYW1lGAIgASgJEhYKDmhlYWRQb3J0cmFpdElkGAMgASgJEg0K" + "BWxldmVsGAQgASgFEhAKCGludGltYWN5GAUgASgFEgsKA3NleBgHIAEoCBIQ" + "Cghpc09uTGluZRgIIAEoCBIcChRpc1RvbmdNYWlPckNodWFuR29uZxgJIAEo" + "CCJHChJRaW5nQW5SZXNwXzEwMTQwMTUSDAoEY29kZRgBIAEoBRIRCglpc1N1" + "Y2Nlc3MYAiABKAgSEAoIaW50aW1hY3kYAyABKAUiJAoSVG9uZ01haVJlcV8x" + "MDE0MDE2Eg4KBnJvbGVJZBgBIAEoCSJ5ChNUb25nTWFpUmVzcF8xMDE0MDE2" + "EgwKBGNvZGUYASABKAUSEQoJaXNTdWNjZXNzGAIgASgIEhAKCGludGltYWN5" + "GAMgASgFEhQKDGN1cnJlbnRWaWdvchgEIAEoBRIZChF0dURpQWN1cG9pbnRQ" + "b2ludBgFIAEoBSImChRDaHVhbkdvbmdSZXFfMTAxNDAxNxIOCgZyb2xlSWQY" + "ASABKAkidgoVQ2h1YW5Hb25nUmVzcF8xMDE0MDE3EgwKBGNvZGUYASABKAUS" + "EQoJaXNTdWNjZXNzGAIgASgIEhAKCGludGltYWN5GAMgASgFEhQKDGN1cnJl" + "bnRWaWdvchgEIAEoBRIUCgx0dURpU3VwZXJBaXIYBSABKAUi5AEKFUJhaVNo" + "aVRpZVJlc3BfMTAxNDAxOBIMCgRjb2RlGAEgASgFEhEKCWlzU3VjY2VzcxgC" + "IAEoCBJCCgtiYWlTaGlEaVppcxgDIAMoCzItLkJhaVNoaVRpZVJlc3BfMTAx" + "NDAxOC5CYWlTaGlEaVppUmVzcF8xMDE0MDE5GmYKFkJhaVNoaURpWmlSZXNw" + "XzEwMTQwMTkSCgoCaWQYASABKAkSDAoEbmFtZRgCIAEoCRIWCg5oZWFkUG9y" + "dHJhaXRJZBgDIAEoCRINCgVsZXZlbBgEIAEoBRILCgNzZXgYByABKAgiIwoR" + "U2hvdVR1UmVxXzEwMTQwMjASDgoGcm9sZUlkGAEgASgJIjUKElNob3VUdVJl" + "c3BfMTAxNDAyMBIMCgRjb2RlGAEgASgFEhEKCWlzU3VjY2VzcxgCIAEoCCIl" + "ChFCYWlTaGlSZXFfMTAxNDAyMRIQCghyb2xlTmFtZRgBIAEoCSI1ChJCYWlT" + "aGlSZXNwXzEwMTQwMjESDAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiAB" + "KAgiOgoXVHVvTGlTaGlNZW5SZXNwXzEwMTQwMjISDAoEY29kZRgBIAEoBRIR" + "Cglpc1N1Y2Nlc3MYAiABKAgiKAoWQmFpU2hpSHVMdWVSZXFfMTAxNDAyMxIO" + "CgZyb2xlSWQYASABKAkiOgoXQmFpU2hpSHVMdWVSZXNwXzEwMTQwMjMSDAoE" + "Y29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgiXQoXVG9uZ1poaVRNQ0dS" + "ZXNwXzEwMTQwMjQSDAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgS" + "EAoIdG1jZ1R5cGUYAyABKAUSDwoHdG1jZ051bRgEIAEoBSJMChdKaWVTaG91" + "VE1DR1Jlc3BfMTAxNDAyNRIMCgRjb2RlGAEgASgFEhEKCWlzU3VjY2VzcxgC" + "IAEoCBIQCghpbnRpbWFjeRgDIAEoBSIpChdaaHVDaHVTaGlNZW5SZXFfMTAx" + "NDAyNhIOCgZyb2xlSWQYASABKAkiOwoYWmh1Q2h1U2hpTWVuUmVzcF8xMDE0" + "MDI2EgwKBGNvZGUYASABKAUSEQoJaXNTdWNjZXNzGAIgASgIIiMKEUNodVNo" + "aVJlcV8xMDE0MDI3Eg4KBnJvbGVJZBgBIAEoCSJEChJDaHVTaGlSZXNwXzEw" + "MTQwMjcSDAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSDQoFcWlZ" + "dW4YAyABKAVCLgofY29tLmxvbmdtby5nYW1lc2VydmVyLm5ldC5wcm90b0IL" + "RnJpZW5kUHJvdG9iBnByb3RvMw=="), new FileDescriptor[0], new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[34]
		{
			new GeneratedClrTypeInfo(typeof(EnterFriendResp_1014001), EnterFriendResp_1014001.Parser, new string[4] { "Code", "IsSuccess", "Friend", "StrangerCount" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(EnterFriendResp_1014001.Types.FriendResp_1014002), EnterFriendResp_1014001.Types.FriendResp_1014002.Parser, new string[8] { "Id", "Name", "HeadPortraitId", "Level", "Intimacy", "LiveType", "Sex", "IsOnLine" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(EnterAddFriendResp_1014003), EnterAddFriendResp_1014003.Parser, new string[3] { "Code", "IsSuccess", "Stranger" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(EnterAddFriendResp_1014003.Types.StrangerResp_1014004), EnterAddFriendResp_1014003.Types.StrangerResp_1014004.Parser, new string[7] { "Id", "Name", "HeadPortraitId", "Level", "LiveType", "Sex", "IsOnLine" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(RemoteSendGoodsReq_1014005), RemoteSendGoodsReq_1014005.Parser, new string[2] { "TargetFriendId", "Goods" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(RemoteSendGoodsReq_1014005.Types.GoodsReq_1014011), RemoteSendGoodsReq_1014005.Types.GoodsReq_1014011.Parser, new string[2] { "GoodsId", "Count" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(RemoteSendGoodsResp_1014005), RemoteSendGoodsResp_1014005.Parser, new string[3] { "Code", "IsSuccess", "CurrentShinyStone" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(AddFriendReq_1014006), AddFriendReq_1014006.Parser, new string[1] { "TargetFriendId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(AddFriendResp_1014006), AddFriendResp_1014006.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(AddFriendByNameReq_1014007), AddFriendByNameReq_1014007.Parser, new string[1] { "Name" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(AddFriendByNameResp_1014007), AddFriendByNameResp_1014007.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(AddFriendByRoleIdReq_1014008), AddFriendByRoleIdReq_1014008.Parser, new string[1] { "TargetFriendId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(AddFriendByRoleIdResp_1014008), AddFriendByRoleIdResp_1014008.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(RefuseFriendReq_1014009), RefuseFriendReq_1014009.Parser, new string[1] { "TargetFriendId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(RefuseFriendResp_1014009), RefuseFriendResp_1014009.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(DeleteFriendReq_1014010), DeleteFriendReq_1014010.Parser, new string[1] { "TargetFriendId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(DeleteFriendResp_1014010), DeleteFriendResp_1014010.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(EnterShiTuResp_1014012), EnterShiTuResp_1014012.Parser, new string[11]
			{
				"Code", "IsSuccess", "ShiFu", "TuDis", "ChuShiNum", "QinChuanNum", "JiMingNum", "BaiShiTieNum", "IsGetTmcg", "TmcgType",
				"TmcgNum"
			}, null, null, new GeneratedClrTypeInfo[2]
			{
				new GeneratedClrTypeInfo(typeof(EnterShiTuResp_1014012.Types.ShiFuResp_1014013), EnterShiTuResp_1014012.Types.ShiFuResp_1014013.Parser, new string[7] { "Name", "HeadPortraitId", "Level", "Intimacy", "Sex", "IsOnLine", "IsQingAn" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(EnterShiTuResp_1014012.Types.TuDiResp_1014014), EnterShiTuResp_1014012.Types.TuDiResp_1014014.Parser, new string[8] { "Id", "Name", "HeadPortraitId", "Level", "Intimacy", "Sex", "IsOnLine", "IsTongMaiOrChuanGong" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(QingAnResp_1014015), QingAnResp_1014015.Parser, new string[3] { "Code", "IsSuccess", "Intimacy" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(TongMaiReq_1014016), TongMaiReq_1014016.Parser, new string[1] { "RoleId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(TongMaiResp_1014016), TongMaiResp_1014016.Parser, new string[5] { "Code", "IsSuccess", "Intimacy", "CurrentVigor", "TuDiAcupointPoint" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ChuanGongReq_1014017), ChuanGongReq_1014017.Parser, new string[1] { "RoleId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ChuanGongResp_1014017), ChuanGongResp_1014017.Parser, new string[5] { "Code", "IsSuccess", "Intimacy", "CurrentVigor", "TuDiSuperAir" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(BaiShiTieResp_1014018), BaiShiTieResp_1014018.Parser, new string[3] { "Code", "IsSuccess", "BaiShiDiZis" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(BaiShiTieResp_1014018.Types.BaiShiDiZiResp_1014019), BaiShiTieResp_1014018.Types.BaiShiDiZiResp_1014019.Parser, new string[5] { "Id", "Name", "HeadPortraitId", "Level", "Sex" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(ShouTuReq_1014020), ShouTuReq_1014020.Parser, new string[1] { "RoleId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ShouTuResp_1014020), ShouTuResp_1014020.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(BaiShiReq_1014021), BaiShiReq_1014021.Parser, new string[1] { "RoleName" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(BaiShiResp_1014021), BaiShiResp_1014021.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(TuoLiShiMenResp_1014022), TuoLiShiMenResp_1014022.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(BaiShiHuLueReq_1014023), BaiShiHuLueReq_1014023.Parser, new string[1] { "RoleId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(BaiShiHuLueResp_1014023), BaiShiHuLueResp_1014023.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(TongZhiTMCGResp_1014024), TongZhiTMCGResp_1014024.Parser, new string[4] { "Code", "IsSuccess", "TmcgType", "TmcgNum" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(JieShouTMCGResp_1014025), JieShouTMCGResp_1014025.Parser, new string[3] { "Code", "IsSuccess", "Intimacy" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ZhuChuShiMenReq_1014026), ZhuChuShiMenReq_1014026.Parser, new string[1] { "RoleId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ZhuChuShiMenResp_1014026), ZhuChuShiMenResp_1014026.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ChuShiReq_1014027), ChuShiReq_1014027.Parser, new string[1] { "RoleId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ChuShiResp_1014027), ChuShiResp_1014027.Parser, new string[3] { "Code", "IsSuccess", "QiYun" }, null, null, null)
		}));
	}
}
