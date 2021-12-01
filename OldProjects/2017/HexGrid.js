var HexGrid = function() {
  this.cells = {};
  this.curPos = [0, 0];
  this.furthest = 0;
}

HexGrid.prototype = {
  move: function(direction) {
    switch (direction) {
      case "n":
        this.curPos = [this.curPos[0], this.curPos[1] + 1]
        break;
      case "ne":
        this.curPos = [this.curPos[0] + 1, this.curPos[1] + 1]
        break;
      case "se":
        this.curPos = [this.curPos[0] + 1, this.curPos[1]]
        break;
      case "s":
        this.curPos = [this.curPos[0], this.curPos[1] - 1]
        break;
      case "sw":
        this.curPos = [this.curPos[0] - 1, this.curPos[1] - 1]
        break;
      case "nw":
        this.curPos = [this.curPos[0] - 1, this.curPos[1]]
        break;
      default:
        throw Error("unknown direction: " + direction);
    }
    var curDist = this.calcShortestPath();
    this.furthest = (this.furthest < curDist) ? curDist : this.furthest;
  },
  calcShortestPath: function() {
    if ((this.curPos[0] === 0 && this.curPos[1] === 0) || Math.sign(this.curPos[0]) === Math.sign(this.curPos[1]))
    {
      return Math.max(Math.abs(this.curPos[0]), Math.abs(this.curPos[1]));
    }
    else {
      return Math.abs(this.curPos[0]) + Math.abs(this.curPos[1]);
    }
  },
  walkPath: function(path) {
    for (var i = 0; i < path.length; i++) {
      this.move(path[i]);
    }
  }
}
