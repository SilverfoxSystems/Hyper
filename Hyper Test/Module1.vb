Imports HyperLib


Module Module1

    Sub Main()

        Hyper.maxDigitsInString = 100 'this number gets aligned to a multiple of 18 in the decimal ToString method's output 
        Hyper.ExtraDigitsForConversionFromDecimal = 50 ' reserve 50 * 64bits extra space for calculation - some values in decimal require higher precision during the conversion
        ' The integral part size (when inputting from string) is currently limited to maximum of 18 characters, use subsequent multiplication to get larger values
        Dim a As New Hyper("-123456789012345678.9012345678901234567890123456789012345678901234567890")
        Dim b As New Hyper(".000000000000000000000000000000000000000000000000000000000000001")
        'Hyper.displayMode = Hyper.displayModeType.inHex ' use any other evaluation method than pure decimal when working with extremely large values to avoid wasting time for conversion to decimal 

        Console.WriteLine(a) : Console.WriteLine()
        Console.WriteLine(b) : Console.WriteLine()
        Console.WriteLine(a * b) : Console.WriteLine()
        Console.ReadKey()


    End Sub

    Sub oldExample()
        Dim a As New Hyper(5, 0) ' initialize an integer with 6 * 64 bits
        Dim b As New Hyper(-1, -4) ' float with 4 * 64 bits
        a(5) = 50000 : a(3) = 50000 ' this is 50000*(2^64)^5 + 50000*(2^64)^3
        b(-1) = -50000 : b(-3) = 50000 ' = -50000*(2^64)^(-1) + 50000*(2^64)^(-3)

        Dim precision% = 914 'we need high precision since we're dividing with a multiple of 5, which results in a repetitive non-integral value in binary system
        Dim c As New Hyper(0, -precision)
        c(0) = 1

        'get value of 1e-60 into c
        For i = 1 To 4
            c.Divide(10 ^ 15, precision)
        Next

        Hyper.maxDigitsInString = 255 'this number gets aligned to a multiple of 18 in the decimal ToString method's output 
        Console.WriteLine(c) : Console.WriteLine()

        Console.WriteLine(a)
        Console.WriteLine(a * c) : Console.WriteLine()

        Console.WriteLine(b)
        Console.WriteLine(b * c) : Console.WriteLine()

        Console.WriteLine(a * b)
        Console.WriteLine(a * b * c)

        Console.ReadKey()

    End Sub

    ' output:
    ' 0.000001 e-54
    '
    ' 106799351796 045504119751085308 477605730449081204 601972535543869862 271369609837145273 371306072473600000
    ' 106799351796045504119751085308477605730449.081204601972535543869862271369609837145273371306072473600000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
    '
    ' -0.00000000000000271050543121376108501863200217485427855648766544433773861485559801116144097204447722403690606963057073306918549243470919528455062639908657029508276536944322288036346435546875
    ' -0.00271050543121376108501863200217485427855648766544433773861485559801116144097204447722403690606963057073306918549243470919528455062639908657029508276536944322288036346435546875 e-72
    '
    ' -289480223093290 488558927462521719 769633174961664101 410098643960019782 824099837500000000
    ' -289480223093290488558927462.521719769633174961664101410098643960019782824099837500000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000

End Module


Class HyperM
 Inherits Hyper

'This class occasionaly works, you can rewrite whole of it, I think this would be one of the fastest ways
 Const TenPow18& = 10 ^ 18

 Sub New()
  MyBase.New(0, 0) 'initialize integer
 End Sub

 Public Overrides Function NewFromString%(ByVal S$)
  'the idea is to read the string by 18 characters and add them into the Hyper value

  expP% = InStr(S, "e")
  expP% = Math.Max(InStr(S, "E"), expP)
  DecimalPt% = InStr(S, ".")
  LoExp% = 0 : HiExp% = 0

  If DecimalPt <= 0 Then
   If expP > 0 Then DecimalPt = expP Else DecimalPt = Len(S) + 1
  End If


  startChar% = Len(S) : endChar% = 1 'starting from right to left

  exp10max& = 0 : exp10& = 0 : exp10min& = 0

  If expP > 0 Then startChar = expP - 1
  If Left(S, 1) = "-" Then endChar = 2


  'get top and bottom exponents
  If expP > 0 Then

   exp10max& = DecimalPt - endChar - 1
   Dim st$ = Strings.Right(S, Len(S) - expP)
   exp10 = CLng(st)

   exp10min = DecimalPt - expP ' + 1 '- 1

   If DecimalPt < expP Then exp10min += 1
   exp10max += exp10
   exp10min += exp10
  Else
   exp10max& = DecimalPt - startChar  ' - Len(S) + expP
   exp10min = DecimalPt - startChar - 1

  End If

  HiExp% = exp10max \ 18


  If exp10min < 0 Then
   LoExp% = -ExtraDigitsForConversionToDecimal + exp10min \ 18
  Else
   LoExp% = exp10min \ 18

  End If
  base& = 1
  Dim factor As New Hyper(exp10max \ 18, 0)
  If Left(S, 1) = "-" Then
   base = -1
   factor(0) = -1
   mid(S, 1, 1) = "0"

  Else
   factor(0) = 1
  End If

  If exp10min > 0 Then
   i% = 0
   For i = 1 To LoExp

	factor.Multiply(TenPow18)

   Next

   i = exp10min Mod 18
  Else


   i = 18 + exp10min Mod 18

  End If



  If DecimalPt < expP Then
   S = Left(S, DecimalPt - 1) + Mid(S, DecimalPt + 1)
   startChar -= 1
  End If






  curr% = 0
  bsz% = Math.Abs(HiExp - LoExp)
  del = -Math.Min(LoExp, HiExp)
  ReDim buff(bsz)

  midPt% = startChar + exp10min

  '  On Error GoTo fault

  If exp10max < 0 Then
   midPt = 1
   GoTo bothBelow
  ElseIf exp10min > 0 Then
   GoTo bothAbove
  End If

  ' here, the decimal point is somewhere in the middle of string, probably this procedure won't work:


  curr = midPt - 1 ' endChar


  factor = New Hyper(HiExp, 0)
  factor(0) = base


  While curr >= endChar

   Add(factor * CLng(Mid(S, curr%, 18)))

   factor.Multiply(TenPow18)
   curr -= 18
  End While

  p% = CInt((exp10max - exp10min + 1) Mod 18)

  S = StrDup(p, "0") + Mid(S, startChar - p, 18 - p)

  '  S += StrDup(p, "0")
  Add(factor * CLng(S))


  GoTo aus


bothBelow:
  curr = midPt ' endChar


  factor = New Hyper(0, LoExp)
  factor(0) = base 'assign the value of +1 or -1 to "factor" 

  For i = 0 To -exp10max \ 18
   factor.Divide(TenPow18, -LoExp)
  Next


  p% = -((exp10max + 1) Mod 18)

  S = StrDup(p, "0") + Mid(S, endChar)
  startChar += p
  If curr + 18 <= startChar Then
   s1$ = Mid(S, curr, 18)
   curr += 18
   Add(factor * CLng(s1))
   factor.Divide(TenPow18, -LoExp)
  Else
   s1$ = Mid(S, curr, startChar - endChar + 1)
   curr += Len(s1) ' - 1
   s1 = StrDup(p, "0") + s1
   s1 = s1.PadRight(18, "0")
   LTEST% = s1.Length

   Add(factor * CLng(s1))
   factor.Divide(TenPow18, -LoExp)
  End If

  While curr + 18 <= startChar
   Add(factor * CLng(Mid(S, curr%, 18)))
   factor.Divide(TenPow18, -LoExp)
   curr += 18
  End While


  'p% = CInt((exp10max - exp10min + 1) Mod 18)
  p = (startChar - curr + 1) Mod 18
  S = Mid(S, startChar - p + 1, p) + StrDup(18 - p, "0")

  Add(factor * CLng(S))

  GoTo aus



bothAbove:


  curr = startChar

  factor = New Hyper(HiExp, 0)
  factor(0) = base

  For i = 1 To exp10min \ 18
   factor.Multiply(TenPow18)
  Next

  p% = CInt((exp10min) Mod 18)
  'p=how many zeros to append

  e% = Math.Min(18 - p, startChar - endChar + 1)



  Strt% = startChar - e + 1

  '   p = 18 - e

  s1 = Mid(S, Strt, startChar - Strt + 1)
  curr -= Len(s1)

  s1 += StrDup(p, "0")
  Add(factor * CLng(s1))
  factor.Multiply(TenPow18)


  While curr - 17 >= endChar

   Add(factor * CLng(Mid(S, curr% - 17, 18)))

   factor.Multiply(TenPow18)
   curr -= 18
  End While

  '   l% = curr

  'p% = CInt((exp10max - exp10min + 1) Mod 18)
  'If e <> endChar Then
  If curr >= endChar Then
   p = curr - endChar + 1
   s1 = Mid(S, endChar, p)
   'S = StrDup(p, "0") + S 'Mid(S, startChar - p, 18 - p)
   Add(factor * CLng(s1))
  End If



aus:
  On Error GoTo 0

  Return 0



fault:



  Return -1

 End Function




End Class
