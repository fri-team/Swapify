Ÿ
8C:\Projects\Swapify\WebAPI\Controllers\BaseController.cs
	namespace 	
WebAPI
 
. 
Controllers 
{ 
public 

class 
BaseController 
:  !

Controller" ,
{		 
[

 	
	NonAction

	 
]

 
public 
IActionResult 
ValidationError ,
(, - 
ModelStateDictionary- A
keyValuePairsB O
)O P
{ 	
return 
new #
ValidationErrorResponse .
(. /
keyValuePairs/ <
)< =
.= >
ToResult> F
(F G
)G H
;H I
} 	
[ 	
	NonAction	 
] 
public 
IActionResult 
ValidationError ,
(, -

Dictionary- 7
<7 8
string8 >
,> ?
string@ F
[F G
]G H
>H I
keyValuePairsJ W
)W X
{ 	
return 
new #
ValidationErrorResponse .
(. /
keyValuePairs/ <
)< =
.= >
ToResult> F
(F G
)G H
;H I
} 	
[ 	
	NonAction	 
] 
public 
IActionResult 
ErrorResponse *
(* +
string+ 1
error2 7
)7 8
{ 	
return 
new 
ErrorResponse $
($ %
error% *
)* +
.+ ,
ToResult, 4
(4 5
)5 6
;6 7
} 	
} 
} ¡
<C:\Projects\Swapify\WebAPI\Controllers\ExchangeController.cs
	namespace 	
WebAPI
 
. 
Controllers 
{ 
[ 
Produces 
( 
$str  
)  !
]! "
[ 
Route 

(
 
$str 
) 
] 
public 

class 
ExchangeController #
:$ %

Controller& 0
{ 
private 
readonly  
IBlockChangesService - 
_blockChangesService. B
;B C
public 
ExchangeController !
(! " 
IBlockChangesService" 6
blockChangeService7 I
)I J
:K L
baseM Q
(Q R
)R S
{ 	 
_blockChangesService  
=! "
blockChangeService# 5
;5 6
} 	
[ 	
HttpPost	 
] 
[ 	
Route	 
( 
$str  
)  !
]! "
public 
async 
Task 
< 
IActionResult '
>' (
ExchangeConfirm) 8
(8 9
[9 :
FromBody: B
]B C 
ExchangeRequestModelC W
requestX _
)_ `
{ 	
var 
blockChangeRequest "
=# $
new% (
BlockChangeRequest) ;
(; <
)< =
;= >
blockChangeRequest 
. 
	BlockFrom (
=) *!
BlockForExchangeModel+ @
.@ A
ConvertToBlockA O
(O P
requestP W
.W X
	BlockFromX a
)a b
;b c
blockChangeRequest 
. 
BlockTo &
=' (!
BlockForExchangeModel) >
.> ?
ConvertToBlock? M
(M N
requestN U
.U V
BlockToV ]
)] ^
;^ _
blockChangeRequest 
. 
Status %
=& '
ExchangeStatus( 6
.6 7
WaitingForExchange7 I
;I J
blockChangeRequest   
.   
DateOfCreation   -
=  . /
DateTime  0 8
.  8 9
Now  9 <
;  < =
blockChangeRequest!! 
.!! 
	StudentId!! (
=!!) *
Guid!!+ /
.!!/ 0
Parse!!0 5
(!!5 6
request!!6 =
.!!= >
	StudentId!!> G
)!!G H
;!!H I
var## 
res## 
=## 
await##  
_blockChangesService## 0
.##0 1
AddAndFindMatch##1 @
(##@ A
blockChangeRequest##A S
)##S T
;##T U
return$$ 
Ok$$ 
($$ 
res$$ 
)$$ 
;$$ 
}%% 	
}&& 
}'' §
8C:\Projects\Swapify\WebAPI\Controllers\HomeController.cs
	namespace 	
WebAPI
 
. 
Controllers 
{ 
public 

class 
HomeController 
:  !

Controller" ,
{ 
public 
IActionResult 
RouteToReact )
() *
)* +
{ 	
return		 
RedirectPermanent		 $
(		$ %
$str		% (
)		( )
;		) *
}

 	
} 
} ¶E
;C:\Projects\Swapify\WebAPI\Controllers\StudentController.cs
	namespace		 	
WebAPI		
 
.		 
Controllers		 
{

 
[ 
Produces 
( 
$str  
)  !
]! "
[ 
Route 

(
 
$str 
) 
] 
public 

class 
StudentController "
:# $
BaseController% 3
{ 
private 
readonly 
IStudentService (
_studentService) 8
;8 9
private 
readonly 
IUserService %
_userService& 2
;2 3
public 
StudentController  
(  !
IStudentService! 0
studentService1 ?
,? @
IUserServiceA M
userServiceN Y
)Y Z
{ 	
_studentService 
= 
studentService ,
;, -
_userService 
= 
userService &
;& '
} 	
[ 	
HttpGet	 
( 
$str &
)& '
]' (
public 
async 
Task 
< 
IActionResult '
>' (
GetStudentTimetable) <
(< =
string= C
	studentIdD M
)M N
{ 	
bool 
isValidGUID 
= 
Guid #
.# $
TryParse$ ,
(, -
	studentId- 6
,6 7
out8 ;
Guid< @
guidA E
)E F
;F G
if 
( 
! 
isValidGUID 
) 
{ 
return   
ErrorResponse   $
(  $ %
$"  % '
Student id:   ' 3
{  3 4
	studentId  4 =
}  = >
 is not valid GUID.  > Q
"  Q R
)  R S
;  S T
}!! 
var## 
student## 
=## 
await## 
_studentService##  /
.##/ 0
FindByIdAsync##0 =
(##= >
guid##> B
)##B C
;##C D
if%% 
(%% 
student%% 
==%% 
null%% 
)%%  
{&& 
return'' 
ErrorResponse'' $
(''$ %
$"''% '
Student with id: ''' 8
{''8 9
	studentId''9 B
}''B C
 does not exist.''C S
"''S T
)''T U
;''U V
}(( 
if** 
(** 
student** 
.** 
	Timetable** !
==**" $
null**% )
)**) *
{++ 
return,, 
ErrorResponse,, $
(,,$ %
$",,% '+
Timetable for student with id: ,,' F
{,,F G
	studentId,,G P
},,P Q
 does not exist.,,Q a
",,a b
),,b c
;,,c d
}-- 
return// 
Ok// 
(// 
student// 
.// 
	Timetable// '
)//' (
;//( )
}00 	
[22 	
HttpPost22	 
(22 
$str22 
)22  
]22  !
public33 
async33 
Task33 
<33 
IActionResult33 '
>33' (
AddNewBlock33) 4
(334 5
[335 6
FromBody336 >
]33> ?
AddNewBlockModel33? O
newBlockModel33P ]
)33] ^
{44 	
var55 
_user55 
=55 
await55 
_userService55 *
.55* +
GetUserByEmailAsync55+ >
(55> ?
newBlockModel55? L
.55L M
User55M Q
.55Q R
Email55R W
)55W X
;55X Y
var77 
student77 
=77 
_user77 
.77  
Student77  '
;77' (
if99 
(99 
student99 
==99 
null99 
)99  
{:: 
return;; 
ErrorResponse;; $
(;;$ %
$";;% '
Student with id: ;;' 8
{;;8 9
student;;9 @
.;;@ A
Id;;A C
};;C D
 does not exist.;;D T
";;T U
);;U V
;;;V W
}<< 
if>> 
(>> 
student>> 
.>> 
	Timetable>> !
==>>" $
null>>% )
)>>) *
{?? 
return@@ 
ErrorResponse@@ $
(@@$ %
$"@@% '+
Timetable for student with id: @@' F
{@@F G
student@@G N
.@@N O
Id@@O Q
}@@Q R
 does not exist.@@R b
"@@b c
)@@c d
;@@d e
}AA 
studentCC 
.CC 
	TimetableCC 
.CC 
AddNewBlockCC )
(CC) *
newBlockModelCC* 7
.CC7 8
BlockCC8 =
)CC= >
;CC> ?
awaitDD 
_studentServiceDD !
.DD! "
UpdateStudentAsyncDD" 4
(DD4 5
studentDD5 <
)DD< =
;DD= >
returnFF 
OkFF 
(FF 
newBlockModelFF #
.FF# $
BlockFF$ )
)FF) *
;FF* +
}GG 	
[JJ 	

HttpDeleteJJ	 
(JJ 
$strJJ ]
)JJ] ^
]JJ^ _
publicKK 
asyncKK 
TaskKK 
<KK 
IActionResultKK '
>KK' (
RemoveBlockKK) 4
(KK4 5
stringKK5 ;
	studentIdKK< E
,KKE F
DayLL5 8
dayLL9 <
,LL< =
stringMM5 ;
teacherMM< C
,MMC D
stringNN5 ;
roomNN< @
,NN@ A
byteOO5 9
	startHourOO: C
,OOC D
bytePP5 9
durationPP: B
,PPB C
	BlockTypeQQ5 >
typeQQ? C
)QQC D
{RR 	
BlockSS 
blockSS 
=SS 
newSS 
BlockSS #
(SS# $
typeSS$ (
,SS( )
daySS* -
,SS- .
	startHourSS/ 8
,SS8 9
durationSS: B
,SSB C
roomSSD H
,SSH I
teacherSSJ Q
)SSQ R
;SSR S
boolUU 
isValidGUIDUU 
=UU 
GuidUU #
.UU# $
TryParseUU$ ,
(UU, -
	studentIdUU- 6
,UU6 7
outUU8 ;
GuidUU< @
guidUUA E
)UUE F
;UUF G
ifVV 
(VV 
!VV 
isValidGUIDVV 
)VV 
{WW 
returnXX 
ErrorResponseXX $
(XX$ %
$"XX% '
Student id: XX' 3
{XX3 4
	studentIdXX4 =
}XX= >
 is not valid GUID.XX> Q
"XXQ R
)XXR S
;XXS T
}YY 
var[[ 
student[[ 
=[[ 
await[[ 
_studentService[[  /
.[[/ 0
FindByIdAsync[[0 =
([[= >
guid[[> B
)[[B C
;[[C D
if]] 
(]] 
student]] 
==]] 
null]] 
)]]  
{^^ 
return__ 
ErrorResponse__ $
(__$ %
$"__% '
Student with id: __' 8
{__8 9
	studentId__9 B
}__B C
 does not exist.__C S
"__S T
)__T U
;__U V
}`` 
ifbb 
(bb 
studentbb 
.bb 
	Timetablebb !
==bb" $
nullbb% )
)bb) *
{cc 
returndd 
ErrorResponsedd $
(dd$ %
$"dd% '+
Timetable for student with id: dd' F
{ddF G
	studentIdddG P
}ddP Q
 does not exist.ddQ a
"dda b
)ddb c
;ddc d
}ee 
ifhh 
(hh 
studenthh 
.hh 
	Timetablehh !
.hh! "
RemoveBlockhh" -
(hh- .
blockhh. 3
)hh3 4
)hh4 5
{ii 
awaitjj 
_studentServicejj %
.jj% &
UpdateStudentAsyncjj& 8
(jj8 9
studentjj9 @
)jj@ A
;jjA B
}kk 
elsell 
{mm 
returnnn 
ErrorResponsenn $
(nn$ %
$"nn% '
Block nn' -
{nn- .
blocknn. 3
.nn3 4
ToStringnn4 <
(nn< =
)nn= >
}nn> ?'
 does not exist in student nn? Z
{nnZ [
	studentIdnn[ d
}nnd e
 timetable.nne p
"nnp q
)nnq r
;nnr s
}oo 
returnpp 
Okpp 
(pp 
)pp 
;pp 
}qq 	
}rr 
}ss Ái
=C:\Projects\Swapify\WebAPI\Controllers\TimetableController.cs
	namespace 	
WebAPI
 
. 
Controllers 
{ 
[ 
Route 

(
 
$str 
) 
] 
public 

class 
TimetableController $
:% &
BaseController' 5
{ 
private 
readonly 
ILogger  
<  !
TimetableController! 4
>4 5
_logger6 =
;= >
private 
readonly 
IStudyGroupService +
_groupService, 9
;9 :
private 
readonly 
IStudentService (
_studentService) 8
;8 9
private 
readonly 
IUserService %
_userService& 2
;2 3
public 
TimetableController "
(" #
ILogger# *
<* +
TimetableController+ >
>> ?
logger@ F
,F G
IStudyGroupServiceH Z
groupService[ g
,g h
IStudentService 
studentService *
,* +
IUserService, 8
userService9 D
)D E
{ 	
_logger 
= 
logger 
; 
_groupService 
= 
groupService (
;( )
_studentService 
= 
studentService ,
;, -
_userService 
= 
userService &
;& '
} 	
[ 	
HttpPost	 
( 
$str 0
)0 1
]1 2
public   
async   
Task   
<   
IActionResult   '
>  ' ((
SetStudentTimetableFromGroup  ) E
(  E F
[  F G
FromBody  G O
]  O P
StudentModel  Q ]
body  ^ b
)  b c
{!! 	
_logger"" 
."" 
LogInformation"" "
(""" #
$"""# %+
Setting timetable for student: ""% D
{""D E
body""E I
.""I J
Email""J O
}""O P
.""P Q
"""Q R
)""R S
;""S T
User## 
user## 
=## 
await## 
_userService## *
.##* +
GetUserByEmailAsync##+ >
(##> ?
body##? C
.##C D
Email##D I
)##I J
;##J K
if$$ 
($$ 
user$$ 
==$$ 
null$$ 
)$$ 
{%% 
_logger&& 
.&& 
LogError&&  
(&&  !
$"&&! #
User with email: &&# 4
{&&4 5
body&&5 9
.&&9 :
Email&&: ?
}&&? @
 does not exist.&&@ P
"&&P Q
)&&Q R
;&&R S
return'' 
ErrorResponse'' $
(''$ %
$"''% '
User with email: ''' 8
{''8 9
body''9 =
.''= >
Email''> C
}''C D
 does not exist.''D T
"''T U
)''U V
;''V W
}(( 

StudyGroup)) 
sg)) 
=)) 
await)) !
_groupService))" /
.))/ 0
GetStudyGroupAsync))0 B
())B C
body))C G
.))G H
GroupNumber))H S
)))S T
;))T U
if** 
(** 
sg** 
==** 
null** 
)** 
return++ 
ErrorResponse++ $
(++$ %
$"++% '%
Study group with number: ++' @
{++@ A
body++A E
.++E F
GroupNumber++F Q
}++Q R
 does not exist.++R b
"++b c
)++c d
;++d e
Student-- 
student-- 
=-- 
user-- "
.--" #
Student--# *
;--* +
if.. 
(.. 
student.. 
==.. 
null.. 
)..  
{// 
student00 
=00 
new00 
Student00 %
(00% &
)00& '
;00' (
await11 
_studentService11 %
.11% &
AddAsync11& .
(11. /
student11/ 6
)116 7
;117 8
user33 
.33 
Student33 
=33 
student33 &
;33& '
await44 
_studentService44 %
.44% &'
UpdateStudentTimetableAsync44& A
(44A B
student44B I
,44I J
sg44K M
)44M N
;44N O
await66 
_userService66 "
.66" #
UpdateUserAsync66# 2
(662 3
user663 7
)667 8
;668 9
return77 
Ok77 
(77 
student77 !
.77! "
	Timetable77" +
)77+ ,
;77, -
}88 
if99 
(99 
student99 
.99 

StudyGroup99 "
.99" #
Equals99# )
(99) *
sg99* ,
)99, -
)99- .
{:: 
return;; 
Ok;; 
(;; 
student;; !
.;;! "
	Timetable;;" +
);;+ ,
;;;, -
}<< 
else== 
{>> 
await?? 
_studentService?? %
.??% &'
UpdateStudentTimetableAsync??& A
(??A B
student??B I
,??I J
sg??K M
)??M N
;??N O
await@@ 
_userService@@ "
.@@" #
UpdateUserAsync@@# 2
(@@2 3
user@@3 7
)@@7 8
;@@8 9
returnAA 
OkAA 
(AA 
studentAA !
.AA! "
	TimetableAA" +
)AA+ ,
;AA, -
}BB 
}CC 	
[EE 	
HttpGetEE	 
(EE 
$strEE $
)EE$ %
]EE% &
publicFF 
IActionResultFF 
GetCourseTimetableFF /
(FF/ 0
stringFF0 6
courseIdFF7 ?
)FF? @
{GG 	
ifHH 
(HH 
!HH 
stringHH 
.HH 
EqualsHH 
(HH 
courseIdHH '
,HH' (
$strHH) /
)HH/ 0
)HH0 1
{II 
returnJJ 
ErrorResponseJJ $
(JJ$ %
$"JJ% '
Course with id: JJ' 7
{JJ7 8
courseIdJJ8 @
}JJ@ A
 does not exist.JJA Q
"JJQ R
)JJR S
;JJS T
}KK 
varLL 
	timetableLL 
=LL 
newLL 
	TimetableLL  )
{MM 
BlocksNN 
=NN 
newNN 
ListNN !
<NN! "
TimetableBlockNN" 0
>NN0 1
{OO 
newPP 
TimetableBlockPP &
{QQ 
IdRR 
=RR 
GuidRR  
.RR  !
NewGuidRR! (
(RR( )
)RR) *
.RR* +
ToStringRR+ 3
(RR3 4
)RR4 5
,RR5 6
DaySS 
=SS 
$numSS 
,SS  

StartBlockTT "
=TT# $
$numTT% &
,TT& '
EndBlockUU  
=UU! "
$numUU# $
,UU$ %

CourseNameVV "
=VV# $
$strVV% :
,VV: ;
CourseShortcutWW &
=WW' (
$strWW) /
,WW/ 0
RoomXX 
=XX 
$strXX &
,XX& '
TeacherYY 
=YY  !
$strYY" 2
,YY2 3
TypeZZ 
=ZZ 
TimetableBlockTypeZZ 1
.ZZ1 2

LaboratoryZZ2 <
}[[ 
,[[ 
new\\ 
TimetableBlock\\ &
{]] 
Id^^ 
=^^ 
Guid^^  
.^^  !
NewGuid^^! (
(^^( )
)^^) *
.^^* +
ToString^^+ 3
(^^3 4
)^^4 5
,^^5 6
Day__ 
=__ 
$num__ 
,__  

StartBlock`` "
=``# $
$num``% &
,``& '
EndBlockaa  
=aa! "
$numaa# $
,aa$ %

CourseNamebb "
=bb# $
$strbb% :
,bb: ;
CourseShortcutcc &
=cc' (
$strcc) /
,cc/ 0
Roomdd 
=dd 
$strdd &
,dd& '
Teacheree 
=ee  !
$stree" 2
,ee2 3
Typeff 
=ff 
TimetableBlockTypeff 1
.ff1 2

Laboratoryff2 <
}gg 
,gg 
newhh 
TimetableBlockhh &
{ii 
Idjj 
=jj 
Guidjj  
.jj  !
NewGuidjj! (
(jj( )
)jj) *
.jj* +
ToStringjj+ 3
(jj3 4
)jj4 5
,jj5 6
Daykk 
=kk 
$numkk 
,kk  

StartBlockll "
=ll# $
$numll% &
,ll& '
EndBlockmm  
=mm! "
$nummm# $
,mm$ %

CourseNamenn "
=nn# $
$strnn% :
,nn: ;
CourseShortcutoo &
=oo' (
$stroo) /
,oo/ 0
Roompp 
=pp 
$strpp &
,pp& '
Teacherqq 
=qq  !
$strqq" 2
,qq2 3
Typerr 
=rr 
TimetableBlockTyperr 1
.rr1 2

Laboratoryrr2 <
}ss 
,ss 
newtt 
TimetableBlocktt &
{uu 
Idvv 
=vv 
Guidvv  
.vv  !
NewGuidvv! (
(vv( )
)vv) *
.vv* +
ToStringvv+ 3
(vv3 4
)vv4 5
,vv5 6
Dayww 
=ww 
$numww 
,ww  

StartBlockxx "
=xx# $
$numxx% &
,xx& '
EndBlockyy  
=yy! "
$numyy# $
,yy$ %

CourseNamezz "
=zz# $
$strzz% :
,zz: ;
CourseShortcut{{ &
={{' (
$str{{) /
,{{/ 0
Room|| 
=|| 
$str|| &
,||& '
Teacher}} 
=}}  !
$str}}" /
,}}/ 0
Type~~ 
=~~ 
TimetableBlockType~~ 1
.~~1 2

Laboratory~~2 <
} 
, 
new
ÄÄ 
TimetableBlock
ÄÄ &
{
ÅÅ 
Id
ÇÇ 
=
ÇÇ 
Guid
ÇÇ  
.
ÇÇ  !
NewGuid
ÇÇ! (
(
ÇÇ( )
)
ÇÇ) *
.
ÇÇ* +
ToString
ÇÇ+ 3
(
ÇÇ3 4
)
ÇÇ4 5
,
ÇÇ5 6
Day
ÉÉ 
=
ÉÉ 
$num
ÉÉ 
,
ÉÉ  

StartBlock
ÑÑ "
=
ÑÑ# $
$num
ÑÑ% &
,
ÑÑ& '
EndBlock
ÖÖ  
=
ÖÖ! "
$num
ÖÖ# $
,
ÖÖ$ %

CourseName
ÜÜ "
=
ÜÜ# $
$str
ÜÜ% :
,
ÜÜ: ;
CourseShortcut
áá &
=
áá' (
$str
áá) /
,
áá/ 0
Room
àà 
=
àà 
$str
àà &
,
àà& '
Teacher
ââ 
=
ââ  !
$str
ââ" /
,
ââ/ 0
Type
ää 
=
ää  
TimetableBlockType
ää 1
.
ää1 2

Laboratory
ää2 <
}
ãã 
,
ãã 
new
åå 
TimetableBlock
åå &
{
çç 
Id
éé 
=
éé 
Guid
éé  
.
éé  !
NewGuid
éé! (
(
éé( )
)
éé) *
.
éé* +
ToString
éé+ 3
(
éé3 4
)
éé4 5
,
éé5 6
Day
èè 
=
èè 
$num
èè 
,
èè  

StartBlock
êê "
=
êê# $
$num
êê% '
,
êê' (
EndBlock
ëë  
=
ëë! "
$num
ëë# %
,
ëë% &

CourseName
íí "
=
íí# $
$str
íí% :
,
íí: ;
CourseShortcut
ìì &
=
ìì' (
$str
ìì) /
,
ìì/ 0
Room
îî 
=
îî 
$str
îî &
,
îî& '
Teacher
ïï 
=
ïï  !
$str
ïï" 2
,
ïï2 3
Type
ññ 
=
ññ  
TimetableBlockType
ññ 1
.
ññ1 2
Lecture
ññ2 9
}
óó 
,
óó 
new
òò 
TimetableBlock
òò &
{
ôô 
Id
öö 
=
öö 
Guid
öö  
.
öö  !
NewGuid
öö! (
(
öö( )
)
öö) *
.
öö* +
ToString
öö+ 3
(
öö3 4
)
öö4 5
,
öö5 6
Day
õõ 
=
õõ 
$num
õõ 
,
õõ  

StartBlock
úú "
=
úú# $
$num
úú% &
,
úú& '
EndBlock
ùù  
=
ùù! "
$num
ùù# $
,
ùù$ %

CourseName
ûû "
=
ûû# $
$str
ûû% :
,
ûû: ;
CourseShortcut
üü &
=
üü' (
$str
üü) /
,
üü/ 0
Room
†† 
=
†† 
$str
†† &
,
††& '
Teacher
°° 
=
°°  !
$str
°°" /
,
°°/ 0
Type
¢¢ 
=
¢¢  
TimetableBlockType
¢¢ 1
.
¢¢1 2

Laboratory
¢¢2 <
}
££ 
}
§§ 
}
•• 
;
•• 
return
¶¶ 
Ok
¶¶ 
(
¶¶ 
	timetable
¶¶ 
)
¶¶  
;
¶¶  !
}
ßß 	
}
®® 
}©© ˘ƒ
8C:\Projects\Swapify\WebAPI\Controllers\UserController.cs
	namespace 	
WebAPI
 
. 
Controllers 
{ 
[ 
Produces 
( 
$str  
)  !
]! "
[ 
Route 

(
 
$str 
) 
] 
public 

class 
UserController 
:  !
BaseController" 0
{ 
private 
readonly 
ILogger  
<  !
UserController! /
>/ 0
_logger1 8
;8 9
private 
readonly 
IUserService %
_userService& 2
;2 3
private 
readonly 
IEmailService &
_emailService' 4
;4 5
private 
readonly 
Uri 
_baseUrl %
;% &
public 
UserController 
( 
ILogger %
<% &
UserController& 4
>4 5
logger6 <
,< =
IUserService> J
userServiceK V
,V W
IEmailServiceX e
emailServicef r
,r s
IOptions 
< 
EnvironmentSettings (
>( )
environmentSettings* =
)= >
{ 	
_logger 
= 
logger 
; 
_userService 
= 
userService &
;& '
_emailService 
= 
emailService (
;( )
_baseUrl   
=   
new   
Uri   
(   
environmentSettings   2
.  2 3
Value  3 8
.  8 9
BaseUrl  9 @
)  @ A
;  A B
}!! 	
[## 	
AllowAnonymous##	 
]## 
[$$ 	
HttpPost$$	 
($$ 
$str$$ 
)$$ 
]$$ 
public%% 
async%% 
Task%% 
<%% 
IActionResult%% '
>%%' (
Register%%) 1
(%%1 2
[%%2 3
FromBody%%3 ;
]%%; <
RegisterModel%%= J
body%%K O
)%%O P
{&& 	
body'' 
.'' 
Email'' 
='' 
body'' 
.'' 
Email'' #
.''# $
ToLower''$ +
(''+ ,
)'', -
;''- .
User(( 
user(( 
=(( 
new(( 
User((  
(((  !
body((! %
.((% &
Email((& +
,((+ ,
body((- 1
.((1 2
Name((2 6
,((6 7
body((8 <
.((< =
Surname((= D
)((D E
;((E F
var)) 
	addResult)) 
=)) 
await)) !
_userService))" .
.)). /
AddUserAsync))/ ;
()); <
user))< @
,))@ A
body))B F
.))F G
Password))G O
)))O P
;))P Q
if** 
(** 
!** 
	addResult** 
.** 
	Succeeded** $
)**$ %
{++ 
_logger,, 
.,, 
LogInformation,, &
(,,& ' 
ControllerExtensions,,' ;
.,,; < 
IdentityErrorBuilder,,< P
(,,P Q
$",,Q S%
Error when creating user ,,S l
{,,l m
body,,m q
.,,q r
Email,,r w
},,w x 
. Identity errors: 	,,x ã
"
,,ã å
,
,,å ç
	addResult
,,é ó
.
,,ó ò
Errors
,,ò û
)
,,û ü
)
,,ü †
;
,,† °

Dictionary-- 
<-- 
string-- !
,--! "
string--# )
[--) *
]--* +
>--+ ,
identityErrors--- ;
=--< = 
ControllerExtensions--> R
.--R S&
IdentityErrorsToDictionary--S m
(--m n
	addResult--n w
.--w x
Errors--x ~
)--~ 
;	-- Ä
return.. 
ValidationError.. &
(..& '
identityErrors..' 5
)..5 6
;..6 7
}// 
_logger11 
.11 
LogInformation11 "
(11" #
$"11# %
User 11% *
{11* +
body11+ /
.11/ 0
Email110 5
}115 6
	 created.116 ?
"11? @
)11@ A
;11A B
string22 
token22 
=22 
await22  
_userService22! -
.22- ./
#GenerateEmailConfirmationTokenAsync22. Q
(22Q R
user22R V
)22V W
;22W X
user33 
=33 
await33 
_userService33 %
.33% &
GetUserByEmailAsync33& 9
(339 :
body33: >
.33> ?
Email33? D
)33D E
;33E F
string44 
callbackUrl44 
=44  
new44! $
Uri44% (
(44( )
_baseUrl44) 1
,441 2
$@"443 6
confirmEmail/446 C
{44C D
user44D H
.44H I
Id44I K
}44K L
/44L M
{44M N
token44N S
}44S T
"44T U
)44U V
.44V W
ToString44W _
(44_ `
)44` a
;44a b
if66 
(66 
!66 
_emailService66 
.66 !
SendConfirmationEmail66 4
(664 5
body665 9
.669 :
Email66: ?
,66? @
callbackUrl66A L
,66L M
$str66N a
)66a b
)66b c
{77 
_logger88 
.88 
LogError88  
(88  !
$"88! #:
.Error when sending confirmation email to user 88# Q
{88Q R
body88R V
.88V W
Email88W \
}88\ ]
.88] ^
"88^ _
)88_ `
;88` a
var99 
deleteResult99  
=99! "
await99# (
_userService99) 5
.995 6
DeleteUserAsyc996 D
(99D E
user99E I
)99I J
;99J K
if:: 
(:: 
deleteResult::  
.::  !
	Succeeded::! *
)::* +
_logger;; 
.;; 
LogInformation;; *
(;;* +
$";;+ -
User ;;- 2
{;;2 3
body;;3 7
.;;7 8
Email;;8 =
};;= >
	 deleted.;;> G
";;G H
);;H I
;;;I J
return<< 

BadRequest<< !
(<<! "
)<<" #
;<<# $
}== 
_logger>> 
.>> 
LogInformation>> "
(>>" #
$">># %'
Confirmation email to user >>% @
{>>@ A
user>>A E
.>>E F
Email>>F K
}>>K L
 sent.>>L R
">>R S
)>>S T
;>>T U
return?? 
Ok?? 
(?? 
)?? 
;?? 
}@@ 	
[BB 	
AllowAnonymousBB	 
]BB 
[CC 	
HttpPostCC	 
(CC 
$strCC  
)CC  !
]CC! "
publicDD 
asyncDD 
TaskDD 
<DD 
IActionResultDD '
>DD' (
ConfirmEmailDD) 5
(DD5 6
[DD6 7
FromBodyDD7 ?
]DD? @
ConfirmEmailModelDDA R
bodyDDS W
)DDW X
{EE 	
varFF 
userFF 
=FF 
awaitFF 
_userServiceFF )
.FF) *
GetUserByIdAsyncFF* :
(FF: ;
bodyFF; ?
.FF? @
UserIdFF@ F
)FFF G
;FFG H
ifGG 
(GG 
userGG 
==GG 
nullGG 
)GG 
{HH 
_loggerII 
.II 

LogWarningII "
(II" #
$"II# %=
1Invalid email confirmation attempt. User with id II% V
{IIV W
bodyIIW [
.II[ \
UserIdII\ b
}IIb c
 doesn't exist.IIc r
"IIr s
)IIs t
;IIt u
returnJJ 

BadRequestJJ !
(JJ! "
)JJ" #
;JJ# $
}KK 
ifMM 
(MM 
userMM 
.MM 
EmailConfirmedMM #
)MM# $
returnNN 
OkNN 
(NN 
)NN 
;NN 
varPP 
resultPP 
=PP 
awaitPP 
_userServicePP +
.PP+ ,
ConfirmEmailAsyncPP, =
(PP= >
userPP> B
,PPB C
bodyPPD H
.PPH I
TokenPPI N
)PPN O
;PPO P
ifQQ 
(QQ 
resultQQ 
.QQ 
	SucceededQQ  
)QQ  !
{RR 
_loggerSS 
.SS 
LogInformationSS &
(SS& '
$"SS' )
User SS) .
{SS. /
userSS/ 3
.SS3 4
EmailSS4 9
}SS9 :%
 confirmed email address.SS: S
"SSS T
)SST U
;SSU V
returnTT 
OkTT 
(TT 
)TT 
;TT 
}UU 
_loggerVV 
.VV 
LogInformationVV "
(VV" # 
ControllerExtensionsVV# 7
.VV7 8 
IdentityErrorBuilderVV8 L
(VVL M
$"VVM O*
Confirmation of email address VVO m
{VVm n
bodyVVn r
.VVr s
UserIdVVs y
}VVy z
 failed. Errors: 	VVz ã
"
VVã å
,
VVå ç
result
VVé î
.
VVî ï
Errors
VVï õ
)
VVõ ú
)
VVú ù
;
VVù û
returnWW 

BadRequestWW 
(WW 
)WW 
;WW  
}XX 	
[ZZ 	
AllowAnonymousZZ	 
]ZZ 
[[[ 	
HttpPost[[	 
([[ 
$str[[ .
)[[. /
][[/ 0
public\\ 
async\\ 
Task\\ 
<\\ 
IActionResult\\ '
>\\' (&
SendEmailConfirmTokenAgain\\) C
(\\C D
[\\D E
FromBody\\E M
]\\M N+
SendEmailConfirmTokenAgainModel\\O n
body\\o s
)\\s t
{]] 	
body^^ 
.^^ 
Email^^ 
=^^ 
body^^ 
.^^ 
Email^^ #
.^^# $
ToLower^^$ +
(^^+ ,
)^^, -
;^^- .
var__ 
user__ 
=__ 
await__ 
_userService__ )
.__) *
GetUserByEmailAsync__* =
(__= >
body__> B
.__B C
Email__C H
)__H I
;__I J
if`` 
(`` 
user`` 
==`` 
null`` 
)`` 
{aa 
_loggerbb 
.bb 

LogWarningbb "
(bb" #
$"bb# %:
.Invalid send confirmation email attempt. User bb% S
{bbS T
bodybbT X
.bbX Y
EmailbbY ^
}bb^ _
 doesn't exist.bb_ n
"bbn o
)bbo p
;bbp q
returncc 
ErrorResponsecc $
(cc$ %
$"cc% '
Pou≈æ√≠vateƒæ cc' 2
{cc2 3
bodycc3 7
.cc7 8
Emailcc8 =
}cc= >
 neexistuje.cc> J
"ccJ K
)ccK L
;ccL M
}dd 
ifff 
(ff 
userff 
.ff 
EmailConfirmedff #
)ff# $
returngg 
ErrorResponsegg $
(gg$ %
$"gg% '/
#Emailov√° adresa je u≈æ potvrden√°.gg' G
"ggG H
)ggH I
;ggI J
stringii 
tokenii 
=ii 
awaitii  
_userServiceii! -
.ii- ./
#GenerateEmailConfirmationTokenAsyncii. Q
(iiQ R
useriiR V
)iiV W
;iiW X
stringjj 
callbackUrljj 
=jj  
newjj! $
Urijj% (
(jj( )
_baseUrljj) 1
,jj1 2
$@"jj3 6
confirmEmail/jj6 C
{jjC D
userjjD H
.jjH I
IdjjI K
}jjK L
/jjL M
{jjM N
tokenjjN S
}jjS T
"jjT U
)jjU V
.jjV W
ToStringjjW _
(jj_ `
)jj` a
;jja b
ifll 
(ll 
!ll 
_emailServicell 
.ll !
SendConfirmationEmailll 4
(ll4 5
bodyll5 9
.ll9 :
Emailll: ?
,ll? @
callbackUrlllA L
,llL M
$strllN a
)lla b
)llb c
{mm 
_loggernn 
.nn 
LogErrornn  
(nn  !
$"nn! #:
.Error when sending confirmation email to user nn# Q
{nnQ R
bodynnR V
.nnV W
EmailnnW \
}nn\ ]
.nn] ^
"nn^ _
)nn_ `
;nn` a
returnoo 

BadRequestoo !
(oo! "
)oo" #
;oo# $
}pp 
_loggerqq 
.qq 
LogInformationqq "
(qq" #
$"qq# %'
Confirmation email to user qq% @
{qq@ A
userqqA E
.qqE F
EmailqqF K
}qqK L
 sent.qqL R
"qqR S
)qqS T
;qqT U
returnrr 
Okrr 
(rr 
)rr 
;rr 
}ss 	
[uu 	
AllowAnonymousuu	 
]uu 
[vv 	
HttpPostvv	 
(vv 
$strvv 
)vv 
]vv 
publicww 
asyncww 
Taskww 
<ww 
IActionResultww '
>ww' (
Loginww) .
(ww. /
[ww/ 0
FromBodyww0 8
]ww8 9

LoginModelww: D
bodywwE I
)wwI J
{xx 	
bodyyy 
.yy 
Emailyy 
=yy 
bodyyy 
.yy 
Emailyy #
.yy# $
ToLoweryy$ +
(yy+ ,
)yy, -
;yy- .
varzz 
userzz 
=zz 
awaitzz 
_userServicezz )
.zz) *
GetUserByEmailAsynczz* =
(zz= >
bodyzz> B
.zzB C
EmailzzC H
)zzH I
;zzI J
if{{ 
({{ 
user{{ 
=={{ 
null{{ 
){{ 
{|| 
_logger}} 
.}} 
LogInformation}} &
(}}& '
$"}}' )'
Invalid login attemp. User }}) D
{}}D E
body}}E I
.}}I J
Email}}J O
}}}O P
 doesn't exist.}}P _
"}}_ `
)}}` a
;}}a b
return~~ 
ErrorResponse~~ $
(~~$ %
$"~~% '
Pou≈æ√≠vateƒæ ~~' 2
{~~2 3
body~~3 7
.~~7 8
Email~~8 =
}~~= >
 neexistuje.~~> J
"~~J K
)~~K L
;~~L M
} 
if
ÅÅ 
(
ÅÅ 
!
ÅÅ 
user
ÅÅ 
.
ÅÅ 
EmailConfirmed
ÅÅ $
)
ÅÅ$ %
{
ÇÇ 
_logger
ÉÉ 
.
ÉÉ 
LogInformation
ÉÉ &
(
ÉÉ& '
$"
ÉÉ' ))
Invalid login attemp. User 
ÉÉ) D
{
ÉÉD E
body
ÉÉE I
.
ÉÉI J
Email
ÉÉJ O
}
ÉÉO P,
 didn't confirm email address.
ÉÉP n
"
ÉÉn o
)
ÉÉo p
;
ÉÉp q
return
ÑÑ 

StatusCode
ÑÑ !
(
ÑÑ! "
(
ÑÑ" #
int
ÑÑ# &
)
ÑÑ& '
HttpStatusCode
ÑÑ' 5
.
ÑÑ5 6
	Forbidden
ÑÑ6 ?
,
ÑÑ? @
$str
ÑÑA w
)
ÑÑw x
;
ÑÑx y
}
ÖÖ 
var
áá 
token
áá 
=
áá 
await
áá 
_userService
áá *
.
áá* +
Authenticate
áá+ 7
(
áá7 8
body
áá8 <
.
áá< =
Email
áá= B
,
ááB C
body
ááD H
.
ááH I
Password
ááI Q
)
ááQ R
;
ááR S
if
àà 
(
àà 
token
àà 
==
àà 
null
àà 
)
àà 
{
ââ 
_logger
ää 
.
ää 

LogWarning
ää "
(
ää" #
$"
ää# %)
Invalid login attemp. User 
ää% @
{
ää@ A
body
ääA E
.
ääE F
Email
ääF K
}
ääK L&
 entered wrong password.
ääL d
"
ääd e
)
ääe f
;
ääf g
return
ãã 
ErrorResponse
ãã $
(
ãã$ %
$str
ãã% C
)
ããC D
;
ããD E
}
åå 
var
éé 
authUser
éé 
=
éé 
new
éé $
AuthenticatedUserModel
éé 5
(
éé5 6
user
éé6 :
,
éé: ;
token
éé< A
)
ééA B
;
ééB C
return
èè 
Ok
èè 
(
èè 
authUser
èè 
)
èè 
;
èè  
}
êê 	
[
íí 	
HttpPost
íí	 
(
íí 
$str
íí 
)
íí 
]
íí 
public
ìì 
IActionResult
ìì 
Renew
ìì "
(
ìì" #
[
ìì# $
FromBody
ìì$ ,
]
ìì, -

RenewModel
ìì. 8
body
ìì9 =
)
ìì= >
{
îî 	
var
ïï 
token
ïï 
=
ïï 
_userService
ïï $
.
ïï$ %
Renew
ïï% *
(
ïï* +
body
ïï+ /
.
ïï/ 0
Token
ïï0 5
)
ïï5 6
;
ïï6 7
var
ññ 
authUser
ññ 
=
ññ 
new
ññ $
AuthenticatedUserModel
ññ 5
(
ññ5 6
token
ññ6 ;
)
ññ; <
;
ññ< =
return
óó 
Ok
óó 
(
óó 
authUser
óó 
)
óó 
;
óó  
}
òò 	
[
öö 	
AllowAnonymous
öö	 
]
öö 
[
õõ 	
HttpPost
õõ	 
(
õõ 
$str
õõ !
)
õõ! "
]
õõ" #
public
úú 
async
úú 
Task
úú 
<
úú 
IActionResult
úú '
>
úú' (
ResetPassword
úú) 6
(
úú6 7
[
úú7 8
FromBody
úú8 @
]
úú@ A 
ResetPasswordModel
úúB T
body
úúU Y
)
úúY Z
{
ùù 	
body
ûû 
.
ûû 
Email
ûû 
=
ûû 
body
ûû 
.
ûû 
Email
ûû #
.
ûû# $
ToLower
ûû$ +
(
ûû+ ,
)
ûû, -
;
ûû- .
var
üü 
user
üü 
=
üü 
await
üü 
_userService
üü )
.
üü) *!
GetUserByEmailAsync
üü* =
(
üü= >
body
üü> B
.
üüB C
Email
üüC H
)
üüH I
;
üüI J
if
†† 
(
†† 
user
†† 
==
†† 
null
†† 
)
†† 
{
°° 
_logger
¢¢ 
.
¢¢ 
LogInformation
¢¢ &
(
¢¢& '
$"
¢¢' )2
$Invalid password reset attemp. User 
¢¢) M
{
¢¢M N
body
¢¢N R
.
¢¢R S
Email
¢¢S X
}
¢¢X Y
 doesn't exist.
¢¢Y h
"
¢¢h i
)
¢¢i j
;
¢¢j k
return
££ 
ErrorResponse
££ $
(
££$ %
$"
££% '
Pou≈æ√≠vateƒæ 
££' 2
{
££2 3
body
££3 7
.
££7 8
Email
££8 =
}
££= >
 neexistuje.
££> J
"
££J K
)
££K L
;
££L M
}
§§ 
if
¶¶ 
(
¶¶ 
!
¶¶ 
user
¶¶ 
.
¶¶ 
EmailConfirmed
¶¶ $
)
¶¶$ %
{
ßß 
_logger
®® 
.
®® 
LogInformation
®® &
(
®®& '
$"
®®' )2
$Invalid password reset attemp. User 
®®) M
{
®®M N
body
®®N R
.
®®R S
Email
®®S X
}
®®X Y,
 didn't confirm email address.
®®Y w
"
®®w x
)
®®x y
;
®®y z
return
©© 
ErrorResponse
©© $
(
©©$ %
$"
©©% '>
0Najsk√¥r pros√≠m potvrƒè svoju emailov√∫ adresu.
©©' S
"
©©S T
)
©©T U
;
©©U V
}
™™ 
string
¨¨ 
token
¨¨ 
=
¨¨ 
await
¨¨  
_userService
¨¨! -
.
¨¨- .-
GeneratePasswordResetTokenAsync
¨¨. M
(
¨¨M N
user
¨¨N R
)
¨¨R S
;
¨¨S T
string
≠≠ 
callbackUrl
≠≠ 
=
≠≠  
new
≠≠! $
Uri
≠≠% (
(
≠≠( )
_baseUrl
≠≠) 1
,
≠≠1 2
$@"
≠≠3 6
set-new-password/
≠≠6 G
{
≠≠G H
user
≠≠H L
.
≠≠L M
Id
≠≠M O
}
≠≠O P
/
≠≠P Q
{
≠≠Q R
token
≠≠R W
}
≠≠W X
"
≠≠X Y
)
≠≠Y Z
.
≠≠Z [
ToString
≠≠[ c
(
≠≠c d
)
≠≠d e
;
≠≠e f
if
ÆÆ 
(
ÆÆ 
!
ÆÆ 
_emailService
ÆÆ 
.
ÆÆ #
SendConfirmationEmail
ÆÆ 4
(
ÆÆ4 5
body
ÆÆ5 9
.
ÆÆ9 :
Email
ÆÆ: ?
,
ÆÆ? @
callbackUrl
ÆÆA L
,
ÆÆL M
$str
ÆÆN d
)
ÆÆd e
)
ÆÆe f
{
ØØ 
_logger
∞∞ 
.
∞∞ 
LogError
∞∞  
(
∞∞  !
$"
∞∞! #>
0Error when sending password reset email to user 
∞∞# S
{
∞∞S T
body
∞∞T X
.
∞∞X Y
Email
∞∞Y ^
}
∞∞^ _
.
∞∞_ `
"
∞∞` a
)
∞∞a b
;
∞∞b c
return
±± 

BadRequest
±± !
(
±±! "
)
±±" #
;
±±# $
}
≤≤ 
return
≥≥ 
Ok
≥≥ 
(
≥≥ 
)
≥≥ 
;
≥≥ 
}
¥¥ 	
[
∂∂ 	
AllowAnonymous
∂∂	 
]
∂∂ 
[
∑∑ 	
HttpPost
∑∑	 
(
∑∑ 
$str
∑∑ "
)
∑∑" #
]
∑∑# $
public
∏∏ 
async
∏∏ 
Task
∏∏ 
<
∏∏ 
IActionResult
∏∏ '
>
∏∏' (
SetNewPassword
∏∏) 7
(
∏∏7 8
[
∏∏8 9
FromBody
∏∏9 A
]
∏∏A B!
SetNewPasswordModel
∏∏C V
body
∏∏W [
)
∏∏[ \
{
ππ 	
var
∫∫ 
user
∫∫ 
=
∫∫ 
await
∫∫ 
_userService
∫∫ )
.
∫∫) *
GetUserByIdAsync
∫∫* :
(
∫∫: ;
body
∫∫; ?
.
∫∫? @
UserId
∫∫@ F
)
∫∫F G
;
∫∫G H
if
ªª 
(
ªª 
user
ªª 
==
ªª 
null
ªª 
)
ªª 
{
ºº 
_logger
ΩΩ 
.
ΩΩ 
LogError
ΩΩ  
(
ΩΩ  !
$"
ΩΩ! #:
,Invalid password reset attemp. User with id 
ΩΩ# O
{
ΩΩO P
body
ΩΩP T
.
ΩΩT U
UserId
ΩΩU [
}
ΩΩ[ \
 doesn't exist.
ΩΩ\ k
"
ΩΩk l
)
ΩΩl m
;
ΩΩm n
return
ææ 

BadRequest
ææ !
(
ææ! "
)
ææ" #
;
ææ# $
}
øø 
var
¿¿ 
result
¿¿ 
=
¿¿ 
await
¿¿ 
_userService
¿¿ +
.
¿¿+ , 
ResetPasswordAsync
¿¿, >
(
¿¿> ?
user
¿¿? C
,
¿¿C D
body
¿¿E I
.
¿¿I J
Token
¿¿J O
,
¿¿O P
body
¿¿Q U
.
¿¿U V
Password
¿¿V ^
)
¿¿^ _
;
¿¿_ `
if
¡¡ 
(
¡¡ 
!
¡¡ 
result
¡¡ 
.
¡¡ 
	Succeeded
¡¡ !
)
¡¡! "
{
¬¬ 
_logger
√√ 
.
√√ 
LogInformation
√√ &
(
√√& '"
ControllerExtensions
√√' ;
.
√√; <"
IdentityErrorBuilder
√√< P
(
√√P Q
$"
√√Q S5
'Error when resetting password for user 
√√S z
{
√√z {
user
√√{ 
.√√ Ä
Email√√Ä Ö
}√√Ö Ü#
. Identity errors: √√Ü ô
"√√ô ö
,√√ö õ
result√√ú ¢
.√√¢ £
Errors√√£ ©
)√√© ™
)√√™ ´
;√√´ ¨

Dictionary
ƒƒ 
<
ƒƒ 
string
ƒƒ !
,
ƒƒ! "
string
ƒƒ# )
[
ƒƒ) *
]
ƒƒ* +
>
ƒƒ+ ,
identityErrors
ƒƒ- ;
=
ƒƒ< ="
ControllerExtensions
ƒƒ> R
.
ƒƒR S(
IdentityErrorsToDictionary
ƒƒS m
(
ƒƒm n
result
ƒƒn t
.
ƒƒt u
Errors
ƒƒu {
)
ƒƒ{ |
;
ƒƒ| }
return
≈≈ 
ValidationError
≈≈ &
(
≈≈& '
identityErrors
≈≈' 5
)
≈≈5 6
;
≈≈6 7
}
∆∆ 
return
«« 
Ok
«« 
(
«« 
)
«« 
;
«« 
}
»» 	
}
…… 
}   ⁄
=C:\Projects\Swapify\WebAPI\Extensions\ControllerExtensions.cs
	namespace 	
WebAPI
 
. 

Extensions 
{ 
public		 

static		 
class		  
ControllerExtensions		 ,
{

 
public 
static 
string  
IdentityErrorBuilder 1
(1 2
string2 8
baseErrorMessage9 I
,I J
IEnumerableK V
<V W
IdentityErrorW d
>d e
identityErrorsf t
)t u
{ 	
StringBuilder  
identityErrorBuilder .
=/ 0
identityErrors1 ?
.? @
	Aggregate@ I
(I J
new 
StringBuilder ,
(, -
baseErrorMessage- =
)= >
,> ?
( 
sb 
, 
x  !
)! "
=># %
sb& (
.( )
Append) /
(/ 0
$"0 2
{2 3
x3 4
.4 5
Description5 @
}@ A
"B C
)C D
)D E
;E F
return  
identityErrorBuilder '
.' (
ToString( 0
(0 1
)1 2
;2 3
} 	
internal 
static 

Dictionary "
<" #
string# )
,) *
string+ 1
[1 2
]2 3
>3 4&
IdentityErrorsToDictionary5 O
(O P
IEnumerableP [
<[ \
IdentityError\ i
>i j
errorsk q
)q r
{ 	
return 
errors 
. 
ToDictionary &
(& '
x' (
=>) +
x, -
.- .
Code. 2
,2 3
x4 5
=>6 8
new9 <
string= C
[C D
]D E
{F G
xH I
.I J
DescriptionJ U
}V W
)W X
;X Y
} 	
} 
} ê
=C:\Projects\Swapify\WebAPI\Filters\SettingValidationFilter.cs
	namespace 	
WebAPI
 
. 
Filters 
{		 
public

 

class

 #
SettingValidationFilter

 (
:

) *
IStartupFilter

+ 9
{ 
readonly 
IEnumerable 
< 
IValidatable )
>) *
_validatableObjects+ >
;> ?
public #
SettingValidationFilter &
(& '
IEnumerable' 2
<2 3
IValidatable3 ?
>? @
validatableObjectsA S
)S T
{ 	
_validatableObjects 
=  !
validatableObjects" 4
;4 5
} 	
public 
Action 
< 
IApplicationBuilder )
>) *
	Configure+ 4
(4 5
Action5 ;
<; <
IApplicationBuilder< O
>O P
nextQ U
)U V
{ 	
foreach 
( 
var 
validatableObject *
in+ -
_validatableObjects. A
)A B
{ 
validatableObject !
.! "
Validate" *
(* +
)+ ,
;, -
if 
( 
validatableObject %
.% &
Errors& ,
., -
Length- 3
!=4 6
$num7 8
)8 9
throw 
new 
SettingException .
(. /
validatableObject/ @
.@ A
ConfigFileNameA O
,O P
validatableObjectQ b
.b c
Errorsc i
.i j
ToStringj r
(r s
)s t
)t u
;u v
} 
return 
next 
; 
} 	
} 
} ◊
6C:\Projects\Swapify\WebAPI\Filters\ValidationFilter.cs
	namespace 	
WebAPI
 
. 
Filters 
{ 
public		 

class		 
ValidationFilter		 !
:		" #
IActionFilter		$ 1
{

 
private 
readonly 
ILogger  
<  !
ValidationFilter! 1
>1 2
_logger3 :
;: ;
public 
ValidationFilter 
(  
ILogger  '
<' (
ValidationFilter( 8
>8 9
logger: @
)@ A
{ 	
_logger 
= 
logger 
; 
} 	
public 
void 
OnActionExecuted $
($ %!
ActionExecutedContext% :
context; B
)B C
{ 	
} 	
public 
void 
OnActionExecuting %
(% &"
ActionExecutingContext& <
context= D
)D E
{ 	
if 
( 
! 
context 
. 

ModelState #
.# $
IsValid$ +
)+ ,
{ 
context 
. 
Result 
=  
new! $#
ValidationErrorResponse% <
(< =
context= D
.D E

ModelStateE O
)O P
.P Q
ToResultQ Y
(Y Z
)Z [
;[ \
StringBuilder 
modelStateBuilder /
=0 1
context2 9
.9 :

ModelState: D
.D E
ValuesE K
.K L

SelectManyL V
(V W
xW X
=>Y [
x\ ]
.] ^
Errors^ d
)d e
.e f
	Aggregatef o
(o p
new  #
StringBuilder$ 1
(1 2
$"2 4
ModelState errors: 4 G
"G H
)H I
,I J
(  !
sb! #
,# $
x% &
)& '
=>( *
sb+ -
.- .
Append. 4
(4 5
$"5 7
{7 8
x8 9
.9 :
ErrorMessage: F
}F G
"H I
)I J
)J K
;K L
_logger 
. 
LogInformation &
(& '
modelStateBuilder' 8
.8 9
ToString9 A
(A B
)B C
)C D
;D E
}   
}!! 	
}"" 
}## ñ
FC:\Projects\Swapify\WebAPI\Models\ErrorResponseModels\ErrorResponse.cs
	namespace 	
WebAPI
 
. 
Models 
. 
ErrorResponseModels +
{ 
public 

class 
ErrorResponse 
{ 
public 
string 
Error 
{ 
get !
;! "
}# $
public		 
ErrorResponse		 
(		 
string		 #
error		$ )
)		) *
{

 	
Error 
= 
error 
; 
} 	
public 
IActionResult 
ToResult %
(% &
)& '
{ 	
return 
new "
BadRequestObjectResult -
(- .
this. 2
)2 3
;3 4
} 	
} 
} í
PC:\Projects\Swapify\WebAPI\Models\ErrorResponseModels\ValidationErrorResponse.cs
	namespace 	
WebAPI
 
. 
Models 
. 
ErrorResponseModels +
{ 
public 

class #
ValidationErrorResponse (
{		 
public

 
IDictionary

 
<

 
string

 !
,

! "
string

# )
[

) *
]

* +
>

+ ,
Errors

- 3
{

4 5
get

6 9
;

9 :
}

; <
public #
ValidationErrorResponse &
(& ' 
ModelStateDictionary' ;

modelState< F
)F G
{ 	
Errors 
= 

modelState 
.  
ToDictionary  ,
(, -
x 
=> 
x 
. 
Key 
, 
x 
=> 
x 
. 
Value 
. 
Errors #
.# $
Select$ *
(* +
e+ ,
=>- /
e0 1
.1 2
ErrorMessage2 >
)> ?
.? @
ToArray@ G
(G H
)H I
) 
; 
} 	
public #
ValidationErrorResponse &
(& '
IDictionary' 2
<2 3
string3 9
,9 :
string; A
[A B
]B C
>C D
errorsE K
)K L
{ 	
Errors 
= 
errors 
; 
} 	
public 
IActionResult 
ToResult %
(% &
)& '
{ 	
return 
new "
BadRequestObjectResult -
(- .
this. 2
)2 3
;3 4
} 	
} 
} ⁄
JC:\Projects\Swapify\WebAPI\Models\ExchangesModels\BlockForExchangeModel.cs
	namespace		 	
WebAPI		
 
.		 
Models		 
.		 
	Exchanges		 !
{

 
public 

class !
BlockForExchangeModel &
{ 
public 
int 
Day 
{ 
get 
; 
set !
;! "
}# $
public 
int 
Duration 
{ 
get !
;! "
set# &
;& '
}( )
public 
int 
	StartHour 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	
Required	 
( 
ErrorMessage 
=  
$str! <
)< =
]= >
public 
string 
CourseId 
{  
get! $
;$ %
set& )
;) *
}+ ,
public !
BlockForExchangeModel $
($ %
)% &
{' (
}) *
public 
static 
Block 
ConvertToBlock *
(* +!
BlockForExchangeModel+ @
blockToConvertA O
)O P
{ 	
Block 
blc 
= 
new 
Block !
(! "
)" #
;# $
blc 
. 
CourseId 
= 
Guid 
.  
Parse  %
(% &
blockToConvert& 4
.4 5
CourseId5 =
)= >
;> ?
blc 
. 
Day 
= 
( 
Day 
) 
blockToConvert )
.) *
Day* -
;- .
blc 
. 
Duration 
= 
( 
byte  
)  !
blockToConvert! /
./ 0
Duration0 8
;8 9
blc 
. 
	StartHour 
= 
( 
byte !
)! "
blockToConvert" 0
.0 1
	StartHour1 :
;: ;
return 
blc 
; 
} 	
} 
}  

IC:\Projects\Swapify\WebAPI\Models\ExchangesModels\ExchangeRequestModel.cs
	namespace 	
WebAPI
 
. 
Models 
. 
	Exchanges !
{ 
public		 

class		  
ExchangeRequestModel		 %
{

 
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 8
)8 9
]9 :
public 
string 
	StudentId 
{  !
get" %
;% &
set' *
;* +
}, -
[ 	
Required	 
( 
ErrorMessage 
=  
$str! <
)< =
]= >
public !
BlockForExchangeModel $
	BlockFrom% .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
[ 	
Required	 
( 
ErrorMessage 
=  
$str! <
)< =
]= >
public !
BlockForExchangeModel $
BlockTo% ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
public  
ExchangeRequestModel #
(# $
)$ %
{& '
}( )
} 
} ä
9C:\Projects\Swapify\WebAPI\Models\Student\StudentModel.cs
	namespace 	
WebAPI
 
. 
Models 
. 

UserModels "
{ 
public 

class 
StudentModel 
{ 
[		 	
Required			 
(		 
ErrorMessage		 
=		  
$str		! E
)		E F
]		F G
public

 
string

 
GroupNumber

 !
{

" #
get

$ '
;

' (
set

) ,
;

, -
}

. /
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 4
)4 5
]5 6
[ 	
EmailAddress	 
( 
ErrorMessage "
=# $
$str% M
)M N
]N O
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
} 
} ˇ
EC:\Projects\Swapify\WebAPI\Models\TimetableModels\AddNewBlockModel.cs
	namespace 	
WebAPI
 
. 
Models 
. 

UserModels "
{ 
public 

class 
AddNewBlockModel !
{ 
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 9
)9 :
]: ;
public		 
User		 
User		 
{		 
get		 
;		 
set		  #
;		# $
}		% &
[

 	
Required

	 
(

 
ErrorMessage

 
=

  
$str

! 3
)

3 4
]

4 5
public 
Block 
Block 
{ 
get  
;  !
set" %
;% &
}' (
} 
} Ä
>C:\Projects\Swapify\WebAPI\Models\TimetableModels\Timetable.cs
	namespace 	
WebAPI
 
. 
Models 
. 
TimetableModels '
{ 
public 

class 
	Timetable 
{ 
public 
List 
< 
TimetableBlock "
>" #
Blocks$ *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
} 
}		 ≠
CC:\Projects\Swapify\WebAPI\Models\TimetableModels\TimetableBlock.cs
	namespace 	
WebAPI
 
. 
Models 
. 
TimetableModels '
{ 
public 

class 
TimetableBlock 
{ 
public 
string 
Id 
{ 
get 
; 
set  #
;# $
}% &
public 
int 
Day 
{ 
get 
; 
set !
;! "
}# $
public 
int 

StartBlock 
{ 
get  #
;# $
set% (
;( )
}* +
public 
int 
EndBlock 
{ 
get !
;! "
set# &
;& '
}( )
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
 
CourseShortcut
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
public 
string 
Room 
{ 
get  
;  !
set" %
;% &
}' (
public 
string 
Teacher 
{ 
get  #
;# $
set% (
;( )
}* +
public 
TimetableBlockType !
Type" &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
} 
} œ
GC:\Projects\Swapify\WebAPI\Models\TimetableModels\TimetableBlockType.cs
	namespace 	
WebAPI
 
. 
Models 
. 
TimetableModels '
{ 
[ 
JsonConverter 
( 
typeof 
( 
StringEnumConverter -
)- .
,. /
true0 4
)4 5
]5 6
public 

enum 
TimetableBlockType "
{ 
Lecture		 
=		 
$num		 
,		 

Laboratory

 
=

 
$num

 
,

 
	Excercise 
= 
$num 
} 
} ≥
FC:\Projects\Swapify\WebAPI\Models\UserModels\AuthenticatedUserModel.cs
	namespace 	
WebAPI
 
. 
Models 
. 

UserModels "
{ 
public 

class "
AuthenticatedUserModel '
{ 
public		 
string		 
UserName		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		+ ,
public

 
string

 
Email

 
{

 
get

 !
;

! "
set

# &
;

& '
}

( )
public 
string 
Name 
{ 
get  
;  !
set" %
;% &
}' (
public 
string 
Surname 
{ 
get  #
;# $
set% (
;( )
}* +
public 
string 
Token 
{ 
get !
;! "
set# &
;& '
}( )
public 
DateTime 
ValidTo 
{  !
get" %
;% &
set' *
;* +
}, -
public 
string 
	StudentId 
{  !
get" %
;% &
set' *
;* +
}, -
public "
AuthenticatedUserModel %
(% &
User& *
user+ /
,/ 0
JwtSecurityToken1 A
tokenB G
)G H
{ 	
if 
( 
user 
!= 
null 
) 
{ 
UserName 
= 
user 
.  
UserName  (
;( )
Email 
= 
user 
. 
Email "
;" #
Name 
= 
user 
. 
Name  
;  !
Surname 
= 
user 
. 
Surname &
;& '
	StudentId 
= 
user  
.  !
Student! (
?( )
.) *
Id* ,
., -
ToString- 5
(5 6
)6 7
;7 8
} 
Token 
= 
token 
. 
RawData !
;! "
ValidTo 
= 
token 
. 
ValidTo #
;# $
} 	
public "
AuthenticatedUserModel %
(% &
JwtSecurityToken& 6
token7 <
)< =
:> ?
this@ D
(D E
nullE I
,I J
tokenK P
)P Q
{   	
}!! 	
}"" 
}## Å
AC:\Projects\Swapify\WebAPI\Models\UserModels\ConfirmEmailModel.cs
	namespace 	
WebAPI
 
. 
Models 
. 

UserModels "
{ 
public 

class 
ConfirmEmailModel "
{ 
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 5
)5 6
]6 7
public 
string 
UserId 
{ 
get "
;" #
set$ '
;' (
}) *
[

 	
Required

	 
(

 
ErrorMessage

 
=

  
$str

! 4
)

4 5
]

5 6
public 
string 
Token 
{ 
get !
;! "
set# &
;& '
}( )
} 
} ı
:C:\Projects\Swapify\WebAPI\Models\UserModels\LoginModel.cs
	namespace 	
WebAPI
 
. 
Models 
. 

UserModels "
{ 
public 

class 

LoginModel 
{ 
[ 	
Required	 
( 
ErrorMessage 
=  
$str! A
)A B
]B C
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
[

 	
Required

	 
(

 
ErrorMessage

 
=

  
$str

! 4
)

4 5
]

5 6
public 
string 
Password 
{  
get! $
;$ %
set& )
;) *
}+ ,
} 
} ±
=C:\Projects\Swapify\WebAPI\Models\UserModels\RegisterModel.cs
	namespace 	
WebAPI
 
. 
Models 
. 

UserModels "
{ 
public 

class 
RegisterModel 
{ 
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 3
)3 4
]4 5
public 
string 
Name 
{ 
get  
;  !
set" %
;% &
}' (
[

 	
Required

	 
(

 
ErrorMessage

 
=

  
$str

! 9
)

9 :
]

: ;
public 
string 
Surname 
{ 
get  #
;# $
set% (
;( )
}* +
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 4
)4 5
]5 6
[ 	
EmailAddress	 
( 
ErrorMessage "
=# $
$str% M
)M N
]N O
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 4
)4 5
]5 6
[ 	
StringLength	 
( 
$num 
, 
ErrorMessage '
=( )
$str* R
,R S
MinimumLengthT a
=b c
$numd e
)e f
]f g
public 
string 
Password 
{  
get! $
;$ %
set& )
;) *
}+ ,
[ 	
Required	 
( 
ErrorMessage 
=  
$str! ?
)? @
]@ A
[ 	
Compare	 
( 
$str 
, 
ErrorMessage )
=* +
$str, A
)A B
]B C
public 
string 
PasswordAgain #
{$ %
get& )
;) *
set+ .
;. /
}0 1
} 
} Û
:C:\Projects\Swapify\WebAPI\Models\UserModels\RenewModel.cs
	namespace 	
WebAPI
 
. 
Models 
. 

UserModels "
{ 
public 

class 

RenewModel 
{ 
[ 	
Required	 
] 
public 
string 
Token 
{ 
get !
;! "
set# &
;& '
}( )
}		 
}

 Ì
BC:\Projects\Swapify\WebAPI\Models\UserModels\ResetPasswordModel.cs
	namespace 	
WebAPI
 
. 
Models 
. 

UserModels "
{ 
public 

class 
ResetPasswordModel #
{ 
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 4
)4 5
]5 6
[ 	
EmailAddress	 
( 
ErrorMessage "
=# $
$str% M
)M N
]N O
public		 
string		 
Email		 
{		 
get		 !
;		! "
set		# &
;		& '
}		( )
}

 
} á
OC:\Projects\Swapify\WebAPI\Models\UserModels\SendEmailConfirmTokenAgainModel.cs
	namespace 	
WebAPI
 
. 
Models 
. 

UserModels "
{ 
public 

class +
SendEmailConfirmTokenAgainModel 0
{ 
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 4
)4 5
]5 6
[ 	
EmailAddress	 
( 
ErrorMessage "
=# $
$str% M
)M N
]N O
public		 
string		 
Email		 
{		 
get		 !
;		! "
set		# &
;		& '
}		( )
}

 
} Ü
CC:\Projects\Swapify\WebAPI\Models\UserModels\SetNewPasswordModel.cs
	namespace 	
WebAPI
 
. 
Models 
. 

UserModels "
{ 
public 

class 
SetNewPasswordModel $
{ 
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 5
)5 6
]6 7
public 
string 
UserId 
{ 
get "
;" #
set$ '
;' (
}) *
[

 	
Required

	 
(

 
ErrorMessage

 
=

  
$str

! 4
)

4 5
]

5 6
[ 	
StringLength	 
( 
$num 
, 
ErrorMessage '
=( )
$str* R
,R S
MinimumLengthT a
=b c
$numd e
)e f
]f g
public 
string 
Password 
{  
get! $
;$ %
set& )
;) *
}+ ,
[ 	
Required	 
( 
ErrorMessage 
=  
$str! ?
)? @
]@ A
[ 	
Compare	 
( 
$str 
, 
ErrorMessage )
=* +
$str, A
)A B
]B C
public 
string 
PasswordAgain #
{$ %
get& )
;) *
set+ .
;. /
}0 1
[ 	
Required	 
( 
ErrorMessage 
=  
$str! 4
)4 5
]5 6
public 
string 
Token 
{ 
get !
;! "
set# &
;& '
}( )
} 
} ù
%C:\Projects\Swapify\WebAPI\Program.cs
	namespace 	
WebAPI
 
{		 
public

 

static

 
class

 
Program

 
{ 
public 
static 
int 
Main 
( 
string %
[% &
]& '
args( ,
), -
{ 	
string 
environment 
=  
Environment! ,
., -"
GetEnvironmentVariable- C
(C D
$strD \
)\ ]
;] ^
NLog 
. 
Logger 
logger 
=  
NLog! %
.% &

LogManager& 0
.0 1
LoadConfiguration1 B
(B C
$"C E
nlog.E J
{J K
environmentK V
}V W
.configW ^
"^ _
)_ `
.` a!
GetCurrentClassLoggera v
(v w
)w x
;x y
logger 
. 
Info 
( 
$" $
Application starting in  2
{2 3
environment3 >
}> ?
 environment? K
"K L
)L M
;M N
try 
{ 
BuildWebHost 
( 
args !
)! "
." #
Run# &
(& '
)' (
;( )
return 
$num 
; 
} 
catch 
( 
SettingException #
se$ &
)& '
{ 
logger 
. 
Error 
( 
$str I
+ 
$" 
{ 
se 
. 
ConfigFileName *
}* +
 \n+ .
{. /
se/ 1
.1 2
Message2 9
}9 :
": ;
); <
;< =
return 
$num 
; 
} 
catch 
( 
	Exception 
e 
) 
{ 
logger 
. 
Fatal 
( 
e 
, 
$"  "2
&Application terminated unexpectedly:\n" H
{H I
eI J
}J K
"K L
)L M
;M N
return   
$num   
;   
}!! 
finally"" 
{## 
NLog$$ 
.$$ 

LogManager$$ 
.$$  
Shutdown$$  (
($$( )
)$$) *
;$$* +
}%% 
}&& 	
public(( 
static(( 
IWebHost(( 
BuildWebHost(( +
(((+ ,
string((, 2
[((2 3
]((3 4
args((5 9
)((9 :
{)) 	
return** 
WebHost** 
.**  
CreateDefaultBuilder** /
(**/ 0
args**0 4
)**4 5
.++ 

UseStartup++ 
<++ 
Startup++ #
>++# $
(++$ %
)++% &
.,, 
ConfigureLogging,, !
(,,! "
logging,," )
=>,,* ,
{-- 
logging.. 
... 
ClearProviders.. *
(..* +
)..+ ,
;.., -
logging// 
.// 
SetMinimumLevel// +
(//+ ,
LogLevel//, 4
.//4 5
Trace//5 :
)//: ;
;//; <
}00 
)00 
.11 
UseNLog11 
(11 
)11 
.22 
Build22 
(22 
)22 
;22 
}33 	
}44 
}55 ˛∑
%C:\Projects\Swapify\WebAPI\Startup.cs
	namespace 	
WebAPI
 
{ 
public   

class   
Startup   
{!! 
private"" 
const"" 
string"" 
DatabaseName"" )
=""* +
$str"", 5
;""5 6
public## 
IConfiguration## 
Configuration## +
{##, -
get##. 1
;##1 2
}##3 4
public$$ 
IHostingEnvironment$$ "
Environment$$# .
{$$/ 0
get$$1 4
;$$4 5
}$$6 7
private%% 
readonly%% 
ILogger%%  
<%%  !
Startup%%! (
>%%( )
_logger%%* 1
;%%1 2
public'' 
Startup'' 
('' 
IConfiguration'' %
configuration''& 3
,''3 4
IHostingEnvironment''5 H
environment''I T
,''T U
ILoggerFactory''V d
loggerFactory''e r
)''r s
{(( 	
Configuration)) 
=)) 
configuration)) )
;))) *
Environment** 
=** 
environment** %
;**% &
DbRegistration++ 
.++ 
Init++ 
(++  
)++  !
;++! "
_logger,, 
=,, 
loggerFactory,, #
.,,# $
CreateLogger,,$ 0
<,,0 1
Startup,,1 8
>,,8 9
(,,9 :
),,: ;
;,,; <
}-- 	
public// 
void// 
ConfigureServices// %
(//% &
IServiceCollection//& 8
services//9 A
)//A B
{00 	
_logger11 
.11 
LogInformation11 "
(11" #
$str11# 9
)119 :
;11: ;
if22 
(22 
Environment22 
.22 
IsDevelopment22 )
(22) *
)22* +
)22+ ,
{33 
_logger44 
.44 
LogInformation44 &
(44& '
$str44' :
)44: ;
;44; <
Mongo2Go55 
.55 
MongoDbRunner55 &
.55& '
StartForDebugging55' 8
(558 9
)559 :
;55: ;
MongoClientSettings66 #
settings66$ ,
=66- .
new66/ 2
MongoClientSettings663 F
(66F G
)66G H
;66H I
settings77 
.77 
GuidRepresentation77 +
=77, -
GuidRepresentation77. @
.77@ A
Standard77A I
;77I J
services99 
.99 
AddSingleton99 %
(99% &
new99& )
MongoClient99* 5
(995 6
settings996 >
)99> ?
.99? @
GetDatabase99@ K
(99K L
DatabaseName99L X
)99X Y
)99Y Z
;99Z [
}:: #
LoadAndValidateSettings<< #
(<<# $
services<<$ ,
)<<, -
;<<- ."
ConfigureAuthorization== "
(==" #
services==# +
)==+ ,
;==, -
services?? 
.?? 
	AddScoped?? 
<?? 
IUserService?? +
,??+ ,
UserService??- 8
>??8 9
(??9 :
)??: ;
;??; <
services@@ 
.@@ 
AddSingleton@@ !
<@@! "
IStudentService@@" 1
,@@1 2
StudentService@@3 A
>@@A B
(@@B C
)@@C D
;@@D E
servicesAA 
.AA 
AddSingletonAA !
<AA! "
IStudyGroupServiceAA" 4
,AA4 5
StudyGroupServiceAA6 G
>AAG H
(AAH I
)AAI J
;AAJ K
servicesBB 
.BB 
AddSingletonBB !
<BB! "
IStudentServiceBB" 1
,BB1 2
StudentServiceBB3 A
>BBA B
(BBB C
)BBC D
;BBD E
servicesCC 
.CC 
AddSingletonCC !
<CC! "
ICourseServiceCC" 0
,CC0 1
CourseServiceCC2 ?
>CC? @
(CC@ A
)CCA B
;CCB C
servicesDD 
.DD 
AddSingletonDD !
<DD! " 
ISchoolScheduleProxyDD" 6
,DD6 7
SchoolScheduleProxyDD8 K
>DDK L
(DDL M
)DDM N
;DDN O
servicesEE 
.EE 
AddSingletonEE !
<EE! "
IEmailServiceEE" /
,EE/ 0
EmailServiceEE1 =
>EE= >
(EE> ?
)EE? @
;EE@ A
servicesFF 
.FF 
AddSingletonFF !
<FF! " 
IBlockChangesServiceFF" 6
,FF6 7
BlockChangesServiceFF8 K
>FFK L
(FFL M
)FFM N
;FFN O
servicesHH 
.HH $
ConfigureMongoDbIdentityHH -
<HH- .
UserHH. 2
,HH2 3
MongoIdentityRoleHH4 E
,HHE F
GuidHHG K
>HHK L
(HHL M
ConfigureIdentityHHM ^
(HH^ _
ConfigurationII 
.II 

GetSectionII (
(II( )
$strII) ;
)II; <
.II< =
GetII= @
<II@ A
IdentitySettingsIIA Q
>IIQ R
(IIR S
)IIS T
)IIT U
)IIU V
;IIV W
}JJ 	
publicLL 
voidLL 
	ConfigureLL 
(LL 
IApplicationBuilderLL 1
appLL2 5
,LL5 6
IHostingEnvironmentLL7 J
envLLK N
)LLN O
{MM 	
ifNN 
(NN 
envNN 
.NN 
IsDevelopmentNN !
(NN! "
)NN" #
)NN# $
{OO 
appPP 
.PP %
UseDeveloperExceptionPagePP -
(PP- .
)PP. /
;PP/ 0
appQQ 
.QQ 
UseCorsQQ 
(QQ 
builderQQ #
=>QQ$ &
builderQQ' .
.QQ. /
AllowAnyOriginQQ/ =
(QQ= >
)QQ> ?
.RR 
AllowAnyMethodRR #
(RR# $
)RR$ %
.SS 
AllowAnyHeaderSS #
(SS# $
)SS$ %
.TT 
AllowCredentialsTT %
(TT% &
)TT& '
)TT' (
;TT( )
CreateDbSeedAsyncVV !
(VV! "
appVV" %
.VV% &
ApplicationServicesVV& 9
)VV9 :
;VV: ;
}WW 
appZZ 
.ZZ 
UseDefaultFilesZZ 
(ZZ  
)ZZ  !
;ZZ! "
app[[ 
.[[ 
UseStaticFiles[[ 
([[ 
)[[  
;[[  !
app\\ 
.\\ 
UseAuthentication\\ !
(\\! "
)\\" #
;\\# $
app]] 
.]] 
UseMvc]] 
(]] 
)]] 
;]] 
app^^ 
.^^ 
MapWhen^^ 
(^^ 
x^^ 
=>^^ 
!^^ 
x^^ 
.^^  
Request^^  '
.^^' (
Path^^( ,
.^^, -
Value^^- 2
.^^2 3

StartsWith^^3 =
(^^= >
$str^^> D
)^^D E
,^^E F
builder^^G N
=>^^O Q
{__ 
builder`` 
.`` 
UseMvc`` 
(`` 
routes`` %
=>``& (
{aa 
routesbb 
.bb 
MapRoutebb #
(bb# $
$strbb$ 2
,bb2 3
$strbb4 <
,bb< =
newbb> A
{bbB C

controllerbbD N
=bbO P
$strbbQ W
,bbW X
actionbbY _
=bb` a
$strbbb p
}bbq r
)bbr s
;bbs t
}cc 
)cc 
;cc 
}dd 
)dd 
;dd 
}ee 	
privategg 
voidgg #
LoadAndValidateSettingsgg ,
(gg, -
IServiceCollectiongg- ?
servicesgg@ H
)ggH I
{hh 	
_loggerii 
.ii 
LogInformationii "
(ii" #
$strii# 8
)ii8 9
;ii9 :
servicesjj 
.jj 
AddTransientjj !
<jj! "
IStartupFilterjj" 0
,jj0 1#
SettingValidationFilterjj2 I
>jjI J
(jjJ K
)jjK L
;jjL M
varll 
mailSettingsll 
=ll 
Configurationll ,
.ll, -

GetSectionll- 7
(ll7 8
$strll8 I
)llI J
;llJ K
ifmm 
(mm 
mailSettingsmm 
.mm 
Getmm  
<mm  !
MailingSettingsmm! 0
>mm0 1
(mm1 2
)mm2 3
==mm4 6
nullmm7 ;
)mm; <
thrownn 
newnn 
SettingExceptionnn *
(nn* +
$strnn+ =
,nn= >
$"nn? A
Unable to load nnA P
{nnP Q
nameofnnQ W
(nnW X
MailingSettingsnnX g
)nng h
}nnh i$
 configuration section.	nni Ä
"
nnÄ Å
)
nnÅ Ç
;
nnÇ É
varoo 
identitySettingsoo  
=oo! "
Configurationoo# 0
.oo0 1

GetSectionoo1 ;
(oo; <
$stroo< N
)ooN O
;ooO P
ifpp 
(pp 
identitySettingspp  
.pp  !
Getpp! $
<pp$ %
IdentitySettingspp% 5
>pp5 6
(pp6 7
)pp7 8
==pp9 ;
nullpp< @
)pp@ A
throwqq 
newqq 
SettingExceptionqq *
(qq* +
$strqq+ =
,qq= >
$"qq? A
Unable to load qqA P
{qqP Q
nameofqqQ W
(qqW X
IdentitySettingsqqX h
)qqh i
}qqi j$
 configuration section.	qqj Å
"
qqÅ Ç
)
qqÇ É
;
qqÉ Ñ
varrr 
pathSettingsrr 
=rr 
Configurationrr ,
.rr, -

GetSectionrr- 7
(rr7 8
$strrr8 F
)rrF G
;rrG H
ifss 
(ss 
pathSettingsss 
.ss 
Getss  
<ss  !
PathSettingsss! -
>ss- .
(ss. /
)ss/ 0
==ss1 3
nullss4 8
)ss8 9
throwtt 
newtt 
SettingExceptiontt *
(tt* +
$strtt+ =
,tt= >
$"tt? A
Unable to load ttA P
{ttP Q
nameofttQ W
(ttW X
PathSettingsttX d
)ttd e
}tte f#
 configuration section.ttf }
"tt} ~
)tt~ 
;	tt Ä
servicesvv 
.vv 
	Configurevv 
<vv 
MailingSettingsvv .
>vv. /
(vv/ 0
mailSettingsvv0 <
)vv< =
;vv= >
servicesww 
.ww 
	Configureww 
<ww 
IdentitySettingsww /
>ww/ 0
(ww0 1
identitySettingsww1 A
)wwA B
;wwB C
servicesxx 
.xx 
	Configurexx 
<xx 
PathSettingsxx +
>xx+ ,
(xx, -
pathSettingsxx- 9
)xx9 :
;xx: ;
servicesyy 
.yy 
	Configureyy 
<yy 
EnvironmentSettingsyy 2
>yy2 3
(yy3 4
Configurationyy4 A
)yyA B
;yyB C
services{{ 
.{{ 
AddSingleton{{ !
<{{! "
IValidatable{{" .
>{{. /
({{/ 0
resolver{{0 8
=>{{9 ;
resolver|| 
.|| 
GetRequiredService|| +
<||+ ,
IOptions||, 4
<||4 5
MailingSettings||5 D
>||D E
>||E F
(||F G
)||G H
.||H I
Value||I N
)||N O
;||O P
services}} 
.}} 
AddSingleton}} !
<}}! "
IValidatable}}" .
>}}. /
(}}/ 0
resolver}}0 8
=>}}9 ;
resolver~~ 
.~~ 
GetRequiredService~~ +
<~~+ ,
IOptions~~, 4
<~~4 5
IdentitySettings~~5 E
>~~E F
>~~F G
(~~G H
)~~H I
.~~I J
Value~~J O
)~~O P
;~~P Q
services 
. 
AddSingleton !
<! "
IValidatable" .
>. /
(/ 0
resolver0 8
=>9 ;
resolver
ÄÄ 
.
ÄÄ  
GetRequiredService
ÄÄ +
<
ÄÄ+ ,
IOptions
ÄÄ, 4
<
ÄÄ4 5!
EnvironmentSettings
ÄÄ5 H
>
ÄÄH I
>
ÄÄI J
(
ÄÄJ K
)
ÄÄK L
.
ÄÄL M
Value
ÄÄM R
)
ÄÄR S
;
ÄÄS T
services
ÅÅ 
.
ÅÅ 
AddSingleton
ÅÅ !
<
ÅÅ! "
IValidatable
ÅÅ" .
>
ÅÅ. /
(
ÅÅ/ 0
resolver
ÅÅ0 8
=>
ÅÅ9 ;
resolver
ÇÇ 
.
ÇÇ  
GetRequiredService
ÇÇ +
<
ÇÇ+ ,
IOptions
ÇÇ, 4
<
ÇÇ4 5
PathSettings
ÇÇ5 A
>
ÇÇA B
>
ÇÇB C
(
ÇÇC D
)
ÇÇD E
.
ÇÇE F
Value
ÇÇF K
)
ÇÇK L
;
ÇÇL M
}
ÉÉ 	
private
ÖÖ 
void
ÖÖ $
ConfigureAuthorization
ÖÖ +
(
ÖÖ+ , 
IServiceCollection
ÖÖ, >
services
ÖÖ? G
)
ÖÖG H
{
ÜÜ 	
_logger
áá 
.
áá 
LogInformation
áá "
(
áá" #
$str
áá# >
)
áá> ?
;
áá? @
services
àà 
.
àà 
AddMvc
àà 
(
àà 
options
àà #
=>
àà$ &
{
ââ 
var
ää 
policy
ää 
=
ää 
new
ää  (
AuthorizationPolicyBuilder
ää! ;
(
ää; <
)
ää< =
.
ãã &
RequireAuthenticatedUser
ãã -
(
ãã- .
)
ãã. /
.
åå 
Build
åå 
(
åå 
)
åå 
;
åå 
options
çç 
.
çç 
Filters
çç 
.
çç  
Add
çç  #
(
çç# $
new
çç$ '
AuthorizeFilter
çç( 7
(
çç7 8
policy
çç8 >
)
çç> ?
)
çç? @
;
çç@ A
options
éé 
.
éé 
Filters
éé 
.
éé  
Add
éé  #
(
éé# $
new
éé$ '
ProducesAttribute
éé( 9
(
éé9 :
$str
éé: L
)
ééL M
)
ééM N
;
ééN O
options
èè 
.
èè 
Filters
èè 
.
èè  
Add
èè  #
(
èè# $
typeof
èè$ *
(
èè* +
ValidationFilter
èè+ ;
)
èè; <
)
èè< =
;
èè= >
}
êê 
)
êê 
;
êê 
services
ëë 
.
ëë 
AddAuthentication
ëë &
(
ëë& '
options
ëë' .
=>
ëë/ 1
{
íí 
options
ìì 
.
ìì '
DefaultAuthenticateScheme
ìì 1
=
ìì2 3
JwtBearerDefaults
ìì4 E
.
ììE F"
AuthenticationScheme
ììF Z
;
ììZ [
options
îî 
.
îî $
DefaultChallengeScheme
îî .
=
îî/ 0
JwtBearerDefaults
îî1 B
.
îîB C"
AuthenticationScheme
îîC W
;
îîW X
}
ïï 
)
ïï 
.
ïï 
AddJwtBearer
ïï 
(
ïï 
options
ïï #
=>
ïï$ &
{
ññ 
options
óó 
.
óó "
RequireHttpsMetadata
óó ,
=
óó- .
false
óó/ 4
;
óó4 5
options
òò 
.
òò 
	SaveToken
òò !
=
òò" #
true
òò$ (
;
òò( )
options
ôô 
.
ôô '
TokenValidationParameters
ôô 1
=
ôô2 3
new
ôô4 7'
TokenValidationParameters
ôô8 Q
{
öö &
ValidateIssuerSigningKey
õõ ,
=
õõ- .
true
õõ/ 3
,
õõ3 4
IssuerSigningKey
úú $
=
úú% &
new
úú' *"
SymmetricSecurityKey
úú+ ?
(
úú? @
GetJwtSecret
úú@ L
(
úúL M
)
úúM N
)
úúN O
,
úúO P
ValidateIssuer
ùù "
=
ùù# $
false
ùù% *
,
ùù* +
ValidateAudience
ûû $
=
ûû% &
false
ûû' ,
}
üü 
;
üü 
}
†† 
)
†† 
;
†† 
}
°° 	
private
££ *
MongoDbIdentityConfiguration
££ ,
ConfigureIdentity
££- >
(
££> ?
IdentitySettings
££? O
settings
££P X
)
££X Y
{
§§ 	
_logger
•• 
.
•• 
LogInformation
•• "
(
••" #
$str
••# 9
)
••9 :
;
••: ;*
MongoDbIdentityConfiguration
¶¶ (
configuration
¶¶) 6
=
¶¶7 8
new
¶¶9 <*
MongoDbIdentityConfiguration
¶¶= Y
(
¶¶Y Z
)
¶¶Z [
;
¶¶[ \
if
ßß 
(
ßß 
Environment
ßß 
.
ßß 
IsDevelopment
ßß )
(
ßß) *
)
ßß* +
)
ßß+ ,
configuration
®® 
.
®® 
MongoDbSettings
®® -
=
®®. /
new
®®0 3
MongoDbSettings
®®4 C
{
©© 
ConnectionString
™™ $
=
™™% &
Mongo2Go
™™' /
.
™™/ 0
MongoDbRunner
™™0 =
.
™™= >
StartForDebugging
™™> O
(
™™O P
)
™™P Q
.
™™Q R
ConnectionString
™™R b
,
™™b c
DatabaseName
´´  
=
´´! "
DatabaseName
´´# /
}
¨¨ 
;
¨¨ 
else
≠≠ 
configuration
ÆÆ 
.
ÆÆ 
MongoDbSettings
ÆÆ -
=
ÆÆ. /
new
ÆÆ0 3
MongoDbSettings
ÆÆ4 C
(
ÆÆC D
)
ÆÆD E
;
ÆÆE F
configuration
∞∞ 
.
∞∞ #
IdentityOptionsAction
∞∞ /
=
∞∞0 1
options
∞∞2 9
=>
∞∞: <
{
±± 
options
≤≤ 
.
≤≤ 
Password
≤≤  
.
≤≤  !
RequireDigit
≤≤! -
=
≤≤. /
(
≤≤0 1
bool
≤≤1 5
)
≤≤5 6
settings
≤≤6 >
.
≤≤> ?
RequireDigit
≤≤? K
;
≤≤K L
options
≥≥ 
.
≥≥ 
Password
≥≥  
.
≥≥  !
RequiredLength
≥≥! /
=
≥≥0 1
(
≥≥2 3
int
≥≥3 6
)
≥≥6 7
settings
≥≥7 ?
.
≥≥? @
RequiredLength
≥≥@ N
;
≥≥N O
options
¥¥ 
.
¥¥ 
Password
¥¥  
.
¥¥  !$
RequireNonAlphanumeric
¥¥! 7
=
¥¥8 9
(
¥¥: ;
bool
¥¥; ?
)
¥¥? @
settings
¥¥@ H
.
¥¥H I$
RequireNonAlphanumeric
¥¥I _
;
¥¥_ `
options
µµ 
.
µµ 
Password
µµ  
.
µµ  !
RequireUppercase
µµ! 1
=
µµ2 3
(
µµ4 5
bool
µµ5 9
)
µµ9 :
settings
µµ: B
.
µµB C
RequireUppercase
µµC S
;
µµS T
options
∂∂ 
.
∂∂ 
Password
∂∂  
.
∂∂  !
RequireLowercase
∂∂! 1
=
∂∂2 3
(
∂∂4 5
bool
∂∂5 9
)
∂∂9 :
settings
∂∂: B
.
∂∂B C
RequireLowercase
∂∂C S
;
∂∂S T
options
∏∏ 
.
∏∏ 
SignIn
∏∏ 
.
∏∏ #
RequireConfirmedEmail
∏∏ 4
=
∏∏5 6
(
∏∏7 8
bool
∏∏8 <
)
∏∏< =
settings
∏∏= E
.
∏∏E F#
RequireConfirmedEmail
∏∏F [
;
∏∏[ \
options
∫∫ 
.
∫∫ 
Lockout
∫∫ 
.
∫∫  $
DefaultLockoutTimeSpan
∫∫  6
=
∫∫7 8
TimeSpan
∫∫9 A
.
∫∫A B
FromMinutes
∫∫B M
(
∫∫M N
(
∫∫N O
int
∫∫O R
)
∫∫R S
settings
∫∫S [
.
∫∫[ \$
DefaultLockoutTimeSpan
∫∫\ r
)
∫∫r s
;
∫∫s t
options
ªª 
.
ªª 
Lockout
ªª 
.
ªª  %
MaxFailedAccessAttempts
ªª  7
=
ªª8 9
(
ªª: ;
int
ªª; >
)
ªª> ?
settings
ªª? G
.
ªªG H%
MaxFailedAccessAttempts
ªªH _
;
ªª_ `
options
ΩΩ 
.
ΩΩ 
User
ΩΩ 
.
ΩΩ  
RequireUniqueEmail
ΩΩ /
=
ΩΩ0 1
(
ΩΩ2 3
bool
ΩΩ3 7
)
ΩΩ7 8
settings
ΩΩ8 @
.
ΩΩ@ A 
RequireUniqueEmail
ΩΩA S
;
ΩΩS T
}
ææ 
;
ææ 
return
øø 
configuration
øø  
;
øø  !
}
¿¿ 	
private
¬¬ 
byte
¬¬ 
[
¬¬ 
]
¬¬ 
GetJwtSecret
¬¬ #
(
¬¬# $
)
¬¬$ %
{
√√ 	
_logger
ƒƒ 
.
ƒƒ 
LogInformation
ƒƒ "
(
ƒƒ" #
$str
ƒƒ# 9
)
ƒƒ9 :
;
ƒƒ: ;
var
≈≈ 
secret
≈≈ 
=
≈≈ 
System
≈≈ 
.
≈≈  
Environment
≈≈  +
.
≈≈+ ,$
GetEnvironmentVariable
≈≈, B
(
≈≈B C
$str
≈≈C N
)
≈≈N O
;
≈≈O P
var
∆∆ 
bytes
∆∆ 
=
∆∆ 
Encoding
∆∆  
.
∆∆  !
ASCII
∆∆! &
.
∆∆& '
GetBytes
∆∆' /
(
∆∆/ 0
secret
∆∆0 6
)
∆∆6 7
;
∆∆7 8
return
«« 
bytes
«« 
;
«« 
}
»» 	
private
   
async
   
Task
   
CreateDbSeedAsync
   ,
(
  , -
IServiceProvider
  - =
serviceProvider
  > M
)
  M N
{
ÀÀ 	
_logger
ÃÃ 
.
ÃÃ 
LogInformation
ÃÃ "
(
ÃÃ" #
$str
ÃÃ# 5
)
ÃÃ5 6
;
ÃÃ6 7
try
ÕÕ 
{
ŒŒ 
_logger
œœ 
.
œœ 
LogInformation
œœ &
(
œœ& '
$str
œœ' >
)
œœ> ?
;
œœ? @
await
–– 
DbSeed
–– 
.
–– $
CreateTestingUserAsync
–– 3
(
––3 4
serviceProvider
––4 C
)
––C D
;
––D E
_logger
—— 
.
—— 
LogInformation
—— &
(
——& '
$str
——' 5
)
——5 6
;
——6 7
_logger
““ 
.
““ 
LogInformation
““ &
(
““& '
$str
““' 9
)
““9 :
;
““: ;
DbSeed
”” 
.
”” "
CreateTestingCourses
”” +
(
””+ ,
serviceProvider
””, ;
)
””; <
;
””< =
_logger
‘‘ 
.
‘‘ 
LogInformation
‘‘ &
(
‘‘& '
$str
‘‘' 8
)
‘‘8 9
;
‘‘9 :
}
’’ 
catch
÷÷ 
(
÷÷ 
	Exception
÷÷ 
e
÷÷ 
)
÷÷ 
{
◊◊ 
_logger
ÿÿ 
.
ÿÿ 
LogError
ÿÿ  
(
ÿÿ  !
$"
ÿÿ! #3
%Exception during creating DB seed :\n
ÿÿ# H
{
ÿÿH I
e
ÿÿI J
.
ÿÿJ K
Message
ÿÿK R
}
ÿÿR S
"
ÿÿS T
)
ÿÿT U
;
ÿÿU V
}
ŸŸ 
}
⁄⁄ 	
}
€€ 
}‹‹ 