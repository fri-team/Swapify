export function dayHourToString(day, hour)
{
    var days = ["Pondelok", "Utorok", "Streda", "Štvrtok", "Piatok"];
    return days[day - 1] + " " + hour + ":00";
}