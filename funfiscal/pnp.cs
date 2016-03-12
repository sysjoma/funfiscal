/*
 */
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Data;
using System.Threading;

namespace funfiscal
{
	public class pnp
	{
		public pnp()
		{
		}
	
		[DllImport("PNPDLL.dll")] private static extern string PFAbreNF();
		[DllImport("PNPDLL.dll")] private static extern string PFabrefiscal(String Razon, String RIF);
		[DllImport("PNPDLL.dll")] private static extern string PFtotal();
		[DllImport("PNPDLL.dll")] private static extern string PFrepz();
		[DllImport("PNPDLL.dll")] private static extern string PFrepx();
		[DllImport("PNPDLL.dll")] private static extern string PFrenglon(String Descripcion, String cantidad, String monto, String iva);
		[DllImport("PNPDLL.dll")] private static extern string PFabrepuerto(String numero);
		[DllImport("PNPDLL.dll")] private static extern string PFcierrapuerto();
		[DllImport("PNPDLL.dll")] private static extern string PFDisplay950(String edlinea);
		[DllImport("PNPDLL.dll")] private static extern string PFLineaNF(String edlinea);
		[DllImport("PNPDLL.dll")] private static extern string PFCierraNF();
		[DllImport("PNPDLL.dll")] private static extern string PFDescuento(String edbarra, String monto);
		[DllImport("PNPDLL.dll")] private static extern string PFCortar();
		[DllImport("PNPDLL.dll")] private static extern string PFTfiscal(String edlinea);
		[DllImport("PNPDLL.dll")] private static extern string PFparcial();
		[DllImport("PNPDLL.dll")] private static extern string PFSerial();
		[DllImport("PNPDLL.dll")] private static extern string PFtoteconomico();
		[DllImport("PNPDLL.dll")] private static extern string PFCancelaDoc(String edlinea, String monto);
		[DllImport("PNPDLL.dll")] private static extern string PFGaveta();
		[DllImport("PNPDLL.dll")] private static extern string PFDevolucion(String razon, String rif, String comp, String maqui, String fecha, String hora);
		[DllImport("PNPDLL.dll")] private static extern string PFSlipON();
		[DllImport("PNPDLL.dll")] private static extern string PFSLIPOFF();
		[DllImport("PNPDLL.dll")] private static extern string PFestatus(String edlinea);
		[DllImport("PNPDLL.dll")] private static extern string PFreset();
		[DllImport("PNPDLL.dll")] private static extern string PFendoso(String campo1, String campo2, String campo3, String tipoendoso);
		[DllImport("PNPDLL.dll")] private static extern string PFvalida675(String campo1, String campo2, String campo3, String campo4);
		[DllImport("PNPDLL.dll")] private static extern string PFCheque2(String mon, String ben, String fec, String c1, String c2, String c3, String c4, String campo1, String campo2);
		[DllImport("PNPDLL.dll")] private static extern string PFcambiofecha(String edfecha, String edhora);
		[DllImport("PNPDLL.dll")] private static extern string PFcambiatasa(String t1, String t2, String t3);
		[DllImport("PNPDLL.dll")] private static extern string PFBarra(String edbarra);
		[DllImport("PNPDLL.dll")] private static extern string PFVoltea();
		[DllImport("PNPDLL.dll")] private static extern string PFLeereloj();
		[DllImport("PNPDLL.dll")] private static extern string PFrepMemNF(String desf, String hasf, String modmem);
		[DllImport("PNPDLL.dll")] private static extern string PFRepMemoriaNumero(String desn, String hasn, String modmem);
		[DllImport("PNPDLL.dll")] private static extern string PFCambtipoContrib(String tip);
		[DllImport("PNPDLL.dll")] private static extern string PFultimo();

		public static bool GeneraFactura(string puerto,string referencia,
		                                 string rifoci,string nomcli,string dircli,
		                                 string tlfcli,string vendedor,string condipago,
		                                 decimal totaldoc,DateTime fechavence,
		                                 DataTable dtDetalle,DataTable dtPagos,
		                                 string checkin,string huesped,string habino,
		                                 string placa,string cajero,string estacion)
		{
			string  respuesta;
			bool    ok         = false;
			string  descrip, cant, precio, iva, talla, color, seriales, notas;
			bool    lineaExtra = false;

			if (false)
			{
				abrir_gaveta(puerto);
			}

			if (abrir_puerto(puerto) )
		    {
				rifoci    = rifoci.Substring(0,(rifoci.Length<12?rifoci.Length:12)).Trim();
				nomcli    = nomcli.Substring(0,(nomcli.Length<80?nomcli.Length:80)).Trim();
				dircli    = dircli.Trim();

	 			respuesta = PFabrefiscal(nomcli,rifoci);
				respuesta = PFTfiscal("DIRECCION: "+dircli);

				foreach (DataRow row in dtDetalle.Rows)
				{
					descrip    = row["descrip"].ToString();
					cant       = Convert.ToDecimal(row["cant"]).ToString("###0.000");
					cant       = cant.Replace(",",".");
					precio     = Convert.ToDecimal(row["precio"]).ToString("#####0.00");
					precio     = precio.Replace(",",".");
					iva        = Convert.ToDecimal(row["tasaiva"]).ToString("#0.00");
					iva        = iva.Replace(",","");
					talla      = row["talla"].ToString();
					color      = row["color"].ToString();
					seriales   = row["seriales"].ToString();
					notas      = row["notas"].ToString();
					lineaExtra = false;

					respuesta = PFrenglon(descrip,cant,precio,iva);
					Thread.Sleep(50);

					if (!string.IsNullOrEmpty(talla))
					{
						respuesta  = PFTfiscal(talla+" / "+color);
						lineaExtra = true;
					}

					if (!string.IsNullOrEmpty(seriales))
					{
						respuesta  = PFTfiscal("S/N:"+seriales);
						lineaExtra = true;
					}
				}

				if (lineaExtra)
				{
					respuesta = PFrenglon("","0","0","0");
				}

				respuesta = PFparcial();

				respuesta = PFTfiscal("REF. "+referencia+" / "+cajero+" / "+estacion);

				if (!string.IsNullOrEmpty(huesped))
				{
					respuesta = PFTfiscal("HUESPED: "+huesped);
				}

				if (!string.IsNullOrEmpty(habino))
				{
					respuesta = PFTfiscal("HAB. No: "+habino);
				}

				respuesta = PFtotal();

				ok = evalua_errores(respuesta);

				respuesta = PFcierrapuerto();
		    }

			return(ok);
		}

		public static bool GeneraNotaCredito(string puerto,string referencia,string rifoci,string nomcli,string dircli,string tlfcli,string vendedor,DataTable dtDetalle,string checkin,string huesped,string habino,string placa,string cajero,string estacion,string facturaNC,DateTime fechaFactura,string serialFiscal)
		{
			string  respuesta;
			bool    ok         = false;
			string  fecha, hora, descrip, cant, precio, iva, talla, color, seriales, notas;
			bool    lineaExtra = false;

			if (false)
			{
				abrir_gaveta(puerto);
			}

			if (abrir_puerto(puerto) )
		    {
				rifoci    = rifoci.Substring(0,(rifoci.Length<12?rifoci.Length:12)).Trim();
				nomcli    = nomcli.Substring(0,(nomcli.Length<80?nomcli.Length:80)).Trim();
				fecha     = fechaFactura.ToString("ddMMyy");
				hora      = fechaFactura.ToString("hhmm");

	 			respuesta = PFDevolucion(nomcli,rifoci,facturaNC,serialFiscal,fecha,hora);
	 			Thread.Sleep(50);

				foreach (DataRow row in dtDetalle.Rows)
				{
					descrip    = row["descrip"].ToString();
					cant       = Convert.ToDecimal(row["cant"]).ToString("###0.000");
					cant       = cant.Replace(",",".");
					precio     = Convert.ToDecimal(row["precio"]).ToString("#####0.00");
					precio     = precio.Replace(",",".");
					iva        = Convert.ToDecimal(row["tasaiva"]).ToString("#0.00");
					iva        = iva.Replace(",","");
					talla      = row["talla"].ToString();
					color      = row["color"].ToString();
					seriales   = row["seriales"].ToString();
					notas      = row["notas"].ToString();
					lineaExtra = false;

					respuesta = PFrenglon(descrip,cant,precio,iva);

					if (!string.IsNullOrEmpty(talla))
					{
						respuesta  = PFTfiscal(talla+" / "+color);
						lineaExtra = true;
					}

					if (!string.IsNullOrEmpty(seriales))
					{
						respuesta  = PFTfiscal("S/N:"+seriales);
						lineaExtra = true;
					}
				}

				if (lineaExtra)
				{
					respuesta = PFrenglon("","0","0","0");
				}

				respuesta = PFparcial();

				respuesta = PFTfiscal("REF. "+referencia+" / "+cajero+" / "+estacion);

				if (!string.IsNullOrEmpty(huesped))
				{
					respuesta = PFTfiscal("HUESPED: "+huesped);
				}

				if (!string.IsNullOrEmpty(habino))
				{
					respuesta = PFTfiscal("HAB. No: "+habino);
				}

				respuesta = PFtotal();

				ok = evalua_errores(respuesta);

				respuesta = PFcierrapuerto();
		    }

			return(ok);
		}

		public static bool DocNoFiscal(string puerto,DataTable dtNofis)
		{
			string  respuesta;
			bool    ok         = false;

			if (abrir_puerto(puerto) )
		    {
				respuesta = PFAbreNF();

				foreach (DataRow row in dtNofis.Rows)
				{
					respuesta = PFLineaNF(row["linea"].ToString());
				}

				respuesta = PFCierraNF();

				ok = evalua_errores(respuesta);

				respuesta = PFcierrapuerto();
		    }

			return(ok);
		}

		public static string NumSerial(string puerto)
		{
			string respuesta = "";
			string serial    = "";
			bool   ok        = false;

			if ( abrir_puerto(puerto) )
		    {

				respuesta = PFSerial();
				respuesta = PFultimo();

				ok        = evalua_errores(respuesta);

				if ( ok ) serial = respuesta.Substring(10,10);

				respuesta = PFcierrapuerto();

			}

			return(serial);
		}

		public static string UltNumFactura(string puerto)
		{
			string respuesta = "";
			string numero    = "";
			bool   ok        = false;

			if ( abrir_puerto(puerto) )
		    {

				respuesta = PFestatus("N");
				respuesta = PFultimo();

				ok        = evalua_errores(respuesta);

				if ( ok ) numero = respuesta.Substring(43,8);

				respuesta = PFcierrapuerto();

			}

			return(numero);
		}

		public static string UltNumNotaCredito(string puerto)
		{
			string respuesta = "";
			string numero    = "";
			bool   ok        = false;

			if ( abrir_puerto(puerto) )
		    {

				respuesta = PFestatus("T");
				respuesta = PFultimo();

				ok        = evalua_errores(respuesta);

				if ( ok ) numero = respuesta.Substring(33);

				respuesta = PFcierrapuerto();

			}

			return(numero);
		}

		public static bool ReporteZ(string puerto)
		{
			string respuesta;
			bool ok = false;

			if ( abrir_puerto(puerto) )
		    {
				respuesta = PFrepz();
			    ok        = evalua_errores(respuesta);
			    respuesta = PFcierrapuerto();
		    }

			return(ok);
		}

		public static bool ReporteX(string puerto)
		{
			string respuesta;
			bool ok = false;

			if ( abrir_puerto(puerto) )
		    {
				respuesta = PFrepx();
			    ok        = evalua_errores(respuesta);
			    respuesta = PFcierrapuerto();
		    }

			return(ok);
		}

		public static bool ExtraerRepZ(string puerto)	
		{
			string   respuesta = "";
			bool     ok        = false;
			int      i,j;
			string   repznum, ultfactnum, ultncnum;
			DateTime ultfactfecha, ultncfecha;
			decimal  exento, ncexento, base1, base2, base3, ncbase1, ncbase2, ncbase3, iva1, iva2, iva3, nciva1, nciva2, nciva3;


			if ( abrir_puerto(puerto) )
		    {

				respuesta    = PFestatus("N");
				respuesta    = PFultimo();
				
				i            = (respuesta.LastIndexOf(",") + 1);
				repznum      = (Convert.ToInt32(respuesta.Substring(i)) + 1).ToString().PadLeft(4,'0');

				respuesta    = PFestatus("E");
				respuesta    = PFultimo();

				i            = (respuesta.LastIndexOf(",") + 1);
				exento       = (Convert.ToDecimal(respuesta.Substring(i)) / 100);

				respuesta    = PFestatus("A");
				respuesta    = PFultimo();

				j            = (respuesta.LastIndexOf(",") + 1);
				i            = (j - 13);
				base1        = (Convert.ToDecimal(respuesta.Substring(i,12)) / 100);
				iva1         = (Convert.ToDecimal(respuesta.Substring(j)) / 100);

				respuesta    = PFestatus("B");
				respuesta    = PFultimo();

				j            = (respuesta.LastIndexOf(",") + 1);
				i            = (j - 13);
				base2        = (Convert.ToDecimal(respuesta.Substring(i,12)) / 100);
				iva2         = (Convert.ToDecimal(respuesta.Substring(j)) / 100);

				respuesta    = PFestatus("C");
				respuesta    = PFultimo();

				j            = (respuesta.LastIndexOf(",") + 1);
				i            = (j - 13);
				base3        = (Convert.ToDecimal(respuesta.Substring(i,12)) / 100);
				iva3         = (Convert.ToDecimal(respuesta.Substring(j)) / 100);
				
				ncexento     = 0;
				ncbase1      = 0;
				nciva1       = 0;
				ncbase2      = 0;
				nciva2       = 0;
				ncbase3      = 0;
				nciva3       = 0;
				ultfactnum   = "";
				ultfactfecha = DateTime.Now;
				ultncnum     = "";
				ultncfecha   = DateTime.Now;

				ok           = evalua_errores(respuesta);

				respuesta    = PFcierrapuerto();

				if (ok)
				{
					fiscal.RepZtoXML(repznum,exento,base1,iva1,base2,iva2,base3,iva3,ncexento,ncbase1,nciva1,ncbase2,nciva2,ncbase3,nciva3,ultfactnum,ultfactfecha,ultncnum,ultncfecha);
				}

			}

			return(ok);
		}

		public static bool CancelaDoc(string puerto)
		{
			string respuesta;
			bool ok = false;

			if ( abrir_puerto(puerto) )
		    {
				respuesta = PFCancelaDoc("C","0");
			    ok        = evalua_errores(respuesta);
			    respuesta = PFcierrapuerto();
		    }

			return(ok);
		}

		public static bool abrir_gaveta(string puerto)
		{
			string respuesta;
			bool ok = false;

			if ( abrir_puerto(puerto) )
		    {
			    respuesta = PFGaveta();
			    ok        = evalua_errores(respuesta);	
			    respuesta = PFcierrapuerto();
		    }

			return(ok);
		}

		public static bool abrir_puerto(string puerto)
		{
			string respuesta;
			bool ok;

			respuesta = PFabrepuerto(puerto);
			ok        = evalua_errores(respuesta);

			return(ok);
		}

		public static bool evalua_errores(string respuesta)
		{
			bool ok = true;

			switch (respuesta)
			{
				case "ER":
					MessageBox.Show("Existe un error");
					ok = false;
					break;
				case "NP":
					MessageBox.Show("Puerto no abierto");
					ok = false;
					break;
				case "TO":
					MessageBox.Show("Se excedió el tiempo de respuesta esperado del equipo");
					ok = false;
					break;
			}

			return(ok);
		}
	}
}