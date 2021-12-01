var ParticleAccelerator = function (inputLines) {
  var parsedLines = inputLines.map((r, idx) => r.split('>,').map(re => re.trim().replace('>', '').slice(3).split(',').map(n => parseInt(n))).concat(idx))
  this.particles = []
  for (var i = 0; i < parsedLines.length; i++) {
    this.particles.push({id: parsedLines[i][3],
      px: parsedLines[i][0][0],
      py: parsedLines[i][0][1],
      pz: parsedLines[i][0][2],
      vx: parsedLines[i][1][0],
      vy: parsedLines[i][1][1],
      vz: parsedLines[i][1][2],
      ax: parsedLines[i][2][0],
      ay: parsedLines[i][2][1],
      az: parsedLines[i][2][2]})
  }
  this.removedParticles = 0
}

ParticleAccelerator.prototype = {
  simulateStep: function () {
    for (var i = 0; i < this.particles.length; i++) {
      this.updateParticle(this.particles[i])
    }
    var collisions = this.detectCollisions()
    this.removedParticles += this.removeCollisions(collisions)
  },
  updateParticle: function (particle) {
    particle.vx += particle.ax
    particle.vy += particle.ay
    particle.vz += particle.az
    particle.px += particle.vx
    particle.py += particle.vy
    particle.pz += particle.vz
  },
  detectCollisions: function () {
    var positions = {}
    for (var i = 0; i < this.particles.length; i++) {
      var coordKey = [this.particles[i].px, this.particles[i].py, this.particles[i].pz].toString()
      if ((coordKey in positions) === false) {
        positions[coordKey] = []
      }
      positions[coordKey].push(this.particles[i])
    }
    return Object.entries(positions).filter(p => p[1].length > 1)
  },
  removeCollisions: function (collisions) {
    var removedParticles = 0
    for (var i = 0; i < collisions.length; i++) {
      for (var j = 0; j < collisions[i][1].length; j++) {
        var index = this.particles.indexOf(collisions[i][1][j])
        if (index > -1) {
          this.particles.splice(index, 1)
          removedParticles += 1
        }
      }
    }
    return removedParticles
  },
  simulateNSteps: function (n) {
    var collisions = this.detectCollisions()
    this.removedParticles += this.removeCollisions(collisions)
    var percentageComplete = 0
    var start = new Date()
    for (var i = 0; i < n; i++) {
      this.simulateStep()
      if (Math.floor((i / n) * 100) > percentageComplete) {
        percentageComplete = Math.floor((i / n) * 100)
        var cur = new Date()
        console.log('Progress: ', percentageComplete, '% complete in ', ((cur - start) * (1 / (i / n) - 1)) / 1000.0, 'seconds. Time elapsed:', (cur - start) / 1000.0, 'seconds.')
      }
    }
    var end = new Date()
    console.log('Time taken: ' + ((end.getTime() - start.getTime()) / 1000.0) + ' seconds')
  },
  closestParticles: function () {
    var minDistance = Number.MAX_SAFE_INTEGER
    var pair = []
    for (var i = 0; i < this.particles.length; i++) {
      for (var j = i + 1; j < this.particles.length; j++) {
        var p1 = this.particles[i]
        var p2 = this.particles[j]
        var distance = [p1.px - p2.px, p1.py - p2.py, p1.pz - p2.pz].map(x => Math.abs(x)).reduce((a, c) => a + c)
        if (distance < minDistance) {
          minDistance = distance
          pair = [p1, p2]
        }
      }
    }
    return [minDistance, pair]
  }
}
