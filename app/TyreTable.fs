module BikeHackers.App.TyreTable

open BikeHackers.Components
open Feliz

let private showTyreType =
  function
  | TyreType.Clincher -> "Clincher"
  | TyreType.Tubeless -> "Tubeless"
  | TyreType.Tubular -> "Tubular"

let private showTyreApplication =
  function
    | TyreApplication.Track -> "Track"
    | TyreApplication.Road -> "Road"
    | TyreApplication.RoughRoad -> "Rough Road"
    | TyreApplication.LightGravel -> "Light Gravel"
    | TyreApplication.RoughGravel -> "Rough Gravel"
    | TyreApplication.Singletrack -> "Singletrack"

let private viewRow (tyre : Tyre) (tyreSize : TyreSize) =
  Html.tr [
    // Html.td [ Html.code (string tyre.ID) ]
    Html.td tyre.ManufacturerCode
    Html.td tyre.Name
    Html.td [ Html.code (string tyreSize.BeadSeatDiameter) ]
    Html.td [ Html.code (string tyreSize.Width) ]
    Html.td [ Html.code (tyreSize.Weight |> Option.map string |> Option.defaultValue "-") ]
    Html.td (showTyreType tyreSize.Type)
    Html.td [ Html.code (tyreSize.Tpi |> Option.map string |> Option.defaultValue "-") ]
    Html.td [ Html.code tyreSize.TreadColor ]
    Html.td [ Html.code tyreSize.SidewallColor ]
    Html.td [
      tyre.Application
      |> Option.map (fun xs ->
        Html.ul [
          prop.classes [ "inline-list" ]
          prop.children (
            xs
            |> Seq.map (showTyreApplication >> Html.code >> Html.li)
            |> Seq.toList
          )
        ]
      )
      |> Option.defaultValue (Html.span "-")
    ]
  ]

let view (model : (Tyre * TyreSize) list) =
  // let tyres = model

  // let tyreSizes =
  //   tyres
  //   |> List.collect (fun tyre ->
  //     tyre.Sizes
  //     |> List.map (fun size -> tyre, size)
  //   )

  Html.table [
    Html.thead [
      Html.tr [
        // Html.th "ID"
        Html.th "Manufacturer"
        Html.th "Name"
        Html.th "BSD (mm)"
        Html.th "Width (mm)"
        Html.th "Weight (g)"
        Html.th "Type"
        Html.th "TPI"
        Html.th "Tread Color"
        Html.th "Sidewall Color"
        Html.th "Application"
      ]
    ]
    Html.tbody (
      model
      |> Seq.map (fun (tyre, tyreSize) -> viewRow tyre tyreSize)
      |> Seq.toList
    )
  ]
