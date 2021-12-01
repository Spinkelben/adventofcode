var TuringMachine = function (input) {
    this.parseProgram = function (inputLines)
    {
        this.startState = inputLines[0].split(' ')[3].replace('.', '')
        this.state = this.startState
        this.numSteps = parseInt(inputLines[1].split(' ')[5])

        this.states = {}
        for (var i = 3; i < inputLines.length; i += 10) {
            var curState = {}
            curState.name = inputLines[i].split(' ')[2].replace(':', '')
            curState.instructions = {}

            curState.instructions['0'] = {}
            curState.instructions['1'] = {}

            curState.instructions['0'].write = inputLines[i + 2].split(' ').filter(x => x !== '')[4].replace('.', '')
            curState.instructions['1'].write = inputLines[i + 6].split(' ').filter(x => x !== '')[4].replace('.', '')

            curState.instructions['0'].move = inputLines[i + 3].split(' ').filter(x => x !== '')[6].replace('.', '')
            curState.instructions['1'].move = inputLines[i + 7].split(' ').filter(x => x !== '')[6].replace('.', '')

            curState.instructions['0'].nextState = inputLines[i + 4].split(' ').filter(x => x !== '')[4].replace('.', '')
            curState.instructions['1'].nextState = inputLines[i + 8].split(' ').filter(x => x !== '')[4].replace('.', '')
            this.states[curState.name] = curState
        }
    }
    this.parseProgram(input)
    this.tapeIdx = 0
    this.tape = [0]
    this.moveRight = function () {
        if (this.tapeIdx === this.tape.length - 1)
        {
            this.tape.push(0)
        }
        this.tapeIdx += 1
    }
    this.moveLeft = function () {
        if (this.tapeIdx === 0) {
            this.tape.unshift(0)
        } else {
            this.tapeIdx -= 1
        }
    }
    this.write = function (symbol) {
        this.tape[this.tapeIdx] = symbol
    }
    this.read = function () {
        return this.tape[this.tapeIdx]
    }
    this.setState = function (state) {
        this.state = state
    }
    this.executeInstruction = function () {
        var stateObj = this.states[this.state]
        var instruction = stateObj.instructions[this.read()]
        this.write(instruction.write)
        if (instruction.move === 'right') {
            this.moveRight()
        } else if (instruction.move === 'left') {
            this.moveLeft()
        } else {
            throw Error('Unknown move instruction ' + instruction)
        }
        this.setState(instruction.nextState)

    }
    this.getChecksum = function ()
    {
        return this.tape.reduce((a, c) => a + parseInt(c), 0)
    }
    this.executeNumSteps = function () {
        for (var i = 0; i < this.numSteps; i++) {
            this.executeInstruction()
        }
        return this.getChecksum()
    }
}
