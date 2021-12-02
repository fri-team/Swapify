export function dayHourToString(day, hour)
{
    var days = ["Pondelok", "Utorok", "Streda", "Štvrtok", "Piatok", "Error"];
    return days[dayToInt(day)] + " " + hour + ":00";
}

function dayToInt(day)
{    
    switch (day) {
        case "Monday":
          return 0;          
        case "Tuesday":
         return 1;          
        case "Wednesday":
         return 2;
        case "Thursday":
         return 3;          
        case "Friday":
         return 4;
        default:
         return 5;          
      }   
}