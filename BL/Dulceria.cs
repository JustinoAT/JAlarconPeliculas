using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Dulceria
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();

            try
            {
                using(DL.JalarconCineContext context = new DL.JalarconCineContext())
                {
                    var query = context.Dulceria.FromSqlRaw("DulceriaGetAll").ToList();

                    if(query != null)
                    {
                        result.Objects = new List<object>();
                        foreach(var obj in query)
                        {
                            ML.Dulceria dulceria = new ML.Dulceria();
                            dulceria.IdDulceria = obj.IdDulceria;
                            dulceria.Nombre = obj.Nombre;
                            dulceria.Precio = obj.Precio;
                            dulceria.Imagen = obj.Imagen;
                            result.Objects.Add(dulceria);
                            result.Correct = true;
                        }
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "La tabla no contiene registros";
                    }
                }
                

            }catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Exception = ex;

            }
            return result;
        }

        public static ML.Result GetById(int idDulceria)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JalarconCineContext context = new DL.JalarconCineContext())
                {
                    var query = context.Dulceria.FromSqlRaw($"DulceriaGetById {idDulceria}").AsEnumerable().SingleOrDefault();
                    result.Objects = new List<object>();
                    if (query != null)
                    {

                        ML.Dulceria dulceria = new ML.Dulceria();
                        dulceria.Nombre = query.Nombre;
                        dulceria.Precio = query.Precio;
                        dulceria.Imagen = query.Imagen;
                        result.Object = dulceria;
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "La tabla no contiene un registro con el ID indicado";
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
