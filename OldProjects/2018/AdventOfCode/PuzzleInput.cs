using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    class PuzzleInput
    {
        private readonly string session;
        internal PuzzleInput(string session) => this.session = session;

        internal async Task<string> GetPuzzleInput(string day)
        {
            var request = WebRequest.Create($"https://adventofcode.com/2018/day/{day}/input");
            request.Headers.Add("Cookie", $"session={session}");
            var response = (HttpWebResponse)(await request.GetResponseAsync());
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (var rStream = response.GetResponseStream())
                using (var sr = new StreamReader(rStream))
                {
                    return await sr.ReadToEndAsync();
                }
            }
            else
            {
                return null;
            }
        }
    }
}
