using System.Diagnostics;

namespace EasyConsole;

public static class Input
{
	public static int ReadInt(string prompt, int min, int max)
	{
		Output.DisplayPrompt(prompt);
		return ReadInt(min, max);
	}

	public static int ReadInt(int min, int max)
	{
		int value = ReadInt();

		while (value < min || value > max)
		{
			Output.DisplayPrompt("Please enter an integer between {0} and {1} (inclusive)", min, max);
			value = ReadInt();
		}

		return value;
	}

	public static int ReadInt()
	{
		string? input = Console.ReadLine();
		int value;

		while (!int.TryParse(input, out value))
		{
			Output.DisplayPrompt("Please enter an integer");
			input = Console.ReadLine();
		}

		return value;
	}

	public static string? ReadString(string prompt)
	{
		Output.DisplayPrompt(prompt);
		return Console.ReadLine();
	}

	public static ConsoleKeyInfo ReadKey(string prompt)
	{
		Output.DisplayPrompt(prompt);
		return Console.ReadKey();
	}

	public static Boolean ReadKeyYesOrNo(string prompt)
	{
		var key = ReadKey($"{prompt} (y/n)").KeyChar;
		return key == 'y';
	}

	public static async Task<TEnum> ReadEnumAsync<TEnum>(
    string prompt, CancellationToken cancellationToken = default
    ) where TEnum : struct, IConvertible, IComparable, IFormattable
	{
		Type type = typeof(TEnum);
		if (!type.IsEnum)
    {
      throw new ArgumentException("TEnum must be an enumerated type");
    }
    Output.WriteLine(prompt);
		Menu menu = new ();
		TEnum choice = default(TEnum);
		foreach (var value in Enum.GetValues(type))
    {
      string? option = Enum.GetName(type, value);
      Debug.Assert(option is not null);
      if (option is not null)
			{
				menu.Add(option, () => { choice = (TEnum)value; });
			}
    }

  await menu.DisplayAsync(cancellationToken);
  return choice;
	}
}
