using System;
using Google.Protobuf.Reflection;

public static class RoleProtoReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static RoleProtoReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("Cg9Sb2xlUHJvdG8ucHJvdG8i1QEKFVJvbGVDcmVhdGVSZXFfMTAwMjAwMRIO" + "CgZ1c2VySWQYASABKAkSEAoIY2xpZW50SXAYAiABKAkSEAoIc2VydmVySWQY" + "AyABKAkSDQoFbWFwSWQYBCABKAkSCwoDc2V4GAUgASgIEhYKDmhlYWRQb3J0" + "cmFpdElkGAYgASgFEgwKBG5hbWUYByABKAkSDAoEZ29sZBgIIAEoARIMCgR3" + "b29kGAkgASgBEg0KBXdhdGVyGAogASgBEgwKBGZpcmUYCyABKAESDQoFZWFy" + "dGgYDCABKAEiSQoWUm9sZUNyZWF0ZVJlc3BfMTAwMjAwMRIMCgRjb2RlGAEg" + "ASgFEhEKCWlzU3VjY2VzcxgCIAEoCBIOCgZyb2xlSWQYAyABKAkiawoUUm9s" + "ZUxvZ2luUmVxXzEwMDIwMDISDwoHTG9naW5JcBgBIAEoCRIcChRnYW1lU2Vy" + "dmVySXBNYW5hZ2VJZBgCIAEoCRIOCgZyb2xlSWQYAyABKAkSFAoMZ2FtZVNl" + "cnZlcklkGAQgASgJItcLChVSb2xlTG9naW5SZXNwXzEwMDIwMDISDAoEY29k" + "ZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSCgoCaWQYAyABKAkSDQoFbWFw" + "SWQYBCABKAkSDwoHbWFwTmFtZRgFIAEoCRILCgNzZXgYBiABKAUSFgoOaGVh" + "ZFBvcnRyYWl0SWQYByABKAUSDAoEbmFtZRgIIAEoCRINCgVsZXZlbBgJIAEo" + "BRISCgpjdXJyZW50QWdlGAogASgFEg4KBm1heEFnZRgLIAEoBRISCgpzaGlu" + "eVN0b25lGAwgASgDEhEKCXNoaW55SmFkZRgNIAEoBRIRCglwdXJlQmxvb2QY" + "DiABKAUSFAoMZmxleGlibGVSb290GA8gASgJEhAKCHN1cGVyQWlyGBAgASgD" + "EhIKCnBlcmNlcHRpb24YESABKAMSEAoIcGh5c2lxdWUYEiABKAMSFwoPY3Vy" + "cmVudFN1cGVyQWlyGBMgASgDEhkKEWN1cnJlbnRQZXJjZXB0aW9uGBQgASgD" + "EhcKD2N1cnJlbnRQaHlzaXF1ZRgVIAEoAxIUCgxjdXJyZW50Vmlnb3IYFiAB" + "KAUSFQoNY3VycmVudENsZXZlchgXIAEoERIYChBjdXJyZW50UG9pc29uVmFs" + "GBggASgFEhgKEGN1cnJlbnRQdXJlQmxvb2QYGSABKAUSEQoJbGl2ZVJlbElk" + "GBogASgFEhEKCWxpdmUxVHlwZRgbIAEoBRIRCglsaXZlMlR5cGUYHCABKAUS" + "EgoKbGl2ZTFsZXZlbBgdIAEoBRISCgpsaXZlMmxldmVsGB4gASgFEhIKCmxp" + "dmUxdmFsdWUYHyABKAUSEgoKbGl2ZTJ2YWx1ZRggIAEoBRIRCgl5dWVLYURh" + "eXMYISABKAUSDQoFdGl0bGUYIiABKAUSDwoHaGFzV29vZBgjIAEoCBIPCgdo" + "YXNHb2xkGCQgASgIEhAKCGhhc1dhdGVyGCUgASgIEg8KB2hhc0ZpcmUYJiAB" + "KAgSEAoIaGFzRWFydGgYJyABKAgSFgoOYmF0dGxlR3VpZGVWYWwYKCABKAUS" + "GAoQcHJhY3RpY2VHdWlkZVZhbBgpIAEoBRITCgtpc05ld0xldHRlchgqIAEo" + "CBIYChBhY3Vwb2ludEd1aWRlVmFsGCsgASgFEhQKDHNob3BHdWlkZVZhbBgs" + "IAEoBRIQCghodW5MaVZhbBgtIAEoBRIUCgxzaWduR3VpZGVWYWwYLiABKAUS" + "FgoOZWxpeGlyR3VpZGVWYWwYLyABKAUSEwoLaXNHZXRTaWduSW4YMCABKAgS" + "EQoJc2lnbkluRGF5GDEgASgFEg8KB2d1aWxkSWQYMiABKAUSEQoJZ3VpbGRO" + "YW1lGDMgASgJEhcKD2d1aWxkU2ltcGxlTmFtZRg0IAEoCRISCgp0b2tlbklu" + "ZGV4GDUgASgFEhUKDWxhc3ROYW1lSW5kZXgYNiABKAUSDgoGdGFza0lkGDcg" + "ASgFEhIKCnRhc2tTdGF0dXMYOCABKAUSGAoQdGlhblNodUJlaVN0YXR1cxg5" + "IAEoBRINCgVwb3dlchg6IAEoBRIUCgxjdXJyZW50UG93ZXIYOyABKAUSDQoF" + "dmlnb3IYPCABKAUSDgoGY2xldmVyGD0gASgREhEKCWxpdmUzVHlwZRg+IAEo" + "BRISCgpsaXZlM2xldmVsGD8gASgFEhIKCmxpdmUzdmFsdWUYQCABKAUSFQoN" + "aXNUaWFuRGFvTGluZxhBIAEoCBIPCgd0aXRsZUlkGEIgAygFEg0KBXFpWXVu" + "GEMgASgFEhEKCWxpdmU0VHlwZRhEIAEoBRISCgpsaXZlNGxldmVsGEUgASgF" + "EhIKCmxpdmU0dmFsdWUYRiABKAUSEAoIbGVpQ2hvbmcYRyABKAUSEAoIdmlw" + "TGV2ZWwYSCABKAUSEgoKdGl0bGVGcmFtZRhJIAEoBRIRCgloZWFkRnJhbWUY" + "SiABKAUSEgoKaXNIYW9ZdW5GdRhLIAEoCCLrAQoXUm9sZU9mZmxpbmVSZXNw" + "XzEwMDIwMDMSDAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSEAoI" + "c3VwZXJBaXIYAyABKAMSEgoKcGVyY2VwdGlvbhgEIAEoAxIQCghwaHlzaXF1" + "ZRgFIAEoAxILCgNhZ2UYBiABKAESEwoLb2ZmbGluZVRpbWUYByABKAUSFAoM" + "b2ZmbGluZVZpZ29yGAggASgFEhUKDW9mZmxpbmVDbGV2ZXIYCSABKAUSEgoK" + "aXNGdWxsVGltZRgKIAEoCBIUCgxvZmZsaW5lUG93ZXIYCyABKAUiMQoZUm9s" + "ZUdldE9mZmxpbmVSZXFfMTAwMjAwNBIUCgxpc0dldE9mZmxpbmUYASABKAgi" + "vQIKGlJvbGVHZXRPZmZsaW5lUmVzcF8xMDAyMDA0EgwKBGNvZGUYASABKAUS" + "EQoJaXNTdWNjZXNzGAIgASgIEhAKCHN1cGVyQWlyGAMgASgDEhcKD2N1cnJl" + "bnRTdXBlckFpchgEIAEoAxISCgpwZXJjZXB0aW9uGAUgASgFEhkKEWN1cnJl" + "bnRQZXJjZXB0aW9uGAYgASgFEhAKCHBoeXNpcXVlGAcgASgFEhcKD2N1cnJl" + "bnRQaHlzaXF1ZRgIIAEoBRILCgNhZ2UYCSABKAUSDQoFdmlnb3IYCiABKAUS" + "DgoGY2xldmVyGAsgASgFEhEKCXBvaXNvblZhbBgMIAEoBRIRCglwdXJlQmxv" + "b2QYDSABKAUSGAoQY3VycmVudFB1cmVCbG9vZBgOIAEoBRINCgVwb3dlchgP" + "IAEoBSIkChRDaGVja05hbWVSZXFfMTAwMjAwNRIMCgRuYW1lGAEgASgJIjgK" + "FUNoZWNrTmFtZVJlc3BfMTAwMjAwNRIMCgRjb2RlGAEgASgFEhEKCWlzU3Vj" + "Y2VzcxgCIAEoCCJJChdJbmZvcm1UaXRsZVJlc3BfMTAwMjAwNhIMCgRjb2Rl" + "GAEgASgFEhEKCWlzU3VjY2VzcxgCIAEoCBINCgV0aXRsZRgDIAEoBSIkChNU" + "aXRsZVVzZVJlcV8xMDAyMDA3Eg0KBXRpdGxlGAEgASgFIjcKFFRpdGxlVXNl" + "UmVzcF8xMDAyMDA3EgwKBGNvZGUYASABKAUSEQoJaXNTdWNjZXNzGAIgASgI" + "IjoKF1JvbGVTdWljaWRlUmVzcF8xMDAyMDA4EgwKBGNvZGUYASABKAUSEQoJ" + "aXNTdWNjZXNzGAIgASgIIqIGCiBFbnRlclRodW5kZXJDYWxhbWl0eVJlc3Bf" + "MTAwMjAwORIMCgRjb2RlGAEgASgFEhEKCWlzU3VjY2VzcxgCIAEoCBIeChZj" + "dXJyZW50UGh5c2lxdWVEZWZlbnNlGAMgASgDEh8KF2N1cnJlbnRQdXJlQmxv" + "b2REZWZlbnNlGAQgASgFEhcKD2N1cnJlbnRTdXBlckFpchgFIAEoAxIZChFj" + "dXJyZW50UGVyY2VwdGlvbhgGIAEoBRIXCg9kYW1hZ2VSZWR1Y3Rpb24YByAB" + "KAESFQoNZG9kZ2VDb29sRG93bhgIIAEoARJJCgZza2lsbHMYCSADKAsyOS5F" + "bnRlclRodW5kZXJDYWxhbWl0eVJlc3BfMTAwMjAwOS5CYXR0bGVTa2lsbFJl" + "c3BfMTAwMjAxMBJSCg10aHVuZGVyRGFtYWdlGAogAygLMjsuRW50ZXJUaHVu" + "ZGVyQ2FsYW1pdHlSZXNwXzEwMDIwMDkuVGh1bmRlckRhbWFnZVJlc3BfMTAw" + "MjAxMRrrAgoXQmF0dGxlU2tpbGxSZXNwXzEwMDIwMTASDAoEaWNvbhgBIAEo" + "BRIMCgR0eXBlGAIgASgFEhEKCXNraWxsVHlwZRgMIAEoBRIMCgRuYW1lGAMg" + "ASgJEhIKCnRlbXBsYXRlSWQYBCABKAkSEAoIY2FzdFRpbWUYBSABKAUSFAoM" + "Y29udGludWVUaW1lGAYgASgFEhAKCGNvb2xEb3duGAcgASgFEhMKC2ZpdmVF" + "bGVtZW50GAggASgFEhYKDmNvbnZlcnNpb25SYXRlGAkgASgBEhIKCmJhc2VE" + "YW1hZ2UYCiABKAESFgoOdXNlZFBlcmNlcHRpb24YCyABKAUSDQoFbGV2ZWwY" + "DSABKAUSEgoKc2tpbGxJbmRleBgOIAEoBRIUCgx1c2VkU3VwZXJBaXIYDyAB" + "KAMSEwoLY2FuVXNlQ291bnQYECABKAUSDAoEcmFuaxgRIAEoBRIQCghVbml0" + "VXNlZBgSIAEoARorChlUaHVuZGVyRGFtYWdlUmVzcF8xMDAyMDExEg4KBmRh" + "bWFnZRgBIAEoAyIqChpHdWlkZUluZm9VcGRhdGVSZXFfMTAwMjAxMxIMCgR0" + "eXBlGAEgASgFIj4KG0d1aWRlSW5mb1VwZGF0ZVJlc3BfMTAwMjAxMxIMCgRj" + "b2RlGAEgASgFEhEKCWlzU3VjY2VzcxgCIAEoCCIoChhHdWlkZUluZm9Ta2lw" + "UmVxXzEwMDIwMTQSDAoEdHlwZRgBIAEoBSI8ChlHdWlkZUluZm9Ta2lwUmVz" + "cF8xMDAyMDE0EgwKBGNvZGUYASABKAUSEQoJaXNTdWNjZXNzGAIgASgIIm8K" + "KlNlYXJjaElzU2V0dGluZ0JhdHRsZUVxdWlwbWVudFJlc3BfMTAwMjAxNRIM" + "CgRjb2RlGAEgASgFEhEKCWlzU3VjY2VzcxgCIAEoCBIgChhpc1NldHRpbmdF" + "cXVpcG1lbnRCYXR0bGUYAyABKAgijAEKIVNlYXJjaFNoaW55U3RvbmVSYXRp" + "b1Jlc3BfMTAwMjAxNhIMCgRjb2RlGAEgASgFEhEKCWlzU3VjY2VzcxgCIAEo" + "CBIVCg1leGNoYW5nZVJhdGlvGAMgASgDEh0KFWV4Y2hhbmdlU2hpbnlTdG9u" + "ZVN1bRgEIAEoAxIQCgh1cGRhdGVBdBgFIAEoAyI7CiZTaGlueUphZGVFeGNo" + "YW5nZVNoaW55U3RvbmVSZXFfMTAwMjAxNxIRCglzaGlueUphZGUYASABKAMi" + "cQonU2hpbnlKYWRlRXhjaGFuZ2VTaGlueVN0b25lUmVzcF8xMDAyMDE3EgwK" + "BGNvZGUYASABKAUSEQoJaXNTdWNjZXNzGAIgASgIEhIKCnNoaW55U3RvbmUY" + "AyABKAMSEQoJc2hpbnlKYWRlGAQgASgFIuYCCh1TZWFyY2hTaWduSW5Bd2Fy" + "ZFJlc3BfMTAwMjAxOBIMCgRjb2RlGAEgASgFEhEKCWlzU3VjY2VzcxgCIAEo" + "CBJLCgtzaWduSW5Bd2FyZBgDIAMoCzI2LlNlYXJjaFNpZ25JbkF3YXJkUmVz" + "cF8xMDAyMDE4LlNpZ25JbkF3YXJkUmVzcF8xMDAyMDE5GtYBChdTaWduSW5B" + "d2FyZFJlc3BfMTAwMjAxORIRCglzaWduSW5EYXkYASABKAUSVwoFYXdhcmQY" + "AiADKAsySC5TZWFyY2hTaWduSW5Bd2FyZFJlc3BfMTAwMjAxOC5TaWduSW5B" + "d2FyZFJlc3BfMTAwMjAxOS5Bd2FyZFJlc3BfMTAwMjAyMBpPChFBd2FyZFJl" + "c3BfMTAwMjAyMBIRCglnb29kc05hbWUYASABKAkSCwoDbnVtGAIgASgFEgwK" + "BHR5cGUYAyABKAUSDAoEcmFuaxgEIAEoBSI9ChpHZXRTaWduSW5Bd2FyZFJl" + "c3BfMTAwMjAyMRIMCgRjb2RlGAEgASgFEhEKCWlzU3VjY2VzcxgCIAEoCCLQ" + "AgoYTGlCYW9CdXlJbmZvUmVzcF8xMDAyMDIyEgwKBGNvZGUYASABKAUSEQoJ" + "aXNTdWNjZXNzGAIgASgIEhYKDmlzR2V0U2hvdUNob25nGAMgASgFEhQKDGlz" + "QnV5WGl1WmhlbhgEIAEoBRISCgppc0J1eU1laVJpGAUgASgFEhgKEGlzQnV5" + "TWVpUmlKaW5KaWUYBiABKAUSGQoRaXNCdXlNZWlSaUNoYW9aaGkYByABKAUS" + "GAoQaXNCdXlNZWlSaUhhb0h1YRgIIAEoBRIbChNpc0J1eVNob3VDaG9uZ1Rl" + "SHVpGAkgASgFEhkKEWlzQnV5SmluZ0ppZVRlSHVpGAogASgFEhEKCWlzQnV5" + "TmV3MBgLIAEoBRIRCglpc0J1eU5ldzEYDCABKAUSEQoJaXNCdXlOZXcyGA0g" + "ASgFEhEKCWlzQnV5TmV3MxgOIAEoBSJACh1TaG91Q2hvbmdMaUJhb0dldFJl" + "c3BfMTAwMjAyMxIMCgRjb2RlGAEgASgFEhEKCWlzU3VjY2VzcxgCIAEoCCIm" + "ChZSZWFsbVJhbmtlZFJlcV8xMDAyMDI0EgwKBHR5cGUYASABKAUijQIKF1Jl" + "YWxtUmFua2VkUmVzcF8xMDAyMDI0EgwKBGNvZGUYASABKAUSEQoJaXNTdWNj" + "ZXNzGAIgASgIEhAKCG15UmFua2VkGAMgASgFEg4KBm15SGVhdBgEIAEoBRJG" + "CgZvYmplY3QYBSADKAsyNi5SZWFsbVJhbmtlZFJlc3BfMTAwMjAyNC5SZWFs" + "bVJhbmtlZE9iamVjdFJlc3BfMTAwMjAyNRIPCgdteVN0YWdlGAYgASgFGlYK" + "HVJlYWxtUmFua2VkT2JqZWN0UmVzcF8xMDAyMDI1EgoKAmlkGAEgASgJEgwK" + "BG5hbWUYAiABKAkSDQoFbGV2ZWwYAyABKAUSDAoEaGVhdBgEIAEoBSIiChBN" + "b0JhaVJlcV8xMDAyMDI2Eg4KBnJvbGVJZBgBIAEoCSJHChFNb0JhaVJlc3Bf" + "MTAwMjAyNhIMCgRjb2RlGAEgASgFEhEKCWlzU3VjY2VzcxgCIAEoCBIRCglz" + "aGlueUphZGUYAyABKAMiXAoURW50ZXJDU0xSZXNwXzEwMDIwMjcSDAoEY29k" + "ZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSEAoIaXNCdXlDU0wYAyABKAgS" + "EQoJZ2V0TGV2ZWxzGAQgAygFIicKFkdldENTTEdvb2RzUmVxXzEwMDIwMjgS" + "DQoFbGV2ZWwYASABKAUiTQoXR2V0Q1NMR29vZHNSZXNwXzEwMDIwMjgSDAoE" + "Y29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgSEQoJc2hpbnlKYWRlGAMg" + "ASgDIhUKE3ZpcExldmVsUmVxXzEwMDIwMjkivQIKFFZpcExldmVsUmVzcF8x" + "MDAyMDI5EgwKBGNvZGUYASABKAUSEQoJaXNTdWNjZXNzGAIgASgIEg0KBWxl" + "dmVsGAMgASgFEg4KBnBheUFsbBgEIAEoBRIRCgl2MUlzR2V0ZWQYBSABKAgS" + "EQoJdjJJc0dldGVkGAYgASgIEhEKCXYzSXNHZXRlZBgHIAEoCBIRCgl2NElz" + "R2V0ZWQYCCABKAgSEQoJdjVJc0dldGVkGAkgASgIEhEKCXY2SXNHZXRlZBgK" + "IAEoCBIRCgl2N0lzR2V0ZWQYCyABKAgSEQoJdjhJc0dldGVkGAwgASgIEhEK" + "CXY5SXNHZXRlZBgNIAEoCBISCgp2MTBJc0dldGVkGA4gASgIEhIKCnYxMUlz" + "R2V0ZWQYDyABKAgSEgoKdjEySXNHZXRlZBgQIAEoCCIuChhUaXRsZUZyYW1l" + "VXNlUmVxXzEwMDIwMzASEgoKdGl0bGVGcmFtZRgBIAEoBSI8ChlUaXRsZUZy" + "YW1lVXNlUmVzcF8xMDAyMDMwEgwKBGNvZGUYASABKAUSEQoJaXNTdWNjZXNz" + "GAIgASgIIiwKF0hlYWRGcmFtZVVzZVJlcV8xMDAyMDMxEhEKCWhlYWRGcmFt" + "ZRgBIAEoBSI7ChhIZWFkRnJhbWVVc2VSZXNwXzEwMDIwMzESDAoEY29kZRgB" + "IAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgizwIKHEdldFNoaUtvbmdNaUppbmdS" + "ZXNwXzEwMDIwMzISDAoEY29kZRgBIAEoBRIRCglpc1N1Y2Nlc3MYAiABKAgS" + "EAoIc3VwZXJBaXIYAyABKAMSFwoPY3VycmVudFN1cGVyQWlyGAQgASgDEhIK" + "CnBlcmNlcHRpb24YBSABKAUSGQoRY3VycmVudFBlcmNlcHRpb24YBiABKAUS" + "EAoIcGh5c2lxdWUYByABKAUSFwoPY3VycmVudFBoeXNpcXVlGAggASgFEgsK" + "A2FnZRgJIAEoBRIRCglwdXJlQmxvb2QYCiABKAUSGAoQY3VycmVudFB1cmVC" + "bG9vZBgLIAEoBRIYChByZWZpbmluZ1N1cGVyQWlyGAwgASgDEhoKEnJlZmlu" + "aW5nUGVyY2VwdGlvbhgNIAEoBRIZChFyZWZpbmluZ3RQaHlzaXF1ZRgOIAEo" + "BUIsCh9jb20ubG9uZ21vLmdhbWVzZXJ2ZXIubmV0LnByb3RvQglSb2xlUHJv" + "dG9iBnByb3RvMw=="), new FileDescriptor[0], new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[40]
		{
			new GeneratedClrTypeInfo(typeof(RoleCreateReq_1002001), RoleCreateReq_1002001.Parser, new string[12]
			{
				"UserId", "ClientIp", "ServerId", "MapId", "Sex", "HeadPortraitId", "Name", "Gold", "Wood", "Water",
				"Fire", "Earth"
			}, null, null, null),
			new GeneratedClrTypeInfo(typeof(RoleCreateResp_1002001), RoleCreateResp_1002001.Parser, new string[3] { "Code", "IsSuccess", "RoleId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(RoleLoginReq_1002002), RoleLoginReq_1002002.Parser, new string[4] { "LoginIp", "GameServerIpManageId", "RoleId", "GameServerId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(RoleLoginResp_1002002), RoleLoginResp_1002002.Parser, new string[75]
			{
				"Code", "IsSuccess", "Id", "MapId", "MapName", "Sex", "HeadPortraitId", "Name", "Level", "CurrentAge",
				"MaxAge", "ShinyStone", "ShinyJade", "PureBlood", "FlexibleRoot", "SuperAir", "Perception", "Physique", "CurrentSuperAir", "CurrentPerception",
				"CurrentPhysique", "CurrentVigor", "CurrentClever", "CurrentPoisonVal", "CurrentPureBlood", "LiveRelId", "Live1Type", "Live2Type", "Live1Level", "Live2Level",
				"Live1Value", "Live2Value", "YueKaDays", "Title", "HasWood", "HasGold", "HasWater", "HasFire", "HasEarth", "BattleGuideVal",
				"PracticeGuideVal", "IsNewLetter", "AcupointGuideVal", "ShopGuideVal", "HunLiVal", "SignGuideVal", "ElixirGuideVal", "IsGetSignIn", "SignInDay", "GuildId",
				"GuildName", "GuildSimpleName", "TokenIndex", "LastNameIndex", "TaskId", "TaskStatus", "TianShuBeiStatus", "Power", "CurrentPower", "Vigor",
				"Clever", "Live3Type", "Live3Level", "Live3Value", "IsTianDaoLing", "TitleId", "QiYun", "Live4Type", "Live4Level", "Live4Value",
				"LeiChong", "VipLevel", "TitleFrame", "HeadFrame", "IsHaoYunFu"
			}, null, null, null),
			new GeneratedClrTypeInfo(typeof(RoleOfflineResp_1002003), RoleOfflineResp_1002003.Parser, new string[11]
			{
				"Code", "IsSuccess", "SuperAir", "Perception", "Physique", "Age", "OfflineTime", "OfflineVigor", "OfflineClever", "IsFullTime",
				"OfflinePower"
			}, null, null, null),
			new GeneratedClrTypeInfo(typeof(RoleGetOfflineReq_1002004), RoleGetOfflineReq_1002004.Parser, new string[1] { "IsGetOffline" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(RoleGetOfflineResp_1002004), RoleGetOfflineResp_1002004.Parser, new string[15]
			{
				"Code", "IsSuccess", "SuperAir", "CurrentSuperAir", "Perception", "CurrentPerception", "Physique", "CurrentPhysique", "Age", "Vigor",
				"Clever", "PoisonVal", "PureBlood", "CurrentPureBlood", "Power"
			}, null, null, null),
			new GeneratedClrTypeInfo(typeof(CheckNameReq_1002005), CheckNameReq_1002005.Parser, new string[1] { "Name" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(CheckNameResp_1002005), CheckNameResp_1002005.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(InformTitleResp_1002006), InformTitleResp_1002006.Parser, new string[3] { "Code", "IsSuccess", "Title" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(TitleUseReq_1002007), TitleUseReq_1002007.Parser, new string[1] { "Title" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(TitleUseResp_1002007), TitleUseResp_1002007.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(RoleSuicideResp_1002008), RoleSuicideResp_1002008.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(EnterThunderCalamityResp_1002009), EnterThunderCalamityResp_1002009.Parser, new string[10] { "Code", "IsSuccess", "CurrentPhysiqueDefense", "CurrentPureBloodDefense", "CurrentSuperAir", "CurrentPerception", "DamageReduction", "DodgeCoolDown", "Skills", "ThunderDamage" }, null, null, new GeneratedClrTypeInfo[2]
			{
				new GeneratedClrTypeInfo(typeof(EnterThunderCalamityResp_1002009.Types.BattleSkillResp_1002010), EnterThunderCalamityResp_1002009.Types.BattleSkillResp_1002010.Parser, new string[18]
				{
					"Icon", "Type", "SkillType", "Name", "TemplateId", "CastTime", "ContinueTime", "CoolDown", "FiveElement", "ConversionRate",
					"BaseDamage", "UsedPerception", "Level", "SkillIndex", "UsedSuperAir", "CanUseCount", "Rank", "UnitUsed"
				}, null, null, null),
				new GeneratedClrTypeInfo(typeof(EnterThunderCalamityResp_1002009.Types.ThunderDamageResp_1002011), EnterThunderCalamityResp_1002009.Types.ThunderDamageResp_1002011.Parser, new string[1] { "Damage" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(GuideInfoUpdateReq_1002013), GuideInfoUpdateReq_1002013.Parser, new string[1] { "Type" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(GuideInfoUpdateResp_1002013), GuideInfoUpdateResp_1002013.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(GuideInfoSkipReq_1002014), GuideInfoSkipReq_1002014.Parser, new string[1] { "Type" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(GuideInfoSkipResp_1002014), GuideInfoSkipResp_1002014.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(SearchIsSettingBattleEquipmentResp_1002015), SearchIsSettingBattleEquipmentResp_1002015.Parser, new string[3] { "Code", "IsSuccess", "IsSettingEquipmentBattle" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(SearchShinyStoneRatioResp_1002016), SearchShinyStoneRatioResp_1002016.Parser, new string[5] { "Code", "IsSuccess", "ExchangeRatio", "ExchangeShinyStoneSum", "UpdateAt" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ShinyJadeExchangeShinyStoneReq_1002017), ShinyJadeExchangeShinyStoneReq_1002017.Parser, new string[1] { "ShinyJade" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ShinyJadeExchangeShinyStoneResp_1002017), ShinyJadeExchangeShinyStoneResp_1002017.Parser, new string[4] { "Code", "IsSuccess", "ShinyStone", "ShinyJade" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(SearchSignInAwardResp_1002018), SearchSignInAwardResp_1002018.Parser, new string[3] { "Code", "IsSuccess", "SignInAward" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(SearchSignInAwardResp_1002018.Types.SignInAwardResp_1002019), SearchSignInAwardResp_1002018.Types.SignInAwardResp_1002019.Parser, new string[2] { "SignInDay", "Award" }, null, null, new GeneratedClrTypeInfo[1]
				{
					new GeneratedClrTypeInfo(typeof(SearchSignInAwardResp_1002018.Types.SignInAwardResp_1002019.Types.AwardResp_1002020), SearchSignInAwardResp_1002018.Types.SignInAwardResp_1002019.Types.AwardResp_1002020.Parser, new string[4] { "GoodsName", "Num", "Type", "Rank" }, null, null, null)
				})
			}),
			new GeneratedClrTypeInfo(typeof(GetSignInAwardResp_1002021), GetSignInAwardResp_1002021.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(LiBaoBuyInfoResp_1002022), LiBaoBuyInfoResp_1002022.Parser, new string[14]
			{
				"Code", "IsSuccess", "IsGetShouChong", "IsBuyXiuZhen", "IsBuyMeiRi", "IsBuyMeiRiJinJie", "IsBuyMeiRiChaoZhi", "IsBuyMeiRiHaoHua", "IsBuyShouChongTeHui", "IsBuyJingJieTeHui",
				"IsBuyNew0", "IsBuyNew1", "IsBuyNew2", "IsBuyNew3"
			}, null, null, null),
			new GeneratedClrTypeInfo(typeof(ShouChongLiBaoGetResp_1002023), ShouChongLiBaoGetResp_1002023.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(RealmRankedReq_1002024), RealmRankedReq_1002024.Parser, new string[1] { "Type" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(RealmRankedResp_1002024), RealmRankedResp_1002024.Parser, new string[6] { "Code", "IsSuccess", "MyRanked", "MyHeat", "Object", "MyStage" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(RealmRankedResp_1002024.Types.RealmRankedObjectResp_1002025), RealmRankedResp_1002024.Types.RealmRankedObjectResp_1002025.Parser, new string[4] { "Id", "Name", "Level", "Heat" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(MoBaiReq_1002026), MoBaiReq_1002026.Parser, new string[1] { "RoleId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(MoBaiResp_1002026), MoBaiResp_1002026.Parser, new string[3] { "Code", "IsSuccess", "ShinyJade" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(EnterCSLResp_1002027), EnterCSLResp_1002027.Parser, new string[4] { "Code", "IsSuccess", "IsBuyCSL", "GetLevels" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(GetCSLGoodsReq_1002028), GetCSLGoodsReq_1002028.Parser, new string[1] { "Level" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(GetCSLGoodsResp_1002028), GetCSLGoodsResp_1002028.Parser, new string[3] { "Code", "IsSuccess", "ShinyJade" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(vipLevelReq_1002029), vipLevelReq_1002029.Parser, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(VipLevelResp_1002029), VipLevelResp_1002029.Parser, new string[16]
			{
				"Code", "IsSuccess", "Level", "PayAll", "V1IsGeted", "V2IsGeted", "V3IsGeted", "V4IsGeted", "V5IsGeted", "V6IsGeted",
				"V7IsGeted", "V8IsGeted", "V9IsGeted", "V10IsGeted", "V11IsGeted", "V12IsGeted"
			}, null, null, null),
			new GeneratedClrTypeInfo(typeof(TitleFrameUseReq_1002030), TitleFrameUseReq_1002030.Parser, new string[1] { "TitleFrame" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(TitleFrameUseResp_1002030), TitleFrameUseResp_1002030.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(HeadFrameUseReq_1002031), HeadFrameUseReq_1002031.Parser, new string[1] { "HeadFrame" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(HeadFrameUseResp_1002031), HeadFrameUseResp_1002031.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(GetShiKongMiJingResp_1002032), GetShiKongMiJingResp_1002032.Parser, new string[14]
			{
				"Code", "IsSuccess", "SuperAir", "CurrentSuperAir", "Perception", "CurrentPerception", "Physique", "CurrentPhysique", "Age", "PureBlood",
				"CurrentPureBlood", "RefiningSuperAir", "RefiningPerception", "RefiningtPhysique"
			}, null, null, null)
		}));
	}
}
