module GraphHelper
open Feliz
open Logic
open Feliz.Recharts

type EventForGraph = {
  time: string;
  pooCount: int; peeCount: int; playCount: int;
  walkCount: int; eatCount: int; sleepCount: int;
}

let private groupByTheHour =
  List.groupBy (fun e -> e.Time.Hour)

let private countSpecificType eventType =
  List.filter (fun e -> e.Type = eventType)
  >> List.length

let private countTypesForEveryHour =
  groupByTheHour
  >> List.sortBy (fun (h, _) -> h)
  >> List.map (fun (h, l) ->
      {
        time=sprintf "%d:00" h;
        pooCount=(countSpecificType Poo l);
        peeCount=(countSpecificType Pee l);
        playCount=(countSpecificType Play l);
        walkCount=(countSpecificType Walk l);
        sleepCount=(countSpecificType Sleep l);
        eatCount=(countSpecificType Eat l);
      })

let rechartBarReusable (hexColor, barName, selectorFunction:EventForGraph->int) =
  Recharts.bar [
    bar.dataKey selectorFunction
    bar.fill hexColor
    bar.stackId "1"
    bar.name barName
  ]

[<ReactComponent>]
let Charts(eventList : Event list) =
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
      rechartBarReusable("#D3A625", "Poo", (fun e -> e.pooCount))
      rechartBarReusable("#AE0001", "Eat", (fun e -> e.eatCount))
    ]
  ]