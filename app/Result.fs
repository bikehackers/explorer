module BikeHackers.App.Result

let toOption =
  function
  | Ok x -> Some x
  | _ -> None

let toErrorOption =
  function
  | Error e -> Some e
  | _ -> None
