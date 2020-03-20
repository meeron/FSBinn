namespace FSBinn

module Decoder =
    let decode<'T> (data: byte[]): 'T =
        Unchecked.defaultof<'T>