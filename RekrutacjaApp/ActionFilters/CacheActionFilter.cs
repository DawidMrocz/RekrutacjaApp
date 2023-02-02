using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using MyWebApplication.Dtos;
using Newtonsoft.Json;
using RekrutacjaApp.Controllers;
using RekrutacjaApp.Data;
using RekrutacjaApp.Entities;
using RekrutacjaApp.Helpers;
using RekrutacjaApp.Repositories;

namespace RekrutacjaApp.ActionFilters
{
    public class CacheActionFilter : ActionFilterAttribute, IActionFilter
    {
        private readonly IDistributedCache _cache;
        public required string DTOName { get; set; }
        public CacheActionFilter
            (
                IDistributedCache cache
            )
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }
        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            bool exist = context.ActionArguments.ContainsKey((string)context.ActionArguments[DTOName]);
            if (!exist)
            {
                await _cache.DeleteRecordAsync<User>($"User_{context.ActionArguments[DTOName]}");
                await _cache.DeleteRecordAsync<List<User>>(JsonConvert.SerializeObject(new QueryParams()));
                return;
            }
        }
    }
}
