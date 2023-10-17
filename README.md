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
to get the value of **0.125** into a _Hyper_, use

```
Dim h1 as New Hyper(0, 0) ' initialize an integer (with buffer length of 8 bytes), of course, it is wiser to adjust the lowet exponent here, in order to avoid resizing of the buffer when performing divide operation. In this case, you would, for instance, write `New Hyper(0, -4)` to reserve 4 * 64 bits for the value after decimal point. 

h1(0) = 125
h1 /= 1000  ' Buffer automatically resizes so that it fits the QuotientPrecision (Shared Integer)
'/// or   r& = h1.Divide(1000, precision)
'/// r = the returned remainder , precision = -exp64 , lowest exponent = (2^64)^exp64
```



The number of 64-bit digits used in the result of Divide operation can be set by using **QuotientPrecision**.

All operations (+, -, *), except Divide, can have **Hyper** type for the second argument. Division by Int64 is only supported.

 `Dim h1 as New Hyper(0, -1)` is the same as `New Hyper(-1, 0)`, it doesn't matter in which order exponents are passed. Both exponents (lowest and highest) can be positive or negative.

The Default Property is _DigitAt(exp64%)_, digit refers to a 64-bit value.
i.e. The statement `h1(7) = 1234` automatically resizes _h1_'s buffer and increases it's highest exponent64 if it's out of range.  Same goes for the negative values and exponents64 below h1's lowest.

The code of the managed library is not available on GitHub anymore but you can refer to the old project, the principle remains the same.

On this GitHub repo, it's only shown how to override the NewFromString function in order to write a functional one.
### This repo doesn't contain the dll files required for it to run. 
**Follow the instructions here http://silverfox.systems/Hyper/ to download and include the needed files. Or, if you prefer a secure connection(https), you can accomplish the same here -> https://iz.azurewebsites.net/hyper/**

If you have any questions or you want to contribute in any way, you are welcome to contact me or write in discussion.
