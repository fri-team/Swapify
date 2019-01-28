
QFRITeam.Swapify.Backend.DbSeed.DbSeed.CreateStudentAsync(System.IServiceProvider)6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.cs0 =(	serviceProvider"0*¨
0Ÿ
œ
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.cs2 2(P
%0"xMicrosoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<T>(System.IServiceProvider)*M"K
IMicrosoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions*

serviceProviderU
S
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.cs2 2(P
	dbService"__id*

%0È
Å
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.cs3$ 3(U
%1"fMongoDB.Driver.IMongoDatabase.GetCollection<TDocument>(string, MongoDB.Driver.MongoCollectionSettings)*

	dbService*
""*
""]
[
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.cs3 3(U
studentCollection"__id*

%1Ÿ
œ
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.cs4$ 4(\
%2"xMicrosoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<T>(System.IServiceProvider)*M"K
IMicrosoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions*

serviceProvider]
[
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.cs4 4(\
studyGroupService"__id*

%2b`
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.cs6 6(+
%3" FRITeam.Swapify.Entities.Studentt
r
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.cs6" 6()
%4"*FRITeam.Swapify.Entities.Student.Student()*

%3S
Q
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.cs6 6(+	
student"__id*

%3²
¯
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.cs7" 7(P
%5"PFRITeam.Swapify.Backend.Interfaces.IStudyGroupService.GetStudyGroupAsync(string)*

studyGroupService*
""N
L
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.cs7 7(P
sg"__id*
""{
y
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.cs8  8(,
%6"1FRITeam.Swapify.Entities.StudyGroup.Timetable.get*

sgt
r
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.cs8  8(4
%7"*FRITeam.Swapify.Entities.Timetable.Clone()*

%6†
ƒ
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.cs8 8(4
%8".FRITeam.Swapify.Entities.Student.Timetable.set*
	
student*

%7‡
„
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.cs9 9(#
%9"/FRITeam.Swapify.Entities.Student.StudyGroup.set*
	
student*

sgý
ú
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.cs; ;(0
%10"„MongoDB.Driver.IMongoCollection<TDocument>.InsertOne(TDocument, MongoDB.Driver.InsertOneOptions, System.Threading.CancellationToken)*

studentCollection*
	
student*
""*
"""E
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.cs< <(
	
student*R
1"M
6
,C:\Projects\Swapify\Backend\DbSeed\DbSeed.cs= =(	
implicit return