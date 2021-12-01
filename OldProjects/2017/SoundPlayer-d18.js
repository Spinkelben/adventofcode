var SoundPlayer = function (id) {
  this.program = []
  this.registers = {}
  this.inqueue = []
  this.sendCallBack = null
  // Initialize registers
  for (var i = 'a'.charCodeAt(0); i <= 'z'.charCodeAt(0); i++) {
    this.registers[String.fromCharCode(i)] = 0
  }
  this.instructionPointer = 0
  this.registers['p'] = id
  this.sendCounter = 0
}

SoundPlayer.prototype = {
  loadProgram: function (instructions) {
    this.program = instructions.map(i => i.split(' ').map(j => j.trim())).map(x => {
      if (x.length === 3) {
        if (isNaN(parseInt(x[2]))) {
          return [x[0] + 'r', x[1], x[2]]
        } else {
          return [x[0] + 'v', x[1], parseInt(x[2])]
        }
      } else if (x[0] === 'snd') {
        if (isNaN(parseInt(x[1]))) {
          return [x[0] + 'r', x[1]]
        }
        return [x[0] + 'v', parseInt(x[1])]
      } else {
        return x
      }
    })
  },
  executeInstruction: function (instruction) {
    var instructionCode = instruction[0]
    var dest = instruction[1]
    switch (instructionCode) {
      case 'sndr':
        this.sendMessage(this.loadRegister(dest))
        return true
      case 'sndv':
        this.sendMessage(dest)
        return true
      case 'setr':
        this.saveRegister(dest, this.loadRegister(instruction[2]))
        return true
      case 'setv':
        this.saveRegister(dest, instruction[2])
        return true
      case 'addr':
        this.addValues(dest, this.loadRegister(instruction[2]))
        return true
      case 'addv':
        this.addValues(dest, instruction[2])
        return true
      case 'mulr':
        this.multiplyValues(dest, this.loadRegister(instruction[2]))
        return true
      case 'mulv':
        this.multiplyValues(dest, instruction[2])
        return true
      case 'modr':
        this.remainderValue(dest, this.loadRegister(instruction[2]))
        return true
      case 'modv':
        this.remainderValue(dest, instruction[2])
        return true
      case 'rcv':
        return this.recieveMessage(dest)
      case 'jgzr':
        this.conditionalJump(dest, this.loadRegister(instruction[2]))
        return true
      case 'jgzv':
        this.conditionalJump(dest, instruction[2])
        return true
      default:
        throw new Error('Unknown instruction ' + instruction)
    }
  },
  sendMessage: function (message) {
    console.assert(message !== undefined, 'undefined messsage sent')
    this.sendCounter += 1
    this.sendCallBack(message)
  },
  loadRegister: function (register) {
    console.assert(register !== undefined, 'undefined register loaded')
    return this.registers[register]
  },
  saveRegister: function (register, value) {
    console.assert(register !== undefined, 'undefined reguster saved')
    console.assert(value !== undefined, 'undefined value saved')
    this.registers[register] = value
  },
  addValues: function (dest, value) {
    console.assert(dest !== undefined, 'undefined dest added')
    console.assert(value !== undefined, 'undefined value added')
    this.registers[dest] += value
  },
  multiplyValues: function (dest, value) {
    console.assert(dest !== undefined, 'undefined dest multiplied')
    console.assert(value !== undefined, 'undefined value multiplied')
    this.registers[dest] *= value
  },
  remainderValue: function (dest, value) {
    console.assert(dest !== undefined, 'undefined dest mod')
    console.assert(value !== undefined, 'undefined value mod')
    this.registers[dest] %= value
  },
  recieveMessage: function (dest) {
    console.assert(dest !== undefined, 'undefined dest revieve')
    if (this.inqueue.length > 0) {
      this.saveRegister(dest, this.inqueue.shift())
      return true
    }
    return false
  },
  conditionalJump: function (conditionalValue, jump) {
    conditionalValue = isNaN(parseInt(conditionalValue)) ? this.loadRegister(conditionalValue) : parseInt(conditionalValue)
    console.assert(jump !== undefined, 'undefined jump')
    console.assert(conditionalValue !== undefined, 'undefined jump condition')
    if (conditionalValue > 0) {
      this.instructionPointer += jump
      this.instructionPointer -= 1 // The execution engine will increment the instruction after this command is executed
    }
  },
  executeSingleStep: function () {
    if (this.instructionPointer < 0 || this.instructionPointer >= this.program.length) {
      return false
    }
    var IsNotBlocked = this.executeInstruction(this.program[this.instructionPointer])
    if (IsNotBlocked) {
      this.instructionPointer += 1
    }
    return IsNotBlocked
  },
  getSendMessageFunction: function () {
    var self = this
    return function (message) {
      console.assert(message !== undefined, 'undefined message sent')
      self.inqueue.push(message)
    }
  }
}

var TaskScheduler = function () {
  this.programs = [new SoundPlayer(0), new SoundPlayer(1)]
  this.programs[0].sendCallBack = this.programs[1].getSendMessageFunction()
  this.programs[1].sendCallBack = this.programs[0].getSendMessageFunction()
}

TaskScheduler.prototype = {
  executePrograms: function (maxIterations) {
    var allBlocked = false
    var step = 0
    // Run util both program are blocked
    while (allBlocked === false) {
      var firstStep = []
      for (var i = 0; i < this.programs.length; i++) {
        // console.log('progam', i, 'instructionPointer', this.programs[i].instructionPointer, 'next instruction', this.programs[i].program[this.programs[i].instructionPointer], "register", Object.entries(this.programs[i].registers))
        var isNotBlocked = this.programs[i].executeSingleStep()
        // console.log('progam', i, 'instructionPointer', this.programs[i].instructionPointer, 'next instruction', this.programs[i].program[this.programs[i].instructionPointer], "register", Object.entries(this.programs[i].registers))
        firstStep.push(isNotBlocked)
        while (this.programs[i].executeSingleStep()) {
          // console.log('progam', i, 'instructionPointer', this.programs[i].instructionPointer, 'next instruction', this.programs[i].program[this.programs[i].instructionPointer], "register", Object.entries(this.programs[i].registers))
        }
      }
      allBlocked = firstStep.every(r => r === false)
      step += 1
      if (maxIterations !== undefined && step >= maxIterations) {
        throw Error('Max iterations exceeded')
      }
    }
    console.log('Deadlock, step', step)
  },
  loadProgram: function (program) {
    this.programs.forEach(p => p.loadProgram(program))
  }
}
