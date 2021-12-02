using FRITeam.Swapify.SwapifyBase.Entities.Enums;
using System;
using WebAPI.Models.Exchanges;
using WebAPI.Models.UserModels;

namespace IntegrationTest.ExchangeControllerTest
{
    public static class ExchangeControllerTestsData
    {
        public static readonly Guid StduentGuid = Guid.Parse("72338e48-9829-47b5-a666-766bbbecd799");
        public static readonly Guid CourseGuid = Guid.Parse("180ce481-85a3-4246-93b5-ba0a0229c59f");
        public static readonly Guid Course2Guid = Guid.Parse("3F2504E0-4F89-11D3-9A0C-0305E82C3301");

        public static LoginModel Login1 = new LoginModel()
        {
            Email = "oleg@swapify.com",
            Password = "Heslo123"
        };

        public static LoginModel Login2 = new LoginModel()
        {
            Email = "oleg2@swapify.com",
            Password = "Heslo123"
        };

        public static LoginModel Login3 = new LoginModel()
        {
            Email = "oleg3@swapify.com",
            Password = "Heslo123"
        };

        public static ExchangeRequestModel ExchangeModel11 = new ExchangeRequestModel()
        {
            BlockFrom = new BlockForExchangeModel()
            {
                Day = (int)Day.Monday,
                Duration = 2,
                StartHour = 9,
                CourseId = CourseGuid.ToString()
            },
            BlockTo = new BlockForExchangeModel()
            {
                Day = (int)Day.Thursday,
                Duration = 2,
                StartHour = 15,
                CourseId = CourseGuid.ToString()
            },
        };

        public static ExchangeRequestModel ExchangeModel12 = new ExchangeRequestModel()
        {
            BlockFrom = new BlockForExchangeModel()
            {
                Day = (int)Day.Monday,
                Duration = 2,
                StartHour = 9,
                CourseId = CourseGuid.ToString()
            },
            BlockTo = new BlockForExchangeModel()
            {
                Day = (int)Day.Wednesday,
                Duration = 2,
                StartHour = 10,
                CourseId = CourseGuid.ToString()
            },
        };

        public static ExchangeRequestModel ExchangeModel13 = new ExchangeRequestModel()
        {
            BlockFrom = new BlockForExchangeModel()
            {
                Day = (int)Day.Monday,
                Duration = 2,
                StartHour = 9,
                CourseId = Course2Guid.ToString()
            },
            BlockTo = new BlockForExchangeModel()
            {
                Day = (int)Day.Tuesday,
                Duration = 2,
                StartHour = 14,
                CourseId = Course2Guid.ToString()
            },
        };

        public static ExchangeRequestModel ExchangeModel21 = new ExchangeRequestModel()
        {
            BlockFrom = new BlockForExchangeModel()
            {
                Day = (int)Day.Friday,
                Duration = 2,
                StartHour = 13,
                CourseId = Course2Guid.ToString()
            },
            BlockTo = new BlockForExchangeModel()
            {
                Day = (int)Day.Tuesday,
                Duration = 2,
                StartHour = 14,
                CourseId = Course2Guid.ToString()
            },
        };

        public static ExchangeRequestModel ExchangeModel22 = new ExchangeRequestModel()
        {
            BlockFrom = new BlockForExchangeModel()
            {
                Day = (int)Day.Wednesday,
                Duration = 2,
                StartHour = 10,
                CourseId = CourseGuid.ToString()
            },
            BlockTo = new BlockForExchangeModel()
            {
                Day = (int)Day.Monday,
                Duration = 2,
                StartHour = 9,
                CourseId = CourseGuid.ToString()
            },
        };
    }
}
