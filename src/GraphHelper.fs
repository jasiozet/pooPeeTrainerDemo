module GraphHelper
open Feliz
open Logic
open System
open Feliz.Recharts

type EventForGraph = {
  time: string;
  pooCount: int; peeCount: int; playCount: int;
  walkCount: int; eatCount: int; sleepCount: int;
}

let groupByTheHour eventList =
  eventList
  |> List.groupBy (fun e -> e.Time.Hour)

let countSpecificType eventType eventList =
  eventList
  |> List.filter (fun e -> e.Type = eventType)
  |> List.length

let countTypesForEveryHour eventList =
  eventList
  |> groupByTheHour
  |> List.map (fun (h,l) ->
      {
        time=sprintf "%d:00" h;
        pooCount=(countSpecificType Poo l);
        peeCount=(countSpecificType Pee l);
        playCount=(countSpecificType Play l);
        walkCount=(countSpecificType Walk l);
        sleepCount=(countSpecificType Sleep l);
        eatCount=(countSpecificType Eat l);
      })

[<ReactComponent>]
let charts(eventList : Event list) =
  Recharts.barChart [
    barChart.width 500
    barChart.height 300
    barChart.data (countTypesForEveryHour eventList)
    barChart.children [
      Recharts.cartesianGrid [ cartesianGrid.strokeDasharray(3, 3) ]
      Recharts.xAxis [ xAxis.dataKey (fun e -> e.time) ]
      Recharts.yAxis [ ]
      Recharts.tooltip [ tooltip.cursor false ]
      Recharts.legend [ legend.align.center ]
      Recharts.bar [
          bar.dataKey (fun e -> e.pooCount)
          bar.fill "#A06528"
          bar.stackId "1"
          bar.name "Poo"
      ]
      Recharts.bar [
          bar.dataKey (fun e -> e.peeCount)
          bar.fill "#BAE1FF"
          bar.stackId "1"
          bar.name "Pee"
      ]
      Recharts.bar [
          bar.dataKey (fun e -> e.playCount)
          bar.fill "#FFB3BA"
          bar.stackId "1"
          bar.name "Play"
      ]
      Recharts.bar [
          bar.dataKey (fun e -> e.walkCount)
          bar.fill "#BAFFC9"
          bar.stackId "1"
          bar.name "Walk"
      ]
      Recharts.bar [
          bar.dataKey (fun e -> e.eatCount)
          bar.fill "#D11141"
          bar.stackId "1"
          bar.name "Eat"
      ]
      Recharts.bar [
          bar.dataKey (fun e -> e.sleepCount)
          bar.fill "#011F4B"
          bar.stackId "1"
          bar.name "Sleep"
      ]
    ]
  ]