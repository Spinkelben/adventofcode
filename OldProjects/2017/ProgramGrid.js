var ProgramGrid = function(size) {
  this.grid = [];
  for (var i = 0; i < size; i++) {
    this.grid.push([]);
    for (var j = 0; j < size; j++) {
      this.grid[i].push(0)
    }
    this.grid[i][i] = 1;
  }
}

ProgramGrid.prototype = {
  addConnections: function(source, dests) {
    for (var i = 0; i < dests.length; i++) {
      this.grid[source][dests[i]] = 1;
      this.grid[dests[i]][source] = 1;
    }
  },
  calcGroup: function(init) {
    var group = new Set([init]);
    var queue = [init];
    while (queue.length > 0)
    {
      var cur = queue.shift();
      for (var i = 0; i < this.grid[cur].length; i++) {
        if (this.grid[cur][i] !== 0) {
          if (group.has(i) === false) {
            queue.push(i);
            group.add(i);
          }
        }
      }
    }
    return group;
  },
  calcAllGroups: function() {
    var groups = [];
    var seen_programs = new Set([]);
    for (var i = 0; i < this.grid.length; i++) {
      if (seen_programs.has(i) === false)
      {
        newGroup = this.calcGroup(i);
        groups.push(newGroup);
        for (var elem of newGroup)
        {
          seen_programs.add(elem);
        }
      }
    }
    return groups;
  }
}
