using System.ComponentModel;
using nights.test.sandbox.Template.Configuration;

namespace nights.test.sandbox.Configuration;

public class Config : Configurable<Config>
{
	/*
		User Properties:
			- Please put all of your configurable properties here.
	
		By default, configuration saves as "Config.json" in mod user config folder.    
		Need more config files/classes? See Configuration.cs
	
		Available Attributes:
		- Category
		- DisplayName
		- Description
		- DefaultValue

		// Technically Supported but not Useful
		- Browsable
		- Localizable

		The `DefaultValue` attribute is used as part of the `Reset` button in Reloaded-Launcher.
	*/

	[Category("General")]
	[DisplayName("Event")]
	[Description("A preset Date and Time. Use \"Manual\" to manually input the date and time.\n" +
		"Preset Dates are:\n" +
		"\tNothing: 1996-07-05\n" +
		"\tSummerClaris: 1996-07-20\n" +
		"\tSummerElliot: 1996-07-21\n" +
		"\tHalloweenClaris: 1996-10-01\n" +
		"\tHalloweenElliot: 1996-10-31\n" +
		"\tWinter: 1996-11-01\n" +
		"\tChristmasNoSanta: 1996-11-25\n" +
		"\tChristmasSanta: 1996-12-25\n" +
		"\tNewYear: 1997-01-01\n" +
		"\tAprilFools: 1997-04-01\n"
	)]
	[DefaultValue(Event.Manual)]
	public Event Event { get; set; }

	[Category("Event: Manual")]
	[DisplayName("Year")]
	[DefaultValue((ushort)1996)]
	public ushort Year { get; set; }
	[Category("Event: Manual")]
	[DisplayName("Month")]
	[DefaultValue(Month.July)]
	public Month Month { get; set; }
	[Category("Event: Manual")]
	[DisplayName("Day")]
	[DefaultValue((byte)5)]
	public byte Day { get; set; }
	[Category("Event: Manual")]
	[DisplayName("Hour")]
	[DefaultValue((byte)0)]
	public byte Hour { get; set; }
	[Category("Event: Manual")]
	[DisplayName("Minute")]
	[DefaultValue((byte)0)]
	public byte Minute { get; set; }
	[Category("Event: Manual")]
	[DisplayName("Second")]
	[DefaultValue((byte)0)]
	public byte Second { get; set; }
	[Category("Event: Manual")]
	[DisplayName("Day of Week")]
	[Description("I am not sure if this is used in the game, but its value is set in the game's code.\n" +
		"\"Auto\" automatically calculates the day of the week from the date given.")]
	[DefaultValue(DayOfWeek.Auto)]
	public DayOfWeek DayOfWeek { get; set; }

	[Category("General")]
	[DisplayName("Replace ALL calls")]
	[Description("Events seem to only use the Date and Time set when launching the game.\n" +
		"If this is \"false\", only this call to get the Date and Time will be replaced.\n" +
		"If this is \"true\", every call to get the Date and Time will be replaced - " +
		"this includes objects like the Clock in Splash Valley.")]
	[DefaultValue(false)]
	public bool ReplaceAllCalls { get; set; }
	[Category("General")]
	[DisplayName("Log Address on Call")]
	[Description("Logs the address written to on call.")]
	[DefaultValue(false)]
	public bool LogAddressOnCall { get; set; }
}

public enum Event {
	Manual,
	Nothing,
	SummerClaris,
	SummerElliot,
	HalloweenClaris,
	HalloweenElliot,
	Winter,
	ChristmasNoSanta,
	ChristmasSanta,
	NewYear,
	AprilFools
}

public enum DayOfWeek {
	Sunday = System.DayOfWeek.Sunday,
	Monday = System.DayOfWeek.Monday,
	Tuesday = System.DayOfWeek.Tuesday,
	Wednesday = System.DayOfWeek.Wednesday,
	Thursday = System.DayOfWeek.Thursday,
	Friday = System.DayOfWeek.Friday,
	Saturday = System.DayOfWeek.Saturday,
	Auto
}

public enum Month {
	January = 1,
	February,
	March,
	April,
	May,
	June,
	July,
	August,
	September,
	October,
	November,
	December
}

/// <summary>
/// Allows you to override certain aspects of the configuration creation process (e.g. create multiple configurations).
/// Override elements in <see cref="ConfiguratorMixinBase"/> for finer control.
/// </summary>
public class ConfiguratorMixin : ConfiguratorMixinBase
{
	// 
}
