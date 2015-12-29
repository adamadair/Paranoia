/* This is a solo paranoia game taken from the Jan/Feb issue (No 77) of
   "SpaceGamer/FantasyGamer" magazine.

   Article by Sam Shirley.

   Implemented in C on Vax 11/780 under UNIX by Tim Lister
   Implemented in C# on some lousy HP laptop running Windows 7 by Adam Adair

   This is a public domain adventure and may not be sold for profit 
 */
using System;
using C = System.Console;

namespace Paranoia
{
    class Program
    {
        public int screenLinesPrinted;
        public const int MAX_LINES = 24;
        Paranoia p;
        ParanoiaResponse currentResponse;

        static int GetKey {
            get {
                //Mono fix: numpad keys don't return correct scancodes
                if (Type.GetType ("Mono.Runtime") != null) {
                    int i = (int)C.ReadKey (true).Key;
                    return i > 48 && i < 58 ? i + 48 : i;
                }

                return (int)C.ReadKey (true).Key;
            }
        }

        static void Main (string[] args)
        {
            new Program ().main ();
        }

        private void main ()
        {
            Screen.InitScreen ();
            p = new Paranoia ();
            Clear ();
            PrintResponse (p.Instructions ());
            More ();
            PrintResponse (p.Character ());
            More ();
            Play ();
        }

        private void Play ()
        {
            bool playing = true;
            currentResponse = p.Start ();
            do {
                PrintResponse (currentResponse);
                if (currentResponse.Choices == null || currentResponse.Choices.Length == 0) {
                    playing = false;
                } else {
                    ParanoiaChoice c = GetPlayerChoice ();
                    currentResponse = p.PlayerChoice (c);
                }
            } while (playing);
            More ();
        }

        ParanoiaChoice GetPlayerChoice ()
        {
            ParanoiaChoice theChoice = null;
            while (theChoice==null) {
                printChoices ();
                ConsoleKey cki = C.ReadKey (true).Key;
                if (cki == ConsoleKey.Q) {
                    // quit
                } else if (cki == ConsoleKey.P) {
                    // Display Player Information
                } else {
                    int pr = KeyToInt (cki);
                    if (pr > -1 && pr < currentResponse.Choices.Length) {
                        theChoice = currentResponse.Choices [pr];
                    }
                } 
            }
            return theChoice;         
        }

        private void printChoices ()
        {
            C.ForegroundColor = ConsoleColor.Green;
            C.WriteLine ("\nSelect:");
            char a = 'a';
            for (int i = 0; i < currentResponse.Choices.Length; ++i) {
                C.WriteLine ((char)(a + (char)i) + " - " + currentResponse.Choices [i].Description);
            }
            C.ForegroundColor = ConsoleColor.White;
        }

        private int KeyToInt (ConsoleKey k)
        {
            if (k == ConsoleKey.A) {
                return 0;
            }
            if (k == ConsoleKey.B) {
                return 1;
            }
            if (k == ConsoleKey.C) {
                return 2;
            }
            if (k == ConsoleKey.D) {
                return 3;
            }
            return -1;
        }

        static void ResetCursor ()
        {
            C.SetCursorPosition (0, 0);
        }

        /// <summary>
        /// There is more text, wait for the user to hit a key.
        /// </summary>
        private void More ()
        {
            C.WriteLine ("---------- More ----------");
            int i = GetKey;
            Clear ();
            screenLinesPrinted = 0;
        }

        private void PrintResponse (ParanoiaResponse r)
        {
            Clear ();
            for (int i = 0; i < r.TextLines.Length; ++i) {
                string l = r.TextLines [i];
                if (l == Paranoia.MORE_STRING) {
                    if (screenLinesPrinted > 0)
                        More ();
                } else {
                    C.WriteLine (l);
                    screenLinesPrinted++;
                }
                if (((screenLinesPrinted % 24) == 0) && screenLinesPrinted > 0) {
                    More ();
                }
            }
        }

        private void Clear ()
        {
            Screen.Clear ();
            screenLinesPrinted = 0;
            ResetCursor ();
        }
    }
}
