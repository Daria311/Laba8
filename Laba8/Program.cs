using System;
using System.Threading;

namespace LabWork8
{
    class Program
    {
        static void Main()
        {
            string[] userInput = File.ReadAllLines("Laba8.txt");
            Subtitles[] subtitles = new Subtitles[userInput.Length];
            SubtitleCreator subtitleCreator = new SubtitleCreator();
            for (int i = 0; i < userInput.Length; i++)
            {
                subtitles[i] = subtitleCreator.CreateSubtitle(userInput[i]);
            }

            SubtitleOutputer display = new SubtitleOutputer(subtitles);
            display.BeignWork();

            Console.ReadLine();
        }

    }
    public class Subtitles
    {
        public int StartTime { get; }
        public int EndTime { get; }
        public string Position { get; }
        public ConsoleColor Color { get; }
        public string Text { get; }

        public Subtitles(int startTime, int endTime, string position, ConsoleColor color, string text)
        {
            StartTime = startTime;
            EndTime = endTime;
            Position = position;
            Color = color;
            Text = text;
        }

    }
    public class SubtitleCreator
    {
        public Subtitles CreateSubtitle(string input)
        {
            int st = GetStartTime(input);
            int et = GetEndTime(input);
            string position = GetPosition(input);
            ConsoleColor color = GetColor(input);
            string text = GetText(input);
            return new Subtitles(st, et, position, color, text);
        }

        private int GetStartTime(string input)
        {
            int startTime = int.Parse(input.Split(" - ")[0].Split(':')[1]);
            return startTime;
        }

        private int GetEndTime(string input)
        {
            int endTime = int.Parse(input.Split(" - ")[1].Split(' ')[0].Split(':')[1]);
            return endTime;
        }

        private string GetPosition(string input)
        {
            string position = "";
            if (input.Contains('['))
                position = input.Split('[')[1].Split(',')[0];
            else
                position = "Bottom";
            return position;
        }

        private ConsoleColor GetColor(string input)
        {
            ConsoleColor color;
            string subColor = "";
            if (input.Contains(']'))
                subColor = input.Split(']')[0].Split(", ")[1];
            switch (subColor)
            {
                case "Red":
                    color = ConsoleColor.Red;
                    break;
                case "Blue":
                    color = ConsoleColor.Blue;
                    break;
                case "Green":
                    color = ConsoleColor.Green;
                    break;
                default:
                    color = ConsoleColor.White;
                    break;
            }
            return color;
        }

        private string GetText(string input)
        {
            string text;
            if (input.Contains('['))
                text = input.Split("] ")[1];
            else
                text = input.Substring(14);
            return text;
        }

    }
    public class SubtitleOutputer
    {
        private static int currentTime;
        private Subtitles[] subtitles;
        public SubtitleOutputer(Subtitles[] subtitles)
        {
            this.subtitles = subtitles;
        }

        public void BeignWork()
        {
            TimerCallback timerCallback = new TimerCallback(Check);
            Timer timer = new Timer(timerCallback, subtitles, 0, 1000);
        }

        private static void Check(object obj)
        {
            Subtitles[] input = (Subtitles[])obj;
            foreach (Subtitles sub in input)
            {
                if (sub.StartTime == currentTime) ShowSubtitleOnConsole(sub);
                else if (sub.EndTime == currentTime) DeleteSubtitleFromConsole(sub);
            }

            currentTime++;
        }

        private static void ShowSubtitleOnConsole(Subtitles sub)
        {
            SetPosition(sub);
            Console.ForegroundColor = sub.Color;
            Console.Write(sub.Text);
        }

        private static void DeleteSubtitleFromConsole(Subtitles sub)
        {
            SetPosition(sub);
            for (int i = 0; i < sub.Text.Length; i++)
                Console.Write(" ");
        }

        private static void SetPosition(Subtitles sub)
        {
            switch (sub.Position)
            {
                case "Top":
                    Console.SetCursorPosition((Console.WindowWidth - sub.Text.Length) / 2, 1);
                    break;
                case "Right":
                    Console.SetCursorPosition(Console.WindowWidth - sub.Text.Length, (Console.WindowHeight - 1) / 2);
                    break;
                case "Bottom":
                    Console.SetCursorPosition((Console.WindowWidth - sub.Text.Length) / 2, Console.WindowHeight);
                    break;
                case "Left":
                    Console.SetCursorPosition(0, (Console.WindowHeight - 1) / 2);
                    break;
                default:
                    break;
            }

        }

    }


}
