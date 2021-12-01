var Generator = function(type, startValue) {
  if (type.toLowerCase() === "a")
  {
    this.factor = 16807;
    this.requiredFactor = 4;
  }
  else if (type.toLowerCase() === "b")
  {
    this.factor = 48271;
    this.requiredFactor = 8;
  }
  else {
    throw Error("Unknown number generator type " + type);
  }
  this.previousValue = startValue;
}

Generator.prototype = {
  nextValue: function() {
    do {
      this.previousValue = (this.previousValue * this.factor) % 2147483647;
    } while (this.previousValue % this.requiredFactor !== 0);
    return this.getLeastSignificantBytes(16);
  },
  getLeastSignificantBytes: function(num) {
    var fullvalue = this.previousValue.toString(2);
    return fullvalue.split("").slice(fullvalue.length - num).join("");
  }
}

var compareGenerators = function(genA, genB, sampleSize) {
  var percentageComplete = 0;
  var numMatches = 0;
  for (var i = 0; i < sampleSize; i++) {
    if (Math.floor((i / sampleSize) * 100) > percentageComplete)
    {
      percentageComplete = Math.floor((i / sampleSize) * 100);
      console.log("Percent Done: " + percentageComplete + " %");
    }
    if (genA.nextValue() === genB.nextValue())
    {
      numMatches += 1;
    }
  }
  return numMatches;
}
