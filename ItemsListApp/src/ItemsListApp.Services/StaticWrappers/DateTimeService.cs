﻿using System;
using System.Threading.Tasks;
using ItemsListApp.Contracts.Services;

namespace ItemsListApp.Services.StaticWrappers
{
    internal class DateTimeService : IDateTimeService
    {
        public Task<System.DateTime> GetCurrentDateAsync()
        {
            return Task.FromResult(DateTime.Now);
        }
    }
}