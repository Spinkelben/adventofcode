var StreamScorer = function() {};

StreamScorer.prototype = {
  scoreStream: function(stream) {
    var score = 0;
    var stack = [];
    var mode = "group";
    var garbageCount = 0;
    for (var i = 0; i < stream.length; i++) {
      if (mode === "group")
      {
        if (stream[i] === "{") {
          stack.push("g");
          //console.log(stack.length, stack);
          score += stack.length;
        }
        else if (stream[i] === "}")
        {
          stack.shift();
        }
        else if (stream[i] === "<")
        {
          mode = "garbage";
        }
      }
      else if (mode === "garbage")
      {
        if (stream[i] === "!")
        {
          // Skip the next character
          i++;
        }
        else if (stream[i] === ">")
        {
          mode = "group";
        }
        else {
          garbageCount++;
        }
      }
    }
    return [score, garbageCount];
  },
  runTests: function() {
    console.assert(this.scoreStream("{}") === 1, "{}, score of 1." + " actual: " + this.scoreStream("{}"))
    console.assert(this.scoreStream("{{{}}}") === 6, "{{{}}}, score of 1 + 2 + 3 = 6" + " actual: " + this.scoreStream("{{{}}}"))
    console.assert(this.scoreStream("{{},{}}") === 5, "{{},{}}, score of 1 + 2 + 2 = 5." + " actual: " + this.scoreStream("{}"))
    console.assert(this.scoreStream("{{{},{},{{}}}}") === 16, "{{{},{},{{}}}}, score of 1 + 2 + 3 + 3 + 3 + 4 = 16" + " actual: " + this.scoreStream("{{{},{},{{}}}}"))
    console.assert(this.scoreStream("{<a>,<a>,<a>,<a>}") === 1, "{<a>,<a>,<a>,<a>}, score of 1." + " actual: " + this.scoreStream("{<a>,<a>,<a>,<a>}"))
    console.assert(this.scoreStream("{{<ab>},{<ab>},{<ab>},{<ab>}},") === 9, "{{<ab>},{<ab>},{<ab>},{<ab>}}, score of 1 + 2 + 2 + 2 + 2 = 9." + " actual: " + this.scoreStream("{{<ab>},{<ab>},{<ab>},{<ab>}},"))
    console.assert(this.scoreStream("{{<!!>},{<!!>},{<!!>},{<!!>}}") === 9, "{{<!!>},{<!!>},{<!!>},{<!!>}}, score of 1 + 2 + 2 + 2 + 2 = 9." + " actual: " + this.scoreStream("{{<!!>},{<!!>},{<!!>},{<!!>}}"))
    console.assert(this.scoreStream("{{<a!>},{<a!>},{<a!>},{<ab>}}") === 3, "{{<a!>},{<a!>},{<a!>},{<ab>}}, score of 1 + 2 = 3." + " actual: " + this.scoreStream("{{<a!>},{<a!>},{<a!>},{<ab>}}"))
  }

}
