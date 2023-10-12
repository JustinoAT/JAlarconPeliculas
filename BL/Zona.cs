using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Zona
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {

                using (DL.JalarconCineContext context = new DL.JalarconCineContext())
                {
                    var query = context.Zonas.FromSqlRaw("ZonaGetAll").ToList();
                    result.Objects = new List<object>();
                    if (query != null)
                    {
                        result.Objects = new List<object>();
                        foreach (var obj in query)
                        {
                            ML.Zona zona = new ML.Zona();
                           
                            zona.IdZona = obj.IdZona;
                            zona.Nombre = obj.Nombre;
                            result.Objects.Add(zona);


                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "La tabla no contiene registros";
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
