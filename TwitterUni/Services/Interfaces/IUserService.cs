﻿using Microsoft.AspNetCore.Identity;
using TwitterUni.Data.Entities;
using TwitterUni.Services.ModelData;

namespace TwitterUni.Services.Interfaces
{
    public interface IUserService
    {
        Task CreateUser(UserData user);
        UserData? GetUserById(string id);
        UserData? GetUserByUserName(string userName);
        IEnumerable<UserData> GetAllUsers();
        void UpdateUser(UserData user);
        Task DeleteUser(string id);
        Task<bool> SignInUser(string userName, string password);
        Task SignOutUser();
        void CompleteUserSetup(UserData user, string password);
    }
}
