var ProgramDance = function (size) {
  this.programArray = []
  for (var i = 0; i < size; i++) {
    this.programArray.push(String.fromCharCode('a'.charCodeAt(0) + i))
  }
}

ProgramDance.prototype = {
  spin: function (num) {
    var split = this.programArray.length - num
    var result = []
    for (var i = 0; i < this.programArray.length; i++) {
      result.push(this.programArray[(i + split) % this.programArray.length])
    }
    this.programArray = result
  },
  exchange: function (posA, posB) {
    var tmp = this.programArray[posA]
    this.programArray[posA] = this.programArray[posB]
    this.programArray[posB] = tmp
  },
  partner: function (nameA, nameB) {
    var posA = this.programArray.indexOf(nameA)
    var posB = this.programArray.indexOf(nameB)
    this.exchange(posA, posB)
  },
  decodeAndExecuteDanceMove: function (move) {
    this.doDecodedDanceMove(this.decodeDanceMove(move))
  },
  doDance: function (moves) {
    for (var i = 0; i < moves.length; i++) {
      this.decodeAndExecuteDanceMove(moves[i])
    }
  },
  doDecodedDanceMove: function (move) {
    switch (move[0]) {
      case 's':
        this.spin(move[1])
        break
      case 'x':
        this.exchange(move[1], move[2])
        break
      case 'p':
        this.partner(move[1], move[2])
        break
      default:
        throw Error('Unknown dance move: ' + move)
    }
  },
  decodeDanceMove: function (move) {
    switch (move[0]) {
      case 's':
        return ['s', parseInt(move.substring(1))]
      case 'x':
        var positions = move.substring(1).split('/').map(x => parseInt(x))
        return ['x', positions[0], positions[1]]
      case 'p':
        var names = move.substring(1).split('/')
        return ['p', names[0], names[1]]
      default:
        throw Error('Unknown dance move: ' + move)
    }
  },
  decodeDance: function (moves) {
    var result = []
    for (var i = 0; i < moves.length; i++) {
      result.push(this.decodeDanceMove(moves[i]))
    }
    return result
  },
  doDecodedDance: function (moves) {
    for (var i = 0; i < moves.length; i++) {
      this.doDecodedDanceMove(moves[i])
    }
  },
  doDanceRepatedly: function (moves, times) {
    var decodedMoves = this.decodeDance(moves)
    var shuffleMap = {}

    var percentageComplete = 0
    var start = new Date()
    for (var i = 0; i < times; i++) {
      if (Math.floor((i / times) * 100) > percentageComplete) {
        percentageComplete = Math.floor((i / times) * 100)
        var cur = new Date();
        console.log('Progress: ', percentageComplete, '% complete in ',  ((cur - start) * (1 / (i / times) - 1)) / 1000.0, 'seconds')
      }
      var oldConfig = this.programArray.join('')
      if (oldConfig in shuffleMap) {
        this.programArray = shuffleMap[oldConfig]
      } else {
        this.doDecodedDance(decodedMoves)
        shuffleMap[oldConfig] = this.programArray.slice(0)
      }
    }
    var end = new Date()
    console.log('Time taken: ' + ((end.getTime() - start.getTime()) / 1000.0) + ' seconds')
  }
}
