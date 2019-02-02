
SFRITeam.Swapify.Backend.DbSeed.DbSeed.CreateTestingCourses(System.IServiceProvider)6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.cs? R(	serviceProvider"0*ﬂ
0ü
ú
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.csA A(P
%0"xMicrosoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<T>(System.IServiceProvider)*M"K
IMicrosoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions*

serviceProviderU
S
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.csA A(P
	dbService"__id*

%0»
≈
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.csB# B(R
%1"fMongoDB.Driver.IMongoDatabase.GetCollection<TDocument>(string, MongoDB.Driver.MongoCollectionSettings)*

	dbService*
""*
""\
Z
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.csB B(R
courseCollection"__id*

%1ü
ú
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.csD D(S
%2"xMicrosoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<T>(System.IServiceProvider)*M"K
IMicrosoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions*

serviceProviderP
N
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.csD D(S
path"__id*

%2Ü
É
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.csE( E(2
%3"9Microsoft.Extensions.Options.IOptions<TOptions>.Value.get*

pathå
â
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.csE( E(B
%4"AFRITeam.Swapify.Backend.Settings.PathSettings.CoursesJsonPath.get*

%3Ä
~
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.csE E(C
%5""System.IO.File.ReadAllText(string)*"
System.IO.File*

%4P
N
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.csE E(C
json"__id*

%5¶
£
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.csF F(_
%6"8Newtonsoft.Json.JsonConvert.DeserializeObject<T>(string)*"
Newtonsoft.Json.JsonConvert*

jsonS
Q
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.csF F(_	
courses"__id*

%6us
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.csH- H(M
%7"3System.Collections.Generic.Dictionary<TKey, TValue>ã
à
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.csH1 H(K
%8"@System.Collections.Generic.Dictionary<TKey, TValue>.Dictionary()*

%7O
M
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.csH' H(M
dic"__id*

%7*
1*
1*
2
3*Ç
2ä
á
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.csK K("
%9">FRITeam.Swapify.Backend.CourseParser.CourseItem.CourseCode.get*

crsb`
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.csK& O(
%10"FRITeam.Swapify.Entities.Courset
r
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.csK* K(0
%11"(FRITeam.Swapify.Entities.Course.Course()*

%10ã
à
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.csM! M(/
%12">FRITeam.Swapify.Backend.CourseParser.CourseItem.CourseCode.get*

crsÑ
Å
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.csM M(/
%13".FRITeam.Swapify.Entities.Course.CourseCode.set*

%10*

%12ã
à
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.csN! N(/
%14">FRITeam.Swapify.Backend.CourseParser.CourseItem.CourseName.get*

crsÑ
Å
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.csN N(/
%15".FRITeam.Swapify.Entities.Course.CourseName.set*

%10*

%14g
e
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.csK O(
%16"
__arraySet*

dic*

%9*

%10*
1*†
3
Ì
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.csQ( Q(@
%17"System.Linq.Enumerable.Select<TSource, TResult>(System.Collections.Generic.IEnumerable<TSource>, System.Func<TSource, TResult>)*"
System.Linq.Enumerable*

dic*
""¢
ü
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.csQ Q(A
%18"ÆMongoDB.Driver.IMongoCollection<TDocument>.InsertMany(System.Collections.Generic.IEnumerable<TDocument>, MongoDB.Driver.InsertManyOptions, System.Threading.CancellationToken)*

courseCollection*

%17*
""*
""*
4*R
4"M
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.csR R(	
implicit return