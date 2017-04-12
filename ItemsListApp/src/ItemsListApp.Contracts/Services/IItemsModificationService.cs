﻿using System;
using System.Threading.Tasks;
using ItemsListApp.Contracts.Models;

namespace ItemsListApp.Contracts.Services
{
    public interface IItemsModificationService
    {
        Task<Item> ReplaceAsync(Item item);

        Task<bool> DoesExistAsync(Guid id);
    }
}