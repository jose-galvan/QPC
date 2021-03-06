﻿using System.Collections.Generic;
using System.Threading.Tasks;
using QPC.Core.Models;

namespace QPC.Core.Repositories
{
    public interface IQualityControlRepository: IRepository<QualityControl>
    {
        Task<QualityControl> GetWithDetailsAsync(int id);
        Task<List<QualityControl>> GetAllWithDetailsAsync();
    }
}