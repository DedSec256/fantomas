module Fantomas.Tests.InterpolatedStringTests

open NUnit.Framework
open FsUnit
open Fantomas.Tests.TestHelper

[<Test>]
let ``basic string interpolation`` () =
    formatSourceString
        false
        """
let text = "foo"
let s = $"%s{text} bar"
"""
        config
    |> prepend newline
    |> should
        equal
        """
let text = "foo"
let s = $"%s{text} bar"
"""

[<Test>]
let ``modifiers before interpolation`` () =
    formatSourceString
        false
        """
let x = 1
let pi = 3.1414
let text = "cats"

let s = $"I say {x} is one and %0.2f{pi} is pi and %10s{text} are dogs"
"""
        config
    |> prepend newline
    |> should
        equal
        """
let x = 1
let pi = 3.1414
let text = "cats"

let s =
    $"I say {x} is one and %0.2f{pi} is pi and %10s{text} are dogs"
"""

[<Test>]
let ``triple quote string interpolation`` () =
    formatSourceString
        false
        "
let text = \"foo\"
let s = $\"\"\"%s{text} bar\"\"\"
"
        config
    |> prepend newline
    |> should
        equal
        "
let text = \"foo\"
let s = $\"\"\"%s{text} bar\"\"\"
"

[<Test>]
let ``interpolation in strict mode`` () =
    formatSourceString
        false
        """
let text = "foo"
let s = $"%s{text} bar"
"""
        { config with StrictMode = true }
    |> prepend newline
    |> should
        equal
        """
let text = "foo"
let s = $"%s{text} bar"
"""

[<Test>]
let ``multiline expression in multiline string`` () =
    formatSourceString
        false
        "
let str =
    $\"\"\"
    {
        let square x = x  * x
        let isOdd x = x % 2 <> 0
        let oddSquares =
            List.filter isOdd >> List.map square
        oddSquares [  1 .. 0 ]
    }\"\"\"
"
        config
    |> prepend newline
    |> should
        equal
        "
let str =
    $\"\"\"
    {
        let square x = x * x
        let isOdd x = x % 2 <> 0
        let oddSquares = List.filter isOdd >> List.map square
        oddSquares [ 1 .. 0 ]
    }\"\"\"
"

[<Test>]
let ``indentation in interpolation`` () =
    formatSourceString
        false
        """
    $"abc {let x = 3
           x + x} def {let x = 4
                       x + x} xyz"
"""
        config
    |> prepend newline
    |> should
        equal
        """
$"abc {
           let x = 3
           x + x
} def {
           let x = 4
           x + x
} xyz"
"""

[<Test>]
let ``backslash in interpolation, issue 1344`` () =
    formatSourceString
        false
        """
$"\"{bar}\" {1} {2}"
"""
        config
    |> prepend newline
    |> should
        equal
        """
$"\"{bar}\" {1} {2}"
"""

[<Test>]
let ``multiline string literal, issue 1451`` () =
    formatSourceString
        false
        "
$\"\"\"one: {1}<
>two: {2}\"\"\"
"
        config
    |> prepend newline
    |> should
        equal
        "
$\"\"\"one: {1}<
>two: {2}\"\"\"
"

[<Test>]
let ``prefix application, 1414`` () =
    formatSourceString
        false
        """
!- $".{s}"
"""
        config
    |> prepend newline
    |> should
        equal
        """
!- $".{s}"
"""

[<Test>]
let ``format in FillExpr, 1549`` () =
    formatSourceString
        false
        """
let percent =0.1548486

Console.WriteLine($"Formatted: {percent:p2}")
"""
        config
    |> prepend newline
    |> should
        equal
        """
let percent = 0.1548486

Console.WriteLine($"Formatted: {percent:p2}")
"""

[<Test>]
let ``extra newlines in interpolated string, 1613`` () =
    formatSourceString
        false
        "
$\"\"\"
{1}
{2}
\"\"\"

$\"\"\"
- {1}
- 2

- {4}
- 5

\"\"\"
"
        config
    |> prepend newline
    |> should
        equal
        "
$\"\"\"
{1}
{2}
\"\"\"

$\"\"\"
- {1}
- 2

- {4}
- 5

\"\"\"
"
