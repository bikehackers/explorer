#load @".paket/load/net50/Thoth.Json.fsx"
#load @".paket/load/net50/Thoth.Json.Net.fsx"

#load @"external/components-db/components/Types.fs"
#load @"external/components-db/components/Thoth.fs"

open System.IO
open Thoth.Json.Net
open BikeHackers.Components
open BikeHackers.Components.Thoth

let rec private filesUnderPath (basePath : string) =
  seq {
    if Directory.Exists basePath
    then
      for f in Directory.GetFiles (basePath) do
        yield f

      for d in Directory.GetDirectories (basePath) do
        yield! filesUnderPath d
  }

let files =
  filesUnderPath "./external/components-db/data/tyres"
  |> Seq.toList

printfn "%A" files

async {
  let! tyres =
    files
    |> Seq.map (fun file -> async {
      let! content =
        File.ReadAllTextAsync file
        |> Async.AwaitTask

      match Decode.fromString Decode.tyre content with
      | Ok x -> return x
      | Error e -> return failwithf "Error parsing file %s: %s" file e
    })
    |> Async.Parallel

  // printfn "%A" tyres

  printfn "There are %i tyres. " (Seq.length tyres)
  printfn "There are %i tyre sizes. " (Seq.length (Seq.collect (fun x -> x.Sizes) tyres))

  let blob =
    tyres
    |> Seq.map Encode.tyre
    |> Seq.toList
    |> Encode.list
    |> Encode.toString 2

  // printfn "%s" blob

  do!
    File.WriteAllTextAsync ("./app/public/tyres.json", blob)
    |> Async.AwaitTask

  printfn "Done. "
}
|> Async.RunSynchronously
