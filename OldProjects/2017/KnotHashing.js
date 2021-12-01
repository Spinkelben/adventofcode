var Knot = function(listLength) {
  this.currentPosition = 0;
  this.skipSize = 0;
  this.list = [];
  for (var i = 0; i < listLength; i++) {
    this.list[i] = i;
  }
}

Knot.prototype = {
  tie: function(length) {
    if (length > this.list.length)
    {
      throw Error("Length cannot be longer than the list. List length: " + this.list.length + " Prodvided length: " + length);
    }
    var reversedSection = null;
    if (this.currentPosition + length > this.list.length)
    {
      reversedSection = this.list.slice(this.currentPosition).concat(this.list.slice(0, (this.currentPosition + length) % this.list.length)).reverse();
    }
    else
    {
      reversedSection = this.list.slice(this.currentPosition, this.currentPosition + length).reverse();
    }
    for (var i = 0; i < length; i++) {
      this.list[(this.currentPosition + i) % this.list.length] = reversedSection[i];
    }
    this.currentPosition = (this.currentPosition + length + this.skipSize) % this.list.length;
    this.skipSize += 1;

  },
  convertFromASCII: function(characterString) {
    var numbers = [];
    for (var i = 0; i < characterString.length; i++) {
      numbers.push(characterString.charCodeAt(i))
    }
    return numbers;
  },
  addEndSequence: function(sequence) {
    return sequence.concat([17, 31, 73, 47, 23]);
  },
  tieRound: function(lengths) {
    for (var i = 0; i < lengths.length; i++) {
      this.tie(lengths[i]);
    }
  },
  getDenseHash: function() {
    var denseHash = []
    for (var i = 0; i < 16; i++) {
      denseHash.push(this.list.slice(i * 16, (i * 16) + 16).reduce((a, c) => a ^ c));
    }
    return denseHash;
  },
  toHexList: function(numbers) {
    return numbers.map(n => n.toString(16));
  },
  hashString: function(input) {
    var tmp = new Knot(256);
    var convertedString = tmp.addEndSequence(tmp.convertFromASCII(input));
    for (var i = 0; i < 64; i++) {
      tmp.tieRound(convertedString);
    }
    return tmp.toHexList(tmp.getDenseHash());
  }
}


var groupingFunction = function(inArray) {
  var curGroup = 0;
  for (var i = 0; i < inArray.length; i++) {
    for (var j = 0; j < inArray[i].length; j++) {
      if (inArray[i][j] !== "#") {
        continue;
      }
      curGroup += 1;
      inArray[i][j] = curGroup;
      findAdjacents(inArray, i, j, curGroup);
    }
  }
  return curGroup;
}


var findAdjacents = function(inArray, posX, posY, groupSymbol) {
  if (posX - 1 >= 0 && inArray[posX - 1][posY] === "#") {
    inArray[posX - 1][posY] = groupSymbol;
    findAdjacents(inArray, posX - 1, posY, groupSymbol);
  }
  if (posX + 1 < inArray.length && inArray[posX + 1][posY] === "#")
  {
    inArray[posX + 1][posY] = groupSymbol;
    findAdjacents(inArray, posX + 1, posY, groupSymbol);
  }
  if (posY - 1 >= 0 && inArray[posX][posY - 1] === "#") {
    inArray[posX][posY - 1] = groupSymbol;
    findAdjacents(inArray, posX, posY - 1, groupSymbol);
  }
  if (posY + 1 < inArray[posX].length && inArray[posX][posY + 1] === "#")
  {
    inArray[posX][posY + 1] = groupSymbol;
    findAdjacents(inArray, posX, posY + 1, groupSymbol);
  }
}
