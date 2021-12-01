var CoProcessor = function (input) {
    this.registers = {}
    for (var i = 0; i < 8; i++) {
        var charCode = "a".charCodeAt(0) + i
        this.registers[String.fromCharCode(charCode)] = 0
    }
    this.program = []
    this.pc = 0
    this.numMul = 0
    this.registers['a'] = 1

    this.parseArg = function (arg) {
        if (isNaN(parseInt(arg))){
            return this.registers[arg]
        }
        else {
            return parseInt(arg)
        }
    }

    this.setRegister = function (reg, value) {
        this.registers[reg] = value
        return 1
    }

    this.jmpNotZero = function (cond, offset) {
        if (cond !== 0) {
            return offset
        }
        return 1
    }

    this.subtract = function (dest, value) {
        this.registers[dest] -= value
        return 1
    }

    this.multiply = function (dest, value) {
        this.numMul += 1
        this.registers[dest] *= value
        return 1
    }

    this.loadProgram = function (inputLines) {
        this.program = []
        for (var i = 0; i < inputLines.length; i++) {
            parts = inputLines[i].split(' ')
            this.program.push([parts[0], parts.slice(1)])
        }
    }

    this.executeInstruction = function (print) {
        if (this.pc < 0 || this.pc >= this.program.length) {
            return false
        }
        var instruction = this.program[this.pc][0]
        var args = this.program[this.pc][1]
        switch (instruction) {
            case 'set':
                this.pc += this.setRegister(args[0], this.parseArg(args[1]))
                break;
            case 'sub':
                this.pc += this.subtract(args[0], this.parseArg(args[1]))
                break;
            case 'mul':
                this.pc += this.multiply(args[0], this.parseArg(args[1]))
                break;
            case 'jnz':
                this.pc += this.jmpNotZero(...args.map(x => this.parseArg(x)))
                break;
            default:
                throw Error('Unknown command ' + instruction)
        }
        if (print !== undefined) {
            console.log(instruction, args, this.registers, this.pc)
        }
        return true
    }

    if (input !== undefined) {
        this.loadProgram(input)
    }

    this.executeNInstructios = function (n) {
        for (var i = 0; i < n; i++) {
            if (this.executeInstruction() === false) {
                return 'Halted'
            }
        }
        return 'Running'
    }

}
