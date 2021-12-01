var FireWall = function (layers)
{
  this.layers = {};
  for (var i = 0; i < layers.length; i++) {
    this.layers[layers[i][0]] = { range: layers[i][1], scannerPos: 0, direction: "up" };
    this.lastLayerIndex = layers[i][0];
  }
  this.time = 0;
  this.packets = [];
}

FireWall.prototype = {
  simulateStep: function() {
    this.packets.push({pos: -1, caught: false, delay: this.time})
    this.movepackets();
    this.markCaughtPackets();
    this.moveScanners();
    this.time += 1;
    return this.completedPackets();
  },
  completedPackets: function() {
    if (this.packets.length === this.lastLayerIndex + 1)
    {
      return this.packets[0].caught ? null : this.packets[0];
    }
    return null;
  },
  movepackets: function() {
    for (var i = 0; i < this.packets.length; i++) {
      this.packets[i].pos += 1;
    }
    if (this.packets.length > this.lastLayerIndex + 1) {
      var removed = this.packets.shift();
      //console.assert(removed.pos > this.lastLayerIndex, "Removed packet with wrong index " + removed);
    }
  },
  markCaughtPackets: function() {
    for (var i = 0; i < this.packets.length; i++) {
      var pos = this.packets[i].pos;
      //console.log(this.packets[i]);
      if (pos in this.layers && this.layers[pos].scannerPos === 0)
      {
        this.packets[i].caught = true;
      }
    }
  },
  moveScanners: function() {
    for (k in this.layers) {
      //console.log(this.layers[k], k)
      var layer = this.layers[k];
      if (layer.direction === "up")
      {
        layer.scannerPos += 1;
      }
      else
      {
        layer.scannerPos -= 1;
      }
      if (layer.scannerPos === 0)
      {
        layer.direction = "up";
      }
      if (layer.scannerPos === layer.range - 1)
      {
        layer.direction = "down";
      }
    }
  },
  reset: function() {
    this.packets = [];
    this.time = 0;
    for (l in this.layers) {
      this.layers[l].scannerPos = 0;
      this.layers[l].direction = "up";
    }
  },
  delay: function(picoseconds) {
    for (var i = 0; i < picoseconds; i++) {
      this.moveScanners();
      this.time += 1;
    }
  }
}
