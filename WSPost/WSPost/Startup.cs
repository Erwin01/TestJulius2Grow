using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Text;
using WSPost.Context;
using WSPost.Logs;

namespace WSPost
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
            services.AddDbContext<PostDbContext>(opts => opts.UseSqlServer(Configuration["sqlconnection:connectionString"]));

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            #region [ SERVICE CONFIGURE BUFFER / MORE WEIGTH FILE]
            services.Configure<FormOptions>(o =>
               {
                   o.ValueLengthLimit = int.MaxValue;
                   o.MultipartBodyLengthLimit = int.MaxValue;
                   o.MemoryBufferThreshold = int.MaxValue;
               });
            #endregion


            services.AddControllers();

            services.AddSingleton<IServiceLogger, ServiceLogger>();

            #region [ JSON WEB TOKEN ]
            //Metodo de autenticacion/json web token
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;//Valor para el esquema autenticacion
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;//Esquema de desafío
            })


                //Json Web Tokens: Sirve para transferir de forma segura los datos de un usuario entre cliente y servidor.
                //install Microsoft.AspNetCore.Authentication.JwtBearer
                //1.El encabezado: Objeto JSON codificado base64/tipo token y algoritmo
                //2.La carga útil: Objeto javascript codificado base64/contiene atributos del usuario que iniciá sesión/identificacion,Si admin o no
                //3.Firma: El servidor es el encargado de verificar si el token es; valida la información/Mezcla de los 2 anteriores/clave secreta

                //Metodo de extension para liberar nuestros servicios de configuracion, metodo lineas de codigo adicionales

                //configura opciones necesarias para la autenticacion
                .AddJwtBearer(option =>
                {
                    //Propiedad de los parametros de validacion del token y creando una nueva validacion del token
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true, //El token se valida si el emisor es real que creo el token
                        ValidateAudience = true, //El destinatario
                        ValidateLifetime = true, //El token no ha expirado la firma(vida)
                        ValidateIssuerSigningKey = true, //La vclave es valida y el servidor confia en ella

                        //Se proporciona un valor para el válido(token)
                        ValidIssuer = "https://localhost:5001",
                        ValidAudience = "https://localhost:5001",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretTopKeyP@$$0369")),
                        ClockSkew = TimeSpan.Zero //No permite hacer ajustes en el algoritmo que determina si ya el token ha expirado
                    };

                });
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {

            //Proveedor crea documento
            loggerFactory.AddFile("Log-{Date}.txt");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors("CorsPolicy");

            #region [ STATIC FILES ]
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"StaticFiles")),
                RequestPath = new PathString("/StaticFiles")
            });
            #endregion

            app.UseRouting();

            app.UseAuthentication(); //jwt

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
