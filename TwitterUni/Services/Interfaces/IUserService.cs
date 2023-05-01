﻿using Microsoft.AspNetCore.Identity;
using TwitterUni.Data.Entities;
using TwitterUni.Services.ModelData;

namespace TwitterUni.Services.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUser(UserData user, string password);
        UserData? GetUserById(string id);
        UserData? GetUserByUserName(string userName);
        IEnumerable<UserData> GetAllUsers();
        public IEnumerable<UserData> GetAllUsersWithFollows();
        void UpdateUser(UserData user);
        Task<IdentityResult> DeleteUser(string id);
        Task<SignInResult?> SignInUser(string userName, string password);
        Task SignOutUser();
        void CompleteUserSetup(UserData user, string password);
        bool FollowUser(string followerId, string followingId);
        bool UnfollowUser(string followerUserName, string followingUserName);
        Task<bool> CheckPassword(string userName, string password);
    }
}
