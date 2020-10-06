using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PanelShop.Models;

namespace PanelShop.Repository
{
    public class PanelShopRepoContext : DbContext
    {
        public PanelShopRepoContext(DbContextOptions<PanelShopRepoContext> opt) : base(opt)
        {

        }

        public DbSet<Config> Configs { get; set; }

    }
}
