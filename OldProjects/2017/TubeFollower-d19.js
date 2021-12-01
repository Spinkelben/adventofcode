var TubeFollower = function (inputLines) {
  this.grid = inputLines.map(l => l.split(''))
  this.position = [0, this.grid[0].indexOf('|')]
  this.direction = 'down'
  this.symbols = []
  this.stepCount = 0
}

TubeFollower.prototype = {
  takeStep: function () {
    this.stepCount++
    this.position = this.getFieldCoordinates(this.direction)
    var currentSymbol = this.grid[this.position[0]][this.position[1]]
    if (currentSymbol === '+') {
      var possibleDirections = ['up', 'down', 'left', 'right'].filter(d => d !== this.direction && d !== this.oppositeDirection[this.direction])
      var fields = possibleDirections.map(d => this.getFieldCoordinates(d)).map(c => this.grid[c[0]][c[1]])
      if (!fields.some(f => f !== ' ')) {
        return true
      }
      this.direction = possibleDirections[fields.map(f => f !== ' ').indexOf(true)]
    } else if (currentSymbol !== '-' && currentSymbol !== '|' && currentSymbol !== ' ') {
      this.symbols.push(currentSymbol)
    } else if (currentSymbol === ' ') {
      return true
    }
    return false
  },
  getFieldCoordinates: function (direction) {
    switch (direction) {
      case 'down':
        return [this.position[0] + 1, this.position[1]]
      case 'up':
        return [this.position[0] - 1, this.position[1]]
      case 'right':
        return [this.position[0], this.position[1] + 1]
      case 'left':
        return [this.position[0], this.position[1] - 1]
      default:
        throw Error('Unknown direction' + direction)
    }
  },
  oppositeDirection: {down: 'up', up: 'down', right: 'left', left: 'right'},
  walkPath: function () {
    var pathEnded = false
    while (pathEnded === false) {
      pathEnded = this.takeStep()
    }
    return this.symbols
  }
}
