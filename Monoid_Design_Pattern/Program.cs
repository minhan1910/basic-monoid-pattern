int a = 1 + 1 + 1 + 1; // + is monoid
bool b = true || true || true || false || true; // || is monoid
bool c = true && true && false; // && is monoid

static int Add10(int a) => a + 10;

// Collections Of things
Function<int> identity = new(a => a);
Function<int> add5 = new(a => a + 5);
Function<int> add10 = new(Add10);
Function<int> sub3 = new(a => a - 3);
Function<int> mul3 = new(a => a * 3);

Function<int> mul5 = new(a => a * 5);
Function<int> div7 = new(a => a / 7);

// associativity - doesn't matter how you group

var add_sub_1 = add5 + (sub3 + add10);
var add_sub_2 = (add5 + sub3) + add10;
var add_sub_3 = add5.Then(sub3).Then(add10);
Console.WriteLine(add_sub_1.run(5));
Console.WriteLine(add_sub_2.run(5));
Console.WriteLine(add_sub_3.run(5));

var mul_div_1 = mul3 + (div7 + mul5) + (sub3 + mul3);
var mul_div_2 = mul3 + div7 + (mul5 + sub3) + mul3;
Console.WriteLine(mul_div_1.run(77));
Console.WriteLine(mul_div_2.run(77));

// special number (abstract 0)
var res1 = ((div7 + identity).run(5)) == (div7.run(5));
var res2 = ((identity + div7).run(5)) == (div7.run(5));
Console.WriteLine(res1);
Console.WriteLine(res2);

static void Example1()
{
    Function<int> add5 = new(a => a + 5);
    Function<int> add10 = new(Add10);

    var add15 = add5 + add10;
    var result = add15.run(5);

    Console.WriteLine(result);

}

public struct Function<T>
{
    public Func<T, T> run;

    public Function(Func<T, T> fun)
        => run = fun;

    // then is a monoid
    public Function<T> Then(Function<T> next)
    {
        var runCopy = run;
        return new(x => runCopy(next.run(x)));
    }

    // + is a monoid
    public static Function<T> operator +(
        Function<T> left,
        Function<T> right
        )
        => new(x => left.run(right.run(x)));
}

public static class FunctionExt
{
    // then is a monoid
    public static Function<T> Then<T>(this Function<T> @this, Function<T> next)
    {
        return new(x => @this.run(next.run(x)));
    }
}