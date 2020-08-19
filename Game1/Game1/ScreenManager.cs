using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Game1
{
    class ScreenManager
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int MAXIMIZE = 3;

        private const int MF_BYCOMMAND = 0x00000000;
        private const int SC_SIZE = 0xF000;
        private const int SC_MAXIMIZE = 0xF030;
        private const int SC_MINIMIZE = 0xF020;
        private const int SC_RESTORE = 0xF120;

        public static ConsoleColor[] colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));

        public static List<(int row, int col, Sprite sprite)> changes;

        public static void Init()
        {
            Console.Title = "Console Rain";
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Reset();
            ShowWindow(GetConsoleWindow(), MAXIMIZE);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_SIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MINIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_RESTORE, MF_BYCOMMAND);
            changes = new List<(int row, int col, Sprite sprite)>();
        }

        public static void Reset()
        {
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            Console.CursorVisible = false;
        }

        public static void ScreenUpdate()
        {
            foreach ((int row, int col, Sprite sprite) change in changes)
            {
                Console.BackgroundColor = colors[change.sprite.bColor];
                Console.ForegroundColor = colors[change.sprite.fColor];
                Console.SetCursorPosition(change.col * 2, change.row + 1);
                Console.Write(change.sprite.content);
            }

            changes.Clear();

            HudUpdate();
        }

        private static void HudUpdate()
        {
            int width = Console.WindowWidth;
            Entity player = ObjectManager.entities[0];
            string health = $"Health: {player.health}";
            string score = $"Score: {GameManager.score}";
            string powerUps = $"(): x{player.powerUpCounter[0]} <>: x{player.powerUpCounter[1]} ^^: x{player.powerUpCounter[2]} ||: x{player.powerUpCounter[3]} {{}}: x{player.powerUpCounter[4]}";
            string buffer = "";

            for (int b = 0; b < (width - (health.Length + score.Length + powerUps.Length)) / 4; b++)
            {
                buffer += " ";
            }

            Console.BackgroundColor = colors[0];
            Console.ForegroundColor = colors[7];
            Console.SetCursorPosition(0, 0);
            Console.Write(buffer + health + buffer + powerUps + buffer + score + buffer);
        }

        public static void ClearScreen()
        {
            Console.BackgroundColor = colors[0];
            Console.ForegroundColor = colors[0];
            Console.Clear();
        }
    }
}
