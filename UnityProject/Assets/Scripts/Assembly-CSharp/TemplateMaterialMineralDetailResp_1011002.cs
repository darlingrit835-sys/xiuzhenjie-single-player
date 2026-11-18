using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class TemplateMaterialMineralDetailResp_1011002 : IMessage<TemplateMaterialMineralDetailResp_1011002>, IMessage, IEquatable<TemplateMaterialMineralDetailResp_1011002>, IDeepCloneable<TemplateMaterialMineralDetailResp_1011002>
{
	private static readonly MessageParser<TemplateMaterialMineralDetailResp_1011002> _parser = new MessageParser<TemplateMaterialMineralDetailResp_1011002>(() => new TemplateMaterialMineralDetailResp_1011002());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int MapNameFieldNumber = 3;

	private string mapName_ = "";

	public const int StageFieldNumber = 4;

	private int stage_;

	public const int FiveElementFieldNumber = 5;

	private int fiveElement_;

	public const int IntensityFieldNumber = 6;

	private double intensity_;

	public const int TenacityFieldNumber = 7;

	private double tenacity_;

	[DebuggerNonUserCode]
	public static MessageParser<TemplateMaterialMineralDetailResp_1011002> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => TemplateProtoReflection.Descriptor.MessageTypes[3];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int Code
	{
		get
		{
			return code_;
		}
		set
		{
			code_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsSuccess
	{
		get
		{
			return isSuccess_;
		}
		set
		{
			isSuccess_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string MapName
	{
		get
		{
			return mapName_;
		}
		set
		{
			mapName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int Stage
	{
		get
		{
			return stage_;
		}
		set
		{
			stage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int FiveElement
	{
		get
		{
			return fiveElement_;
		}
		set
		{
			fiveElement_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double Intensity
	{
		get
		{
			return intensity_;
		}
		set
		{
			intensity_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double Tenacity
	{
		get
		{
			return tenacity_;
		}
		set
		{
			tenacity_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public TemplateMaterialMineralDetailResp_1011002()
	{
	}

	[DebuggerNonUserCode]
	public TemplateMaterialMineralDetailResp_1011002(TemplateMaterialMineralDetailResp_1011002 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		mapName_ = other.mapName_;
		stage_ = other.stage_;
		fiveElement_ = other.fiveElement_;
		intensity_ = other.intensity_;
		tenacity_ = other.tenacity_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public TemplateMaterialMineralDetailResp_1011002 Clone()
	{
		return new TemplateMaterialMineralDetailResp_1011002(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as TemplateMaterialMineralDetailResp_1011002);
	}

	[DebuggerNonUserCode]
	public bool Equals(TemplateMaterialMineralDetailResp_1011002 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Code != other.Code)
		{
			return false;
		}
		if (IsSuccess != other.IsSuccess)
		{
			return false;
		}
		if (MapName != other.MapName)
		{
			return false;
		}
		if (Stage != other.Stage)
		{
			return false;
		}
		if (FiveElement != other.FiveElement)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Intensity, other.Intensity))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Tenacity, other.Tenacity))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Code != 0)
		{
			num ^= Code.GetHashCode();
		}
		if (IsSuccess)
		{
			num ^= IsSuccess.GetHashCode();
		}
		if (MapName.Length != 0)
		{
			num ^= MapName.GetHashCode();
		}
		if (Stage != 0)
		{
			num ^= Stage.GetHashCode();
		}
		if (FiveElement != 0)
		{
			num ^= FiveElement.GetHashCode();
		}
		if (Intensity != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Intensity);
		}
		if (Tenacity != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Tenacity);
		}
		if (_unknownFields != null)
		{
			num ^= _unknownFields.GetHashCode();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public override string ToString()
	{
		return JsonFormatter.ToDiagnosticString(this);
	}

	[DebuggerNonUserCode]
	public void WriteTo(CodedOutputStream output)
	{
		if (Code != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Code);
		}
		if (IsSuccess)
		{
			output.WriteRawTag(16);
			output.WriteBool(IsSuccess);
		}
		if (MapName.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(MapName);
		}
		if (Stage != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Stage);
		}
		if (FiveElement != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(FiveElement);
		}
		if (Intensity != 0.0)
		{
			output.WriteRawTag(49);
			output.WriteDouble(Intensity);
		}
		if (Tenacity != 0.0)
		{
			output.WriteRawTag(57);
			output.WriteDouble(Tenacity);
		}
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (Code != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Code);
		}
		if (IsSuccess)
		{
			num += 2;
		}
		if (MapName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MapName);
		}
		if (Stage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Stage);
		}
		if (FiveElement != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FiveElement);
		}
		if (Intensity != 0.0)
		{
			num += 9;
		}
		if (Tenacity != 0.0)
		{
			num += 9;
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(TemplateMaterialMineralDetailResp_1011002 other)
	{
		if (other != null)
		{
			if (other.Code != 0)
			{
				Code = other.Code;
			}
			if (other.IsSuccess)
			{
				IsSuccess = other.IsSuccess;
			}
			if (other.MapName.Length != 0)
			{
				MapName = other.MapName;
			}
			if (other.Stage != 0)
			{
				Stage = other.Stage;
			}
			if (other.FiveElement != 0)
			{
				FiveElement = other.FiveElement;
			}
			if (other.Intensity != 0.0)
			{
				Intensity = other.Intensity;
			}
			if (other.Tenacity != 0.0)
			{
				Tenacity = other.Tenacity;
			}
			_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
		}
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CodedInputStream input)
	{
		uint num;
		while ((num = input.ReadTag()) != 0)
		{
			switch (num)
			{
			default:
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
				break;
			case 8u:
				Code = input.ReadInt32();
				break;
			case 16u:
				IsSuccess = input.ReadBool();
				break;
			case 26u:
				MapName = input.ReadString();
				break;
			case 32u:
				Stage = input.ReadInt32();
				break;
			case 40u:
				FiveElement = input.ReadInt32();
				break;
			case 49u:
				Intensity = input.ReadDouble();
				break;
			case 57u:
				Tenacity = input.ReadDouble();
				break;
			}
		}
	}
}
