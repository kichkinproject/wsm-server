using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ScenarioManager.Mappers;
using ScenarioManager.Mappers.AdminMapper;
using ScenarioManager.Mappers.ScenarioMapper;
using ScenarioManager.Mappers.User;
using ScenarioManager.Mappers.UserGroupMappers;
using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DBModel.DBContexts;
using ScenarioManager.Model.DTO;
using ScenarioManager.Model.DTO.AdminDTO;
using ScenarioManager.Model.DTO.UserGroupDTO;
using ScenarioManager.Repositories;
using ScenarioManager.Services;

namespace ScenarioManager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MainDbContext>(options =>
            options.UseSqlServer(Configuration["MainConnectionString"]));
            services.AddDbContext<UserDbContext>(options =>
            options.UseSqlServer(Configuration["AccountConnectionString"]));
            services.AddScoped<IMapper<Admin, AdminWithPassword>, AdminWithPasswordMapper>();
            services.AddScoped<IMapper<ScenarioDTO, Scenario>, ScenarioMapper>();
            services.AddScoped<IMapper<UserDTO, User>, UserMapper>();

            services.AddScoped<IMapper< UserGroup, CreateUserGroup>, CreateUserGroupMapper>();
            services.AddScoped<IMapper<UserGroup, EditUserGroup>, EditUserGroupMapper>();
            services.AddScoped<UserGroupRepository>();
            services.AddScoped<ScenarioRepository>();
            services.AddScoped<TokenRepository>();
            services.AddScoped<AdminRepository>();
            services.AddScoped<UserLoginInfoRepository>();
            services.AddScoped<UserRepository>();
            services.AddScoped<AccountService>();
            services.AddScoped<TokenService>();
            services.AddScoped<LoginService>();

            

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
