using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class DayFour : IDaySolution
    {
        public string PartOne(string input)
        {
            var lines = input.Split('\n').Select(s => s.Replace("\r", "")).Where(s => string.IsNullOrWhiteSpace(s) == false);
            

            var guards = ParseEvents(lines);

            var maxGuard = guards.OrderByDescending(g => g.Value.TotalSleepTime).First().Value;
            var maxMinute = maxGuard.SleepMinutes.ToList().IndexOf(maxGuard.SleepMinutes.Max());

            return $"Guard {maxGuard.Id}, total time: {maxGuard.TotalSleepTime}, Max minute {maxMinute}, Result: {maxGuard.Id * maxMinute}";
        }

        private static Dictionary<int, Guard> ParseEvents(IEnumerable<string> lines)
        {
            Regex re = new Regex(@"\[(?<time>.+)\] (?<event>.+)");
            var events = lines
                .Select(l => re.Match(l))
                .Select(m => (time: DateTimeOffset.Parse(m.Groups["time"].Value), @event: m.Groups["event"].Value))
                .OrderBy(e => e.time);

            var newGuardRe = new Regex(@"Guard #(?<id>\d+) begins shift");
            var guards = new Dictionary<int, Guard>();

            Guard currentGuard = null;
            DateTimeOffset fallAsleepTime = default;
            foreach (var @event in events)
            {
                if (newGuardRe.IsMatch(@event.@event))
                {
                    var id = int.Parse(newGuardRe.Match(@event.@event).Groups["id"].Value);
                    if (guards.ContainsKey(id) == false)
                    {
                        guards[id] = new Guard(id);
                    }
                    currentGuard = guards[id];
                    fallAsleepTime = default;
                }
                else if (@event.@event == "falls asleep")
                {
                    fallAsleepTime = @event.time;
                }
                else if (@event.@event == "wakes up")
                {
                    var start = fallAsleepTime.Minute;
                    var end = @event.time.Minute;
                    for (int i = start; i < end; i++)
                    {
                        currentGuard.SleepMinutes[i]++;
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Unknown event {@event}");
                }
            }

            return guards;
        }

        public string PartTwo(string input)
        {
            var lines = input.Split('\n').Select(s => s.Replace("\r", "")).Where(s => string.IsNullOrWhiteSpace(s) == false);


            var guards = ParseEvents(lines);

            var maxGuard = guards.OrderByDescending(g => g.Value.SleepMinutes.Max()).First().Value;
            var maxMinute = maxGuard.SleepMinutes.ToList().IndexOf(maxGuard.SleepMinutes.Max());

            return $"Guard {maxGuard.Id}, Max minute {maxMinute}, Result: {maxGuard.Id * maxMinute}";
        }

        private class Guard
        {
            internal Guard(int id)
            {
                Id = id;
            }

            internal int Id { get; }

            internal int[] SleepMinutes { get; } = new int[60];

            internal int TotalSleepTime { get => SleepMinutes.Sum(); }
        }
    }
}
