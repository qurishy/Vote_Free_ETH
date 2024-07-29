using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Voter_Data_API.MODELS;
using Voter_Data_API.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Serielizing the data that are going to the database

BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));


//add mongoIdentity congfigration
var mongoidentityconfig = new MongoDbIdentityConfiguration
{
    MongoDbSettings = new MongoDbSettings
    {
        ConnectionString = builder.Configuration.GetSection("MyDb:ConnectionString").Value,
        DatabaseName = builder.Configuration.GetSection("MyDb:DatabaseName").Value
    },IdentityOptionsAction = options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 8;
        
        //lockou
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
        options.Lockout.MaxFailedAccessAttempts = 5;
       

        options.User.RequireUniqueEmail = true;
    }
};

// Add Identity
builder.Services.ConfigureMongoDbIdentity<ApplicationUsers, ApplicationRole, Guid>(mongoidentityconfig)
    .AddUserManager<UserManager<ApplicationUsers>>()
    .AddSignInManager<SignInManager<ApplicationUsers>>()
    .AddRoleManager<RoleManager<ApplicationRole>>()
    .AddDefaultTokenProviders();

// Adding Authentication
builder.Services.AddAuthentication(options =>
{

    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ClockSkew = TimeSpan.Zero
    };

});

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<Mongo_DataSettings>(
    builder.Configuration.GetSection("MyDb")
    );

//Resolving the VotersPersonelService Dependency Injection
builder.Services.AddTransient<IVotersPersonelService, VotersPersonelService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
