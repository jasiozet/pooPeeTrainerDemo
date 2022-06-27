module App
open Elmish
open Feliz
open Feliz.DaisyUI
open Logic
open System
open Fable.SimpleJson

type State = {
    Pup : Pup
    Events : Event list
}

type Msg =
    | NewEvent of EventType
    | DeleleteLastEvent
    | SaveState

let getBasicState =
    {
      Pup = {Name = "Lusia"; Birthday=DateOnly(2022,05,04); Gender=Female};
      Events = []
    }

let getEmojiForGender gender =
    match gender with
    | Male -> "♂️"
    | Female -> "♀️"

let getDogAgeInDays (currentDate:DateOnly) (birthday:DateOnly) =
    currentDate.DayNumber - birthday.DayNumber

let getDogAge birthday =
    let now = DateTime.Now
    let currentDate = DateOnly(now.Year, now.Month, now.Day)
    getDogAgeInDays currentDate birthday

let getFormattedDogAge birthday =
    let daysTotal = getDogAge birthday
    let weeks = daysTotal / 7
    let days = daysTotal % 7
    $"{weeks} weeks and {days} days old"

let retrieveStateFromLocalStorage =
    let jsonState = Browser.WebStorage.localStorage.getItem("state")
    jsonState
    |> Json.tryParseAs<State>
    |> function
        | Ok state ->
            state
        | _ -> getBasicState

let inline storeStateInLocalStorage state =
    let jsonState = Json.serialize state
    Browser.WebStorage.localStorage.setItem("state", jsonState)

let getNewEvent eventType =
    {Time=DateTime.Now;Type=eventType}

let init() =
    retrieveStateFromLocalStorage,
    Cmd.none

let update (msg: Msg) (state: State): State * Cmd<Msg> =
    match msg with
    | NewEvent eventType -> {state with Events=(getNewEvent eventType)::state.Events}, Cmd.ofMsg SaveState
    | SaveState ->
        storeStateInLocalStorage state
        state, Cmd.none
    | DeleleteLastEvent -> {state with Events= (List.tail state.Events)}, Cmd.ofMsg SaveState

let render (state: State) (dispatch: Msg -> unit) =
  Html.div [

    Daisy.button.button [
      prop.onClick (fun _ -> dispatch (NewEvent Poo))
      prop.text (GetIconForEventType Poo)
    ]
    Daisy.button.button [
      prop.onClick (fun _ -> dispatch (NewEvent Pee))
      prop.text (GetIconForEventType Pee)
    ]
    Daisy.button.button [
      prop.onClick (fun _ -> dispatch (NewEvent Walk))
      prop.text (GetIconForEventType Walk)
    ]
    Daisy.button.button [
      prop.onClick (fun _ -> dispatch (NewEvent Play))
      prop.text (GetIconForEventType Play)
    ]
    Daisy.button.button [
      prop.onClick (fun _ -> dispatch (NewEvent Eat))
      prop.text (GetIconForEventType Eat)
    ]
    Daisy.button.button [
      prop.onClick (fun _ -> dispatch (NewEvent Sleep))
      prop.text (GetIconForEventType Sleep)
    ]

    Daisy.button.button [
      prop.onClick (fun _ -> dispatch DeleleteLastEvent)
      prop.text ("❌ last")
    ]

    // Pup info
    Html.div [
      prop.classes [ "w-96" ]
      prop.children [
        Daisy.card [
          card.bordered
          card.full
          prop.children [
            Html.figure [
              Html.img [prop.src "lusiaC.png"]
            ]
            Daisy.cardBody [
              prop.style [ style.alignItems.center ]
              prop.children [
                Daisy.cardTitle (state.Pup.Name + " " + getEmojiForGender state.Pup.Gender)
                Daisy.cardTitle (getFormattedDogAge state.Pup.Birthday)
              ]
            ]
          ]
        ]
      ]
    ]

    for event in state.Events do
        Html.div [
            Html.text (GetIconForEventType event.Type)
            let formattedTime = event.Time.ToString("dd.MM hh:mm")
            Html.text ($" {formattedTime}")
        ]
  ]
