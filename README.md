# Hyper
Hyper (-precision) type is a new version of Broad library (https://github.com/SilverfoxSystems/BroadLib) with corrections, optimizations and method descriptions.
It can be used like any other numeric data type.  But at this point only the basic calculations are available, including _Mod_ and \ operators.

Besides ones mentioned in BroadLib,
- The ToString method also prints out the value in pure decimal format (by default).
- Multiplication by another Hyper variable (use the * operator)
- The buffer gets resized according to the lowest and the highest exponent
- Comparison operators (<, >, ...) should also work now.

The only thing that doesn't work is conversion from a decimal string.
but you can use other ways to input numbers, for example:
to get the value of 0.125 into a Hyper, use

```
h1(0) = 125
h1 /= 1000  
'/// or   r& = h1.Divide(1000, precision)
'/// r = the returned remainder , precision = -exp64 , lowest exponent = (2^64)^exp64
```

The number of 64-bit digits used in the result of Divide operation can be set by using **QuotientPrecision**.

All operations (+, -, *), except Divide, can have **Hyper** type as the second argument. Division by Int64 is only supported.

 `Dim h1 as New Hyper(0, -1)` is the same as `New Hyper(-1, 0)`, it doesn't matter in which order exponents are passed. Both exponents (lowest and highest) can be positive or negative.

I'm not going to continue debugging NewFromString myself because, well, I don't need it in mmy other projects... I don't even need the decimal ToString method, I only made it to show off :)


The Default Property is _DigitAt(exp64%)_, digit refers to a 64-bit value.
i.e. The statement `h1(7) = 1234` automatically resizes _h1_'s buffer and increases it's highest exponent64 if it's out of range.  Same goes for the negative values and exponents64 below h1's lowest.

I currently don't have the time to develop other mathematical functions like square root and such. I haven't thought it out yet, how to elegantly solve for sqrt.

The code of the managed library is not available on GitHub anymore but you can refer to the old project, the principle remains the same.

On this GitHub repo, it's only shown how to override the NewFromString function in order to write a functional one.
### This repo doesn't contain the dll files required for it to run. Follow the instructions here http://silverfox.systems/Hyper/ to download and include the needed files.

If you have any questions or you want to contribute in any way, you are welcome to contact me or write in discussion.
