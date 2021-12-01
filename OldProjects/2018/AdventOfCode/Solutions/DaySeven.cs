using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class DaySeven : IDaySolution
    {
        private readonly int numWorkers;
        private readonly int baseStepTime;

        public DaySeven()
            : this(5, 60)
        {

        }

        public DaySeven(int numWorkers, int baseStepTime)
        {
            this.numWorkers = numWorkers;
            this.baseStepTime = baseStepTime;
        }

        public string PartOne(string input)
        {
            var map = ParseInput(input);
            var sb = new StringBuilder(map.Count);
            while (map.Count > 0)
            {
                var next = map.Where(kvp => kvp.Value.IsReady).OrderBy(kvp => kvp.Key).First().Value;
                map.Remove(next.Id);
                sb.Append(next.Id);
                foreach (var stepKvp in map)
                {
                    stepKvp.Value.RemoveDependency(next.Id);
                }
            }
            return sb.ToString();
        }

        public string PartTwo(string input)
        {
            var map = ParseInput(input);
            var workers = new List<Worker>();
            for (int i = 0; i < numWorkers; i++)
            {
                workers.Add(new Worker());
            }

            var timePassed = 0;
            while (map.Count > 0)
            {
                var readyWork = map.Values.Where(s => s.IsReady).OrderBy(s => s.Id);
                var readyWorkers = workers.Where(w => w.IsTaskDone);
                readyWork.Zip(readyWorkers, (step, worker) => 
                {
                    worker.AssignWork(step);
                    return step;
                }).ToList().ForEach(s => map.Remove(s.Id));

                var nextdelta = workers.Where(w => w.CurrentStep != null).Min(w => w.TimeToCompletion);
                timePassed += nextdelta;
                foreach (var worker in workers)
                {
                    worker.AdvanceTime(nextdelta);
                }

                foreach (var worker in workers.Where(w => w.IsTaskDone && w.CurrentStep != null))
                {
                    foreach (var step in map.Values)
                    {
                        step.RemoveDependency(worker.CurrentStep.Id);
                    }
                    worker.AssignWork(null);
                }
            }

            return timePassed.ToString();
        }

        public Dictionary<char, Step> ParseInput(string input)
        {
            var parseExpression = new Regex(@"Step (?<dependency>.) must be finished before step (?<step>.) can begin.");
            var map = new Dictionary<char, Step>();
            foreach (var step in input
                .Split('\n')
                .Select(s => s.Replace("\r", ""))
                .Where(s => string.IsNullOrWhiteSpace(s) == false))
            {
                var match = parseExpression.Match(step);
                var stepId = match.Groups["step"].Value[0];
                var dependencyId = match.Groups["dependency"].Value[0];
                if (map.ContainsKey(stepId) == false)
                {
                    map[stepId] = new Step(stepId, baseStepTime);
                }
                if (map.ContainsKey(dependencyId) == false)
                {
                    map[dependencyId] = new Step(dependencyId, baseStepTime);
                }
                map[stepId].AddDependency(dependencyId);
            }
            return map;
        }

        public class Step
        {
            private readonly HashSet<char> dependencies = new HashSet<char>();
            private readonly int baseStepTime;

            public Step(char id, int baseStepTime)
            {
                this.Id = id;
                this.baseStepTime = baseStepTime;
            }

            public char Id { get; }

            public bool IsReady { get => dependencies.Count == 0; }

            public int StepTime { get => (Id - 'A') + 1 + baseStepTime; }

            public void AddDependency(char id)
            {
                this.dependencies.Add(id);
            }

            public void RemoveDependency(char id)
            {
                dependencies.Remove(id);
            }
        }

        public class Worker
        {
            public Step CurrentStep { get; private set; }

            public int TimeSpent { get; private set; }

            public int TimeToCompletion { get => Math.Max((CurrentStep?.StepTime ?? 0) - TimeSpent, 0); }

            public bool IsTaskDone { get => TimeToCompletion == 0; }

            public void AdvanceTime(int seconds)
            {
                TimeSpent += seconds;
            }

            public void AssignWork(Step step)
            {
                CurrentStep = step;
                TimeSpent = 0;
            }
        }
    }
}
