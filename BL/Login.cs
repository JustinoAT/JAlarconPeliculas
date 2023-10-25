using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Login
    {
        public static ML.Result UsuarioGetByEmail(string email)
        {

            ML.Result result = new ML.Result();
            try
            {
                using (DL.JalarconCineContext context = new DL.JalarconCineContext())
                {
                    var query = context.Usuarios.FromSqlRaw($"UsuarioGetByEmail '{email}'").AsEnumerable().SingleOrDefault();

                    if (query != null)
                    {

                        ML.Usuario usuario = new ML.Usuario();
                        usuario.Email = query.Email;
                        usuario.Contrasena = query.Contrasena;
                        result.Object = usuario;

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
            }
            return result;

        }
        public static ML.Result Add(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JalarconCineContext context = new DL.JalarconCineContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"UsuarioAdd '{usuario.Username}','{usuario.Nombre}','{usuario.Email}',@Contrasena", new SqlParameter("@Contrasena", usuario.Contrasena));

                    if (query >= 1)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Error no se agrego el usuario";
                    }
                    result.Correct = true;

                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;

        }

        public static ML.Result Update(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JalarconCineContext context = new DL.JalarconCineContext())
                {
                    int rowAffected = context.Database.ExecuteSqlRaw($"UsuarioUpdatePassword '{usuario.Email}',@Contrasena ", new SqlParameter("@Contrasena", usuario.Contrasena));
                    if (rowAffected > 0)
                    {
                        result.Correct = true;

                    }
                    else
                    {
                        result.Correct = false;

                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Exception = ex;
            }

            return result;

        }

    }
}
