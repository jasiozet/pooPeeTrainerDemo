module Logic
open System

type Gender = Male | Female

type Pup = {
    Name : string
    Gender : Gender
    Birthday : DateOnly
}

type EventType = Poo | Pee | PooPee | Play | Eat | Sleep | Walk

type Event = {
    Type : EventType
    Time : DateTime
}

let GetIconForEventType eventType =
    match eventType with
    | Poo -> "💩"
    | Pee -> "💦"
    | PooPee -> "💩💦"
    | Play -> "🥎"
    | Eat -> "🍖"
    | Sleep -> "💤"
    | Walk -> "🦮"
