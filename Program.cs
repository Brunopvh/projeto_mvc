using Microsoft.EntityFrameworkCore;
using ProjetoMvc.Models;

var builder = WebApplication.CreateBuilder(args);

// Adicionar o contexto do banco de dados ao contêiner de injeção de dependência
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

// Configuração do pipeline de solicitação HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
