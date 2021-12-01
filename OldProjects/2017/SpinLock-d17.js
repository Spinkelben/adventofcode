var SpinLock = function (stepSize = 312, splitLength = 20000) {
  this.stepSize = stepSize
  this.currentPosition = 0

  var SegmentedArray = function () {
    this.segments = [{startIdx: 0, values: [], next: null, id: 0}]
    this.length = 0
    this.splitLength = splitLength
    this.nextId = 1
  }

  SegmentedArray.prototype = {
    addValue: function (index, value) {
      var segmentList = this.segments.filter(s => this._isIndexWithinSegment(index, s))
      console.assert(segmentList.length === 1, 'Unable to find segment for index value: ' + index)
      var segment = segmentList[0]
      if (segment.values.length + 1 > this.splitLength) {
        // Do split
        var midPoint = Math.floor(segment.values.length / 2)
        var firstHalf = segment.values.slice(0, midPoint)
        var secondHalf = segment.values.slice(midPoint)
        var newSegment = { startIdx: segment.startIdx + midPoint, values: secondHalf, next: segment.next, id: this.nextId }
        this.nextId += 1
        segment.next = newSegment
        segment.values = firstHalf
        var segmentInsertIndex = this.segments.indexOf(segment) + 1
        this.segments.splice(segmentInsertIndex, 0, newSegment)
        segment = index >= newSegment.startIdx ? newSegment : segment
      }
      segment.values.splice(index - segment.startIdx, 0, value)
      var control = 0
      while (segment.next !== null) {
        segment = segment.next
        segment.startIdx += 1
        control++
        console.assert(control < this.segments.length, 'Next pointers out of control')
      }
      this.length += 1
    },
    indexOf: function (value) {
      for (var i = 0; i < this.segments.length; i++) {
        var relativeIndex = this.segments[i].values.indexOf(value)
        if (relativeIndex !== -1) {
          return this.segments[i].startIdx + relativeIndex
        }
      }
    },
    getValue: function (index) {
      var segment = this.segments.filter(s => this._isIndexWithinSegment(index, s))[0]
      return segment.values[index - segment.startIdx]
    },
    _isIndexWithinSegment: function (index, segment) {
      return segment.startIdx <= index && (segment.next === null || segment.next.startIdx > index)
    },
    toArray: function () {
      var result = []
      for (var i = 0; i < this.segments.length; i++) {
        result = result.concat(this.segments[i].values)
      }
      return result
    }
  }

  this.buffer = new SegmentedArray()
  this.buffer.addValue(0, 0)
}

SpinLock.prototype = {
  doStep: function (value) {
    this.currentPosition = (this.currentPosition + this.stepSize) % this.buffer.length
    this.buffer.addValue(this.currentPosition + 1, value)
    this.currentPosition++
  },
  doLoop: function (iterations) {
    var percentageComplete = 0
    var start = new Date()
    for (var i = 1; i <= iterations; i++) {
      if (Math.floor((i / iterations) * 100) > percentageComplete) {
        percentageComplete = Math.floor((i / iterations) * 100)
        var cur = new Date()
        console.log('Progress: ', percentageComplete, '% complete in ', ((cur - start) * (1 / (i / iterations) - 1)) / 1000.0, 'seconds. Time elapsed:', (cur - start) / 1000.0, 'seconds. Length of segments list', this.buffer.segments.length)
      }
      this.doStep(i)
    }
    var end = new Date()
    console.log('Time taken: ' + ((end.getTime() - start.getTime()) / 1000.0) + ' seconds')
  },
  getValueAfterLastInserted: function () {
    var maxValue = this.buffer.toArray().reduce((a, c) => a > c ? a : c)
    return this.getValueAfter(maxValue)
  },
  getValueAfter: function (value) {
    return this.buffer.getValue((this.buffer.indexOf(value) + 1) % this.buffer.length)
  }
}
