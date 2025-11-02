(*
An implementation of high precision time developed for MSc thesis measurement of
distributed answer set solving benchmarks.
(c) 2009 Jonathan Leaver, University of Western Ontario - Artificial Intelligence Lab

Available to you under the GNU L-GPL: https://www.gnu.org/licenses/lgpl.html
All other uses only with written permission.
*)
namespace LambdaFun.Core

open System
open System.Diagnostics

/// an implementation of high-precision time
type public Time (ticks:int64) =

    /// low-precision system date time
    static let globalTicks = bigint DateTime.UtcNow.Ticks

    /// high-precision local clock measurement
    static let localTicks = bigint (Stopwatch.GetTimestamp())

    /// .NET constant: http://msdn.microsoft.com/en-us/library/system.datetime.ticks.aspx
    static let globalFrequency = 10000I * 1000I

    /// runtime constant: http://msdn.microsoft.com/en-us/library/system.diagnostics.stopwatch.frequency.aspx
    static let localFrequency = bigint Stopwatch.Frequency

    /// current high precision time
    static member Now =
        // capture the high precision time
        let timestamp = bigint (Stopwatch.GetTimestamp())

        // determine how much has passed since our high-precision baseline
        let delta = timestamp - localTicks

        // map the high precision delta from the local clock space into the global clock space:
        let globalDelta = (delta * globalFrequency) / localFrequency

        // high-precision system time
        let ticks = globalTicks + globalDelta

        new Time(int64 ticks)

    /// Returns a 0 based time used as a comparison value for defaults or optional parameters
    static member Default =
        new Time(0L)

    /// high precision age of the current instance
    member x.Age =
        new TimeSpan(Time.Now.Ticks - ticks)

    /// high precision DateTime
    member x.DateTime =
        new DateTime(ticks)

    /// high precision tick count
    member x.Ticks =
        ticks

    /// year value for this time
    member x.Year : int =
        x.DateTime.Year

    /// month value for this time
    member x.Month : int =
        x.DateTime.Month

    /// day value for this time
    member x.Day : int =
        x.DateTime.Day

    /// hour value for this time
    member x.Hour : int =
        x.DateTime.Hour

    /// minute value for this time
    member x.Minute : int =
        x.DateTime.Minute

    /// second value for this time
    member x.Second : int =
        x.DateTime.Second

    /// convenience constructor (ymdhms)
    new (year:int, month:int, day:int, h:int, m:int, s:int) =
        new Time(DateTime(year, month, day, h, m, s).Ticks)

    /// convenience constructor (ymd)
    new (year:int, month:int, day:int) =
        new Time(DateTime(year, month, day).Ticks)

    /// convenience constructor (dt)
    new (dateTime:DateTime) =
        new Time(dateTime.Ticks)

    /// equality, for use in collections
    member private x.Equals (other:Time) =
        ticks = other.Ticks

    /// object equality, for use in collections
    override x.Equals (other:Object) =
        match other with
        | :? Time as otherTime -> x.Equals(otherTime)
        | _ -> false

    /// hash codes, for use in collections (set, dictionary, map)
    override x.GetHashCode () =
        ticks.GetHashCode()

    /// prints the time as a long string
    override x.ToString () =
        x.DateTime.ToString()

    /// inclusively generates an infinite sequence of time steppings
    static member public InfiniteSequence (start:Time) (step:TimeSpan) =
        Seq.initInfinite (fun index -> start + new TimeSpan((int64 index) * step.Ticks))

    /// inclusively generates a sequence of time steppings
    static member public Sequence (start:Time) (finish:Time) (step:TimeSpan) =
        if finish >= start then
            // going forward in time
            Seq.unfold (fun time -> if (time > finish) then None else Some(time, time + step)) start
        else
            // going backward in time
            Seq.unfold (fun time -> if (time < finish) then None else Some(time, time - step)) start

    /// Time == Time -> bool
    static member public op_Equality (left:Time, right : Time) =
        left.Ticks = right.Ticks

    /// Time != Time -> bool
    static member public op_Inequality (left:Time, right : Time) =
        left.Ticks <> right.Ticks

    /// Time > Time -> bool
    static member public op_GreaterThan (left:Time, right : Time) =
        left.Ticks > right.Ticks

    /// Time >= Time -> bool
    static member public op_GreaterThanOrEqual (left:Time, right : Time) =
        left.Ticks >= right.Ticks

    /// Time < Time -> bool
    static member public op_LessThan (left:Time, right : Time) =
        left.Ticks < right.Ticks

    /// Time <= Time -> bool
    static member public op_LessThanOrEqual (left:Time, right : Time) =
        left.Ticks <= right.Ticks

    /// Time + int ms -> Time
    static member public op_Addition (time:Time, ms : int) =
        time + TimeSpan.FromMilliseconds(float ms)

    /// Time + decimal ms -> Time
    static member public op_Addition (time:Time, ms : decimal) =
        time + TimeSpan.FromMilliseconds(float ms)

    /// Time + TimeSpan -> Time
    static member public op_Addition (time:Time, span : TimeSpan) :Time =
        new Time(time.Ticks + span.Ticks)

    /// Time - TimeSpan -> Time
    static member public op_Subtraction (time:Time, span : TimeSpan) =
        new Time(time.Ticks - span.Ticks)

    /// Time - Time -> TimeSpan
    static member public op_Subtraction (left:Time, right : Time) =
        new TimeSpan(left.Ticks - right.Ticks)

    interface IComparable with

        /// IComparable - for use in collections (set, etc)
        member x.CompareTo (other) =
            match other with
            | null -> raise (new ArgumentException())
            | :? Time as otherTime -> ticks.CompareTo(otherTime.Ticks)
            | _ -> 0
