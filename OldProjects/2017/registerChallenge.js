Computer.prototype = {
  highest_all_time_value: 0,
  evaluate_cond: function(cond) {
    var lvalue = this.access_register(cond[0]);
    var rvalue = parseInt(cond[2]);
    if (cond[1] === ">")
    {
      return lvalue > rvalue;
    }
    else if (cond[1] === "<")
    {
      return lvalue < rvalue;
    }
    else if (cond[1] === "==")
    {
      return lvalue === rvalue;
    }
    else if (cond[1] === "!=")
    {
      return lvalue !== rvalue;
    }
    else if (cond[1] === ">=")
    {
      return lvalue >= rvalue;
    }
    else if (cond[1] === "<=")
    {
      return lvalue <= rvalue;
    }
    else
    {
      throw Error("Invalid operation '" + cond[1] + "' in command: " + cond)
    }
  },
  access_register: function(register) {
    if (register in this)
    {
      return this[register]
    }
    else
    {
      this[register] = 0;
      return 0;
    }
  },
  add_register: function(register, amount) {
    var new_value = this.access_register(register) + amount;
    this.highest_all_time_value = this.highest_all_time_value < new_value ? new_value : this.highest_all_time_value;
    this[register] = new_value;
  },
  execute_command: function(cmd, cnd) {
    if (this.evaluate_cond(cnd)) {
      var prev = this.access_register(cmd[0]);
      if (cmd[1] === "inc")
      {
        this.add_register(cmd[0], parseInt(cmd[2]));
        console.assert(prev + parseInt(cmd[2]) === this.access_register(cmd[0]), "Addition not good prev: " + prev + " now: " + this.access_register(cmd[0]) + " cmd: " + cmd);
      }
      else if (cmd[1] === "dec")
      {
        this.add_register(cmd[0], 0 - parseInt(cmd[2]));
        console.assert(prev - parseInt(cmd[2]) === this.access_register(cmd[0]), "Subtraction not good prev: " + prev + " now: " + this.access_register(cmd[0]) + " cmd: " + cmd);
      }
      else
      {
        throw Error ("Unknown instruction: '" + cmd[1] + "' command: " + cmd);
      }
    }
  },
  get_largest_value: function()
  {
	  return Object.keys(this).filter(k => this.hasOwnProperty(k)).map(k => this[k]).reduce((acc, curr) => acc > curr ? acc : curr);
  }
}
