using FRITeam.Swapify.Entities;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FRITeam.Swapify.Backend.Interfaces
{
    public interface IStudentService
    {
        Task AddStudentAsync(Student entityToAdd);

        Student FindStudentById(string guid);
    }
}
