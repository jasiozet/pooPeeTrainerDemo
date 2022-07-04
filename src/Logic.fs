module Logic
open System

type Gender = Male | Female

type Pup = {
    Name : string
    Gender : Gender
    Birthday : DateOnly
}

type EventType = Poo | Pee | Play | Eat | Sleep | Walk

type Event = {
    Type : EventType
    Time : DateTime
}

let GetIconForEventType eventType =
    match eventType with
    | Poo -> "💩"
    | Pee -> "💦"
    | Play -> "🥎"
    | Eat -> "🍖"
    | Sleep -> "💤"
    | Walk -> "🦮"

let private getDogAgeInDays (currentDate:DateOnly) (birthday:DateOnly) =
  currentDate.DayNumber - birthday.DayNumber

let private getDogAge birthday =
  let now = DateTime.Now
  let currentDate = DateOnly(now.Year, now.Month, now.Day)
  getDogAgeInDays currentDate birthday

let GetFormattedDogAge birthday =
  let daysTotal = getDogAge birthday
  let weeks = daysTotal / 7
  let days = daysTotal % 7
  $"{weeks} weeks and {days} days old"

let GetNewEventNow eventType =
    {Time=DateTime.Now;Type=eventType}

let GetEmojiForGender gender =
  match gender with
  | Male -> "♂️"
  | Female -> "♀️"