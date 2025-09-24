namespace SoftwareArkitektur
{
    class Program
    {
        public static void Main()
        {
            
            var builder = WebApplication.CreateBuilder();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();   
            builder.Services.AddSwaggerGen();         
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });  

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();                       
                app.UseSwaggerUI();                      
            }

            app.UseCors("AllowAll");
            app.MapControllers();

            app.Run();

        }
    }
}

