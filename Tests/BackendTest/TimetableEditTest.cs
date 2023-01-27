using FluentAssertions;
using FRITeam.Swapify.Backend;
using FRITeam.Swapify.SwapifyBase.Entities;
using FRITeam.Swapify.SwapifyBase.Entities.Enums;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BackendTest
{
    [Collection("Database collection")]
    public class TimetableEditTest : IClassFixture<Mongo2GoFixture>
    {
        private readonly Mongo2GoFixture _mongoFixture;


        public TimetableEditTest(Mongo2GoFixture mongoFixture)
        {
            _mongoFixture = mongoFixture;
        }

        [Fact]
        public async Task EditBlockStudentTimetablePassed()
        {
            IMongoDatabase database = _mongoFixture.MongoClient.GetDatabase("StudentsDB");
            TimetableDataService timetableDataService = new TimetableDataService(database);
            TimetableData ttData = new TimetableData();
            ttData.Timetable = FakeTimetable.GetFakeTimetable();
            await timetableDataService.AddAsync(ttData);

            var loadedTtData = await timetableDataService.FindByIdAsync(ttData.Id);
            Block blckToAdd = new Block { BlockId = Guid.NewGuid(), BlockType = BlockType.Lecture, Day = Day.Friday, StartHour = 9 };

            loadedTtData.Timetable.AllBlocks.Count(x => x.Equals(blckToAdd)).Should().Be(0);
            //add new block
            loadedTtData.Timetable.AddNewBlock(blckToAdd);
            //save new block
            await timetableDataService.UpdateTimetableDataAsync(loadedTtData);
            //load from db
            TimetableData updatedTtData = await timetableDataService.FindByIdAsync(loadedTtData.Id);
            //test
            updatedTtData.Timetable.AllBlocks.Count(x => x.Equals(blckToAdd)).Should().Be(1);
            updatedTtData.Timetable.ContainsBlock(blckToAdd).Should().Be(true);

            Block updtBlock = new Block { BlockId = blckToAdd.BlockId, BlockType = BlockType.Excercise, Day = Day.Friday, StartHour = 9 };
            // update blckToAdd to updtBlock
            updatedTtData.Timetable.UpdateBlock(updtBlock);
            //save updated block
            await timetableDataService.UpdateTimetableDataAsync(updatedTtData);
            //load from db
            updatedTtData = await timetableDataService.FindByIdAsync(loadedTtData.Id);
            //test
            updatedTtData.Timetable.AllBlocks.Count(x => x.Equals(updtBlock)).Should().Be(1);

            //delete added block
            updatedTtData.Timetable.RemoveBlock(updtBlock.BlockId);
            //save deleted
            await timetableDataService.UpdateTimetableDataAsync(updatedTtData);
            //load from db
            updatedTtData = await timetableDataService.FindByIdAsync(loadedTtData.Id);
            //test
            updatedTtData.Timetable.AllBlocks.Count(x => x.Equals(updtBlock)).Should().Be(0);
            updatedTtData.Timetable.ContainsBlock(updtBlock).Should().Be(false);
        }
    }

    public class FakeTimetable
    {
        public static Timetable GetFakeTimetable()
        {
            Timetable tt = new Timetable(Semester.GetSemester());

            tt.AddNewBlock(new Block
            {
                BlockType = BlockType.Excercise,
                Day = Day.Monday,
                Duration = 2,
                StartHour = 7
            });
            tt.AddNewBlock(new Block
            {
                BlockType = BlockType.Excercise,
                Day = Day.Monday,
                Duration = 2,
                StartHour = 10
            });
            tt.AddNewBlock(new Block
            {
                BlockType = BlockType.Excercise,
                Day = Day.Tuesday,
                Duration = 2,
                StartHour = 16
            });
            tt.AddNewBlock(new Block
            {
                BlockType = BlockType.Lecture,
                Day = Day.Wednesday,
                Duration = 2,
                StartHour = 12
            });
            tt.AddNewBlock(new Block
            {
                BlockType = BlockType.Lecture,
                Day = Day.Thursday,
                Duration = 2,
                StartHour = 11
            });
            return tt;
        }

    }
}

