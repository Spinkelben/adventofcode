var FractalImage = function (rawRules) {
    this.image = [
        ['.', '#', '.'],
        ['.', '.', '#'],
        ['#', '#', '#']
    ]
    var transformRule = function(rule) {
        var split = rule.split(' => ')
        var inputPattern = split[0]
        var outputPattern = split[1]
        return [inputPattern.split('/').map(r => r.split('')), outputPattern.split('/').map(r => r.split(''))]
    }

    var allRules = rawRules.map(r => transformRule(r))
    this.size2Rules = allRules.filter(r => r[0].length === 2)
    this.size3Rules = allRules.filter(r => r[0].length === 3)
}

FractalImage.prototype = {
    enhance: function () {
        var newImage = []
        var blockLength = null
        var rules = null
        if (this.image.length % 2 === 0) {
            blockLength = 2
            rules = this.size2Rules
        } else if (this.image.length % 3 === 0) {
            blockLength = 3
            rules = this.size3Rules
        } else {
            throw Error('Image size is not divisable by 2 or 3. Image size: ' + this.image.length)
        }
        newI = 0
        for (var i = 0; i < this.image.length; i += blockLength) {
            for (var j = 0; j < this.image[i].length; j += blockLength) {
                var newBlock = this.enhanceBlock(rules, i, j, blockLength)
                console.assert(newBlock !== undefined, 'no new block')
                if (j === 0) {
                    for (var k = 0; k < newBlock.length; k++) {
                        newImage.push([])
                    }
                }
                for (var k = 0; k < newBlock.length; k++) {
                    newImage[newI + k] = newImage[newI + k].concat(newBlock[k])
                }
            }
            newI += newBlock.length
        }
        return newImage
    },
    enhanceBlock: function (rules, startI, startJ, blockLength) {
        var self = this
        var mappers = [
            this.directMapper(),
            this.horizontalMirror(),
            this.verticalMirror(),
            this.rotateRight(),
            this.rotateTwice(),
            this.rotateLeft(),
            function (i, j, iMax, jMax) {
                var right = self.rotateRight()(i, j, iMax, jMax)
                return self.verticalMirror()(right[0], right[1], iMax, jMax)
            },
            function (i, j, iMax, jMax) {
                var right = self.rotateRight()(i, j, iMax, jMax)
                return self.horizontalMirror()(right[0], right[1], iMax, jMax)
            },
            function (i, j, iMax, jMax) {
                var twice = self.rotateTwice()(i, j, iMax, jMax)
                return self.verticalMirror()(twice[0], twice[1], iMax, jMax)
            },
            function (i, j, iMax, jMax) {
                var twice = self.rotateTwice()(i, j, iMax, jMax)
                return self.horizontalMirror()(twice[0], twice[1], iMax, jMax)
            },
            function (i, j, iMax, jMax) {
                var left = self.rotateLeft()(i, j, iMax, jMax)
                return self.verticalMirror()(left[0], left[1], iMax, jMax)
            },
            function (i, j, iMax, jMax) {
                var left = self.rotateLeft()(i, j, iMax, jMax)
                return self.horizontalMirror()(left[0], left[1], iMax, jMax)
            }
        ]
        //console.log(rules, startI, startJ, blockLength)
        for (var i = 0; i < mappers.length; i++) {
            for (var j = 0; j < rules.length; j++) {
                if (this.comparePattern(rules[j], mappers[i], startI, startJ, blockLength) === true) {
                    return rules[j][1]
                }
            }
        }
    },
    comparePattern: function (rule, inputMapper, startI, startJ, blockLength) {
        for (var i = 0; i < blockLength; i++) {
            for (var j = 0; j < blockLength; j++) {
                var mappedCoords = inputMapper(i, j, blockLength - 1, blockLength - 1)
                if (this.image[i + startI][startJ + j] !== rule[0][mappedCoords[0]][mappedCoords[1]])
                {
                    return false
                }
            }
        }
        return true
    },
    directMapper: function () {
        return function (i, j, iMax, jMax) {
            return [i, j]
        }
    },
    horizontalMirror: function() {
        return function (i, j, iMax, jMax) {
            return [iMax - i, j]
        }
    },
    verticalMirror: function () {
        return function (i, j, iMax, jMax) {
            return [i, jMax - j]
        }
    },
    rotateRight: function () {
        return function (i, j, iMax, jMax) {
            return [jMax - j, i]
        }
    },
    rotateTwice: function () {
        var self = this
        return function (i, j, iMax, jMax) {
            var rightFun = self.rotateRight()
            var once =  rightFun(i, j, iMax, jMax)
            return rightFun(once[0], once[1], iMax, jMax)
        }

    },
    rotateLeft: function () {
        var self = this
        return function (i, j, iMax, jMax) {
            var twiceFun = self.rotateTwice()
            var rightFun = self.rotateRight()
            var twice = twiceFun(i, j, iMax, jMax)
            return rightFun(twice[0], twice[1], iMax, jMax)
        }
    },
    printMapping: function (inputMapper) {
        for (var i = 0; i < this.image.length; i++) {
            var string = []
            for (var j = 0; j < this.image[i].length; j++) {
                var inMap = inputMapper(i, j, this.image.length - 1, this.image[i].length - 1)
                string.push(this.image[inMap[0]][inMap[1]])
            }
            console.log(string)
        }
    }
}
