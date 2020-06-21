module EncoderTests

open System
open Xunit
open FSBinn.Encoder
open FSBinn.Decoder

[<Fact>]
let ``Encode string`` () =
    let value = "May The Force be with you"
    let longValue = "May The Force be with you May The Force be with you May The Force be with you May The Force be with you May The Force be with you May The Force be with you"

    let result = value |> encode |> decode
    let resultLong = longValue |> encode |> decode

    Assert.Equal(Value value, result)
    Assert.Equal(Value longValue, resultLong)

