# LambdaFun.Time
High-precision time utilities for F#. A drop-in, ergonomic type for timestamping, measuring durations, sequencing time points, and doing precise arithmetic with wall-clock time.
Built on top of .NET’s Stopwatch and DateTime to map high-resolution local ticks into UTC ticks, giving you high-precision “now” with familiar DateTime semantics.
- Target frameworks: .NET Framework 4.8.1, .NET Standard 2.0, .NET Core 3.1, .NET 8, .NET 9
- Language: F#, compatible with C# clients.

## Why use Time?
- High-precision `Now`: Combines `Stopwatch.GetTimestamp()` with UTC ticks for precise current time.
- Safe arithmetic: Add/subtract `TimeSpan` or milliseconds directly on `Time`.
- Natural comparisons: `=`, `<>`, `<`, `<=`, `>`, `>=` work as expected.
- Interop friendly: Convert to `DateTime`, `TimeSpan`, and raw ticks easily.
- Sequences: Generate forward/backward time series, including infinite streams.

## Install
- NuGet Package Manager:
  - `PM> Install-Package LambdaFun.Time`
  
- Paket CLI:
  - `> dotnet paket add LambdaFun.Time --project _my_project_`

## Quick start

``` fsharp
// F#
open System
open LambdaFun.Core

let t0 = Time.Now
do something()
let t1 = Time.Now

let elapsed: TimeSpan = t1 - t0
printfn "Elapsed: %A ms" elapsed.TotalMilliseconds

// Arithmetic
let in500ms = t0 + 500
let in2_5ms = t0 + 2.5m
let in1s = t0 + TimeSpan.FromSeconds 1

// Comparison
if Time.Now >= in1s then printfn "At or past 1 second mark."

// Interop
let dt: DateTime = t0.DateTime
let ticks: int64 = t0.Ticks
```
## API overview
- Construction
    - `Time.Now`: high-precision current time
    - `Time(year, month, day)`
    - `Time(year, month, day, hour, minute, second)`
    - `Time(dateTime: DateTime)`
    - `Time.Default`: zero-based sentinel time

- Properties
    - `Ticks: int64`
    - `DateTime: DateTime`
    - `Age: TimeSpan` (since creation, using high-precision Now)
    - `Year` | `Month` | `Day` | `Hour` | `Minute` | `Second`

- Operators
    - time `+ TimeSpan -> Time`
    - time `+ int (milliseconds) -> Time`
    - time `+ decimal (milliseconds) -> Time`
    - time `- TimeSpan -> Time`
    - time1 `-` time2 `-> TimeSpan`
    - `=`, `<>`, `<`, `<=`, `>`, `>=`

- Sequences
    - `Time.InfiniteSequence start step: seq`
    - `Time.Sequence start finish step: seq`(forward or backward)

## Usage patterns
- High-precision elapsed timing
    - Capture `let t = Time.Now` before work, subtract later for TimeSpan.

- Scheduling/checkpoints
    - Compute future instants with `Time + TimeSpan` and compare.

- Time series generation
    - Build sampling points using `Time.Sequence start finish (TimeSpan.FromMilliseconds 10.0)`.

- Sentinel/default values
    - Use `Time.Default` when an optional timestamp isn’t set.

## Notes
- Timezone: Time.Now maps to **UTC** ticks; Time.DateTime is _**UTC** unless you convert_.
- Determinism: Precision relies on `Stopwatch.Frequency` and _monotonic ticks_ at runtime.
- Collections: Implements equality, hash code, and `IComparable` for maps/sets.

## License
LGPL. See the project license for details.
