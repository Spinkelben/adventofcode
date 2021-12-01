var VirusCarrier = function (inputMap) {
    this.grid = inputMap.map(r => r.split(''))
    this.row = Math.floor(this.grid.length / 2)
    this.column = Math.floor(this.grid[0].length / 2)
    this.numInfected = 0
    this.directionNum = 0
    this.directions = ['up', 'right', 'down', 'left']
}

VirusCarrier.prototype = {
    doBursts: function(num) {
        for (var i = 0; i < num; i++) {
            this.doWork()
        }
    },
    doWork: function () {
        var curSymbol = this.grid[this.row][this.column]
        if (curSymbol === '#') {
            this.grid[this.row][this.column] = 'F'
            this.turnRight()
            this.moveForward()
        }
        else if (curSymbol === '.') {
            this.grid[this.row][this.column] = 'W'
            this.turnLeft()
            this.moveForward()
        }
        else if (curSymbol === 'W') {
            this.grid[this.row][this.column] = '#'
            this.numInfected += 1
            this.moveForward()
        }
        else if (curSymbol === 'F') {
            this.grid[this.row][this.column] = '.'
            this.turnRight()
            this.turnRight()
            this.moveForward()
        }
        // console.log(this.grid, this.row, this.column, this.getDirection())
    },
    turnRight: function () {
        this.directionNum = (this.directionNum + 1) % this.directions.length
    },
    turnLeft: function () {
        this.directionNum = this.directionNum > 0 ? this.directionNum - 1 : this.directions.length - 1
    },
    getDirection: function () {
        return this.directions[this.directionNum]
    },
    moveForward: function () {
        switch (this.getDirection()) {
            case 'up':
                this.row -= 1
                if (this.row < 0) {
                    this.grid.unshift([])
                    for (var i = 0; i < this.grid[1].length; i++) {
                        this.grid[0].push('.')
                    }
                    this.row += 1
                }
                break;
            case 'down':
                this.row += 1
                if (this.row === this.grid.length)
                {
                    var newRow = []
                    for (var i = 0; i < this.grid[0].length; i++) {
                        newRow.push('.')
                    }
                    this.grid.push(newRow)
                }
                break;
            case 'left':
                this.column -= 1
                if (this.column < 0)
                {
                    for (var i = 0; i < this.grid.length; i++) {
                        this.grid[i].unshift('.')
                    }
                    this.column += 1
                }
                break;
            case 'right':
                this.column += 1
                if (this.column === this.grid[0].length)
                {
                    for (var i = 0; i < this.grid.length; i++) {
                        this.grid[i].push('.')
                    }
                }
                break;
            default:
                throw Error('Unknown direction ' + this.getDirection())
        }
    }
}
