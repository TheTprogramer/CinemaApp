namespace CinemaApp.ConsoleUI
{
    /// <summary>
    /// Луксозен конзолен интерфейс — злато на тъмно синьо.
    /// </summary>
    public static class ConsoleHelper
    {
        // ── Цветова палитра ───────────────────────────────────────────────
        public static readonly ConsoleColor Gold = ConsoleColor.Yellow;
        public static readonly ConsoleColor GoldDim = ConsoleColor.DarkYellow;
        public static readonly ConsoleColor Silver = ConsoleColor.White;
        public static readonly ConsoleColor Muted = ConsoleColor.DarkGray;
        public static readonly ConsoleColor SuccessCol = ConsoleColor.Green;
        public static readonly ConsoleColor ErrorCol = ConsoleColor.DarkRed;
        public static readonly ConsoleColor WarnCol = ConsoleColor.DarkYellow;
        public static readonly ConsoleColor AccentCol = ConsoleColor.Cyan;

        // ── Символи ───────────────────────────────────────────────────────
        private const char CornerTL = '╔';
        private const char CornerTR = '╗';
        private const char CornerBL = '╚';
        private const char CornerBR = '╝';
        private const char LineH = '═';
        private const char LineV = '║';
        private const char TeeL = '╠';
        private const char TeeR = '╣';
        private const char Arrow = '›';
        private const char Star = '★';
        private const int BoxWidth = 62;

        // ── Лого ──────────────────────────────────────────────────────────
        public static void PrintLogo()
        {
            Console.Clear();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            string[] logo = {
             "  ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓",
             "  ▓ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▓",
             "  ▓                                                             ▓",
             "  ▓                  ★ C I N E M A   A P P ★                    ▓",
             "  ▓                                                             ▓",
             "  ▓                Система за управление на кино                ▓",
             "  ▓                                                             ▓",
             "  ▓ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▪ ▓",
             "  ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓",
            };

            Console.ForegroundColor = Gold;
            foreach (var line in logo) Console.WriteLine(line);
        }

        // ── Заглавие с рамка ─────────────────────────────────────────────
        public static void PrintTitle(string title)
        {
            string content = $"  {Star}  {title}  {Star}  ";
            int width = Math.Max(content.Length + 4, BoxWidth);
            Console.ForegroundColor = GoldDim;
            Console.WriteLine("  " + CornerTL + new string(LineH, width) + CornerTR);
            Console.ForegroundColor = Gold;
            Console.WriteLine("  " + LineV + CenterText(content, width) + LineV);
            Console.ForegroundColor = GoldDim;
            Console.WriteLine("  " + TeeL + new string(LineH, width) + TeeR);
            Console.ResetColor();
        }

        public static void PrintSeparator()
        {
            Console.ForegroundColor = GoldDim;
            Console.WriteLine("  " + TeeL + new string(LineH, BoxWidth) + TeeR);
            Console.ResetColor();
        }

        public static void PrintThinSeparator()
        {
            Console.ForegroundColor = Muted;
            Console.WriteLine("  " + new string('─', BoxWidth + 2));
            Console.ResetColor();
        }
        public static void PrintTableHeader(params string[] columns)
        {
            Console.ForegroundColor = GoldDim;
            Console.Write("  " + TeeL);
            foreach (var col in columns)
                Console.Write(new string(LineH, col.Length + 2) + (col == columns[^1] ? "" : "╦"));
            Console.WriteLine(TeeR);

            Console.Write("  " + LineV);
            foreach (var col in columns)
            {
                Console.ForegroundColor = Gold;
                Console.Write($" {col} ");
                Console.ForegroundColor = GoldDim;
                Console.Write(col == columns[^1] ? "" : "║");
            }
            Console.ForegroundColor = GoldDim;
            Console.WriteLine(LineV);

            Console.Write("  " + TeeL);
            foreach (var col in columns)
                Console.Write(new string(LineH, col.Length + 2) + (col == columns[^1] ? "" : "╬"));
            Console.WriteLine(TeeR);
            Console.ResetColor();
        }

        // ── Меню опция ───────────────────────────────────────────────────
        public static void PrintMenuOption(int number, string text)
        {
            Console.ForegroundColor = GoldDim;
            Console.Write($"  {LineV}  ");
            Console.ForegroundColor = Gold;
            Console.Write($"[{number}]");
            Console.ForegroundColor = Muted;
            Console.Write($" {Arrow} ");
            Console.ForegroundColor = Silver;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void PrintMenuFooter()
        {
            Console.ForegroundColor = GoldDim;
            Console.WriteLine("  " + CornerBL + new string(LineH, BoxWidth) + CornerBR);
            Console.ResetColor();
        }

        // ── Съобщения ────────────────────────────────────────────────────
        public static void PrintSuccess(string message)
        {
            Console.WriteLine();
            Console.ForegroundColor = SuccessCol;
            Console.WriteLine($"  ╔══ ✔  {message}");
            Console.ForegroundColor = GoldDim;
            Console.WriteLine("  ╚" + new string('═', message.Length + 6));
            Console.ResetColor();
        }

        public static void PrintError(string message)
        {
            Console.WriteLine();
            Console.ForegroundColor = ErrorCol;
            Console.WriteLine($"  ╔══ ✘  Грешка: {message}");
            Console.ForegroundColor = GoldDim;
            Console.WriteLine("  ╚" + new string('═', message.Length + 14));
            Console.ResetColor();
        }

        public static void PrintWarning(string message)
        {
            Console.ForegroundColor = WarnCol;
            Console.WriteLine($"  ⚠  {message}");
            Console.ResetColor();
        }

        public static void PrintInfo(string message)
        {
            Console.ForegroundColor = AccentCol;
            Console.WriteLine($"  ◆  {message}");
            Console.ResetColor();
        }

        // ── Input методи ────────────────────────────────────────────────
        public static string ReadNonEmptyString(string prompt)
        {
            string? value;
            do
            {
                Console.ForegroundColor = GoldDim;
                Console.Write($"  {LineV}  ");
                Console.ForegroundColor = AccentCol;
                Console.Write(prompt);
                Console.ForegroundColor = Gold;
                Console.Write(" › ");
                Console.ResetColor();
                value = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(value))
                    PrintWarning("Полето не може да е празно.");
            }
            while (string.IsNullOrWhiteSpace(value));
            return value.Trim();
        }

        public static int ReadInt(string prompt)
        {
            int result;
            while (true)
            {
                Console.ForegroundColor = GoldDim;
                Console.Write($"  {LineV}  ");
                Console.ForegroundColor = AccentCol;
                Console.Write(prompt);
                Console.ForegroundColor = Gold;
                Console.Write(" › ");
                Console.ResetColor();
                if (int.TryParse(Console.ReadLine(), out result)) return result;
                PrintWarning("Въведете валидно цяло число.");
            }
        }

        public static decimal ReadDecimal(string prompt)
        {
            decimal result;
            while (true)
            {
                Console.ForegroundColor = GoldDim;
                Console.Write($"  {LineV}  ");
                Console.ForegroundColor = AccentCol;
                Console.Write(prompt);
                Console.ForegroundColor = Gold;
                Console.Write(" › ");
                Console.ResetColor();
                if (decimal.TryParse(Console.ReadLine(), out result)) return result;
                PrintWarning("Въведете валидна сума (напр. 15.00).");
            }
        }

        public static double ReadDouble(string prompt)
        {
            double result;
            while (true)
            {
                Console.ForegroundColor = GoldDim;
                Console.Write($"  {LineV}  ");
                Console.ForegroundColor = AccentCol;
                Console.Write(prompt);
                Console.ForegroundColor = Gold;
                Console.Write(" › ");
                Console.ResetColor();
                if (double.TryParse(Console.ReadLine(), out result)) return result;
                PrintWarning("Въведете валидно число (напр. 120.5).");
            }
        }

        public static string? ReadChoice()
        {
            Console.WriteLine();
            Console.ForegroundColor = GoldDim;
            Console.Write($"  {CornerBL}{new string(LineH, 10)} ");
            Console.ForegroundColor = Gold;
            Console.Write("Вашият избор ");
            Console.ForegroundColor = GoldDim;
            Console.Write($"{new string(LineH, 4)} ");
            Console.ForegroundColor = Gold;
            Console.Write("› ");
            Console.ResetColor();
            return Console.ReadLine();
        }

        public static void Pause()
        {
            Console.WriteLine();
            Console.ForegroundColor = Muted;
            Console.WriteLine($"  {new string('·', 20)}  Натиснете Enter  {new string('·', 20)}");
            Console.ResetColor();
            Console.ReadLine();
        }

        private static string CenterText(string text, int width)
        {
            int padding = Math.Max(0, width - text.Length);
            int left = padding / 2;
            int right = padding - left;
            return new string(' ', left) + text + new string(' ', right);
        }
    }
}
