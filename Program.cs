using QuanLyDaoTaoLaiXe.Components.HoiDap;
using QuanLyDaoTaoLaiXe.Components.XeTapLai;
using System.Reflection;
using QuanLyDaoTaoLaiXe.Components.KhaoSat;
using QuanLyDaoTaoLaiXe.Components.MonHoc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllers()
    .AddJsonOptions(options =>// dùng enum chứ k dùng số (giá trị mặc định của enum) 
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    //Lấy đường dẫn đến file XML documentation đã tạo
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    //hiển thị summary cho controller
    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

builder.Services.AddTransient<IXeTapLaiRepository, XeTapLaiRepository>();
builder.Services.AddTransient<IHoiDapRepository, HoiDapRepository>();
builder.Services.AddTransient<IMonHocRepository, MonHocRepository>();
builder.Services.AddTransient<IKhaoSatRepository, KhaoSatRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
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
app.MapControllers();
app.Run();
