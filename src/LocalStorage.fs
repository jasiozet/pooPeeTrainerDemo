module LocalStorage
open State
open Fable.SimpleJson

let TryParseState jsonState =
  jsonState
  |> Json.tryParseAs<State>
  |> function
    | Ok state ->
        state
    | _ -> getBasicState

let RetrieveStateFromLocalStorage =
  Browser.WebStorage.localStorage.getItem("state")
  |> TryParseState

let inline StoreStateInLocalStorage state =
    let jsonState = Json.serialize state
    Browser.WebStorage.localStorage.setItem("state", jsonState)