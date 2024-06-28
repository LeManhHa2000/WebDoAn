using AspNetCoreHero.ToastNotification;
using Microsoft.EntityFrameworkCore;
using WebDoAn.dbs;
using WebDoAn.Service.Admin.Blogs;
using WebDoAn.Service.Admin.Categories;
using WebDoAn.Service.Admin.OrderDetails;
using WebDoAn.Service.Admin.Orders;
using WebDoAn.Service.Admin.Products;
using WebDoAn.Service.Admin.Users;
using WebDoAn.Service.Client.Carts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.BottomRight; });

// Dang ki DB
var connectString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<DoAnDbContext>(option => option.UseNpgsql(connectString));

builder.Services.AddScoped<ICategoryService,  CategoryService>();
builder.Services.AddScoped<IProductService,  ProductService>();
builder.Services.AddScoped<IBlogService,  BlogService>();
builder.Services.AddScoped<IUserService,  UserService>();
builder.Services.AddScoped<ICartService,  CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(300);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
      name: "default",
      pattern: "{controller=Home}/{action=Index}/{id?}"
    );
});

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
