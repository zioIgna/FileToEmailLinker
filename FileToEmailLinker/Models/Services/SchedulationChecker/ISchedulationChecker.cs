﻿namespace FileToEmailLinker.Models.Services.SchedulationChecker
{
    public interface ISchedulationChecker
    {
        Task<int> SetSchedulationsTimers();
    }
}