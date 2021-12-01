var b = 84
var c = b
var a = 1
if (a != 0)
{
    b = b * 100
    b = b + 100000
    c = b
    c = c + 17000
}

var calculateF = function (b) {
    var f = 1
    var d = 2

    do // label 5
    {
        var e = 2
        if (b % d === 0)
        {
            do // label 4
            {
                if ((d * e) - b === 0) // jmp label 3
                {
                    f = 0
                    return f
                }
                e = e + 1
            } while ((e - b) !== 0)
        }
        d = d + 1
    } while ((d - b) !== 0)
    return f
}

var h = 0
do
{

    var f = calculateF(b)


    if (f == 0) // jmp label 6
    {
        h = h + 1
    }

    b = b + 17
} while (b - c != 0) // label 2
