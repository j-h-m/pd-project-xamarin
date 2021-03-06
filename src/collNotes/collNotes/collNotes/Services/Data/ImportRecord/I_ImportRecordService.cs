﻿using collNotes.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace collNotes.Services.Data
{
    public interface IImportRecordService
    {
        Task<bool> AddAsync(ImportRecord importRecord);
        Task<bool> HasFileBeenImported(string fileName);
        bool DeleteAll();
    }
}
