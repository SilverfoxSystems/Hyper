# Hyper
Hyper (-precision) type is a new version of Broad library (https://github.com/SilverfoxSystems/BroadLib) with corrections, optimizations and method descriptions.
It can be used like any other numeric data type.  But at this point only the basic calculations are available, including _Mod_ and \ operators.

Besides ones mentioned in BroadLib,
- The ToString method also prints out the value in pure decimal format (by default).
- The type can be initialized from a string of any length
- Multiplication by another Hyper variable (use the * operator)
- The buffer gets resized according to the lowest and the highest exponent
- Comparison operators (<, >, ...) should also work now.


All operations (+, -, *), except Divide, can have **Hyper** type for the second argument. Division by Int64 is only supported.

 `Dim h1 as New Hyper(0, -1)` is the same as `New Hyper(-1, 0)`, it doesn't matter in which order exponents are passed. Both exponents (lowest and highest) can be positive or negative.

The Default Property is _DigitAt(exp64%)_, digit refers to a 64-bit value.
i.e. The statement `h1(7) = 1234` automatically resizes _h1_'s buffer and increases it's highest exponent64 if it's out of range.  Same goes for the negative values and exponents64 below h1's lowest.

The code of the managed library is not available on GitHub anymore but you can refer to the old project, the principle remains the same.

### This repo doesn't contain the dll files required for it to run. 
**Follow the instructions here http://silverfox.systems/Hyper/ to download and include the needed files. Or, if you prefer a secure connection(https), you can accomplish the same here -> https://iz.azurewebsites.net/hyper/**

If you have any questions or you want to contribute in any way, you are welcome to contact me or write in discussion.
