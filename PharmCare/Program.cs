

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PharmCare.BLL.Profiles;
using PharmCare.BLL.Repositories.Accounts.OpeningBalanceModule;
using PharmCare.BLL.Repositories.CategoryModule;
using PharmCare.BLL.Repositories.CountryModule;
using PharmCare.BLL.Repositories.LeafSettingModule;
using PharmCare.BLL.Repositories.SupplierModule;
using PharmCare.BLL.Repositories.MedecineModule;
using PharmCare.BLL.Repositories.PatientModule;
using PharmCare.BLL.Repositories.ProductTypeModule;
using PharmCare.BLL.Repositories.SalesModule;
using PharmCare.BLL.Repositories.ShelfModule;
using PharmCare.BLL.Repositories.StockModule;
using PharmCare.BLL.Repositories.UnitModule;
using PharmCare.DAL.DbContext;
using PharmCare.DAL.Models;
using PharmCare.Extensions;
using PharmCare.SeedAppUsers;
using PharmCare.Services.EmailModule;
using PharmCare.BLL.Repositories.ProductModule;
using PharmCare.BLL.Repositories.MedicalConditionModule;
using PharmCare.BLL.Repositories.PrescriptionModule;
using PharmCare.BLL.Repositories.ApplicationUserModule;
using PharmCare.Services.SMSModule;
using Rotativa.AspNetCore;
using Microsoft.Extensions.Caching.Memory;
using PharmCare.BLL.Repositories.CountyModule;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddDbContext<ApplicationDbContext>(options =>

options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);

builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddTransient<IUserClaimsPrincipalFactory<AppUser>, ApplicationUserClaimsPrincipalFactory>();

builder.Services.AddTransient<IPatientRepository, PatientRepository>();

builder.Services.AddTransient<ICountryRepository, CountryRepository>();

builder.Services.AddTransient<ISupplierRepository, SupplierRepository>();

builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();

builder.Services.AddTransient<IShelfRepository, ShelfRepository>();

builder.Services.AddTransient<IUnitRepository, UnitRepository>();

builder.Services.AddTransient<ILeafSettingRepository, LeafSettingRepository>();

builder.Services.AddTransient<IMedicineRepository, MedicineRepository>();

builder.Services.AddTransient<IStockRepository, StockRepository>();

builder.Services.AddTransient<IProductTypeRepository, ProductTypeRepository>();

builder.Services.AddTransient<IOpeningBalanceRepository, OpeningBalanceRepository>();

builder.Services.AddTransient<ISalesRepository, SalesRepository>();

builder.Services.AddTransient<IProductRepository, ProductRepository>();

builder.Services.AddTransient<IMedicalConditionRepository, MedicalConditionRepository>();

builder.Services.AddTransient<IPrescriptionRepository, PrescriptionRepository>();

builder.Services.AddTransient<IApplicationUserRepository, ApplicationUserRepository>();

builder.Services.AddTransient<IMailService, MailService>();

builder.Services.AddTransient<IMessagingService, MessagingService>();

builder.Services.AddTransient<ICountyRepository, CountyRepository>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

RotativaConfiguration.Setup((Microsoft.AspNetCore.Hosting.IHostingEnvironment)app.Environment);

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles();

app.UseFileServer();

app.UseCors("CorsPolicy");




app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
    name: "Pharmacist",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");    
    
    endpoints.MapControllerRoute(
    name: "Doctor",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");
});


var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

using (var scope = scopeFactory.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    SeedUsers.Seed(userManager, roleManager);

}



app.Run();
