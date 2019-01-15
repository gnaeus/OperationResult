# OperationResult
__Rust-style error handling for C#__

[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://raw.githubusercontent.com/gnaeus/OperationResult/master/LICENSE)
[![NuGet version](https://img.shields.io/nuget/v/CSharp.OperationResult.svg)](https://www.nuget.org/packages/CSharp.OperationResult)

```cs
using OperationResult;
using static OperationResult.Helpers;

public Result<double, string> SqrtOperation(double argument)
{
    if (argument < 0)
    {
        return Error("Argument must be greater than zero");
    }
    double result = Math.Sqrt(argument);
    return Ok(result);
}

public void Method()
{
    var result = SqrtOperation(123);

    if (result)
    {
        Console.WriteLine("Value is: {0}", result.Value);
    }
    else
    {
        Console.WriteLine("Error is: {0}", result.Error);
    }
}
```

---
* [__`Result<TResult>`__](#operation-result-result)

* [__`Result<TResult, TError>`__](#operation-result-result-error)

* [__`Result<TResult, TError1, TError2, ...>`__](#operation-result-result-multiple-errors)

* [__`Status`__](#operation-result-status)

* [__`Status<TError>`__](#operation-result-status-error)

* [__`Status<TError, TError2, ...>`__](#operation-result-status-multiple-errors)

* [__`Helpers`__](#operation-result-helpers)
  - `Ok<>()`
  - `Error<>()`
  - `Ok<TResult>(TResult result)`
  - `Error<TError>(TError error)`

### <a name="operation-result-result"></a>`Result<TResult>`
Result of some method when there is no `TError` type defined
```cs
public struct Result<TResult>
{
    public readonly TResult Value;

    public bool IsError { get; }
    public bool IsSuccsess { get; }

    public static implicit operator bool(Result<TResult> result);
    public static implicit operator Result<TResult>(TResult result);
}
```

Example
```cs
public Result<uint> Square(uint argument)
{
    if (argument >= UInt16.MaxValue)
    {
        return Error();
    }
    return Ok(argument * argument);
}
```

### <a name="operation-result-result-error"></a>`Result<TResult, TError>`
Either Result of some method or Error from this method
```cs
public struct Result<TResult, TError>
{
    public readonly TError Error;
    public readonly TResult Value;

    public bool IsError { get; }
    public bool IsSuccsess { get; }

    public void Deconstruct(out TResult result, out TError error);

    public static implicit operator bool(Result<TResult, TError> result);
    public static implicit operator Result<TResult, TError>(TResult result);
}
```

Also `Result` has shorthand implicit conversion from `TResult` type
```cs
using OperationResult;
using static OperationResult.Helpers;

public async Task<Result<string, HttpStatusCode>> DownloadPage(string url)
{
    using (var client = new HttpClient())
    using (var response = await client.GetAsync(url))
    {
        if (response.IsSuccessStatusCode)
        {
            // return string as Result
            return await response.Content.ReadAsStringAsync();
        }
        return Error(response.StatusCode);
    }
}
```

### <a name="operation-result-result-multiple-errors"></a>`Result<TResult, TError1, TError2, ...>`
Either Result of some method or multiple Errors from this method
```cs
public struct Result<TResult, TError1, TError2, ...>
{
    public readonly TError Error;
    public readonly TResult Value;

    public bool IsError { get; }
    public bool IsSuccsess { get; }

    public void Deconstruct(out TResult result, out object error);
    
    public static implicit operator bool(Result<TResult, TError> result);
    public static implicit operator Result<TResult, TError>(TResult result);
}
```

Example
```cs
public Result<int, InnerError> Inner()
{
    return Error(new InnerError());
}

public Result<int, OuterError, InnerError> Outer()
{
    var result = Inner();
    if (!result)
    {
        return Error(result.Error);
    }
    return Error(new OuterError());
}

public void Method()
{
    var result = Outer();
    if (result)
    {
        // ...
    }
    else if (result.HasError<InnerError>())
    {
        Console.WriteLine("{0}", result.GetError<InnerError>());
    }
    else if (result.HasError<OuterError>())
    {
        Console.WriteLine("{0}", result.GetError<OuterError>());
    }
}
```

### <a name="operation-result-status"></a>`Status`
Status of some operation without result when there is no `TError` type defined
```cs
public struct Status
{
    public bool IsError { get; }
    public bool IsSuccsess { get; }

    public static implicit operator bool(Status status);
}
```

Example
```cs
public Status IsOdd(int value)
{
    if (value % 2 == 1)
    {
        return Ok();
    }
    return Error();
}
```

### <a name="operation-result-status-error"></a>`Status<TError>`
Status of some operation without result
```cs
public struct Status<TError>
{
    public readonly TError Error;

    public bool IsError { get; }
    public bool IsSuccsess { get; }

    public static implicit operator bool(Status<TError> status);
}
```

Example
```cs
public Status<string> Validate(string input)
{
    if (String.IsNullOrEmpty(input))
    {
        return Error("Input is empty");
    }
    if (input.Length > 100)
    {
        return Error("Input is too long");
    }
    return Ok();
}
```

### <a name="operation-result-status-multiple-errors"></a>`Status<TError1, TError2, ...>`
Status of some operation without result but with multiple Errors from this method
```cs
public struct Status<TError1, TError2, ...>
{
    public readonly object Error;

    public bool IsError { get; }
    public bool IsSuccess { get; }

    public TError GetError<TError>();
    public bool HasError<TError>();

    public static implicit operator bool(Status<TError1, TError2> status);
}
```

Example
```cs
public Status<InnerError> Inner()
{
    return Error(new InnerError());
}

public Status<OuterError, InnerError> Outer()
{
    var result = Inner();
    if (!result)
    {
        return Error(result.Error);
    }
    return Error(new OuterError());
}
```
<hr>

### <a name="operation-result-helpers"></a>`Helpers`
```cs
public static class Helpers
{
    public static SuccessTag Ok();
    public static ErrorTag Error();
    public static SuccessTag<TResult> Ok<TResult>(TResult result);
    public static ErrorTag<TError> Error<TError>(TError error);
}
```

## Benchmarks
A performance comparsion with other error handling techniques

|                                                Method | SuccessRate |         Mean |      StdDev |  Gen 0 | Allocated |
|------------------------------------------------------ |------------ |------------- |------------ |------- |---------- |
|                     `TResult Operation() + Exception` |          50 | 1 068 491 ns | 2 754.82 ns |      - |   10.4 kB |
|             **`Result<TResult, TError> Operation()`** |      **50** | **2 025 ns** | **0.67 ns** |  **-** |   **0 B** |
|                  `Tuple<TResult, TError> Operation()` |          50 |       957 ns |     1.71 ns | 0.9669 |    1.6 kB |
| `bool Operation(out TResult value, out TError error)` |          50 |       650 ns |     0.15 ns |      - |       0 B |
|                                                       |             |              |             |        |           |
|                     `TResult Operation() + Exception` |          90 |   212 520 ns |   529.55 ns |      - |   2.08 kB |
|             **`Result<TResult, TError> Operation()`** |      **90** | **1 995 ns** | **1.86 ns** |  **-** |   **0 B** |
|                  `Tuple<TResult, TError> Operation()` |          90 |       815 ns |     1.18 ns | 0.9669 |    1.6 kB |
| `bool Operation(out TResult value, out TError error)` |          90 |       463 ns |     0.41 ns |      - |       0 B |
|                                                       |             |              |             |        |           |
|                     `TResult Operation() + Exception` |          99 |    22 069 ns |    52.44 ns |      - |     208 B |
|             **`Result<TResult, TError> Operation()`** |      **99** | **1 989 ns** | **2.84 ns** |  **-** |   **0 B** |
|                  `Tuple<TResult, TError> Operation()` |          99 |       778 ns |     1.31 ns | 0.9669 |    1.6 kB |
| `bool Operation(out TResult value, out TError error)` |          99 |       430 ns |     0.34 ns |      - |       0 B |

