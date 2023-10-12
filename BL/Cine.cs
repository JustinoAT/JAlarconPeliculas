using Microsoft.EntityFrameworkCore;
using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Cine
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                
                using (DL.JalarconCineContext context = new DL.JalarconCineContext())
                {
                    var query = context.Cines.FromSqlRaw("CineGetAll").ToList();
                    
                    if (query != null)
                    {
                        result.Objects = new List<object>();
                        ML.Cine cine1 = new ML.Cine();
                        cine1.Estadisctica = new ML.Estadisctica(); 
                        foreach (var obj in query)
                        {
                            ML.Cine cine = new ML.Cine();
                            cine.Zona = new ML.Zona(); 
                            cine.IdCine = obj.IdCine;
                            cine.Nombre = obj.Nombre;
                            cine.Direccion = obj.Direccion;
                            cine.Zona.IdZona = obj.IdZona;
                            cine.Zona.Nombre = obj.NombreZona;
                            cine.Ventas = obj.Ventas;

                            if (cine.Zona.Nombre == "Norte")
                            {
                                cine1.Estadisctica.Norte += cine.Ventas;
                              
                            }
                            else if (cine.Zona.Nombre == "Sur")
                            {
                                cine1.Estadisctica.Sur += cine.Ventas;
                            }
                            else if (cine.Zona.Nombre == "Este")
                            {
                                cine1.Estadisctica.Este += cine.Ventas;
                            }
                            else if (cine.Zona.Nombre == "Oeste")
                            {
                                cine1.Estadisctica.Oeste += cine.Ventas;
                            }
                            result.Objects.Add(cine);


                        }
                        cine1.Estadisctica.Total = (cine1.Estadisctica.Norte + cine1.Estadisctica.Sur + cine1.Estadisctica.Este + cine1.Estadisctica.Oeste);
                        cine1.Estadisctica.Norte = (cine1.Estadisctica.Norte /cine1.Estadisctica.Total)*100;
                        cine1.Estadisctica.Sur = (cine1.Estadisctica.Sur /cine1.Estadisctica.Total)*100;
                        cine1.Estadisctica.Este = (cine1.Estadisctica.Este /cine1.Estadisctica.Total)*100;
                        cine1.Estadisctica.Oeste = (cine1.Estadisctica.Oeste /cine1.Estadisctica.Total)*100;
                        result.Object = cine1.Estadisctica;  
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
                result.Correct= false;  
                result.ErrorMessage= ex.Message;
                result.Exception = ex; 


            }
            return result;
        }

        public static ML.Result GetById(int IdCine)
        {
            ML.Result result = new ML.Result();
            try {
                using (DL.JalarconCineContext context = new DL.JalarconCineContext())
                {
                    var query = context.Cines.FromSqlRaw($"CineGeById {IdCine}").AsEnumerable().SingleOrDefault();
                    result.Objects = new List<object>();
                    if (query != null)
                    {

                        ML.Cine cine = new ML.Cine();
                        cine.Zona = new ML.Zona();
                        cine.Nombre = query.Nombre;
                        cine.Direccion = query.Direccion;
                        cine.Zona.IdZona = query.IdZona;
                        cine.Zona.Nombre = query.NombreZona; 
                        cine.Ventas = query.Ventas;
                        result.Object = cine;
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "La tabla no contiene un registro con el ID indicado";
                    }

                }

            
            }catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage=ex.Message;
                result.Exception = ex;

            }

            return result;
        }


        public static ML.Result Delete(int IdCine)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JalarconCineContext context = new DL.JalarconCineContext())
                {
                    int rowAffected = context.Database.ExecuteSqlRaw($"CineDelete {IdCine}");
                    if (rowAffected > 0)
                    {
                        result.Correct = true;

                    }
                    else
                    {
                        result.Correct = false;
                    }
                }

            }catch (Exception ex) {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Exception = ex;

            }
            return result;

        }


        public static ML.Result Add(ML.Cine cine)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JalarconCineContext context = new DL.JalarconCineContext())
                {
                    int rowAffected = context.Database.ExecuteSqlRaw($"CineAdd '{cine.Nombre}', '{cine.Direccion}',{cine.Zona.IdZona},{cine.Ventas}");
                    if (rowAffected >0)
                    {
                        result.Correct = true;

                    }
                    else
                    {
                        result.Correct = false;

                    }
                }

            }catch (Exception ex) {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Exception = ex;
            }

            return result;

        }


        public static ML.Result Update(ML.Cine cine)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JalarconCineContext context = new DL.JalarconCineContext())
                {
                    int rowAffected = context.Database.ExecuteSqlRaw($"CineUpdate {cine.IdCine},'{cine.Nombre}', '{cine.Direccion}',{cine.Zona.IdZona},{cine.Ventas}");
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

        public static ML.Result GetDirecciones()
        {
            ML.Result result = new ML.Result();
            try
            {

                using (DL.JalarconCineContext context = new DL.JalarconCineContext())
                {
                    var query = context.Cines.FromSqlRaw("DireccionesGetAll").ToList();

                    if (query != null)
                    {
                        result.Objects = new List<object>();
                        foreach (var obj in query)
                        {
                            ML.Cine cine = new ML.Cine();
                            cine.Direccion = obj.Direccion;
                            result.Objects.Add(cine);


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
