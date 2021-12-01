var BridgeMaker = function (input) {
    this.components = input.map(x => x.split('/').map(y => parseInt(y)))
    this.makeStrongestBridge = function(existing, lastUsedPort, score) {
        var lastComponent = this.components[existing[existing.length - 1]]
        var nextConnector = lastComponent[0] === lastUsedPort ? lastComponent[1] : lastComponent[0]
        var availableComponents = []
        for (var i = 0; i < this.components.length; i++) {
            if (existing.indexOf(i) === -1 && this.components[i].indexOf(nextConnector) !== -1)
            {
                availableComponents.push(i)
            }
        }
        if (availableComponents.length === 0)
        {
            return [existing, score]
        }
        var newBridge = []
        var newScore = score
        for (var i = 0; i < availableComponents.length; i++) {
            var tmpBrige = [...existing, availableComponents[i]]
            var tmpScore = score + this.components[availableComponents[i]].reduce((a, c) => a + c)
            result = this.makeStrongestBridge(tmpBrige, nextConnector, tmpScore)
            if (result[1] > newScore)
            {
                newScore = result[1]
                newBridge = result[0]
            }
        }
        return [newBridge, newScore]
    }
    this.startBridge = function () {
        var startPieces = this.components.filter(c => c.indexOf(0) !== -1)
        var final = [[], 0]
        for (var i = 0; i < startPieces.length; i++) {
            var index = this.components.indexOf(startPieces[i])
            var score = startPieces[i].reduce((a, c) => a + c)
            var result = this.makeStrongestBridge([index], 0, score)
            if (result[0].length > final[0].length && result[1] > final[1])
            {
                final = result
            }
        }
        return final
    }
}
