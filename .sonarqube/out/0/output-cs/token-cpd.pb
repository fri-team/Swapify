È
2C:\Projects\Swapify\APIWrapper\Enums\LessonType.cs
	namespace 	
FRITeam
 
. 
Swapify 
. 

APIWrapper $
.$ %
Enums% *
{ 
public 

enum 

LessonType 
{ 
None 
= 
$num 
, 

Laboratory 
= 
$num 
, 
	Excercise 
= 
$num 
, 
Lecture 
= 
$num 
}		 
}

 Ê
3C:\Projects\Swapify\APIWrapper\Enums\SubjectType.cs
	namespace 	
FRITeam
 
. 
Swapify 
. 

APIWrapper $
.$ %
Enums% *
{ 
public 

enum 
SubjectType 
{ 
None 
= 
$num 
, 

Compulsory 
= 
$num 
, 
Optional 
= 
$num 
, 
Elective 
= 
$num 
}		 
}

 š
6C:\Projects\Swapify\APIWrapper\ISchoolScheduleProxy.cs
	namespace 	
FRITeam
 
. 
Swapify 
. 

APIWrapper $
{ 
public 

	interface  
ISchoolScheduleProxy )
{ 
ScheduleWeekContent 
GetByStudyGroup +
(+ ,
string, 2
studyGroupNumber3 C
)C D
;D E
ScheduleWeekContent 
GetByTeacherName ,
(, -
string- 3
teacherNumber4 A
)A B
;B C
ScheduleWeekContent 
GetByRoomNumber +
(+ ,
string, 2

roomNumber3 =
)= >
;> ?
ScheduleWeekContent 
GetBySubjectCode ,
(, -
string- 3
subjectCode4 ?
)? @
;@ A
} 
} ˆ
<C:\Projects\Swapify\APIWrapper\Objects\ScheduleDayContent.cs
	namespace 	
FRITeam
 
. 
Swapify 
. 

APIWrapper $
.$ %
Objects% ,
{ 
public 

class 
ScheduleDayContent #
{ 
public

 
readonly

 
List

 
<

 
ScheduleHourContent

 0
>

0 1
BlocksInDay

2 =
=

> ?
new

@ C
List

D H
<

H I
ScheduleHourContent

I \
>

\ ]
(

] ^
)

^ _
;

_ `
} 
} ß 
=C:\Projects\Swapify\APIWrapper\Objects\ScheduleHourContent.cs
	namespace 	
FRITeam
 
. 
Swapify 
. 

APIWrapper $
.$ %
Objects% ,
{ 
public 

class 
ScheduleHourContent $
{ 
public 
int 
BlockNumber 
{  
get! $
;$ %
}& '
public 
bool 
	IsBlocked 
{ 
get  #
;# $
}% &
public 

LessonType 

LessonType $
{% &
get' *
;* +
}, -
public 
string 
TeacherName !
{" #
get$ '
;' (
}) *
public!! 
string!! 
RoomName!! 
{!!  
get!!! $
;!!$ %
}!!& '
public&& 
string&& 
CourseShortcut&& $
{&&% &
get&&' *
;&&* +
}&&, -
public++ 
string++ 

CourseName++  
{++! "
get++# &
;++& '
}++( )
public00 
HashSet00 
<00 
string00 
>00 
StudyGroups00 *
{00+ ,
get00- 0
;000 1
}002 3
public55 
SubjectType55 
SubjectType55 &
{55' (
get55) ,
;55, -
}55. /
public77 
ScheduleHourContent77 "
(77" #
int77# &
blockNumber77' 2
,772 3
bool774 8
	isBlocked779 B
,77B C

LessonType77D N

lessonType77O Y
,77Y Z
string88 
teacherName88 
,88 
string88  &
roomName88' /
,88/ 0
string881 7
subjectShortcut888 G
,88G H
string88I O
subjectName88P [
,88[ \
SubjectType99 
subjectType99 #
,99# $
List99% )
<99) *
string99* 0
>990 1
studyGroups992 =
)99= >
{:: 	
BlockNumber;; 
=;; 
blockNumber;; %
;;;% &
	IsBlocked<< 
=<< 
	isBlocked<< !
;<<! "

LessonType== 
=== 

lessonType== #
;==# $
TeacherName>> 
=>> 
teacherName>> %
;>>% &
RoomName?? 
=?? 
roomName?? 
;??  
CourseShortcut@@ 
=@@ 
subjectShortcut@@ ,
;@@, -

CourseNameAA 
=AA 
subjectNameAA $
;AA$ %
SubjectTypeBB 
=BB 
subjectTypeBB %
;BB% &
StudyGroupsCC 
=CC 
newCC 
HashSetCC %
<CC% &
stringCC& ,
>CC, -
(CC- .
)CC. /
;CC/ 0
StudyGroupsDD 
.DD 
	UnionWithDD !
(DD! "
studyGroupsDD" -
)DD- .
;DD. /
}EE 	
publicGG 
boolGG 
IsSameBlockAsGG !
(GG! "
ScheduleHourContentGG" 5
b2GG6 8
)GG8 9
{HH 	
returnII 
(II 

CourseNameII 
==II !
b2II" $
?II$ %
.II% &

CourseNameII& 0
)II0 1
&&II2 4
(JJ 
TeacherNameJJ  
==JJ! #
b2JJ$ &
?JJ& '
.JJ' (
TeacherNameJJ( 3
)JJ3 4
&&JJ5 7
(KK 
RoomNameKK 
==KK  
b2KK! #
?KK# $
.KK$ %
RoomNameKK% -
)KK- .
&&KK/ 1
(LL 

LessonTypeLL 
==LL  "
b2LL# %
?LL% &
.LL& '

LessonTypeLL' 1
)LL1 2
&&LL3 5
(MM 
StudyGroupsMM  
.MM  !
	SetEqualsMM! *
(MM* +
b2MM+ -
?MM- .
.MM. /
StudyGroupsMM/ :
)MM: ;
)MM; <
;MM< =
}NN 	
}OO 
}PP Ð
=C:\Projects\Swapify\APIWrapper\Objects\ScheduleWeekContent.cs
	namespace 	
FRITeam
 
. 
Swapify 
. 

APIWrapper $
.$ %
Objects% ,
{ 
public 

class 
ScheduleWeekContent $
{ 
public

 
readonly

 
List

 
<

 
ScheduleDayContent

 /
>

/ 0

DaysInWeek

1 ;
=

< =
new

> A
List

B F
<

F G
ScheduleDayContent

G Y
>

Y Z
(

Z [
)

[ \
;

\ ]
public 
static 
ScheduleWeekContent )
Build* /
(/ 0
)0 1
{ 	
var 
weekTimetable 
= 
new  #
ScheduleWeekContent$ 7
(7 8
)8 9
;9 :
for 
( 
int 
i 
= 
$num 
; 
i 
< 
$num  !
;! "
i# $
++$ &
)& '
{ 
weekTimetable 
. 

DaysInWeek (
.( )
Add) ,
(, -
new- 0
ScheduleDayContent1 C
(C D
)D E
)E F
;F G
} 
return 
weekTimetable  
;  !
} 	
} 
} Í/
0C:\Projects\Swapify\APIWrapper\ResponseParser.cs
	namespace

 	
FRITeam


 
.

 
Swapify

 
.

 

APIWrapper

 $
{ 
public 

static 
class 
ResponseParser &
{ 
private 
static 
readonly 
Logger  &
_logger' .
=/ 0

LogManager1 ;
.; <!
GetCurrentClassLogger< Q
(Q R
)R S
;S T
public 
static 
ScheduleWeekContent )
ParseResponse* 7
(7 8
string8 >

myResponse? I
)I J
{ 	
var 
response 
= 
JObject "
." #
Parse# (
(( )

myResponse) 3
)3 4
;4 5
var 
report 
= 
response !
[! "
$str" *
]* +
.+ ,
ToString, 4
(4 5
)5 6
;6 7
if 
( 
! 
string 
. 
IsNullOrWhiteSpace *
(* +
report+ 1
)1 2
)2 3
{ 
var 
ex 
= 
new 
ArgumentException .
(. /
report/ 5
)5 6
;6 7
_logger 
. 
Error 
( 
ex  
)  !
;! "
throw 
ex 
; 
} 
var 
scheduleContent 
=  !
(" #
JArray# )
)) *
response* 2
[2 3
$str3 D
]D E
;E F
var 
weekTimetable 
= 
ScheduleWeekContent  3
.3 4
Build4 9
(9 :
): ;
;; <
foreach!! 
(!! 
var!! 
block!! 
in!! !
scheduleContent!!" 1
)!!1 2
{"" 
try## 
{$$ 
{%% 

LessonType&& "

lessonType&&# -
=&&. /
ConvertLessonType&&0 A
(&&A B
block&&B G
[&&G H
$str&&H L
]&&L M
.&&M N
ToString&&N V
(&&V W
)&&W X
[&&X Y
$num&&Y Z
]&&Z [
)&&[ \
;&&\ ]
string'' 
teacherName'' *
=''+ ,
block''- 2
[''2 3
$str''3 6
]''6 7
.''7 8
ToString''8 @
(''@ A
)''A B
;''B C
string(( 
roomName(( '
=((( )
block((* /
[((/ 0
$str((0 3
]((3 4
.((4 5
ToString((5 =
(((= >
)((> ?
;((? @
string)) 
subjectShortcut)) .
=))/ 0
block))1 6
[))6 7
$str))7 :
])): ;
.)); <
ToString))< D
())D E
)))E F
;))F G
string** 
subjectName** *
=**+ ,
block**- 2
[**2 3
$str**3 6
]**6 7
.**7 8
ToString**8 @
(**@ A
)**A B
;**B C
var++ 
sc++ 
=++  
new++! $
ScheduleHourContent++% 8
(++8 9
int++9 <
.++< =
Parse++= B
(++B C
block++C H
[++H I
$str++I L
]++L M
.++M N
ToString++N V
(++V W
)++W X
)++X Y
,++Y Z
false++[ `
,++` a

lessonType,,9 C
,,,C D
teacherName,,E P
,,,P Q
roomName,,R Z
,,,Z [
subjectShortcut,,\ k
,,,k l
subjectName--9 D
,--D E
SubjectType--F Q
.--Q R
None--R V
,--V W
new--X [
List--\ `
<--` a
string--a g
>--g h
(--h i
)--i j
)--j k
;--k l
weekTimetable// %
.//% &

DaysInWeek//& 0
[//0 1
int//1 4
.//4 5
Parse//5 :
(//: ;
block//; @
[//@ A
$str//A E
]//E F
.//F G
ToString//G O
(//O P
)//P Q
)//Q R
-//S T
$num//U V
]//V W
.//W X
BlocksInDay//X c
.//c d
Add//d g
(//g h
sc//h j
)//j k
;//k l
}00 
}11 
catch22 
(22 
	Exception22  
ex22! #
)22# $
{33 
_logger44 
.44 
Error44 !
(44! "
ex44" $
)44$ %
;44% &
throw55 
;55 
}66 
}77 
return88 
weekTimetable88  
;88  !
}99 	
private;; 
static;; 

LessonType;; !
ConvertLessonType;;" 3
(;;3 4
char;;4 8
lessonShortcutType;;9 K
);;K L
{<< 	
switch== 
(== 
lessonShortcutType== &
)==& '
{>> 
case?? 
$char?? 
:?? 
return??  

LessonType??! +
.??+ ,

Laboratory??, 6
;??6 7
case@@ 
$char@@ 
:@@ 
return@@  

LessonType@@! +
.@@+ ,
Lecture@@, 3
;@@3 4
caseAA 
$charAA 
:AA 
returnAA  

LessonTypeAA! +
.AA+ ,
	ExcerciseAA, 5
;AA5 6
defaultBB 
:BB 
throwBB 
newBB "
ArgumentExceptionBB# 4
(BB4 5
$"BB5 7$
Unexpected lesson type 'BB7 O
{BBO P
lessonShortcutTypeBBP b
}BBb c
'BBc d
"BBd e
)BBe f
;BBf g
}CC 
}DD 	
}EE 
}FF ‹%
5C:\Projects\Swapify\APIWrapper\SchoolScheduleProxy.cs
	namespace 	
FRITeam
 
. 
Swapify 
. 

APIWrapper $
{ 
public 

class 
SchoolScheduleProxy $
:% & 
ISchoolScheduleProxy' ;
{ 
private 
const 
string 
URL  
=! "
$str# E
;E F
private 
const 
string  
SCHEDULE_CONTENT_URL 1
=2 3
$str4 Q
;Q R
private 
static 
readonly 
Logger  &
_logger' .
=/ 0

LogManager1 ;
.; <!
GetCurrentClassLogger< Q
(Q R
)R S
;S T
public 
ScheduleWeekContent "
GetByTeacherName# 3
(3 4
string4 :
teacherNumber; H
)H I
{ 	
return "
CallScheduleContentApi )
() *
$num* +
,+ ,
teacherNumber- :
): ;
;; <
} 	
public 
ScheduleWeekContent "
GetByStudyGroup# 2
(2 3
string3 9
studyGroupNumber: J
)J K
{ 	
return "
CallScheduleContentApi )
() *
$num* +
,+ ,
studyGroupNumber- =
)= >
;> ?
} 	
public 
ScheduleWeekContent "
GetByRoomNumber# 2
(2 3
string3 9

roomNumber: D
)D E
{ 	
return "
CallScheduleContentApi )
() *
$num* +
,+ ,

roomNumber- 7
)7 8
;8 9
}   	
public"" 
ScheduleWeekContent"" "
GetBySubjectCode""# 3
(""3 4
string""4 :
subjectCode""; F
)""F G
{## 	
return$$ "
CallScheduleContentApi$$ )
($$) *
$num$$* +
,$$+ ,
subjectCode$$- 8
)$$8 9
;$$9 :
}%% 	
private'' 
ScheduleWeekContent'' #"
CallScheduleContentApi''$ :
('': ;
int''; >
type''? C
,''C D
string''E K
requestContent''L Z
)''Z [
{(( 	
var)) 
address)) 
=)) 
$")) 
{)) 
URL))  
}))  !
/))! "
{))" # 
SCHEDULE_CONTENT_URL))# 7
}))7 8
?m=))8 ;
{)); <
type))< @
}))@ A
&id=))A E
{))E F
Uri))F I
.))I J
EscapeUriString))J Y
())Y Z
requestContent))Z h
)))h i
}))i j
"))j k
;))k l
var** 

myResponse** 
=** 
$str** 
;**  
try++ 
{,, 
var-- 
request-- 
=-- 
(-- 
HttpWebRequest-- -
)--- .

WebRequest--. 8
.--8 9
Create--9 ?
(--? @
address--@ G
)--G H
;--H I
request.. 
... 
Method.. 
=..  
$str..! &
;..& '
request// 
.// 
	KeepAlive// !
=//" #
true//$ (
;//( )
request00 
.00 
ContentType00 #
=00$ %
$str00& I
;00I J
var22 
response22 
=22 
(22  
HttpWebResponse22  /
)22/ 0
request220 7
.227 8
GetResponse228 C
(22C D
)22D E
;22E F
using44 
(44 
var44 
sr44 
=44 
new44  #
StreamReader44$ 0
(440 1
response441 9
.449 :
GetResponseStream44: K
(44K L
)44L M
)44M N
)44N O
{55 

myResponse66 
=66  
sr66! #
.66# $
	ReadToEnd66$ -
(66- .
)66. /
;66/ 0
}77 
}88 
catch99 
(99 
	Exception99 
ex99 
)99  
{:: 
_logger;; 
.;; 
Error;; 
(;; 
ex;;  
);;  !
;;;! "
throw<< 
ex<< 
;<< 
}== 
return>> 
ResponseParser>> !
.>>! "
ParseResponse>>" /
(>>/ 0

myResponse>>0 :
)>>: ;
;>>; <
}?? 	
}AA 
}CC 