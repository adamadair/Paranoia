using System;
using C = System.Console;

namespace Paranoia
{
	/// <summary>
	/// Represents a character on screen.  The characteristics of a screen character are
	/// the ascii character value, foreground color, and background color;
	/// </summary>
	public struct ScreenChar
	{
		public ConsoleColor Foreground { get; set; }

		public ConsoleColor Background { get; set; }

		public char DisplayChar { get; set; }
	}

	/// <summary>
	/// Represents a string of characters to display to the screen.
	/// </summary>
	public struct ScreenString
	{
		public ConsoleColor Foreground { get; set; }

		public ConsoleColor Background { get; set; }

		public string DisplayString { get; set; }

		/// <summary>
		/// Converts the string to an array of ScreenChar elements.
		/// </summary>
		/// <returns>ScreenChar array</returns>
		public ScreenChar[] ToCharArray ()
		{
			if (DisplayString == null)
				return null;
			char[] stringChars = DisplayString.ToCharArray ();
			ScreenChar[] arr = new ScreenChar[stringChars.Length];
			for (int i = 0; i < stringChars.Length; ++i) {
				arr [i] = new ScreenChar () {
					Foreground = this.Foreground,
					Background = this.Background,
					DisplayChar = stringChars[i]
				};
			}
			return arr;
		}
	}

	public static class Screen
	{
		public const int SCREEN_HEIGHT = 24;
		public const int SCREEN_WIDTH = 80;
		public static ScreenChar BLANK_CHAR = new ScreenChar () {
			DisplayChar = ' ',
			Foreground = ConsoleColor.White,
			Background = ConsoleColor.Black
		};
		/// <summary>
		/// 2d array to keep track of what has been put on screen. Indexing is [y,x].
		/// Not sure if this is needed yet, may be able to get rid of it later.
		/// </summary>
		private static ScreenChar[,] screenChars;

		public static ConsoleColor Foreground {
			get { return C.ForegroundColor; }
			set { C.ForegroundColor = value; }
		}

		public static ConsoleColor Background {
			get { return C.BackgroundColor; }
			set { C.BackgroundColor = value; }
		}

		static Screen ()
		{
			// initiialize the screenChar array to the 
			screenChars = new ScreenChar[SCREEN_HEIGHT, SCREEN_WIDTH];
			Foreground = ConsoleColor.White;
			Background = ConsoleColor.Black;

		}

		/// <summary>
		/// Should be called once before using, but probably won't hurt anything otherwise.
		/// Sets up the screen 
		/// </summary>
		public static void InitScreen ()
		{
			//C.BufferHeight = SCREEN_HEIGHT;
			//C.BufferWidth = SCREEN_WIDTH;
			Clear ();
		}

		/// <summary>
		/// Clears the screen
		/// </summary>
		public static void Clear ()
		{
			C.CursorVisible = false;
			for (int i = 0; i < SCREEN_HEIGHT; ++i) {
				C.SetCursorPosition (0, i);                
				C.Write ("".PadRight (SCREEN_WIDTH));
				for (int j = 0; j < SCREEN_WIDTH; ++j) {
					screenChars [i, j] = BLANK_CHAR;
				}
			}
		}

		private static bool IsPrintable (this char candidate)
		{
			return !(candidate < 0x20 || candidate > 127);
		}

		/// <summary>
		/// Puts a character on screen at a specific position.
		/// </summary>
		/// <param name="y">Screen Row</param>
		/// <param name="x">Screen Column</param>
		/// <param name="sc">Character to display</param>
		public static void PutChar (int y, int x, ScreenChar sc)
		{
			if (!screenChars [y, x].Equals (sc) && IsPrintable (sc.DisplayChar)) {
				screenChars [y, x] = sc;                
				C.SetCursorPosition (x, y);
				C.ForegroundColor = sc.Foreground;
				C.BackgroundColor = sc.Background;
				C.Write (sc.DisplayChar);
			}            
		}

		/// <summary>
		/// Puts a string to screen
		/// </summary>
		/// <param name="y"></param>
		/// <param name="x"></param>
		/// <param name="value"></param>
		public static void PutString (int y, int x, string value)
		{
			PutString (y, x, new ScreenString () {
				Foreground = Screen.Foreground,
				Background = Screen.Background,
				DisplayString = value
			});
		}

		public static void PutString (int y, int x, ScreenString sc)
		{
			ScreenChar[] chars = sc.ToCharArray ();
			for (int i = 0; i < chars.Length; ++i) {
				PutChar (y, x + i, chars [i]);
			}
		}

		public static string GetString (int y, int x)
		{
			bool cv = C.CursorVisible;
			C.SetCursorPosition (x, y);
			C.CursorVisible = true;
			string returnValue = C.ReadLine ();
			C.CursorVisible = cv;
			return returnValue;
		}

		public static string PromptUser (int y, int x, string prompt)
		{
			ScreenString sc = new ScreenString () {
				DisplayString = prompt,
				Foreground = Screen.Foreground,
				Background = Screen.Background
			};
			ScreenChar[] chars = sc.ToCharArray ();
			for (int i = 0; i < chars.Length; ++i) {
				PutChar (y, x + i, chars [i]);
			}
			return GetString (y, x + chars.Length + 2);
		}

		public static ConsoleKeyInfo GetKey (bool cursorVisible, bool intercept)
		{
			C.CursorVisible = cursorVisible;
			return C.ReadKey (intercept);
		}
	}
}
