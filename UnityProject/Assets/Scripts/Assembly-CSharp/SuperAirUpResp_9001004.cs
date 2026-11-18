using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class SuperAirUpResp_9001004 : IMessage<SuperAirUpResp_9001004>, IMessage, IEquatable<SuperAirUpResp_9001004>, IDeepCloneable<SuperAirUpResp_9001004>
{
	private static readonly MessageParser<SuperAirUpResp_9001004> _parser = new MessageParser<SuperAirUpResp_9001004>(() => new SuperAirUpResp_9001004());

	private UnknownFieldSet _unknownFields;

	public const int CurrentSuperAirFieldNumber = 1;

	private long currentSuperAir_;

	public const int SuperAirFieldNumber = 2;

	private long superAir_;

	public const int IsBreakFieldNumber = 3;

	private bool isBreak_;

	public const int SuperAirRefiningValueFieldNumber = 4;

	private long superAirRefiningValue_;

	public const int CurrentRunValueFieldNumber = 5;

	private double currentRunValue_;

	[DebuggerNonUserCode]
	public static MessageParser<SuperAirUpResp_9001004> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ServerInitiativeMsgProtoReflection.Descriptor.MessageTypes[4];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public long CurrentSuperAir
	{
		get
		{
			return currentSuperAir_;
		}
		set
		{
			currentSuperAir_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long SuperAir
	{
		get
		{
			return superAir_;
		}
		set
		{
			superAir_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsBreak
	{
		get
		{
			return isBreak_;
		}
		set
		{
			isBreak_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long SuperAirRefiningValue
	{
		get
		{
			return superAirRefiningValue_;
		}
		set
		{
			superAirRefiningValue_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double CurrentRunValue
	{
		get
		{
			return currentRunValue_;
		}
		set
		{
			currentRunValue_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public SuperAirUpResp_9001004()
	{
	}

	[DebuggerNonUserCode]
	public SuperAirUpResp_9001004(SuperAirUpResp_9001004 other)
		: this()
	{
		currentSuperAir_ = other.currentSuperAir_;
		superAir_ = other.superAir_;
		isBreak_ = other.isBreak_;
		superAirRefiningValue_ = other.superAirRefiningValue_;
		currentRunValue_ = other.currentRunValue_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SuperAirUpResp_9001004 Clone()
	{
		return new SuperAirUpResp_9001004(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SuperAirUpResp_9001004);
	}

	[DebuggerNonUserCode]
	public bool Equals(SuperAirUpResp_9001004 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (CurrentSuperAir != other.CurrentSuperAir)
		{
			return false;
		}
		if (SuperAir != other.SuperAir)
		{
			return false;
		}
		if (IsBreak != other.IsBreak)
		{
			return false;
		}
		if (SuperAirRefiningValue != other.SuperAirRefiningValue)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(CurrentRunValue, other.CurrentRunValue))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (CurrentSuperAir != 0L)
		{
			num ^= CurrentSuperAir.GetHashCode();
		}
		if (SuperAir != 0L)
		{
			num ^= SuperAir.GetHashCode();
		}
		if (IsBreak)
		{
			num ^= IsBreak.GetHashCode();
		}
		if (SuperAirRefiningValue != 0L)
		{
			num ^= SuperAirRefiningValue.GetHashCode();
		}
		if (CurrentRunValue != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(CurrentRunValue);
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
		if (CurrentSuperAir != 0L)
		{
			output.WriteRawTag(8);
			output.WriteInt64(CurrentSuperAir);
		}
		if (SuperAir != 0L)
		{
			output.WriteRawTag(16);
			output.WriteInt64(SuperAir);
		}
		if (IsBreak)
		{
			output.WriteRawTag(24);
			output.WriteBool(IsBreak);
		}
		if (SuperAirRefiningValue != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(SuperAirRefiningValue);
		}
		if (CurrentRunValue != 0.0)
		{
			output.WriteRawTag(41);
			output.WriteDouble(CurrentRunValue);
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
		if (CurrentSuperAir != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CurrentSuperAir);
		}
		if (SuperAir != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(SuperAir);
		}
		if (IsBreak)
		{
			num += 2;
		}
		if (SuperAirRefiningValue != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(SuperAirRefiningValue);
		}
		if (CurrentRunValue != 0.0)
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
	public void MergeFrom(SuperAirUpResp_9001004 other)
	{
		if (other != null)
		{
			if (other.CurrentSuperAir != 0L)
			{
				CurrentSuperAir = other.CurrentSuperAir;
			}
			if (other.SuperAir != 0L)
			{
				SuperAir = other.SuperAir;
			}
			if (other.IsBreak)
			{
				IsBreak = other.IsBreak;
			}
			if (other.SuperAirRefiningValue != 0L)
			{
				SuperAirRefiningValue = other.SuperAirRefiningValue;
			}
			if (other.CurrentRunValue != 0.0)
			{
				CurrentRunValue = other.CurrentRunValue;
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
				CurrentSuperAir = input.ReadInt64();
				break;
			case 16u:
				SuperAir = input.ReadInt64();
				break;
			case 24u:
				IsBreak = input.ReadBool();
				break;
			case 32u:
				SuperAirRefiningValue = input.ReadInt64();
				break;
			case 41u:
				CurrentRunValue = input.ReadDouble();
				break;
			}
		}
	}
}
