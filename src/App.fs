module App
open Elmish
open Feliz
open Feliz.DaisyUI
open Logic
open System
open Fable.SimpleJson
open Feliz.Recharts

type PresentationType = EventStack | Graph

type State = {
  Pup : Pup
  Events : Event list
  PresentationType : PresentationType
}

type Msg =
  | NewEvent of EventType
  | DeleleteLastEvent
  | SwitchPresentationType of PresentationType
  | SaveState

let getBasicState =
  {
    Pup = {Name = "Lusia"; Birthday=DateOnly(2022,05,04); Gender=Female};
    PresentationType = EventStack;
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

let getNewEventNow eventType =
    {Time=DateTime.Now;Type=eventType}

let downLoad fileName fileContent =
    let anchor = Browser.Dom.document.createElement "a"
    let encodedContent = fileContent |> sprintf "data:text/plain;charset=utf-8,%s" |> Fable.Core.JS.encodeURI
    anchor.setAttribute("href",  encodedContent)
    anchor.setAttribute("download", fileName)
    anchor.click()

let ButtonForNewEvent dispatch eventType =
  Daisy.button.button [
    prop.onClick (fun _ -> dispatch (NewEvent eventType))
    prop.text (GetIconForEventType eventType)
  ]

let notImplementedModal (text:string) =
  Html.div [
    prop.style [style.display.inlineFlex]
    prop.children [
      Daisy.button.label [
          prop.htmlFor "my-modal"
          prop.text text
      ]
      Daisy.modalToggle [prop.id "my-modal"]
      Daisy.modal [
          prop.children [
              Daisy.modalBox [
                  Html.p "Not implemented yet"
                  Html.p "🚧 👨‍💻 Work in progress! 👨‍💻 🚧"
                  Daisy.modalAction [
                      Daisy.button.label [
                          prop.htmlFor "my-modal"
                          button.primary
                          prop.text "Close"
                      ]
                  ]
              ]
          ]
      ]
    ]
  ]

type EventForGraph = { time: string; pooCount: int; peeCount: int; playCount: int }

let data = [
  { time= "17:00"; pooCount= 2; playCount = 1; peeCount = 2 };
  { time= "18:00"; pooCount= 1; playCount = 1; peeCount = 2 };
  { time= "19:00"; pooCount= 2; playCount = 1; peeCount = 4 };
  { time= "20:00"; pooCount= 0; playCount = 3; peeCount = 2 };
  { time= "21:00"; pooCount= 0; playCount = 1; peeCount = 5 };
  { time= "22:00"; pooCount= 0; playCount = 1; peeCount = 2 };
]

[<ReactComponent>]
let charts() =
  Recharts.barChart [
        barChart.width 500
        barChart.height 300
        barChart.data data
        barChart.children [
            Recharts.cartesianGrid [ cartesianGrid.strokeDasharray(3, 3) ]
            Recharts.xAxis [ xAxis.dataKey (fun e -> e.time) ]
            Recharts.yAxis [ ]
            Recharts.tooltip [ tooltip.cursor false ]
            Recharts.legend [ legend.align.center ]
            Recharts.bar [
                bar.dataKey (fun e -> e.pooCount)
                bar.fill "#8884d8"
                bar.stackId "1"
                bar.name "Poo"
            ]
            Recharts.bar [
                bar.dataKey (fun e -> e.peeCount)
                bar.fill "#82ca9d"
                bar.stackId "1"
                bar.name "Pee"
            ]
            Recharts.bar [
                bar.dataKey (fun e -> e.playCount)
                bar.fill "#82c000"
                bar.stackId "1"
                bar.name "Play"
            ]
        ]
  ]


let init() =
  retrieveStateFromLocalStorage,
  Cmd.none

let update (msg: Msg) (state: State): State * Cmd<Msg> =
  match msg with
  | NewEvent eventType -> {state with Events=(getNewEventNow eventType)::state.Events}, Cmd.ofMsg SaveState
  | SaveState ->
      storeStateInLocalStorage state
      state, Cmd.none
  | DeleleteLastEvent -> {state with Events= (List.tail state.Events)}, Cmd.ofMsg SaveState
  | SwitchPresentationType presentationType -> {state with PresentationType=presentationType}, Cmd.none

let render (state: State) (dispatch: Msg -> unit) =
  Html.div [
    prop.style [
      style.alignItems.center
      style.display.flex
      style.flexDirection.column
      style.flexWrap.nowrap
      style.marginTop 10
    ]
    prop.children [

      //Event buttons
      Html.div [
        ButtonForNewEvent dispatch Poo
        ButtonForNewEvent dispatch Pee
        ButtonForNewEvent dispatch Play
        ButtonForNewEvent dispatch Eat
        ButtonForNewEvent dispatch Walk
        ButtonForNewEvent dispatch Sleep

        Daisy.button.button [
          prop.onClick (fun _ -> dispatch DeleleteLastEvent)
          prop.text ("❌ last")
        ]
      ]
      // Pup info
      Html.div [
        prop.classes [ "w-96" ]
        prop.style [ style.marginTop 10]
        prop.children [
          Daisy.card [
            prop.children [
              Html.figure [
                Html.img [
                  mask.circle
                  prop.src "lusiaC.png"
                  prop.onClick (fun _ -> Browser.Dom.window.alert("Try petting real Lusia 💛"))
                ]
              ]
              Daisy.cardBody [
                prop.style [ style.alignItems.center]
                prop.children [
                  Daisy.cardTitle (state.Pup.Name + " " + getEmojiForGender state.Pup.Gender)
                  Daisy.cardTitle (getFormattedDogAge state.Pup.Birthday)
                ]
              ]
            ]
          ]
        ]
      ]

      //Buttons for event handling logic
      Html.div [
        Daisy.button.button [
          prop.onClick (fun _ -> dispatch (SwitchPresentationType EventStack))
          prop.text ("📋 list")
        ]
        Daisy.button.button [
          prop.onClick (fun _ ->  dispatch (SwitchPresentationType Graph))
          prop.text ("📈 graph")
        ]
        Daisy.button.button [
          prop.onClick (fun _ -> downLoad "pooPeeTrainer.json" (Json.serialize state))
          prop.text ("💾 save")
        ]
        notImplementedModal "📤 upload"
      ]

      //Event presentation:
      Html.div [
        prop.children [
          if state.PresentationType = EventStack then
            Html.div [
              prop.style [style.marginTop 10]
              prop.children [
              for event in state.Events do
                Html.div [
                  Daisy.labelText (GetIconForEventType event.Type)
                  let formattedTime = event.Time.ToString("dd.MM HH:mm ")
                  Daisy.labelText ($" {formattedTime}")
                  Daisy.labelText (GetIconForEventType event.Type)
                ]
              ]
            ]
          else
            Html.div [
              charts()
              Daisy.labelText "🚧 👨‍💻 Work in progress! 👨‍💻 🚧"
            ]
        ]
      ]
    ]
  ]
