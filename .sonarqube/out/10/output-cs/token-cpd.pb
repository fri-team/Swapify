��
/C:\Projects\Swapify\CoursesParser\BaseParser.cs
	namespace 	
CoursesParser
 
{ 
public 

class 

BaseParser 
{ 
private 
const 
string 
Url  
=! "
$str# V
;V W
private 
readonly 
HtmlDocument %
	_document& /
;/ 0
private 
HtmlNode 
_selectFaculties )
;) *
private 
HtmlNode 
_selectTown $
;$ %
private 
HtmlNode 
_selectStudyType )
;) *
private 
HtmlNode 
_selectStudyYear )
;) *
private 
HtmlNode 
_selectFieldOfStudy ,
;, -
private 
HtmlNode '
_selectFieldOfStudyDetailed 4
;4 5
private 
HtmlNode 
_facAct  
;  !
private 
HtmlNode 
_townAct !
;! "
private 
HtmlNode 
_studyTypeAct &
;& '
private 
HtmlNode 
_studyYearAct &
;& '
private 
HtmlNode 
_fieldOfStudyAct )
;) *
private 
HtmlNode $
_fieldOfStudyDetailedAct 1
;1 2
private 
List 
< 

CourseItem 
>  
_allCourses! ,
;, -
private 
Encoding 
	_encoding "
;" #
public!! 

BaseParser!! 
(!! 
)!! 
{"" 	
	_encoding## 
=## %
CodePagesEncodingProvider## 1
.##1 2
Instance##2 :
.##: ;
GetEncoding##; F
(##F G
$num##G K
)##K L
;##L M
var%% 
web%% 
=%% 
new%% 
HtmlWeb%% !
(%%! "
)%%" #
;%%# $
web&& 
.&& 
AutoDetectEncoding&& "
=&&# $
false&&% *
;&&* +
web'' 
.'' 
OverrideEncoding''  
=''! "
	_encoding''# ,
;'', -
_allCourses)) 
=)) 
new)) 
List)) "
<))" #

CourseItem))# -
>))- .
()). /
$num))/ 3
)))3 4
;))4 5
	_document** 
=** 
web** 
.** 
Load**  
(**  !
Url**! $
)**$ %
;**% &
	_document++ 
.++ '
OptionDefaultStreamEncoding++ 1
=++2 3
	_encoding++4 =
;++= >
_selectFaculties-- 
=-- 
	_document-- (
.--( )
GetElementbyId--) 7
(--7 8
$str--8 ;
)--; <
;--< =
_selectTown.. 
=.. 
	_document.. #
...# $
GetElementbyId..$ 2
(..2 3
$str..3 6
)..6 7
;..7 8
_selectStudyType// 
=// 
	_document// (
.//( )
GetElementbyId//) 7
(//7 8
$str//8 ;
)//; <
;//< =
_selectStudyYear00 
=00 
	_document00 (
.00( )
GetElementbyId00) 7
(007 8
$str008 ;
)00; <
;00< =
_selectFieldOfStudy11 
=11  !
	_document11" +
.11+ ,
GetElementbyId11, :
(11: ;
$str11; >
)11> ?
;11? @'
_selectFieldOfStudyDetailed22 '
=22( )
	_document22* 3
.223 4
GetElementbyId224 B
(22B C
$str22C F
)22F G
;22G H
}33 	
public66 
List66 
<66 

CourseItem66 
>66 
ParseFaculties66  .
(66. /
)66/ 0
{77 	
foreach88 
(88 
var88 
facultyOption88 &
in88' )
_selectFaculties88* :
.88: ;

ChildNodes88; E
)88E F
{99 
_facAct:: 
=:: 
facultyOption:: '
;::' (
Console;; 
.;; 
	WriteLine;; !
(;;! "
facultyOption;;" /
.;;/ 0
	InnerText;;0 9
);;9 :
;;;: ;"
DownloadAndSaveCourses<< &
(<<& '
ChangeLevel<<' 2
.<<2 3
FromFaculty<<3 >
)<<> ?
;<<? @
foreach== 
(== 
var== 
town== !
in==" $
_selectTown==% 0
.==0 1

ChildNodes==1 ;
)==; <
{>> 
_townAct?? 
=?? 
town?? #
;??# $
Console@@ 
.@@ 
	WriteLine@@ %
(@@% &
town@@& *
.@@* +
	InnerText@@+ 4
)@@4 5
;@@5 6"
DownloadAndSaveCoursesAA *
(AA* +
ChangeLevelAA+ 6
.AA6 7
FromTownAA7 ?
)AA? @
;AA@ A
foreachBB 
(BB 
varBB  
	studyTypeBB! *
inBB+ -
_selectStudyTypeBB. >
.BB> ?

ChildNodesBB? I
)BBI J
{CC 
_studyTypeActDD %
=DD& '
	studyTypeDD( 1
;DD1 2
ConsoleEE 
.EE  
	WriteLineEE  )
(EE) *
	studyTypeEE* 3
.EE3 4
	InnerTextEE4 =
)EE= >
;EE> ?"
DownloadAndSaveCoursesFF .
(FF. /
ChangeLevelFF/ :
.FF: ;
FromStudyTypeFF; H
)FFH I
;FFI J
foreachGG 
(GG  !
varGG! $
	studyYearGG% .
inGG/ 1
_selectStudyYearGG2 B
.GGB C

ChildNodesGGC M
)GGM N
{HH 
_studyYearActII )
=II* +
	studyYearII, 5
;II5 6
ConsoleJJ #
.JJ# $
	WriteLineJJ$ -
(JJ- .
	studyYearJJ. 7
.JJ7 8
	InnerTextJJ8 A
)JJA B
;JJB C"
DownloadAndSaveCoursesKK 2
(KK2 3
ChangeLevelKK3 >
.KK> ?
FromStudyYearKK? L
)KKL M
;KKM N
foreachLL #
(LL$ %
varLL% (
fieldOfStudyLL) 5
inLL6 8
_selectFieldOfStudyLL9 L
.LLL M

ChildNodesLLM W
)LLW X
{MM 
_fieldOfStudyActNN  0
=NN1 2
fieldOfStudyNN3 ?
;NN? @
ConsoleOO  '
.OO' (
	WriteLineOO( 1
(OO1 2
fieldOfStudyOO2 >
.OO> ?
	InnerTextOO? H
)OOH I
;OOI J"
DownloadAndSaveCoursesPP  6
(PP6 7
ChangeLevelPP7 B
.PPB C
FromFieldOfStudyPPC S
)PPS T
;PPT U
foreachQQ  '
(QQ( )
varQQ) , 
fieldOfStudyDetailedQQ- A
inQQB D'
_selectFieldOfStudyDetailedQQE `
.QQ` a

ChildNodesQQa k
)QQk l
{RR  !$
_fieldOfStudyDetailedActSS$ <
=SS= > 
fieldOfStudyDetailedSS? S
;SSS T"
DownloadAndSaveCoursesTT$ :
(TT: ;
ChangeLevelTT; F
.TTF G$
FromDetailedFieldOfStudyTTG _
)TT_ `
;TT` a
}UU  !
}VV 
}WW 
}XX 
}YY 
}ZZ 
return[[ 
_allCourses[[ 
;[[ 
}\\ 	
private^^ 
void^^ "
DownloadAndSaveCourses^^ +
(^^+ ,
ChangeLevel^^, 7
level^^8 =
)^^= >
{__ 	
if`` 
(`` 
_facAct`` 
.`` 

Attributes`` "
[``" #
$str``# *
]``* +
.``+ ,
Value``, 1
==``2 4
null``5 9
||``: <
_townActaa 
?aa 
.aa 

Attributesaa $
[aa$ %
$straa% ,
]aa, -
?aa- .
.aa. /
Valueaa/ 4
==aa5 7
nullaa8 <
||aa= ?
_studyTypeActbb 
?bb 
.bb 

Attributesbb )
[bb) *
$strbb* 1
]bb1 2
?bb2 3
.bb3 4
Valuebb4 9
==bb: <
nullbb= A
||bbB D
_studyYearActcc 
?cc 
.cc 

Attributescc )
[cc) *
$strcc* 1
]cc1 2
?cc2 3
.cc3 4
Valuecc4 9
==cc: <
nullcc= A
||ccB D
_fieldOfStudyActdd  
?dd  !
.dd! "

Attributesdd" ,
[dd, -
$strdd- 4
]dd4 5
?dd5 6
.dd6 7
Valuedd7 <
==dd= ?
nulldd@ D
||ddE G$
_fieldOfStudyDetailedActee (
?ee( )
.ee) *

Attributesee* 4
[ee4 5
$stree5 <
]ee< =
?ee= >
.ee> ?
Valueee? D
==eeE G
nulleeH L
)eeL M
{ff 
returngg 
;gg 
}hh 
Debugii 
.ii 
	WriteLineii 
(ii 
$"ii 
{ii 
_facActii &
}ii& '
, ii' )
{ii) *
_townActii* 2
}ii2 3
, ii3 5
{ii5 6
_studyTypeActii6 C
}iiC D
, iiD F
{iiF G
_studyYearActiiG T
}iiT U
, iiU W
{iiW X
_fieldOfStudyActiiX h
}iih i
, iii k
{iik l%
_fieldOfStudyDetailedAct	iil �
}
ii� �
, 
ii� �
{
ii� �
level
ii� �
.
ii� �
ToString
ii� �
(
ii� �
)
ii� �
}
ii� �
"
ii� �
)
ii� �
;
ii� �
varjj 
jsonjj 
=jj 
DownloadJsonjj #
(jj# $
_facActkk 
.kk 

Attributeskk "
[kk" #
$strkk# *
]kk* +
.kk+ ,
Valuekk, 1
,kk1 2
_townActll 
.ll 

Attributesll #
[ll# $
$strll$ +
]ll+ ,
.ll, -
Valuell- 2
,ll2 3
_studyTypeActmm 
.mm 

Attributesmm (
[mm( )
$strmm) 0
]mm0 1
.mm1 2
Valuemm2 7
,mm7 8
_studyYearActnn 
.nn 

Attributesnn (
[nn( )
$strnn) 0
]nn0 1
.nn1 2
Valuenn2 7
,nn7 8
_fieldOfStudyActoo  
.oo  !

Attributesoo! +
[oo+ ,
$stroo, 3
]oo3 4
.oo4 5
Valueoo5 :
,oo: ;$
_fieldOfStudyDetailedActpp (
.pp( )

Attributespp) 3
[pp3 4
$strpp4 ;
]pp; <
.pp< =
Valuepp= B
,ppB C
(qq 
(qq 
intqq 
)qq 
levelqq 
)qq 
.qq 
ToStringqq %
(qq% &
)qq& '
)qq' (
;qq( )
varrr 
deserializedrr 
=rr 
JObjectrr &
.rr& '
Parserr' ,
(rr, -
jsonrr- 1
)rr1 2
;rr2 3
ifss 
(ss 
deserializedss 
[ss 
$strss "
]ss" #
!=ss$ &
nullss' +
)ss+ ,
{tt 
returnuu 
;uu 
}vv 
varxx 
tabxx 
=xx !
ConvertJsonToHtmlNodexx +
(xx+ ,
deserializedxx, 8
[xx8 9
$strxx9 @
]xx@ A
.xxA B
ToStringxxB J
(xxJ K
)xxK L
)xxL M
;xxM N
ParseAndSaveCoursesyy 
(yy  
tabyy  #
.yy# $

ChildNodesyy$ .
[yy. /
$stryy/ 6
]yy6 7
)yy7 8
;yy8 9
switch{{ 
({{ 
level{{ 
){{ 
{|| 
case}} 
ChangeLevel}}  
.}}  !$
FromDetailedFieldOfStudy}}! 9
:}}9 :
break~~ 
;~~ 
case 
ChangeLevel  
.  !
FromFieldOfStudy! 1
:1 2)
_selectFieldOfStudyDetailed
�� /
=
��0 1#
ConvertJsonToHtmlNode
��2 G
(
��G H
deserialized
��H T
[
��T U
$str
��U X
]
��X Y
.
��Y Z
ToString
��Z b
(
��b c
)
��c d
)
��d e
;
��e f
break
�� 
;
�� 
case
�� 
ChangeLevel
��  
.
��  !
FromStudyYear
��! .
:
��. /)
_selectFieldOfStudyDetailed
�� /
=
��0 1#
ConvertJsonToHtmlNode
��2 G
(
��G H
deserialized
��H T
[
��T U
$str
��U X
]
��X Y
.
��Y Z
ToString
��Z b
(
��b c
)
��c d
)
��d e
;
��e f!
_selectFieldOfStudy
�� '
=
��( )#
ConvertJsonToHtmlNode
��* ?
(
��? @
deserialized
��@ L
[
��L M
$str
��M P
]
��P Q
.
��Q R
ToString
��R Z
(
��Z [
)
��[ \
)
��\ ]
;
��] ^
break
�� 
;
�� 
case
�� 
ChangeLevel
��  
.
��  !
FromStudyType
��! .
:
��. /)
_selectFieldOfStudyDetailed
�� /
=
��0 1#
ConvertJsonToHtmlNode
��2 G
(
��G H
deserialized
��H T
[
��T U
$str
��U X
]
��X Y
.
��Y Z
ToString
��Z b
(
��b c
)
��c d
)
��d e
;
��e f!
_selectFieldOfStudy
�� '
=
��( )#
ConvertJsonToHtmlNode
��* ?
(
��? @
deserialized
��@ L
[
��L M
$str
��M P
]
��P Q
.
��Q R
ToString
��R Z
(
��Z [
)
��[ \
)
��\ ]
;
��] ^
_selectStudyYear
�� $
=
��% &#
ConvertJsonToHtmlNode
��' <
(
��< =
deserialized
��= I
[
��I J
$str
��J M
]
��M N
.
��N O
ToString
��O W
(
��W X
)
��X Y
)
��Y Z
;
��Z [
break
�� 
;
�� 
case
�� 
ChangeLevel
��  
.
��  !
FromTown
��! )
:
��) *)
_selectFieldOfStudyDetailed
�� /
=
��0 1#
ConvertJsonToHtmlNode
��2 G
(
��G H
deserialized
��H T
[
��T U
$str
��U X
]
��X Y
.
��Y Z
ToString
��Z b
(
��b c
)
��c d
)
��d e
;
��e f!
_selectFieldOfStudy
�� '
=
��( )#
ConvertJsonToHtmlNode
��* ?
(
��? @
deserialized
��@ L
[
��L M
$str
��M P
]
��P Q
.
��Q R
ToString
��R Z
(
��Z [
)
��[ \
)
��\ ]
;
��] ^
_selectStudyYear
�� $
=
��% &#
ConvertJsonToHtmlNode
��' <
(
��< =
deserialized
��= I
[
��I J
$str
��J M
]
��M N
.
��N O
ToString
��O W
(
��W X
)
��X Y
)
��Y Z
;
��Z [
_selectStudyType
�� $
=
��% &#
ConvertJsonToHtmlNode
��' <
(
��< =
deserialized
��= I
[
��I J
$str
��J M
]
��M N
.
��N O
ToString
��O W
(
��W X
)
��X Y
)
��Y Z
;
��Z [
break
�� 
;
�� 
case
�� 
ChangeLevel
��  
.
��  !
FromFaculty
��! ,
:
��, -)
_selectFieldOfStudyDetailed
�� /
=
��0 1#
ConvertJsonToHtmlNode
��2 G
(
��G H
deserialized
��H T
[
��T U
$str
��U X
]
��X Y
.
��Y Z
ToString
��Z b
(
��b c
)
��c d
)
��d e
;
��e f!
_selectFieldOfStudy
�� '
=
��( )#
ConvertJsonToHtmlNode
��* ?
(
��? @
deserialized
��@ L
[
��L M
$str
��M P
]
��P Q
.
��Q R
ToString
��R Z
(
��Z [
)
��[ \
)
��\ ]
;
��] ^
_selectStudyYear
�� $
=
��% &#
ConvertJsonToHtmlNode
��' <
(
��< =
deserialized
��= I
[
��I J
$str
��J M
]
��M N
.
��N O
ToString
��O W
(
��W X
)
��X Y
)
��Y Z
;
��Z [
_selectStudyType
�� $
=
��% &#
ConvertJsonToHtmlNode
��' <
(
��< =
deserialized
��= I
[
��I J
$str
��J M
]
��M N
.
��N O
ToString
��O W
(
��W X
)
��X Y
)
��Y Z
;
��Z [
_selectTown
�� 
=
��  !#
ConvertJsonToHtmlNode
��" 7
(
��7 8
deserialized
��8 D
[
��D E
$str
��E H
]
��H I
.
��I J
ToString
��J R
(
��R S
)
��S T
)
��T U
;
��U V
break
�� 
;
�� 
}
�� 
}
�� 	
private
�� 
HtmlNode
�� #
ConvertJsonToHtmlNode
�� .
(
��. /
string
��/ 5
content
��6 =
)
��= >
{
�� 	
var
�� 
docc
�� 
=
�� 
new
�� 
HtmlDocument
�� '
(
��' (
)
��( )
;
��) *
docc
�� 
.
�� )
OptionDefaultStreamEncoding
�� ,
=
��- .
	_encoding
��/ 8
;
��8 9
docc
�� 
.
�� 
LoadHtml
�� 
(
�� 
content
�� !
)
��! "
;
��" #
return
�� 
docc
�� 
.
�� 
DocumentNode
�� $
;
��$ %
}
�� 	
private
�� 
string
�� 
DownloadJson
�� #
(
��# $
string
��$ *
f
��+ ,
,
��, -
string
��. 4
t
��5 6
,
��6 7
string
��8 >
m
��? @
,
��@ A
string
��B H
r
��I J
,
��J K
string
��- 3
o
��4 5
,
��5 6
string
��7 =
z
��> ?
,
��? @
string
��A G
c
��H I
)
��I J
{
�� 	
string
�� 
retJson
�� 
=
�� 
$str
�� 
;
��  
using
�� 
(
�� 
	WebClient
�� 
wc
�� 
=
��  !
new
��" %
	WebClient
��& /
(
��/ 0
)
��0 1
)
��1 2
{
�� 
wc
�� 
.
�� 
Encoding
�� 
=
�� 
	_encoding
�� '
;
��' (
retJson
�� 
=
�� 
wc
�� 
.
�� 
DownloadString
�� +
(
��+ ,
$"
��, .
{
��. /
Url
��/ 2
}
��2 3
?f=
��3 6
{
��6 7
f
��7 8
}
��8 9
&t=
��9 <
{
��< =
t
��= >
}
��> ?
&m=
��? B
{
��B C
m
��C D
}
��D E
&r=
��E H
{
��H I
r
��I J
}
��J K
&o=
��K N
{
��N O
o
��O P
}
��P Q
&z=
��Q T
{
��T U
z
��U V
}
��V W
&c=
��W Z
{
��Z [
c
��[ \
}
��\ ]
"
��] ^
)
��^ _
;
��_ `
}
�� 
return
�� 
retJson
�� 
;
�� 
}
�� 	
private
�� 
void
�� !
ParseAndSaveCourses
�� (
(
��( )
HtmlNode
��) 1
	htmlTable
��2 ;
)
��; <
{
�� 	
var
�� 
rows
�� 
=
�� 
	htmlTable
��  
.
��  !

ChildNodes
��! +
[
��+ ,
$str
��, 3
]
��3 4
.
��4 5
SelectNodes
��5 @
(
��@ A
$str
��A E
)
��E F
;
��F G
foreach
�� 
(
�� 
var
�� 
x
�� 
in
�� 
rows
�� "
)
��" #
{
�� 
var
�� 
course
�� 
=
�� 
x
�� 
.
�� 

ChildNodes
�� )
[
��) *
$str
��* .
]
��. /
?
��/ 0
.
��0 1

ChildNodes
��1 ;
[
��; <
$str
��< ?
]
��? @
;
��@ A
if
�� 
(
�� 
course
�� 
!=
�� 
null
�� "
)
��" #
{
�� 
var
�� 
crs
�� 
=
�� 
new
�� !

CourseItem
��" ,
(
��, -
)
��- .
;
��. /
crs
�� 
.
�� 
Faculty
�� 
=
��  !
_facAct
��" )
.
��) *
	InnerText
��* 3
;
��3 4
crs
�� 
.
�� 
Town
�� 
=
�� 
_townAct
�� '
.
��' (
	InnerText
��( 1
;
��1 2
crs
�� 
.
�� 
YearOfStudy
�� #
=
��$ %
_studyYearAct
��& 3
.
��3 4
	InnerText
��4 =
;
��= >
crs
�� 
.
�� 
StudyOfField
�� $
=
��% &
_fieldOfStudyAct
��' 7
.
��7 8
	InnerText
��8 A
;
��A B
crs
�� 
.
�� "
DetailedStudyOfField
�� ,
=
��- .&
_fieldOfStudyDetailedAct
��/ G
.
��G H
	InnerText
��H Q
;
��Q R
crs
�� 
.
�� 
	StudyType
�� !
=
��" #
_studyTypeAct
��$ 1
.
��1 2
	InnerText
��2 ;
;
��; <
int
�� 
spaceIdx
��  
=
��! "
course
��# )
.
��) *
	InnerText
��* 3
.
��3 4
IndexOf
��4 ;
(
��; <
$str
��< ?
)
��? @
;
��@ A
crs
�� 
.
�� 

CourseCode
�� "
=
��# $
course
��% +
.
��+ ,
	InnerText
��, 5
.
��5 6
	Substring
��6 ?
(
��? @
$num
��@ A
,
��A B
spaceIdx
��C K
)
��K L
;
��L M
crs
�� 
.
�� 

CourseName
�� "
=
��# $
course
��% +
.
��+ ,
	InnerText
��, 5
.
��5 6
	Substring
��6 ?
(
��? @
spaceIdx
��@ H
+
��I J
$num
��K L
,
��L M
course
��N T
.
��T U
	InnerText
��U ^
.
��^ _
Length
��_ e
-
��f g
spaceIdx
��h p
-
��q r
$num
��s t
)
��t u
;
��u v
_allCourses
�� 
.
��  
Add
��  #
(
��# $
crs
��$ '
)
��' (
;
��( )
}
�� 
}
�� 
}
�� 	
}
�� 
}�� �
6C:\Projects\Swapify\CoursesParser\Enums\ChangeLevel.cs
	namespace 	
CoursesParser
 
. 
Enums 
{ 
enum 
ChangeLevel	 
{ 
FromFaculty		 
=		 
$num		 
,		 
FromTown

 
=

 
$num

 
,

 
FromStudyType 
= 
$num 
, 
FromStudyYear 
= 
$num 
, 
FromFieldOfStudy 
= 
$num 
, $
FromDetailedFieldOfStudy  
=! "
$num# $
} 
} �
,C:\Projects\Swapify\CoursesParser\Program.cs
	namespace 	
CoursesParser
 
{ 
public		 

class		 
Program		 
{

 
static 
void 
Main 
( 
string 
[  
]  !
args" &
)& '
{ 	

BaseParser 
parser 
= 
new  #

BaseParser$ .
(. /
)/ 0
;0 1
List 
< 

CourseItem 
> 

allCourses '
=( )
parser* 0
.0 1
ParseFaculties1 ?
(? @
)@ A
;A B
var 
json 
= 

Newtonsoft !
.! "
Json" &
.& '
JsonConvert' 2
.2 3
SerializeObject3 B
(B C

allCoursesC M
)M N
;N O
var 
actDate 
= 
DateTime "
." #
Now# &
;& '
File 
. 
WriteAllText 
( 
$"  
courses_  (
{( )
actDate) 0
.0 1
Year1 5
}5 6
_6 7
{7 8
actDate8 ?
.? @
Month@ E
}E F
_F G
{G H
actDateH O
.O P
DayP S
}S T
.jsonT Y
"Y Z
,Z [
json\ `
)` a
;a b
} 	
} 
} 