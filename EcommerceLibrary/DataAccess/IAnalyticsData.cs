using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcommerceLibrary.Models;

namespace EcommerceLibrary.DataAccess;
public interface IAnalyticsData
{
    Task<List<AnalyticsModel>> GetAll();
    Task<List<AnalyticsModel>> GetToday();
    Task<List<AnalyticsModel>> getLast30Days();
    Task<List<AnalyticsModel>> GetLast7days();
}
