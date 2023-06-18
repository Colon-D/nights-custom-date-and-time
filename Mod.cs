using Reloaded.Mod.Interfaces;
using nights.test.sandbox.Template;
using nights.test.sandbox.Configuration;
using Reloaded.Hooks.Definitions;
using IReloadedHooks = Reloaded.Hooks.ReloadedII.Interfaces.IReloadedHooks;
using Reloaded.Hooks.Definitions.X86;
using System.Runtime.InteropServices;
using static Reloaded.Hooks.Definitions.X86.FunctionAttribute;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace nights.test.sandbox;

/// <summary>
/// Your mod logic goes here.
/// </summary>
public class Mod : ModBase // <= Do not Remove.
{
	/// <summary>
	/// Provides access to the mod loader API.
	/// </summary>
	private readonly IModLoader _modLoader;

	/// <summary>
	/// Provides access to the Reloaded.Hooks API.
	/// </summary>
	/// <remarks>This is null if you remove dependency on Reloaded.SharedLib.Hooks in your mod.</remarks>
	private readonly IReloadedHooks _hooks;

	/// <summary>
	/// Provides access to the Reloaded logger.
	/// </summary>
	private readonly ILogger _logger;

	/// <summary>
	/// Entry point into the mod, instance that created this class.
	/// </summary>
	private readonly IMod _owner;

	/// <summary>
	/// Provides access to this mod's configuration.
	/// </summary>
	private Config _configuration;

	/// <summary>
	/// The configuration of the currently executing mod.
	/// </summary>
	private readonly IModConfig _modConfig;


	public Mod(ModContext context) {
		_modLoader = context.ModLoader;
		_hooks = context.Hooks;
		_logger = context.Logger;
		_owner = context.Owner;
		_configuration = context.Configuration;
		_modConfig = context.ModConfig;


		unsafe {
			_hookSetToDateAndTime = _hooks.CreateHook<setToDateAndTime>(SetToDateAndTime, 0x5554C0).Activate();
		}

		switch (_configuration.Event) {
			case Event.Manual:
				_newDateAndTime = new DateAndTime {
					year = _configuration.Year,
					month = (byte)_configuration.Month,
					day = _configuration.Day,
					hour = _configuration.Hour,
					minute = _configuration.Minute,
					second = _configuration.Second
				};
				break;
			case Event.Nothing:
				_newDateAndTime = new DateAndTime {
					year = 1996,
					month = 7,
					day = 5,
					hour = 0,
					minute = 0,
					second = 0
				};
				break;
			case Event.SummerClaris:
				_newDateAndTime = new DateAndTime {
					year = 1996,
					month = 7,
					day = 20,
					hour = 0,
					minute = 0,
					second = 0
				};
				break;
			case Event.SummerElliot:
				_newDateAndTime = new DateAndTime {
					year = 1996,
					month = 7,
					day = 21,
					hour = 0,
					minute = 0,
					second = 0
				};
				break;
			case Event.HalloweenClaris:
				_newDateAndTime = new DateAndTime {
					year = 1996,
					month = 10,
					day = 1,
					hour = 0,
					minute = 0,
					second = 0
				};
				break;
			case Event.HalloweenElliot:
				_newDateAndTime = new DateAndTime {
					year = 1996,
					month = 10,
					day = 31,
					hour = 0,
					minute = 0,
					second = 0
				};
				break;
			case Event.Winter:
				_newDateAndTime = new DateAndTime {
					year = 1996,
					month = 11,
					day = 1,
					hour = 0,
					minute = 0,
					second = 0
				};
				break;
			case Event.ChristmasNoSanta:
				_newDateAndTime = new DateAndTime {
					year = 1996,
					month = 11,
					day = 25,
					hour = 0,
					minute = 0,
					second = 0
				};
				break;
			case Event.ChristmasSanta:
				_newDateAndTime = new DateAndTime {
					year = 1996,
					month = 12,
					day = 25,
					hour = 0,
					minute = 0,
					second = 0
				};
				break;
			case Event.NewYear:
				_newDateAndTime = new DateAndTime {
					year = 1997,
					month = 1,
					day = 1,
					hour = 0,
					minute = 0,
					second = 0
				};
				break;
			case Event.AprilFools:
				_newDateAndTime = new DateAndTime {
					year = 1997,
					month = 4,
					day = 1,
					hour = 0,
					minute = 0,
					second = 0
				};
				break;
		}
		if (
			_configuration.Event != Event.Manual
			|| _configuration.DayOfWeek == Configuration.DayOfWeek.Auto
		) {
			DateTime dateTime = new DateTime(
				_configuration.Year, (int)_configuration.Month, _configuration.Day
			);
			_newDateAndTime.dayOfWeek = (byte)dateTime.DayOfWeek;
		} else {
			_newDateAndTime.dayOfWeek = (byte)_configuration.DayOfWeek;
		}

		_replaceAll = _configuration.ReplaceAllCalls;
		_log = _configuration.LogAddressOnCall;
	}

	DateAndTime _newDateAndTime;
	bool _replaceAll;
	bool _log;

	static public IHook<setToDateAndTime> _hookSetToDateAndTime;
	public unsafe int SetToDateAndTime(void* dateAndTime) {
		if (_replaceAll == false && (int)dateAndTime != 0x24A6270) {
			if (_log) {
				Console.WriteLine(
					"nights.test.customdateandtime: date and time NOT written to 0x"
					+ ((int)dateAndTime).ToString("X")
				);
			}
			return _hookSetToDateAndTime.OriginalFunction(dateAndTime);
		}
		if (_log) {
			Console.WriteLine(
				"nights.test.customdateandtime: date and time written to 0x"
				+ ((int)dateAndTime).ToString("X")
			);
		}

		DateAndTime* dateAndTimeCast = (DateAndTime*)dateAndTime;
		*dateAndTimeCast = _newDateAndTime;
		return dateAndTimeCast->dayOfWeek;
	}
	// int __usercall sub_5554C0@<eax>(int a1@<edi>)
	[Function(Register.edi, Register.eax, StackCleanup.Caller)]
	public unsafe delegate int setToDateAndTime(void* a1);


	#region Standard Overrides
	public override void ConfigurationUpdated(Config configuration)
	{
		// Apply settings from configuration.
		// ... your code here.
		_configuration = configuration;
		_logger.WriteLine($"[{_modConfig.ModId}] Config Updated: Applying");
	}
	#endregion

	#region For Exports, Serialization etc.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public Mod() { }
#pragma warning restore CS8618
	#endregion
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
struct DateAndTime {
	public ushort year;
	public byte month;
	public byte day;
	public byte hour;
	public byte minute;
	public byte second;
	public byte dayOfWeek;
}
