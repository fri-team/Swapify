≤G
2C:\Projects\Swapify\Backend\BlockChangesService.cs
	namespace		 	
FRITeam		
 
.		 
Swapify		 
.		 
Backend		 !
{

 
public 

class 
BlockChangesService $
:% & 
IBlockChangesService' ;
{ 
private 
readonly 
IMongoCollection )
<) *
BlockChangeRequest* <
>< =#
_blockChangesCollection> U
;U V
public 
BlockChangesService "
(" #
IMongoDatabase# 1
database2 :
): ;
{ 	#
_blockChangesCollection #
=$ %
database& .
.. /
GetCollection/ <
<< =
BlockChangeRequest= O
>O P
(P Q
nameofQ W
(W X
BlockChangeRequestX j
)j k
)k l
;l m
} 	
public 
async 
Task 
< 
bool 
> 
AddAndFindMatch  /
(/ 0
BlockChangeRequest0 B
entityToAddC N
)N O
{ 	
await 
AddAsync 
( 
entityToAdd &
)& '
;' (
return 
await )
MakeExchangeAndDeleteRequests 6
(6 7
entityToAdd7 B
)B C
;C D
} 	
public 
Task 
< 
List 
< 
BlockChangeRequest +
>+ ,
>, -"
FindAllStudentRequests. D
(D E
GuidE I
	studentIdJ S
)S T
{ 	
return #
_blockChangesCollection *
.* +
Find+ /
(/ 0
x0 1
=>2 4
x5 6
.6 7
	StudentId7 @
==A C
	studentIdD M
)M N
.N O
ToListAsyncO Z
(Z [
)[ \
;\ ]
} 	
private   
async   
Task   
AddAsync   #
(  # $
BlockChangeRequest  $ 6
entityToAdd  7 B
)  B C
{!! 	
entityToAdd"" 
."" 
Id"" 
="" 
Guid"" !
.""! "
NewGuid""" )
("") *
)""* +
;""+ ,
await## #
_blockChangesCollection## )
.##) *
InsertOneAsync##* 8
(##8 9
entityToAdd##9 D
)##D E
;##E F
}$$ 	
private&& 
async&& 
Task&& 
<&& 
BlockChangeRequest&& -
>&&- .
FindExchange&&/ ;
(&&; <
BlockChangeRequest&&< N
blockRequest&&O [
)&&[ \
{'' 	
return(( 
await(( #
_blockChangesCollection(( 0
.((0 1
Find((1 5
(((5 6
x)) 
=>)) 
()) 
x)) 
.)) 
BlockTo)) 
.))  
CourseId))  (
==))) +
blockRequest)), 8
.))8 9
	BlockFrom))9 B
.))B C
CourseId))C K
&&))L N
x** 
.** 
BlockTo** 
.**  
Day**  #
==**$ &
blockRequest**' 3
.**3 4
	BlockFrom**4 =
.**= >
Day**> A
&&**B D
x++ 
.++ 
BlockTo++ 
.++  
Duration++  (
==++) +
blockRequest++, 8
.++8 9
	BlockFrom++9 B
.++B C
Duration++C K
&&++L N
x,, 
.,, 
BlockTo,, 
.,,  
	StartHour,,  )
==,,* ,
blockRequest,,- 9
.,,9 :
	BlockFrom,,: C
.,,C D
	StartHour,,D M
&&,,N P
x-- 
.-- 
	BlockFrom-- !
.--! "
CourseId--" *
==--+ -
blockRequest--. :
.--: ;
BlockTo--; B
.--B C
CourseId--C K
&&--L N
x.. 
... 
	BlockFrom.. !
...! "
Day.." %
==..& (
blockRequest..) 5
...5 6
BlockTo..6 =
...= >
Day..> A
&&..B D
x// 
.// 
	BlockFrom// !
.//! "
Duration//" *
==//+ -
blockRequest//. :
.//: ;
BlockTo//; B
.//B C
Duration//C K
&&//L N
x00 
.00 
	BlockFrom00 !
.00! "
	StartHour00" +
==00, .
blockRequest00/ ;
.00; <
BlockTo00< C
.00C D
	StartHour00D M
&&00N P
x11 
.11 
	StudentId11 !
!=11" $
blockRequest11% 1
.111 2
	StudentId112 ;
&&11< >
x22 
.22 
Status22 
!=22 !
ExchangeStatus22" 0
.220 1
Done221 5
)225 6
)226 7
.227 8
SortBy228 >
(22> ?
x22? @
=>22A C
x22D E
.22E F
DateOfCreation22F T
)22T U
.22U V
FirstOrDefaultAsync22V i
(22i j
)22j k
;22k l
}33 	
private55 
async55 
Task55 
<55 
bool55 
>55  )
MakeExchangeAndDeleteRequests55! >
(55> ?
BlockChangeRequest55? Q
request55R Y
)55Y Z
{66 	
var77 
requestForExchange77 "
=77# $
await77% *
FindExchange77+ 7
(777 8
request778 ?
)77? @
;77@ A
if88 
(88 
requestForExchange88 "
==88# %
null88& *
)88* +
{99 
return:: 
false:: 
;:: 
};; 
await<< 
SetDoneStatus<< 
(<<  
request<<  '
)<<' (
;<<( )
await== 
SetDoneStatus== 
(==  
requestForExchange==  2
)==2 3
;==3 4
await>> !
RemoveStudentRequests>> '
(>>' (
request>>( /
)>>/ 0
;>>0 1
await?? !
RemoveStudentRequests?? '
(??' (
requestForExchange??( :
)??: ;
;??; <
return@@ 
true@@ 
;@@ 
}AA 	
privateCC 
asyncCC 
TaskCC !
RemoveStudentRequestsCC 0
(CC0 1
BlockChangeRequestCC1 C
requestCCD K
)CCK L
{DD 	
awaitEE #
_blockChangesCollectionEE )
.EE) *
DeleteManyAsyncEE* 9
(EE9 :
xEE: ;
=>EE< >
xEE? @
.EE@ A
	StudentIdEEA J
==EEK M
requestEEN U
.EEU V
	StudentIdEEV _
&&EE` b
xFF5 6
.FF6 7
	BlockFromFF7 @
.FF@ A
CourseIdFFA I
==FFJ L
requestFFM T
.FFT U
	BlockFromFFU ^
.FF^ _
CourseIdFF_ g
&&FFh j
xGG5 6
.GG6 7
	BlockFromGG7 @
.GG@ A
	StartHourGGA J
==GGK M
requestGGN U
.GGU V
	BlockFromGGV _
.GG_ `
	StartHourGG` i
&&GGj l
xHH5 6
.HH6 7
	BlockFromHH7 @
.HH@ A
DayHHA D
==HHE G
requestHHH O
.HHO P
	BlockFromHHP Y
.HHY Z
DayHHZ ]
&&HH^ `
xII5 6
.II6 7
	BlockFromII7 @
.II@ A
DurationIIA I
==IIJ L
requestIIM T
.IIT U
	BlockFromIIU ^
.II^ _
DurationII_ g
)IIg h
;IIh i
}JJ 	
privateLL 
asyncLL 
TaskLL 
SetDoneStatusLL (
(LL( )
BlockChangeRequestLL) ;
requestLL< C
)LLC D
{MM 	
ifNN 
(NN 
requestNN 
.NN 
StatusNN 
==NN !
ExchangeStatusNN" 0
.NN0 1
DoneNN1 5
)NN5 6
{OO 
awaitPP 
newPP 
TaskPP 
(PP 
(PP  
)PP  !
=>PP" $
{PP% &
throwPP' ,
newPP- 0
ArgumentExceptionPP1 B
(PPB C
$strPPC ~
)PP~ 
;	PP Ä
}
PPÅ Ç
)
PPÇ É
;
PPÉ Ñ
}QQ 
requestRR 
.RR 
StatusRR 
=RR 
ExchangeStatusRR +
.RR+ ,
DoneRR, 0
;RR0 1
awaitSS #
_blockChangesCollectionSS )
.SS) *
ReplaceOneAsyncSS* 9
(SS9 :
xSS: ;
=>SS< >
xSS? @
.SS@ A
IdSSA C
==SSD F
requestSSG N
.SSN O
IdSSO Q
,SSQ R
requestSSS Z
)SSZ [
;SS[ \
}TT 	
}UU 
}VV ¸O
=C:\Projects\Swapify\Backend\Converter\ConverterApiToDomain.cs
	namespace		 	
FRITeam		
 
.		 
Swapify		 
.		 
Backend		 !
.		! "
	Converter		" +
{

 
public 

static 
class  
ConverterApiToDomain ,
{ 
public 
static 
async 
Task  
<  !
	Timetable! *
>* +)
ConvertTimetableForGroupAsync, I
(I J
ScheduleWeekContentJ ]
groupTimetable^ l
,l m
ICourseServicen |

courseServ	} á
)
á à
{ 	
return 
await !
ConvertTimetableAsync .
(. /
groupTimetable/ =
,= >

courseServ? I
,I J
falseK P
)P Q
;Q R
} 	
public 
static 
async 
Task  
<  !
	Timetable! *
>* +*
ConvertTimetableForCourseAsync, J
(J K
ScheduleWeekContentK ^
courseTimetable_ n
,n o
ICourseServicep ~

courseServ	 â
)
â ä
{ 	
return 
await !
ConvertTimetableAsync .
(. /
courseTimetable/ >
,> ?

courseServ@ J
,J K
trueL P
)P Q
;Q R
} 	
private 
static 
async 
Task !
<! "
	Timetable" +
>+ ,!
ConvertTimetableAsync- B
(B C
ScheduleWeekContentC V
scheduleW _
,_ `
ICourseServicea o

courseServp z
,z {
bool	| Ä"
isTimetableForCourse
Å ï
)
ï ñ
{ 	
	Timetable 
	timetable 
=  !
new" %
	Timetable& /
(/ 0
)0 1
;1 2
for 
( 
int 
idxDay 
= 
$num 
;  
idxDay! '
<( )
schedule* 2
.2 3

DaysInWeek3 =
.= >
Count> C
;C D
idxDayE K
++K M
)M N
{ 
var 
	maxBlocks 
= 
schedule  (
.( )

DaysInWeek) 3
[3 4
idxDay4 :
]: ;
.; <
BlocksInDay< G
.G H
CountH M
;M N
byte   
startingBlock   "
=  # $
$num  % &
;  & '
for!! 
(!! 
int!! 
blckIdx!!  
=!!! "
$num!!# $
;!!$ %
blckIdx!!& -
<!!. /
	maxBlocks!!0 9
;!!9 :
blckIdx!!; B
++!!B D
)!!D E
{"" 
var## 
blockBefore## #
=##$ %
schedule##& .
.##. /

DaysInWeek##/ 9
[##9 :
idxDay##: @
]##@ A
.##A B
BlocksInDay##B M
[##M N
blckIdx##N U
-##V W
$num##X Y
]##Y Z
;##Z [
var$$ 
block$$ 
=$$ 
schedule$$  (
.$$( )

DaysInWeek$$) 3
[$$3 4
idxDay$$4 :
]$$: ;
.$$; <
BlocksInDay$$< G
[$$G H
blckIdx$$H O
]$$O P
;$$P Q
if%% 
(%% 
blockBefore%% #
==%%$ &
null%%' +
)%%+ ,
{&& 
startingBlock'' %
=''& '
(''( )
byte'') -
)''- .
blckIdx''. 5
;''5 6
if)) 
()) 
block)) !
!=))" $
null))% )
&&))* ,
blckIdx))- 4
==))5 7
	maxBlocks))8 A
-))B C
$num))D E
)))E F
{** 
var++ 
bl++  "
=++# $
new++% (
Block++) .
(++. /
)++/ 0
{,, 
	BlockType--  )
=--* +
ConvertToBlockType--, >
(--> ?
block--? D
.--D E

LessonType--E O
)--O P
,--P Q
Day..  #
=..$ %
ConvertToDay..& 2
(..2 3
idxDay..3 9
)..9 :
,..: ;
Teacher//  '
=//( )
block//* /
./// 0
TeacherName//0 ;
,//; <
Room00  $
=00% &
block00' ,
.00, -
RoomName00- 5
,005 6
	StartHour11  )
=11* +
(11, -
byte11- 1
)111 2
(112 3
schedule113 ;
.11; <

DaysInWeek11< F
[11F G
idxDay11G M
]11M N
.11N O
BlocksInDay11O Z
[11Z [
blckIdx11[ b
]11b c
.11c d
BlockNumber11d o
+11p q
$num11r s
)11s t
,11t u
Duration22  (
=22) *
$num22+ ,
}33 
;33 
if44 
(44  
!44  ! 
isTimetableForCourse44! 5
)445 6
{55 
bl66  "
.66" #
CourseId66# +
=66, -
await66. 3

courseServ664 >
.66> ?%
GetOrAddNotExistsCourseId66? X
(66X Y
block66Y ^
.66^ _

CourseName66_ i
,66i j
bl66k m
)66m n
;66n o
}77 
	timetable88 %
.88% &
AddNewBlock88& 1
(881 2
bl882 4
)884 5
;885 6
}99 
continue::  
;::  !
};; 
if<< 
(<< 
!<< 
blockBefore<< $
.<<$ %
IsSameBlockAs<<% 2
(<<2 3
block<<3 8
)<<8 9
)<<9 :
{== 
var>> 
bl>> 
=>>  
new>>! $
Block>>% *
(>>* +
)>>+ ,
{?? 
	BlockType@@ %
=@@& '
ConvertToBlockType@@( :
(@@: ;
blockBefore@@; F
.@@F G

LessonType@@G Q
)@@Q R
,@@R S
DayAA 
=AA  !
ConvertToDayAA" .
(AA. /
idxDayAA/ 5
)AA5 6
,AA6 7
TeacherBB #
=BB$ %
blockBeforeBB& 1
.BB1 2
TeacherNameBB2 =
,BB= >
RoomCC  
=CC! "
blockBeforeCC# .
.CC. /
RoomNameCC/ 7
,CC7 8
	StartHourDD %
=DD& '
(DD( )
byteDD) -
)DD- .
(DD. /
scheduleDD/ 7
.DD7 8

DaysInWeekDD8 B
[DDB C
idxDayDDC I
]DDI J
.DDJ K
BlocksInDayDDK V
[DDV W
startingBlockDDW d
]DDd e
.DDe f
BlockNumberDDf q
+DDr s
$numDDt u
)DDu v
,DDv w
DurationEE $
=EE% &
(EE' (
byteEE( ,
)EE, -
(EE- .
blckIdxEE. 5
-EE6 7
startingBlockEE8 E
)EEE F
}FF 
;FF 
ifGG 
(GG 
!GG  
isTimetableForCourseGG 1
)GG1 2
{HH 
blII 
.II 
CourseIdII '
=II( )
awaitII* /

courseServII0 :
.II: ;%
GetOrAddNotExistsCourseIdII; T
(IIT U
blockBeforeIIU `
.II` a

CourseNameIIa k
,IIk l
blIIm o
)IIo p
;IIp q
}JJ 
	timetableLL !
.LL! "
AddNewBlockLL" -
(LL- .
blLL. 0
)LL0 1
;LL1 2
startingBlockMM %
=MM& '
(MM( )
byteMM) -
)MM- .
blckIdxMM. 5
;MM5 6
}NN 
}OO 
}PP 
returnQQ 
	timetableQQ 
;QQ 
}RR 	
privateTT 
staticTT 
DayTT 
ConvertToDayTT '
(TT' (
intTT( +
idxDayTT, 2
)TT2 3
{UU 	
switchVV 
(VV 
idxDayVV 
)VV 
{WW 
caseXX 
$numXX 
:XX 
returnYY 
DayYY 
.YY 
MondayYY %
;YY% &
caseZZ 
$numZZ 
:ZZ 
return[[ 
Day[[ 
.[[ 
Tuesday[[ &
;[[& '
case\\ 
$num\\ 
:\\ 
return]] 
Day]] 
.]] 
	Wednesday]] (
;]]( )
case^^ 
$num^^ 
:^^ 
return__ 
Day__ 
.__ 
Thursday__ '
;__' (
case`` 
$num`` 
:`` 
returnaa 
Dayaa 
.aa 
Fridayaa %
;aa% &
defaultbb 
:bb 
throwcc 
newcc 
	Exceptioncc '
(cc' (
$strcc( 4
)cc4 5
;cc5 6
}dd 
}ee 	
privategg 
staticgg 
	BlockTypegg  
ConvertToBlockTypegg! 3
(gg3 4

LessonTypegg4 >
typegg? C
)ggC D
{hh 	
switchii 
(ii 
typeii 
)ii 
{jj 
casekk 

LessonTypekk 
.kk  
	Excercisekk  )
:kk) *
returnll 
	BlockTypell $
.ll$ %
	Excercisell% .
;ll. /
casemm 

LessonTypemm 
.mm  

Laboratorymm  *
:mm* +
returnnn 
	BlockTypenn $
.nn$ %

Laboratorynn% /
;nn/ 0
caseoo 

LessonTypeoo 
.oo  
Lectureoo  '
:oo' (
returnpp 
	BlockTypepp $
.pp$ %
Lecturepp% ,
;pp, -
defaultqq 
:qq 
throwrr 
newrr 
	Exceptionrr '
(rr' (
$strrr( ;
)rr; <
;rr< =
}ss 
}tt 	
}uu 
}vv Ω
6C:\Projects\Swapify\Backend\CourseParser\CourseItem.cs
	namespace 	
FRITeam
 
. 
Swapify 
. 
Backend !
.! "
CourseParser" .
{ 
public 

class 

CourseItem 
{ 
public		 
string		 

CourseName		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		- .
public

 
string

 

CourseCode

  
{

! "
get

# &
;

& '
set

( +
;

+ ,
}

- .
public 
string 
Faculty 
{ 
get  #
;# $
set% (
;( )
}* +
public 
string 
Town 
{ 
get  
;  !
set" %
;% &
}' (
public 
string 
	StudyType 
{  !
get" %
;% &
set' *
;* +
}, -
public 
string 
YearOfStudy !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
string 
StudyOfField "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
string  
DetailedStudyOfField *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
} 
} ¨1
,C:\Projects\Swapify\Backend\CourseService.cs
	namespace 	
FRITeam
 
. 
Swapify 
. 
Backend !
{		 
public

 

class

 
CourseService

 
:

  
ICourseService

! /
{ 
private 
readonly 
IMongoDatabase '
	_database( 1
;1 2
private 
IMongoCollection  
<  !
Course! '
>' (
_courseCollection) :
=>; =
	_database> G
.G H
GetCollectionH U
<U V
CourseV \
>\ ]
(] ^
nameof^ d
(d e
Coursee k
)k l
)l m
;m n
public 
CourseService 
( 
IMongoDatabase +
database, 4
)4 5
{ 	
	_database 
= 
database  
;  !
} 	
public 
async 
Task 
AddAsync "
(" #
Course# )
entityToAdd* 5
)5 6
{ 	
entityToAdd 
. 
Id 
= 
Guid !
.! "
NewGuid" )
() *
)* +
;+ ,
await 
_courseCollection #
.# $
InsertOneAsync$ 2
(2 3
entityToAdd3 >
)> ?
;? @
} 	
public 
async 
Task 
< 
Course  
>  !
FindByIdAsync" /
(/ 0
Guid0 4
guid5 9
)9 :
{ 	
return 
await 
_courseCollection *
.* +
Find+ /
(/ 0
x0 1
=>2 4
x5 6
.6 7
Id7 9
.9 :
Equals: @
(@ A
guidA E
)E F
)F G
.G H
FirstOrDefaultAsyncH [
([ \
)\ ]
;] ^
} 	
public 
async 
Task 
< 
Course  
>  !
FindByNameAsync" 1
(1 2
string2 8
name9 =
)= >
{   	
return!! 
await!! 
_courseCollection!! *
.!!* +
Find!!+ /
(!!/ 0
x!!0 1
=>!!2 4
x!!5 6
.!!6 7

CourseName!!7 A
.!!A B
Equals!!B H
(!!H I
name!!I M
)!!M N
)!!N O
.!!O P
FirstOrDefaultAsync!!P c
(!!c d
)!!d e
;!!e f
}"" 	
public$$ 
Task$$ 
<$$ 
List$$ 
<$$ 
Course$$ 
>$$  
>$$  !
FindByStartName$$" 1
($$1 2
string$$2 8
courseStartsWith$$9 I
)$$I J
{%% 	
return&& 
_courseCollection&& $
.&&$ %
Find&&% )
(&&) *
x&&* +
=>&&, .
x&&/ 0
.&&0 1

CourseName&&1 ;
.&&; <

StartsWith&&< F
(&&F G
courseStartsWith&&G W
)&&W X
)&&X Y
.&&Y Z
ToListAsync&&Z e
(&&e f
)&&f g
;&&g h
}'' 	
public-- 
async-- 
Task-- 
<-- 
Guid-- 
>-- %
GetOrAddNotExistsCourseId--  9
(--9 :
string--: @

courseName--A K
,--K L
Block--M R
courseBlock--S ^
)--^ _
{.. 	
var// 
course// 
=// 
await// 
this// #
.//# $
FindByNameAsync//$ 3
(//3 4

courseName//4 >
)//> ?
;//? @
if00 
(00 
course00 
==00 
null00 
)00 
{11 
var22 
	timetable22 
=22 
new22  #
	Timetable22$ -
(22- .
)22. /
;22/ 0
	timetable33 
.33 
AddNewBlock33 %
(33% &
courseBlock33& 1
)331 2
;332 3
course44 
=44 
new44 
Course44 #
(44# $
)44$ %
{44& '

CourseName44( 2
=443 4

courseName445 ?
,44? @
	Timetable44A J
=44K L
	timetable44M V
}44W X
;44X Y
await55 
this55 
.55 
AddAsync55 #
(55# $
course55$ *
)55* +
;55+ ,
}66 
else77 
{88 
if99 
(99 
course99 
.99 
	Timetable99 $
==99% '
null99( ,
)99, -
{:: 
course;; 
.;; 
	Timetable;; $
=;;% &
new;;' *
	Timetable;;+ 4
(;;4 5
);;5 6
;;;6 7
}<< 
if== 
(== 
!== 
course== 
.== 
	Timetable== %
.==% &
ContainsBlock==& 3
(==3 4
courseBlock==4 ?
)==? @
)==@ A
{>> 
courseAA 
.AA 
	TimetableAA $
.AA$ %
AddNewBlockAA% 0
(AA0 1
courseBlockAA1 <
)AA< =
;AA= >
awaitBB 
thisBB 
.BB 
UpdateAsyncBB *
(BB* +
courseBB+ 1
)BB1 2
;BB2 3
}CC 
}DD 
returnEE 
courseEE 
.EE 
IdEE 
;EE 
}GG 	
publicII 
asyncII 
TaskII 
UpdateAsyncII %
(II% &
CourseII& ,
courseII- 3
)II3 4
{JJ 	
awaitKK 
_courseCollectionKK #
.KK# $
ReplaceOneAsyncKK$ 3
(KK3 4
xKK4 5
=>KK6 8
xKK9 :
.KK: ;
IdKK; =
==KK> @
courseKKA G
.KKG H
IdKKH J
,KKJ K
courseKKL R
)KKR S
;KKS T
}LL 	
}MM 
}NN ’	
-C:\Projects\Swapify\Backend\DbRegistration.cs
	namespace 	
Backend
 
{ 
public 

static 
class 
DbRegistration &
{ 
private 
static 
bool 
_isInicialized *
=+ ,
false- 2
;2 3
public		 
static		 
void		 
Init		 
(		  
)		  !
{		" #
if

 
(

 
!

 
_isInicialized

 
)

  
{ 
BsonClassMap 
. 
RegisterClassMap -
<- .
	Timetable. 7
>7 8
(8 9
map9 <
=>= ?
{ 
map 
. 
AutoMap 
(  
)  !
;! "
map 
. 
MapField  
(  !
$str! *
)* +
.+ ,
SetElementName, :
(: ;
$str; F
)F G
;G H
} 
) 
; 
_isInicialized 
=  
true! %
;% &
} 
} 	
} 
} †?
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.cs
	namespace 	
FRITeam
 
. 
Swapify 
. 
Backend !
.! "
DbSeed" (
{ 
public 

static 
class 
DbSeed 
{ 
public 
static 
async 
Task  "
CreateTestingUserAsync! 7
(7 8
IServiceProvider8 H
serviceProviderI X
)X Y
{ 	
var 
	dbService 
= 
serviceProvider +
.+ ,
GetRequiredService, >
<> ?
IMongoDatabase? M
>M N
(N O
)O P
;P Q
var 
usersCollection 
=  !
	dbService" +
.+ ,
GetCollection, 9
<9 :
User: >
>> ?
(? @
$str@ G
)G H
;H I
string 
email 
= 
$str -
;- .
User 
oleg 
= 
usersCollection '
.' (
Find( ,
(, -
x- .
=>/ 1
x2 3
.3 4
Email4 9
==: <
email= B
)B C
.C D
SingleOrDefaultD S
(S T
)T U
;U V
if 
( 
oleg 
== 
null 
) 
{ 
User 
user 
= 
new 
User  $
{ 
Name 
= 
$str !
,! "
Surname 
= 
$str (
,( )
Email   
=   
email   !
,  ! "
NormalizedEmail!! #
=!!$ %
email!!& +
.!!+ ,
ToUpper!!, 3
(!!3 4
)!!4 5
,!!5 6
UserName"" 
="" 
email"" $
,""$ %
NormalizedUserName## &
=##' (
email##) .
.##. /
ToUpper##/ 6
(##6 7
)##7 8
,##8 9
EmailConfirmed$$ "
=$$# $
true$$% )
,$$) *
SecurityStamp%% !
=%%" #
Guid%%$ (
.%%( )
NewGuid%%) 0
(%%0 1
)%%1 2
.%%2 3
ToString%%3 ;
(%%; <
$str%%< ?
)%%? @
,%%@ A
Student&& 
=&& 
await&& #
CreateStudentAsync&&$ 6
(&&6 7
serviceProvider&&7 F
)&&F G
}'' 
;'' 
var)) 
password)) 
=)) 
new)) "
PasswordHasher))# 1
<))1 2
User))2 6
>))6 7
())7 8
)))8 9
;))9 :
var** 
hashed** 
=** 
password** %
.**% &
HashPassword**& 2
(**2 3
user**3 7
,**7 8
$str**9 C
)**C D
;**D E
user++ 
.++ 
PasswordHash++ !
=++" #
hashed++$ *
;++* +
usersCollection,, 
.,,  
	InsertOne,,  )
(,,) *
user,,* .
),,. /
;,,/ 0
}-- 
}.. 	
private00 
static00 
async00 
Task00 !
<00! "
Student00" )
>00) *
CreateStudentAsync00+ =
(00= >
IServiceProvider00> N
serviceProvider00O ^
)00^ _
{11 	
var22 
	dbService22 
=22 
serviceProvider22 +
.22+ ,
GetRequiredService22, >
<22> ?
IMongoDatabase22? M
>22M N
(22N O
)22O P
;22P Q
var33 
studentCollection33 !
=33" #
	dbService33$ -
.33- .
GetCollection33. ;
<33; <
Student33< C
>33C D
(33D E
nameof33E K
(33K L
Student33L S
)33S T
)33T U
;33U V
var44 
studyGroupService44 !
=44" #
serviceProvider44$ 3
.443 4
GetRequiredService444 F
<44F G
IStudyGroupService44G Y
>44Y Z
(44Z [
)44[ \
;44\ ]
Student66 
student66 
=66 
new66 !
Student66" )
(66) *
)66* +
;66+ ,

StudyGroup77 
sg77 
=77 
await77 !
studyGroupService77" 3
.773 4
GetStudyGroupAsync774 F
(77F G
$str77G O
)77O P
;77P Q
student88 
.88 
	Timetable88 
=88 
sg88  "
.88" #
	Timetable88# ,
.88, -
Clone88- 2
(882 3
)883 4
;884 5
student99 
.99 

StudyGroup99 
=99  
sg99! #
;99# $
studentCollection;; 
.;; 
	InsertOne;; '
(;;' (
student;;( /
);;/ 0
;;;0 1
return<< 
student<< 
;<< 
}== 	
public?? 
static?? 
void??  
CreateTestingCourses?? /
(??/ 0
IServiceProvider??0 @
serviceProvider??A P
)??P Q
{@@ 	
varAA 
	dbServiceAA 
=AA 
serviceProviderAA +
.AA+ ,
GetRequiredServiceAA, >
<AA> ?
IMongoDatabaseAA? M
>AAM N
(AAN O
)AAO P
;AAP Q
varBB 
courseCollectionBB  
=BB! "
	dbServiceBB# ,
.BB, -
GetCollectionBB- :
<BB: ;
CourseBB; A
>BBA B
(BBB C
nameofBBC I
(BBI J
CourseBBJ P
)BBP Q
)BBQ R
;BBR S
varDD 
pathDD 
=DD 
serviceProviderDD &
.DD& '
GetRequiredServiceDD' 9
<DD9 :
IOptionsDD: B
<DDB C
PathSettingsDDC O
>DDO P
>DDP Q
(DDQ R
)DDR S
;DDS T
varEE 
jsonEE 
=EE 
FileEE 
.EE 
ReadAllTextEE '
(EE' (
pathEE( ,
.EE, -
ValueEE- 2
.EE2 3
CoursesJsonPathEE3 B
)EEB C
;EEC D
varFF 
coursesFF 
=FF 

NewtonsoftFF $
.FF$ %
JsonFF% )
.FF) *
JsonConvertFF* 5
.FF5 6
DeserializeObjectFF6 G
<FFG H
ListFFH L
<FFL M

CourseItemFFM W
>FFW X
>FFX Y
(FFY Z
jsonFFZ ^
)FF^ _
;FF_ `

DictionaryHH 
<HH 
stringHH 
,HH 
CourseHH %
>HH% &
dicHH' *
=HH+ ,
newHH- 0

DictionaryHH1 ;
<HH; <
stringHH< B
,HHB C
CourseHHD J
>HHJ K
(HHK L
)HHL M
;HHM N
foreachII 
(II 
varII 
crsII 
inII 
coursesII  '
)II' (
{JJ 
dicKK 
[KK 
crsKK 
.KK 

CourseCodeKK "
]KK" #
=KK$ %
newKK& )
CourseKK* 0
(KK0 1
)KK1 2
{LL 

CourseCodeMM 
=MM  
crsMM! $
.MM$ %

CourseCodeMM% /
,MM/ 0

CourseNameNN 
=NN  
crsNN! $
.NN$ %

CourseNameNN% /
}OO 
;OO 
}PP 
courseCollectionQQ 
.QQ 

InsertManyQQ '
(QQ' (
dicQQ( +
.QQ+ ,
SelectQQ, 2
(QQ2 3
xQQ3 4
=>QQ5 7
xQQ8 9
.QQ9 :
ValueQQ: ?
)QQ? @
)QQ@ A
;QQA B
}RR 	
}SS 
}TT Ú+
+C:\Projects\Swapify\Backend\EmailService.cs
	namespace		 	
FRITeam		
 
.		 
Swapify		 
.		 
Backend		 !
{

 
public 

class 
EmailService 
: 
IEmailService  -
{ 
private 
readonly 
ILogger  
_logger! (
;( )
private 
readonly 
ILoggerFactory '
_loggerFactory( 6
;6 7
private 
readonly 
MailingSettings (
_emailSettings) 7
;7 8
private 
readonly 
EnvironmentSettings , 
_environmentSettings- A
;A B
public 
EmailService 
( 
ILoggerFactory *
loggerFactory+ 8
,8 9
IOptions: B
<B C
MailingSettingsC R
>R S
emailSettingsT a
,a b
IOptions 
< 
EnvironmentSettings (
>( )
environmentSettings* =
)= >
{ 	
_logger 
= 
loggerFactory #
.# $
CreateLogger$ 0
(0 1
GetType1 8
(8 9
)9 :
.: ;
FullName; C
)C D
;D E
_loggerFactory 
= 
loggerFactory *
;* +
_emailSettings 
= 
emailSettings *
.* +
Value+ 0
;0 1 
_environmentSettings  
=! "
environmentSettings# 6
.6 7
Value7 <
;< =
} 	
public 
bool !
SendConfirmationEmail )
() *
string* 0
receiver1 9
,9 :
string; A
confirmationLinkB R
,R S
stringT Z
	emailType[ d
)d e
{ 	
try 
{ 
string 
type 
= 
$"  
{  !
_emailSettings! /
./ 0
EmailsNameSpace0 ?
}? @
.@ A
{A B
	emailTypeB K
}K L
"L M
;M N
var   
email   
=   
	Activator   %
.  % &
CreateInstance  & 4
(  4 5
Type  5 9
.  9 :
GetType  : A
(  A B
type  B F
)  F G
,  G H
_loggerFactory  I W
,  W X
_emailSettings  Y g
.  g h
Username  h p
,  p q
_emailSettings!! "
.!!" #
SenderDisplayName!!# 4
,!!4 5
receiver!!6 >
,!!> ? 
_environmentSettings!!@ T
.!!T U
BaseUrl!!U \
,!!\ ]
confirmationLink!!^ n
)!!n o
;!!o p
MailMessage"" 
mailMessage"" '
=""( )
(""* +
MailMessage""+ 6
)""6 7
email""7 <
.""< =
GetType""= D
(""D E
)""E F
.""F G
	GetMethod""G P
(""P Q
$str""Q d
)""d e
.##F G
Invoke##G M
(##M N
email##N S
,##S T
null##U Y
)##Y Z
;##Z [
if$$ 
($$ 
mailMessage$$ 
==$$  "
null$$# '
)$$' (
{%% 
_logger&& 
.&& 
LogError&& $
(&&$ %
$"&&% '9
-Unable to create MailMessage for email type '&&' T
{&&T U
	emailType&&U ^
}&&^ _
'.&&_ a
"&&a b
)&&b c
;&&c d
return'' 
false''  
;''  !
}(( 
	SendEmail)) 
()) 
mailMessage)) %
)))% &
;))& '
return** 
true** 
;** 
}++ 
catch,, 
(,, 
	Exception,, 
e,, 
),, 
{-- 
_logger.. 
... 
LogError..  
(..  !
e..! "
..." #
ToString..# +
(..+ ,
).., -
)..- .
;... /
return// 
false// 
;// 
}00 
}11 	
private33 
void33 
	SendEmail33 
(33 
MailMessage33 *
message33+ 2
)332 3
{44 	
NetworkCredential55 
credentials55 )
=55* +
new55, /
NetworkCredential550 A
(55A B
_emailSettings55B P
.55P Q
Username55Q Y
,55Y Z
_emailSettings55[ i
.55i j
Password55j r
)55r s
;55s t
using66 
(66 

SmtpClient66 
client66 $
=66% &
new66' *

SmtpClient66+ 5
(665 6
_emailSettings666 D
.66D E

SmtpServer66E O
,66O P
(66Q R
int66R U
)66U V
_emailSettings66V d
.66d e
SmtpPort66e m
)66m n
)66n o
{77 
client88 
.88 
Credentials88 "
=88# $
credentials88% 0
;880 1
client99 
.99 
	EnableSsl99  
=99! "
true99# '
;99' (
client:: 
.:: 
Send:: 
(:: 
message:: #
)::# $
;::$ %
};; 
}<< 	
}== 
}>> ˝F
NC:\Projects\Swapify\Backend\Emails\ConfirmationEmails\ConfirmationEmailBase.cs
	namespace 	
FRITeam
 
. 
Swapify 
. 
Backend !
.! "
Emails" (
{ 
public		 

abstract		 
class		 !
ConfirmationEmailBase		 /
:		0 1
	EmailBase		2 ;
{

 
	protected 
const 
string 
	PathToMac (
=) *
$str+ ^
;^ _
	protected 
const 
string 

PathToLogo )
=* +
$str, W
;W X
	protected 
const 
string 
MacContentId +
=, -
$str. 3
;3 4
	protected 
const 
string 
LogoContentId ,
=- .
$str/ 8
;8 9
	protected 
const 
string 
MacImg %
=& '
$str( .
+/ 0
MacContentId1 =
;= >
	protected 
const 
string 
LogoImg &
=' (
$str) /
+0 1
LogoContentId2 ?
;? @
	protected 
string 
ConfirmationLink )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
private 
readonly 
ILogger  
_logger! (
;( )
	protected !
ConfirmationEmailBase '
(' (
ILoggerFactory( 6
loggerFactory7 D
,D E
stringF L
senderM S
,S T
stringU [
senderDisplayName\ m
,m n
stringo u
receiverv ~
,~ 
string
Ä Ü
baseUrl
á é
,
é è
string
ê ñ
confirmationLink
ó ß
)
ß ®
: 
base 
( 
sender 
, 
senderDisplayName ,
,, -
receiver. 6
,6 7
baseUrl8 ?
)? @
{ 	
ConfirmationLink 
= 
confirmationLink /
;/ 0
_logger 
= 
loggerFactory #
.# $
CreateLogger$ 0
(0 1
GetType1 8
(8 9
)9 :
.: ;
FullName; C
)C D
;D E
} 	
public 
override 
MailMessage #
CreateMailMessage$ 5
(5 6
)6 7
{ 	
if 
( 
! 
CreateEmailBody  
(  !
)! "
)" #
{ 
_logger 
. 
LogError  
(  !
$"! #(
Unable to create email body.# ?
"? @
)@ A
;A B
return   
null   
;   
}!! 
MailMessage## 
mailMessage## #
=##$ %
new##& )
MailMessage##* 5
(##5 6
Sender##6 <
,##< =
Receiver##> F
)##F G
;##G H
mailMessage$$ 
.$$ 
Subject$$ 
=$$  !
Subject$$" )
;$$) *
mailMessage%% 
.%% 
Body%% 
=%% 
Body%% #
;%%# $
mailMessage&& 
.&& 
Sender&& 
=&&  
Sender&&! '
;&&' (
mailMessage'' 
.'' 

IsBodyHtml'' "
=''# $
true''% )
;'') *
string)) 

macImgPath)) 
=)) 
Path))  $
.))$ %
Combine))% ,
()), -
OutputDirLocation))- >
,))> ?
	PathToMac))@ I
)))I J
;))J K
string** 
logoImgPath** 
=**  
Path**! %
.**% &
Combine**& -
(**- .
OutputDirLocation**. ?
,**? @

PathToLogo**A K
)**K L
;**L M
if++ 
(++ 

ImgsExists++ 
(++ 

macImgPath++ %
,++% &
logoImgPath++' 2
)++2 3
)++3 4
{,, 

Attachment-- 
macImg-- !
=--" #
CreateImgAttachment--$ 7
(--7 8

macImgPath--8 B
,--B C
MacContentId--D P
)--P Q
;--Q R
mailMessage.. 
... 
Attachments.. '
...' (
Add..( +
(..+ ,
macImg.., 2
)..2 3
;..3 4

Attachment// 

swapifyImg// %
=//& '
CreateImgAttachment//( ;
(//; <
logoImgPath//< G
,//G H
LogoContentId//I V
)//V W
;//W X
mailMessage00 
.00 
Attachments00 '
.00' (
Add00( +
(00+ ,

swapifyImg00, 6
)006 7
;007 8
}11 
return22 
mailMessage22 
;22 
}33 	
	protected55 
override55 
bool55 
CreateEmailBody55  /
(55/ 0
)550 1
{66 	
Body77 
=77 
ReadTemplate77 
(77  
)77  !
;77! "
if88 
(88 
Body88 
==88 
null88 
)88 
return99 
false99 
;99 
var;; 
compiler;; 
=;; 
new;; 
HtmlFormatCompiler;; 1
(;;1 2
);;2 3
;;;3 4
var<< 
	generator<< 
=<< 
compiler<< $
.<<$ %
Compile<<% ,
(<<, -
Body<<- 1
)<<1 2
;<<2 3
Body== 
=== 
	generator== 
.== 
Render== #
(==# $
new==$ '
{>> 
link?? 
=?? 
BaseUrl?? 
,?? 
confirmationLink@@  
=@@! "
ConfirmationLink@@# 3
,@@3 4
img1AA 
=AA 
MacImgAA 
,AA 
img2BB 
=BB 
LogoImgBB 
}CC 
)CC 
;CC 
returnDD 
trueDD 
;DD 
}EE 	
privateGG 
stringGG 
ReadTemplateGG #
(GG# $
)GG$ %
{HH 	
stringII 
templatePathII 
=II  !
PathII" &
.II& '
CombineII' .
(II. /
OutputDirLocationII/ @
,II@ A
PathToTemplateIIB P
)IIP Q
;IIQ R
ifJJ 
(JJ 
!JJ 
FileJJ 
.JJ 
ExistsJJ 
(JJ 
templatePathJJ )
)JJ) *
)JJ* +
{KK 
_loggerLL 
.LL 
LogErrorLL  
(LL  !
$"LL! #
Email template LL# 2
{LL2 3
templatePathLL3 ?
}LL? @
 does not exists.LL@ Q
"LLQ R
)LLR S
;LLS T
returnMM 
nullMM 
;MM 
}NN 
tryOO 
{PP 
returnQQ 
FileQQ 
.QQ 
ReadAllTextQQ '
(QQ' (
templatePathQQ( 4
)QQ4 5
;QQ5 6
}RR 
catchSS 
(SS 
	ExceptionSS 
eSS 
)SS 
{TT 
_loggerUU 
.UU 
LogErrorUU  
(UU  !
$"UU! #$
Error when reading file UU# ;
{UU; <
templatePathUU< H
}UUH I
.\n UUI M
{UUM N
eUUN O
.UUO P
ToStringUUP X
(UUX Y
)UUY Z
}UUZ [
"UU[ \
)UU\ ]
;UU] ^
returnVV 
nullVV 
;VV 
}WW 
}XX 	
privateZZ 
boolZZ 

ImgsExistsZZ 
(ZZ  
stringZZ  &

macImgPathZZ' 1
,ZZ1 2
stringZZ3 9
logoImgPathZZ: E
)ZZE F
{[[ 	
if\\ 
(\\ 
!\\ 
File\\ 
.\\ 
Exists\\ 
(\\ 

macImgPath\\ '
)\\' (
)\\( )
{]] 
_logger^^ 
.^^ 

LogWarning^^ "
(^^" #
$str^^# ;
)^^; <
;^^< =
return__ 
false__ 
;__ 
}`` 
ifaa 
(aa 
!aa 
Fileaa 
.aa 
Existsaa 
(aa 
logoImgPathaa (
)aa( )
)aa) *
{bb 
_loggercc 
.cc 

LogWarningcc "
(cc" #
$strcc# <
)cc< =
;cc= >
returndd 
falsedd 
;dd 
}ee 
returnff 
trueff 
;ff 
}gg 	
}hh 
}ii ¢
JC:\Projects\Swapify\Backend\Emails\ConfirmationEmails\RegistrationEmail.cs
	namespace 	
FRITeam
 
. 
Swapify 
. 
Backend !
.! "
Emails" (
{ 
public 

class 
RegistrationEmail "
:# $!
ConfirmationEmailBase% :
{ 
	protected 
override 
string !
Subject" )
=>* ,
$str- E
;E F
	protected 
override 
string !
PathToTemplate" 0
=>1 3
$str4 f
;f g
public

 
RegistrationEmail

  
(

  !
ILoggerFactory

! /
loggerFactory

0 =
,

= >
string

? E
sender

F L
,

L M
string

N T
senderDisplayName

U f
,

f g
string

h n
receiver

o w
,

w x
string

y 
baseUrl


Ä á
,


á à
string


â è
confirmationLink


ê †
)


† °
: 
base 
( 
loggerFactory  
,  !
sender" (
,( )
senderDisplayName* ;
,; <
receiver= E
,E F
baseUrlG N
,N O
confirmationLinkP `
)` a
{ 	
} 	
} 
} ¨
MC:\Projects\Swapify\Backend\Emails\ConfirmationEmails\RestorePasswordEmail.cs
	namespace 	
FRITeam
 
. 
Swapify 
. 
Backend !
.! "
Emails" (
{ 
public 

class  
RestorePasswordEmail %
:& '!
ConfirmationEmailBase( =
{ 
	protected 
override 
string !
Subject" )
=>* ,
$str- I
;I J
	protected 
override 
string !
PathToTemplate" 0
=>1 3
$str4 i
;i j
public

  
RestorePasswordEmail

 #
(

# $
ILoggerFactory

$ 2
loggerFactory

3 @
,

@ A
string

B H
sender

I O
,

O P
string

Q W
senderDisplayName

X i
,

i j
string

k q
receiver

r z
,

z {
string	

| Ç
baseUrl


É ä
,


ä ã
string


å í
confirmationLink


ì £
)


£ §
: 
base 
( 
loggerFactory  
,  !
sender" (
,( )
senderDisplayName* ;
,; <
receiver= E
,E F
baseUrlG N
,N O
confirmationLinkP `
)` a
{ 	
} 	
} 
} Ë
/C:\Projects\Swapify\Backend\Emails\EmailBase.cs
	namespace 	
FRITeam
 
. 
Swapify 
. 
Backend !
.! "
Emails" (
{ 
public 

abstract 
class 
	EmailBase #
{		 
	protected

 
string

 
OutputDirLocation

 *
{

+ ,
get

- 0
;

0 1
}

2 3
	protected 
abstract 
string !
PathToTemplate" 0
{1 2
get3 6
;6 7
}8 9
	protected 
abstract 
string !
Subject" )
{* +
get, /
;/ 0
}1 2
	protected 
MailAddress 
Sender $
{% &
get' *
;* +
set, /
;/ 0
}1 2
	protected 
MailAddress 
Receiver &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
	protected 
string 
BaseUrl  
{! "
get# &
;& '
set( +
;+ ,
}- .
	protected 
string 
Body 
{ 
get  #
;# $
set% (
;( )
}* +
	protected 
	EmailBase 
( 
string "
sender# )
,) *
string+ 1
senderDisplayName2 C
,C D
stringE K
receiverL T
,T U
stringV \
baseUrl] d
)d e
{ 	
Sender 
= 
new 
MailAddress $
($ %
sender% +
,+ ,
senderDisplayName- >
)> ?
;? @
Receiver 
= 
new 
MailAddress &
(& '
receiver' /
)/ 0
;0 1
OutputDirLocation 
= 
Path  $
.$ %
GetDirectoryName% 5
(5 6
Assembly6 >
.> ? 
GetExecutingAssembly? S
(S T
)T U
.U V
LocationV ^
)^ _
;_ `
BaseUrl 
= 
baseUrl 
; 
} 	
public 
abstract 
MailMessage #
CreateMailMessage$ 5
(5 6
)6 7
;7 8
	protected 
abstract 
bool 
CreateEmailBody  /
(/ 0
)0 1
;1 2
	protected 

Attachment 
CreateImgAttachment 0
(0 1
string1 7
pathToAttachment8 H
,H I
stringJ P
	contentIdQ Z
)Z [
{ 	

Attachment 

attachment !
=" #
new$ '

Attachment( 2
(2 3
pathToAttachment3 C
)C D
;D E

attachment   
.   
	ContentId    
=  ! "
	contentId  # ,
;  , -

attachment!! 
.!! 
ContentDisposition!! )
.!!) *
Inline!!* 0
=!!1 2
true!!3 7
;!!7 8

attachment"" 
."" 
ContentDisposition"" )
."") *
DispositionType""* 9
="": ; 
DispositionTypeNames""< P
.""P Q
Inline""Q W
;""W X
return## 

attachment## 
;## 
}$$ 	
}%% 
}&& Í
:C:\Projects\Swapify\Backend\Exceptions\SettingException.cs
	namespace 	
FRITeam
 
. 
Swapify 
. 
Backend !
.! "

Exceptions" ,
{ 
[ 
Serializable 
] 
public 

class 
SettingException !
:" #
	Exception$ -
{		 
public

 
string

 
ConfigFileName

 $
{

% &
get

' *
;

* +
set

, /
;

/ 0
}

1 2
public 
SettingException 
(  
string  &
configFileName' 5
,5 6
string7 =
message> E
)E F
:G H
baseI M
(M N
messageN U
)U V
{ 	
ConfigFileName 
= 
configFileName +
;+ ,
} 	
	protected 
SettingException "
(" #
SerializationInfo# 4
info5 9
,9 :
StreamingContext; K
contextL S
)S T
:U V
baseW [
([ \
info\ `
,` a
contextb i
)i j
{ 	
ConfigFileName 
= 
info !
.! "
	GetString" +
(+ ,
$str, <
)< =
;= >
} 	
[ 	
SecurityPermission	 
( 
SecurityAction *
.* +
Demand+ 1
,1 2"
SerializationFormatter3 I
=J K
trueL P
)P Q
]Q R
public 
override 
void 
GetObjectData *
(* +
SerializationInfo+ <
info= A
,A B
StreamingContextC S
contextT [
)[ \
{ 	
if 
( 
info 
== 
null 
) 
throw 
new !
ArgumentNullException /
(/ 0
$str0 6
)6 7
;7 8
info 
. 
AddValue 
( 
$str *
,* +
ConfigFileName, :
): ;
;; <
base 
. 
GetObjectData 
( 
info #
,# $
context% ,
), -
;- .
} 	
} 
} ù
>C:\Projects\Swapify\Backend\Interfaces\IBlockChangesService.cs
	namespace 	
FRITeam
 
. 
Swapify 
. 
Backend !
.! "

Interfaces" ,
{ 
public		 

	interface		  
IBlockChangesService		 )
{

 
Task 
< 
bool 
> 
AddAndFindMatch "
(" #
BlockChangeRequest# 5
entityToAdd6 A
)A B
;B C
Task 
< 
List 
< 
BlockChangeRequest $
>$ %
>% &"
FindAllStudentRequests' =
(= >
Guid> B
	studentIdC L
)L M
;M N
} 
} •
8C:\Projects\Swapify\Backend\Interfaces\ICourseService.cs
	namespace 	
FRITeam
 
. 
Swapify 
. 
Backend !
.! "

Interfaces" ,
{ 
public 

	interface 
ICourseService #
{		 
Task

 
AddAsync

 
(

 
Course

 
entityToAdd

 (
)

( )
;

) *
Task 
< 
Course 
> 
FindByIdAsync "
(" #
Guid# '
guid( ,
), -
;- .
Task 
< 
Course 
> 
FindByNameAsync $
($ %
string% +
name, 0
)0 1
;1 2
Task 
< 
List 
< 
Course 
> 
> 
FindByStartName *
(* +
string+ 1
courseStartsWith2 B
)B C
;C D
Task 
< 
Guid 
> %
GetOrAddNotExistsCourseId ,
(, -
string- 3

courseName4 >
,> ?
Block@ E
courseBlockF Q
)Q R
;R S
Task 
UpdateAsync 
( 
Course 
course  &
)& '
;' (
} 
} ◊
7C:\Projects\Swapify\Backend\Interfaces\IEmailService.cs
	namespace 	
FRITeam
 
. 
Swapify 
. 
Backend !
.! "

Interfaces" ,
{ 
public 

	interface 
IEmailService "
{ 
bool !
SendConfirmationEmail "
(" #
string# )
receiver* 2
,2 3
string4 :
confirmationLink; K
,K L
stringM S
	emailTypeT ]
)] ^
;^ _
} 
} Ú
9C:\Projects\Swapify\Backend\Interfaces\IStudentService.cs
	namespace 	
FRITeam
 
. 
Swapify 
. 
Backend !
.! "

Interfaces" ,
{ 
public 

	interface 
IStudentService $
{ 
Task		 
AddAsync		 
(		 
Student		 
entityToAdd		 )
)		) *
;		* +
Task

 
<

 
Student

 
>

 
FindByIdAsync

 #
(

# $
Guid

$ (
guid

) -
)

- .
;

. /
Task 
UpdateStudentAsync 
(  
Student  '
studentToUpdate( 7
)7 8
;8 9
Task '
UpdateStudentTimetableAsync (
(( )
Student) 0
studentToUpdate1 @
,@ A

StudyGroupB L

studygroupM W
)W X
;X Y
} 
} €
<C:\Projects\Swapify\Backend\Interfaces\IStudyGroupService.cs
	namespace 	
FRITeam
 
. 
Swapify 
. 
Backend !
.! "

Interfaces" ,
{ 
public 

	interface 
IStudyGroupService '
{		 
Task

 
AddAsync

 
(

 

StudyGroup

  
entityToAdd

! ,
)

, -
;

- .
Task 
< 

StudyGroup 
> 
FindByIdAsync &
(& '
Guid' +
guid, 0
)0 1
;1 2
Task 
< 

StudyGroup 
> 
GetStudyGroupAsync +
(+ ,
string, 2
studyGroupNumber3 C
)C D
;D E
} 
} À
6C:\Projects\Swapify\Backend\Interfaces\IUserService.cs
	namespace 	
FRITeam
 
. 
Swapify 
. 
Backend !
.! "

Interfaces" ,
{ 
public 

	interface 
IUserService !
{		 
Task

 
<

 
JwtSecurityToken

 
>

 
Authenticate

 +
(

+ ,
string

, 2
login

3 8
,

8 9
string

: @
password

A I
)

I J
;

J K
Task 
< 
IdentityResult 
> 
AddUserAsync )
() *
User* .
user/ 3
,3 4
string5 ;
password< D
)D E
;E F
Task 
< 
string 
> /
#GenerateEmailConfirmationTokenAsync 8
(8 9
User9 =
user> B
)B C
;C D
Task 
< 
string 
> +
GeneratePasswordResetTokenAsync 4
(4 5
User5 9
user: >
)> ?
;? @
Task 
< 
IdentityResult 
> 
ResetPasswordAsync /
(/ 0
User0 4
user5 9
,9 :
string; A
tokenB G
,G H
stringI O
newPasswordP [
)[ \
;\ ]
Task 
< 
IdentityResult 
> 
ConfirmEmailAsync .
(. /
User/ 3
user4 8
,8 9
string: @
tokenA F
)F G
;G H
Task 
< 
User 
> 
GetUserByEmailAsync &
(& '
string' -
email. 3
)3 4
;4 5
Task 
< 
User 
> 
GetUserByIdAsync #
(# $
string$ *
userId+ 1
)1 2
;2 3
Task 
< 
IdentityResult 
> 
DeleteUserAsyc +
(+ ,
User, 0
user1 5
)5 6
;6 7
Task 
< 
IdentityResult 
> 
UpdateUserAsync ,
(, -
User- 1
userToUpdate2 >
)> ?
;? @
JwtSecurityToken 
Renew 
( 
string %
jwtToken& .
). /
;/ 0
} 
} œ
;C:\Projects\Swapify\Backend\Settings\EnvironmentSettings.cs
	namespace 	
FRITeam
 
. 
Swapify 
. 
Backend !
.! "
Settings" *
{ 
public 

class 
EnvironmentSettings $
:% &
SettingsBase' 3
{ 
public 
override 
string 
ConfigFileName -
=>. 0
$str1 G
;G H
public 
string 
Environment !
{" #
get$ '
;' (
set) ,
;, -
}. /
public		 
string		 
	JwtSecret		 
{		  !
get		" %
;		% &
set		' *
;		* +
}		, -
public

 
string

 
BaseUrl

 
{

 
get

  #
;

# $
set

% (
;

( )
}

* +
public 
override 
void 
Validate %
(% &
)& '
{ 	
if 
( 
string 
. 
IsNullOrEmpty $
($ %
Environment% 0
)0 1
)1 2
Errors 
. 

AppendLine !
(! "
$"" $
Setting $ ,
{, -
nameof- 3
(3 4
Environment4 ?
)? @
}@ A
 is missing in A P
{P Q
ConfigFileNameQ _
}_ `
.` a
"a b
)b c
;c d
if 
( 
string 
. 
IsNullOrEmpty $
($ %
	JwtSecret% .
). /
)/ 0
Errors 
. 

AppendLine !
(! "
$"" $
Setting $ ,
{, -
nameof- 3
(3 4
	JwtSecret4 =
)= >
}> ?
 is missing in ? N
{N O
ConfigFileNameO ]
}] ^
.^ _
"_ `
)` a
;a b
if 
( 
string 
. 
IsNullOrEmpty $
($ %
BaseUrl% ,
), -
)- .
Errors 
. 

AppendLine !
(! "
$"" $
Setting $ ,
{, -
nameof- 3
(3 4
BaseUrl4 ;
); <
}< =
 is missing in = L
{L M
ConfigFileNameM [
}[ \
.\ ]
"] ^
)^ _
;_ `
if 
( 
! 
( 
Uri 
. 
	TryCreate 
(  
BaseUrl  '
,' (
UriKind) 0
.0 1
Absolute1 9
,9 :
out; >
Uri? B
	uriResultC L
)L M
&& 
( 
	uriResult 
. 
Scheme $
==% '
Uri( +
.+ ,
UriSchemeHttp, 9
||: <
	uriResult= F
.F G
SchemeG M
==N P
UriQ T
.T U
UriSchemeHttpsU c
)c d
)d e
)e f
Errors 
. 

AppendLine !
(! "
$"" $
Setting $ ,
{, -
nameof- 3
(3 4
BaseUrl4 ;
); <
}< =
 is not valid URL.= O
"O P
)P Q
;Q R
} 	
} 
} Í
8C:\Projects\Swapify\Backend\Settings\IdentitySettings.cs
	namespace 	
FRITeam
 
. 
Swapify 
. 
Backend !
.! "
Settings" *
{ 
public 

class 
IdentitySettings !
:" #
SettingsBase$ 0
{ 
public 
bool 
? 
RequireDigit !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
int 
? 
RequiredLength "
{# $
get% (
;( )
set* -
;- .
}/ 0
public		 
bool		 
?		 "
RequireNonAlphanumeric		 +
{		, -
get		. 1
;		1 2
set		3 6
;		6 7
}		8 9
public

 
bool

 
?

 
RequireUppercase

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

2 3
public 
bool 
? 
RequireLowercase %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
bool 
? !
RequireConfirmedEmail *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
public 
int 
? "
DefaultLockoutTimeSpan *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
public 
int 
? #
MaxFailedAccessAttempts +
{, -
get. 1
;1 2
set3 6
;6 7
}8 9
public 
bool 
? 
RequireUniqueEmail '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
public 
override 
void 
Validate %
(% &
)& '
{ 	
foreach 
( 
PropertyInfo !
info" &
in' )
typeof* 0
(0 1
IdentitySettings1 A
)A B
.B C
GetPropertiesC P
(P Q
)Q R
)R S
{ 
if 
( 
info 
. 
GetValue !
(! "
this" &
,& '
null( ,
), -
==. 0
null1 5
)5 6
Errors 
. 

AppendLine %
(% &
$"& (
Setting ( 0
{0 1
info1 5
.5 6
Name6 :
}: ;
 is missing in ; J
{J K
nameofK Q
(Q R
IdentitySettingsR b
)b c
}c d#
 configuration section.d {
"{ |
)| }
;} ~
} 
} 	
} 
} ˙
4C:\Projects\Swapify\Backend\Settings\IValidatable.cs
	namespace 	
FRITeam
 
. 
Swapify 
. 
Backend !
.! "
Settings" *
{ 
public 

	interface 
IValidatable !
{ 
string 
ConfigFileName 
{ 
get  #
;# $
}% &
StringBuilder 
Errors 
{ 
get "
;" #
}$ %
void		 
Validate		 
(		 
)		 
;		 
}

 
} ⁄+
7C:\Projects\Swapify\Backend\Settings\MailingSettings.cs
	namespace 	
FRITeam
 
. 
Swapify 
. 
Backend !
.! "
Settings" *
{ 
public 

class 
MailingSettings  
:! "
SettingsBase# /
{ 
public 
string 
EmailsNameSpace %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
string 

SmtpServer  
{! "
get# &
;& '
set( +
;+ ,
}- .
public		 
int		 
?		 
SmtpPort		 
{		 
get		 "
;		" #
set		$ '
;		' (
}		) *
public

 
string

 
Username

 
{

  
get

! $
;

$ %
set

& )
;

) *
}

+ ,
public 
string 
SenderDisplayName '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
public 
string 
Password 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
override 
void 
Validate %
(% &
)& '
{ 	
if 
( 
string 
. 
IsNullOrEmpty $
($ %
EmailsNameSpace% 4
)4 5
)5 6
Errors 
. 

AppendLine !
(! "
$"" $
Setting $ ,
{, -
nameof- 3
(3 4
EmailsNameSpace4 C
)C D
}D E
 is missing in E T
{T U
nameofU [
([ \
MailingSettings\ k
)k l
}l m$
 configuration section.	m Ñ
"
Ñ Ö
)
Ö Ü
;
Ü á
if 
( 
string 
. 
IsNullOrEmpty $
($ %

SmtpServer% /
)/ 0
)0 1
Errors 
. 

AppendLine !
(! "
$"" $
Setting $ ,
{, -
nameof- 3
(3 4

SmtpServer4 >
)> ?
}? @
 is missing in @ O
{O P
nameofP V
(V W
MailingSettingsW f
)f g
}g h#
 configuration section.h 
"	 Ä
)
Ä Å
;
Å Ç
if 
( 
string 
. 
IsNullOrEmpty $
($ %
Username% -
)- .
). /
Errors 
. 

AppendLine !
(! "
$"" $
Setting $ ,
{, -
nameof- 3
(3 4
Username4 <
)< =
}= >
 is missing in > M
{M N
nameofN T
(T U
MailingSettingsU d
)d e
}e f#
 configuration section.f }
"} ~
)~ 
;	 Ä
if 
( 
string 
. 
IsNullOrEmpty $
($ %
SenderDisplayName% 6
)6 7
)7 8
Errors 
. 

AppendLine !
(! "
$"" $
Setting $ ,
{, -
nameof- 3
(3 4
SenderDisplayName4 E
)E F
}F G
 is missing in G V
{V W
nameofW ]
(] ^
MailingSettings^ m
)m n
}n o$
 configuration section.	o Ü
"
Ü á
)
á à
;
à â
if 
( 
string 
. 
IsNullOrEmpty $
($ %
Password% -
)- .
). /
Errors 
. 

AppendLine !
(! "
$"" $
Setting $ ,
{, -
nameof- 3
(3 4
Password4 <
)< =
}= >
 is missing in > M
{M N
nameofN T
(T U
MailingSettingsU d
)d e
}e f#
 configuration section.f }
"} ~
)~ 
;	 Ä
if 
( 
SmtpPort 
== 
null  
)  !
Errors 
. 

AppendLine !
(! "
$"" $
Setting $ ,
{, -
nameof- 3
(3 4
SmtpPort4 <
)< =
}= >
 is missing in > M
{M N
nameofN T
(T U
MailingSettingsU d
)d e
}e f#
 configuration section.f }
"} ~
)~ 
;	 Ä
Regex 
regex 
= 
new 
Regex #
(# $
$str$ N
)N O
;O P
if 
( 
! 
regex 
. 
IsMatch 
( 
Username '
)' (
)( )
Errors 
. 

AppendLine !
(! "
$"" $
{$ %
Username% -
}- .(
 is not valid email address.. J
"J K
)K L
;L M
}   	
}!! 
}"" Ñ

4C:\Projects\Swapify\Backend\Settings\PathSettings.cs
	namespace 	
FRITeam
 
. 
Swapify 
. 
Backend !
.! "
Settings" *
{ 
public 

class 
PathSettings 
: 
SettingsBase  ,
{ 
public 
string 
CoursesJsonPath %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
override 
void 
Validate %
(% &
)& '
{ 	
if		 
(		 
string		 
.		 
IsNullOrEmpty		 $
(		$ %
CoursesJsonPath		% 4
)		4 5
)		5 6
Errors

 
.

 

AppendLine

 !
(

! "
$"

" $
Setting 

$ ,
{

, -
nameof

- 3
(

3 4
CoursesJsonPath

4 C
)

C D
}

D E
 is missing in 

E T
{

T U
nameof

U [
(

[ \
PathSettings

\ h
)

h i
}

i j$
 configuration section.	

j Å
"


Å Ç
)


Ç É
;


É Ñ
} 	
} 
} ¯
4C:\Projects\Swapify\Backend\Settings\SettingsBase.cs
	namespace 	
FRITeam
 
. 
Swapify 
. 
Backend !
.! "
Settings" *
{ 
public 

abstract 
class 
SettingsBase &
:' (
IValidatable) 5
{ 
public 
virtual 
string 
ConfigFileName ,
=>- /
$str0 B
;B C
public 
StringBuilder 
Errors #
{$ %
get& )
;) *
}+ ,
	protected

 
SettingsBase

 
(

 
)

  
{ 	
Errors 
= 
new 
StringBuilder &
(& '
)' (
;( )
} 	
public 
abstract 
void 
Validate %
(% &
)& '
;' (
} 
} •
-C:\Projects\Swapify\Backend\StudentService.cs
	namespace 	
FRITeam
 
. 
Swapify 
. 
Backend !
{ 
public		 

class		 
StudentService		 
:		  !
IStudentService		" 1
{

 
private 
readonly 
IMongoDatabase '
	_database( 1
;1 2
private 
IMongoCollection  
<  !
Student! (
>( )
_studentCollection* <
=>= ?
	_database@ I
.I J
GetCollectionJ W
<W X
StudentX _
>_ `
(` a
nameofa g
(g h
Studenth o
)o p
)p q
;q r
public 
StudentService 
( 
IMongoDatabase ,
database- 5
)5 6
{ 	
	_database 
= 
database  
;  !
} 	
public 
async 
Task 
AddAsync "
(" #
Student# *
entityToAdd+ 6
)6 7
{ 	
entityToAdd 
. 
Id 
= 
Guid !
.! "
NewGuid" )
() *
)* +
;+ ,
await 
_studentCollection $
.$ %
InsertOneAsync% 3
(3 4
entityToAdd4 ?
)? @
;@ A
} 	
public 
async 
Task 
< 
Student !
>! "
FindByIdAsync# 0
(0 1
Guid1 5
guid6 :
): ;
{ 	
return 
await 
_studentCollection +
.+ ,
Find, 0
(0 1
x1 2
=>3 5
x6 7
.7 8
Id8 :
.: ;
Equals; A
(A B
guidB F
)F G
)G H
.H I
FirstOrDefaultAsyncI \
(\ ]
)] ^
;^ _
} 	
public 
async 
Task 
UpdateStudentAsync ,
(, -
Student- 4
loadedStudent5 B
)B C
{   	
await!! 
_studentCollection!! $
.!!$ %
ReplaceOneAsync!!% 4
(!!4 5
x!!5 6
=>!!7 9
x!!: ;
.!!; <
Id!!< >
==!!? A
loadedStudent!!B O
.!!O P
Id!!P R
,!!R S
loadedStudent!!T a
)!!a b
;!!b c
}"" 	
public$$ 
async$$ 
Task$$ '
UpdateStudentTimetableAsync$$ 5
($$5 6
Student$$6 =
studentToUpdate$$> M
,$$M N

StudyGroup$$O Y

studyGroup$$Z d
)$$d e
{%% 	
studentToUpdate&& 
.&& 
	Timetable&& %
=&&& '

studyGroup&&( 2
.&&2 3
	Timetable&&3 <
.&&< =
Clone&&= B
(&&B C
)&&C D
;&&D E
studentToUpdate'' 
.'' 

StudyGroup'' &
=''' (

studyGroup'') 3
;''3 4
await(( 
this(( 
.(( 
UpdateStudentAsync(( )
((() *
studentToUpdate((* 9
)((9 :
;((: ;
})) 	
}++ 
},, ∑0
0C:\Projects\Swapify\Backend\StudyGroupService.cs
	namespace 	
FRITeam
 
. 
Swapify 
. 
Backend !
{ 
public 

class 
StudyGroupService "
:# $
IStudyGroupService% 7
{ 
private 
readonly 
ILogger  
<  !
StudyGroupService! 2
>2 3
_logger4 ;
;; <
private 
readonly 
IMongoDatabase '
	_database( 1
;1 2
private 
readonly  
ISchoolScheduleProxy -
_scheduleProxy. <
;< =
private 
readonly 
ICourseService '
_courseService( 6
;6 7
public 
StudyGroupService  
(  !
ILogger! (
<( )
StudyGroupService) :
>: ;
logger< B
,B C
IMongoDatabaseD R
databaseS [
,[ \ 
ISchoolScheduleProxy  
scheduleProxy! .
,. /
ICourseService0 >
courseService? L
)L M
{ 	
_logger 
= 
logger 
; 
	_database 
= 
database  
;  !
_scheduleProxy 
= 
scheduleProxy *
;* +
_courseService 
= 
courseService *
;* +
} 	
public 
async 
Task 
AddAsync "
(" #

StudyGroup# -
entityToAdd. 9
)9 :
{   	
entityToAdd!! 
.!! 
Id!! 
=!! 
Guid!! !
.!!! "
NewGuid!!" )
(!!) *
)!!* +
;!!+ ,
await"" 
	_database"" 
."" 
GetCollection"" )
<"") *

StudyGroup""* 4
>""4 5
(""5 6
nameof""6 <
(""< =

StudyGroup""= G
)""G H
)""H I
.""I J
InsertOneAsync""J X
(""X Y
entityToAdd""Y d
)""d e
;""e f
}## 	
public%% 
async%% 
Task%% 
<%% 

StudyGroup%% $
>%%$ %
FindByIdAsync%%& 3
(%%3 4
Guid%%4 8
guid%%9 =
)%%= >
{&& 	
var'' 

collection'' 
='' 
	_database'' &
.''& '
GetCollection''' 4
<''4 5

StudyGroup''5 ?
>''? @
(''@ A
nameof''A G
(''G H

StudyGroup''H R
)''R S
)''S T
;''T U
return(( 
await(( 

collection(( #
.((# $
Find(($ (
(((( )
x(() *
=>((+ -
x((. /
.((/ 0
Id((0 2
.((2 3
Equals((3 9
(((9 :
guid((: >
)((> ?
)((? @
.((@ A
FirstOrDefaultAsync((A T
(((T U
)((U V
;((V W
})) 	
public++ 
virtual++ 
async++ 
Task++ !
<++! "

StudyGroup++" ,
>++, -
GetStudyGroupAsync++. @
(++@ A
string++A G
studyGroupNumber++H X
)++X Y
{,, 	
var-- 

collection-- 
=-- 
	_database-- &
.--& '
GetCollection--' 4
<--4 5

StudyGroup--5 ?
>--? @
(--@ A
nameof--A G
(--G H

StudyGroup--H R
)--R S
)--S T
;--T U
var.. 
group.. 
=.. 
await.. 

collection.. (
...( )
Find..) -
(..- .
x... /
=>..0 2
x..3 4
...4 5
	GroupName..5 >
...> ?
Equals..? E
(..E F
studyGroupNumber..F V
...V W
ToUpper..W ^
(..^ _
).._ `
)..` a
)..a b
...b c
FirstOrDefaultAsync..c v
(..v w
)..w x
;..x y
if00 
(00 
group00 
==00 
null00 
)00 
{11 
var22 
schedule22 
=22 
_scheduleProxy22 -
.22- .
GetByStudyGroup22. =
(22= >
studyGroupNumber22> N
)22N O
;22O P
if33 
(33 
schedule33 
==33 
null33  $
)33$ %
{44 
_logger55 
.55 
LogError55 $
(55$ %
$"55% '4
(Unable to load schedule for study group 55' O
{55O P
studyGroupNumber55P `
}55` a*
. Schedule proxy returned null55a 
"	55 Ä
)
55Ä Å
;
55Å Ç
return66 
null66 
;66  
}77 
	Timetable88 
t88 
=88 
await88 # 
ConverterApiToDomain88$ 8
.888 9)
ConvertTimetableForGroupAsync889 V
(88V W
schedule88W _
,88_ `
_courseService88a o
)88o p
;88p q
group99 
=99 
new99 

StudyGroup99 &
(99& '
)99' (
;99( )
group:: 
.:: 
	Timetable:: 
=::  !
t::" #
;::# $
group;; 
.;; 
	GroupName;; 
=;;  !
studyGroupNumber;;" 2
;;;2 3
group<< 
.<< 
Courses<< 
=<< 
t<<  !
.<<! "
	AllBlocks<<" +
.<<+ ,
Select<<, 2
(<<2 3
x<<3 4
=><<5 7
x<<8 9
.<<9 :
CourseId<<: B
)<<B C
.<<C D
ToList<<D J
(<<J K
)<<K L
;<<L M
await== 
AddAsync== 
(== 
group== $
)==$ %
;==% &
}?? 
returnAA 
groupAA 
;AA 
}BB 	
}CC 
}DD ÿ[
*C:\Projects\Swapify\Backend\UserService.cs
	namespace 	
FRITeam
 
. 
Swapify 
. 
Backend !
{ 
public 

class 
UserService 
: 
IUserService +
{ 
private 
readonly 
EnvironmentSettings , 
_environmentSettings- A
;A B
private 
readonly 
UserManager $
<$ %
User% )
>) *
_userManager+ 7
;7 8
private 
readonly 
SignInManager &
<& '
User' +
>+ ,
_signInManager- ;
;; <
public 
UserService 
( 
IOptions #
<# $
EnvironmentSettings$ 7
>7 8
environmentSettings9 L
,L M
UserManagerN Y
<Y Z
UserZ ^
>^ _
userManager` k
,k l
SignInManager 
< 
User 
> 
signInManager  -
)- .
{ 	 
_environmentSettings  
=! "
environmentSettings# 6
.6 7
Value7 <
;< =
_userManager 
= 
userManager &
;& '
_signInManager 
= 
signInManager *
;* +
} 	
public 
async 
Task 
< 
JwtSecurityToken *
>* +
Authenticate, 8
(8 9
string9 ?
login@ E
,E F
stringG M
passwordN V
)V W
{ 	
var 
result 
= 
await 
_signInManager -
.- .
PasswordSignInAsync. A
(A B
loginB G
,G H
passwordI Q
,Q R
falseS X
,X Y
falseZ _
)_ `
;` a
if   
(   
result   
.   
	Succeeded    
)    !
return!! 
GenerateJwtToken!! '
(!!' (
login!!( -
)!!- .
;!!. /
return"" 
null"" 
;"" 
}## 	
public%% 
JwtSecurityToken%% 
Renew%%  %
(%%% &
string%%& ,
jwtToken%%- 5
)%%5 6
{&& 	
var'' 
secret'' 
='' 
Encoding'' !
.''! "
ASCII''" '
.''' (
GetBytes''( 0
(''0 1 
_environmentSettings''1 E
.''E F
	JwtSecret''F O
)''O P
;''P Q
var(( 
tokenHandler(( 
=(( 
new(( "#
JwtSecurityTokenHandler((# :
(((: ;
)((; <
;((< =
var))  
validationParameters)) $
=))% &
new))' *%
TokenValidationParameters))+ D
{** 
ValidateAudience++  
=++! "
false++# (
,++( )
ValidateIssuer,, 
=,,  
false,,! &
,,,& '$
ValidateIssuerSigningKey-- (
=--) *
true--+ /
,--/ 0
ValidateLifetime..  
=..! "
false..# (
,..( )
IssuerSigningKey//  
=//! "
new//# & 
SymmetricSecurityKey//' ;
(//; <
secret//< B
)//B C
}00 
;00 
var22 
claims22 
=22 
tokenHandler22 %
.22% &
ValidateToken22& 3
(223 4
jwtToken224 <
,22< = 
validationParameters22> R
,22R S
out22T W
var22X [
validatedToken22\ j
)22j k
;22k l
var33 
jwtSecurityToken33  
=33! "
validatedToken33# 1
as332 4
JwtSecurityToken335 E
;33E F
if44 
(44 
jwtSecurityToken44  
==44! #
null44$ (
||44) +
!44, -
jwtSecurityToken44- =
.44= >
Header44> D
.44D E
Alg44E H
.44H I
Equals44I O
(44O P
SecurityAlgorithms44P b
.44b c

HmacSha25644c m
,44m n
StringComparison44o 
.	44 Ä(
InvariantCultureIgnoreCase
44Ä ö
)
44ö õ
)
44õ ú
throw55 
new55 "
SecurityTokenException55 0
(550 1
$str551 @
)55@ A
;55A B
var77 
login77 
=77 
claims77 
.77 
Identity77 '
.77' (
Name77( ,
;77, -
return88 
GenerateJwtToken88 #
(88# $
login88$ )
)88) *
;88* +
}99 	
public;; 
async;; 
Task;; 
<;; 
IdentityResult;; (
>;;( )
AddUserAsync;;* 6
(;;6 7
User;;7 ;
user;;< @
,;;@ A
string;;B H
password;;I Q
);;Q R
{<< 	
return== 
await== 
_userManager== %
.==% &
CreateAsync==& 1
(==1 2
user==2 6
,==6 7
password==8 @
)==@ A
;==A B
}>> 	
public@@ 
async@@ 
Task@@ 
<@@ 
string@@  
>@@  !/
#GenerateEmailConfirmationTokenAsync@@" E
(@@E F
User@@F J
user@@K O
)@@O P
{AA 	
stringBB 
tokenBB 
=BB 
awaitBB  
_userManagerBB! -
.BB- ./
#GenerateEmailConfirmationTokenAsyncBB. Q
(BBQ R
userBBR V
)BBV W
;BBW X
returnCC 
UriCC 
.CC 
EscapeDataStringCC '
(CC' (
tokenCC( -
)CC- .
;CC. /
}DD 	
publicFF 
asyncFF 
TaskFF 
<FF 
stringFF  
>FF  !+
GeneratePasswordResetTokenAsyncFF" A
(FFA B
UserFFB F
userFFG K
)FFK L
{GG 	
stringHH 
tokenHH 
=HH 
awaitHH  
_userManagerHH! -
.HH- .+
GeneratePasswordResetTokenAsyncHH. M
(HHM N
userHHN R
)HHR S
;HHS T
returnII 
UriII 
.II 
EscapeDataStringII '
(II' (
tokenII( -
)II- .
;II. /
}JJ 	
publicLL 
asyncLL 
TaskLL 
<LL 
IdentityResultLL (
>LL( )
ResetPasswordAsyncLL* <
(LL< =
UserLL= A
userLLB F
,LLF G
stringLLH N
tokenLLO T
,LLT U
stringLLV \
newPasswordLL] h
)LLh i
{MM 	
tokenNN 
=NN 
UriNN 
.NN 
UnescapeDataStringNN *
(NN* +
tokenNN+ 0
)NN0 1
;NN1 2
returnOO 
awaitOO 
_userManagerOO %
.OO% &
ResetPasswordAsyncOO& 8
(OO8 9
userOO9 =
,OO= >
tokenOO? D
,OOD E
newPasswordOOF Q
)OOQ R
;OOR S
}PP 	
publicRR 
asyncRR 
TaskRR 
<RR 
IdentityResultRR (
>RR( )
ConfirmEmailAsyncRR* ;
(RR; <
UserRR< @
userRRA E
,RRE F
stringRRG M
tokenRRN S
)RRS T
{SS 	
tokenTT 
=TT 
UriTT 
.TT 
UnescapeDataStringTT *
(TT* +
tokenTT+ 0
)TT0 1
;TT1 2
returnUU 
awaitUU 
_userManagerUU %
.UU% &
ConfirmEmailAsyncUU& 7
(UU7 8
userUU8 <
,UU< =
tokenUU> C
)UUC D
;UUD E
}VV 	
publicXX 
asyncXX 
TaskXX 
<XX 
UserXX 
>XX 
GetUserByEmailAsyncXX  3
(XX3 4
stringXX4 :
emailXX; @
)XX@ A
{YY 	
returnZZ 
awaitZZ 
_userManagerZZ %
.ZZ% &
FindByEmailAsyncZZ& 6
(ZZ6 7
emailZZ7 <
)ZZ< =
;ZZ= >
}[[ 	
public]] 
async]] 
Task]] 
<]] 
User]] 
>]] 
GetUserByIdAsync]]  0
(]]0 1
string]]1 7
userId]]8 >
)]]> ?
{^^ 	
return__ 
await__ 
_userManager__ %
.__% &
FindByIdAsync__& 3
(__3 4
userId__4 :
)__: ;
;__; <
}`` 	
publicbb 
asyncbb 
Taskbb 
<bb 
IdentityResultbb (
>bb( )
DeleteUserAsycbb* 8
(bb8 9
Userbb9 =
userbb> B
)bbB C
{cc 	
returndd 
awaitdd 
_userManagerdd %
.dd% &
DeleteAsyncdd& 1
(dd1 2
userdd2 6
)dd6 7
;dd7 8
}ee 	
privategg 
JwtSecurityTokengg  
GenerateJwtTokengg! 1
(gg1 2
stringgg2 8
logingg9 >
)gg> ?
{hh 	
varii 
tokenHandlerii 
=ii 
newii "#
JwtSecurityTokenHandlerii# :
(ii: ;
)ii; <
;ii< =
varjj 
secretjj 
=jj 
Encodingjj !
.jj! "
ASCIIjj" '
.jj' (
GetBytesjj( 0
(jj0 1 
_environmentSettingsjj1 E
.jjE F
	JwtSecretjjF O
)jjO P
;jjP Q
varkk 
tokenDescriptorkk 
=kk  !
newkk" %#
SecurityTokenDescriptorkk& =
{ll 
Subjectmm 
=mm 
newmm 
ClaimsIdentitymm ,
(mm, -
newmm- 0
[mm0 1
]mm1 2
{mm3 4
newmm5 8
Claimmm9 >
(mm> ?

ClaimTypesmm? I
.mmI J
NamemmJ N
,mmN O
loginmmP U
)mmU V
}mmW X
)mmX Y
,mmY Z
Expiresnn 
=nn 
DateTimenn "
.nn" #
UtcNownn# )
.nn) *
AddHoursnn* 2
(nn2 3
$numnn3 4
)nn4 5
,nn5 6
SigningCredentialsoo "
=oo# $
newoo% (
SigningCredentialsoo) ;
(oo; <
newoo< ? 
SymmetricSecurityKeyoo@ T
(ooT U
secretooU [
)oo[ \
,oo\ ]
SecurityAlgorithmsoo^ p
.oop q 
HmacSha256Signature	ooq Ñ
)
ooÑ Ö
}pp 
;pp 
varqq 
tokenqq 
=qq 
(qq 
JwtSecurityTokenqq )
)qq) *
tokenHandlerqq* 6
.qq6 7
CreateTokenqq7 B
(qqB C
tokenDescriptorqqC R
)qqR S
;qqS T
returnrr 
tokenrr 
;rr 
}ss 	
publicuu 
asyncuu 
Taskuu 
<uu 
IdentityResultuu (
>uu( )
UpdateUserAsyncuu* 9
(uu9 :
Useruu: >
userToUpdateuu? K
)uuK L
{vv 	
returnww 
awaitww 
_userManagerww %
.ww% &
UpdateAsyncww& 1
(ww1 2
userToUpdateww2 >
)ww> ?
;ww? @
}xx 	
}yy 
}zz 